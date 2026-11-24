using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Models;
using StudyFlow.DTOs; 
using StudyFlow.DAL;
using System.Security.Claims;
namespace StudyFlow.Controllers; 


[ApiController]
[Route("api/[controller]")]
    public class BookingAPIController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingAPIController> _logger;

         


        public BookingAPIController( IBookingRepository bookingRepository, ILogger<BookingAPIController> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;

           // _userManager = userManager; 
        }

        
        
        
     
        [HttpGet("bookinglist")]
        [Authorize]
        public async Task<IActionResult> BookingList()
        {
            var bookings = await _bookingRepository.GetAll();

            if (bookings == null)
            {
                _logger.LogError("[BookingAPICOntroller] Bookings list cannot be found while excecuting GetAll()");
                return NotFound("Item List Not Found");
            } 


                IEnumerable<Booking> bookingList = bookings.OrderBy(b => b.StartTime);
                var bookingDtos = bookingList.Select(booking => new BookingDto
                {
                    RoomId = booking.RoomId,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime

                });



            return Ok(bookingDtos);
        }


        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
    {

        var booking = await _bookingRepository.GetById(id); 
        if (booking !=null)
        {
            return Ok(new BookingDto{
                RoomId = booking.RoomId, 
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
            
            }); 
        }

          _logger.LogWarning("[BookingApiController] Get booking {bookingID} failed", id); 
            return StatusCode(500, "Internal Server Error");
    }
       

       



        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookingDto bookingDto)
        {

            // Convert Time to recognized format
            DateTime start = bookingDto.StartTime.ToUniversalTime();
            DateTime end = bookingDto.EndTime.ToUniversalTime();


            //Legg Til Hent Bruker 


            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            



            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Must be logged in");
            }


            if (start < DateTime.Now)
            {
               return BadRequest("Start Time must be later");
            }

            if (end <= start)
            {
                 return BadRequest("Sluttid må være etter starttid.");
            }

            // Attempt to add booking
            
            bool overlap = await _bookingRepository.HasOverlapAsync(
                bookingDto.RoomId,
                start,
                end); 

            if (overlap) 
            { return BadRequest("Room is booked");}

            var newBooking = new Booking
            {
                UserId = userId, 
                RoomId = bookingDto.RoomId,
                StartTime = start, 
                EndTime = end, 
                IsStudySession = bookingDto.IsStudySession
            };
            bool returnOk = await _bookingRepository.CreateAsync(newBooking); 
            if (returnOk)
                return CreatedAtAction(nameof(BookingList), new BookingDto
                {
                    RoomId = newBooking.RoomId,
                    StartTime = newBooking.StartTime, 
                    EndTime = newBooking.EndTime, 
                    IsStudySession = bookingDto.IsStudySession
                }); 

            _logger.LogWarning("[BookingAPIController] new Booking failed ", newBooking);

            return StatusCode(500, "Internal Server Error"); 
        
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] BookingDto bookingDto,
            int id)
        {

            // Find Booking
            var booking = await _bookingRepository.GetById(id); 
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }


            DateTime start = bookingDto.StartTime.ToUniversalTime();
            DateTime end = bookingDto.EndTime.ToUniversalTime();
 


            //Check Booking belongs to correct user
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.UserId != userId) return Forbid();


             if (start < DateTime.Now)
            {
               return BadRequest("Start Time must be later");
            }

            if (end  <= start )
            {
                return BadRequest("End Time must Be after StartTime.");
            }
        //Check overlapo OK
            var overlap = await _bookingRepository.HasOverlapAsync(
                bookingDto.RoomId, 
                start, 
                end,
                excludeBookingId: id);

            if (overlap)
            {
                return BadRequest("Room is already booked in this time slot.");
            }

            booking.RoomId = bookingDto.RoomId;
            booking.StartTime = start;
            booking.EndTime = end;
            booking.IsStudySession = bookingDto.IsStudySession;

            var returnOk = await _bookingRepository.UpdateAsync(booking);

            if (returnOk){return Ok(new BookingDto
            {
                RoomId = booking.RoomId, 
                StartTime = start,
                EndTime = end,
                IsStudySession = booking.IsStudySession,
            });
            }
            _logger.LogWarning("[BookingApiController] Updating booking {bookingID} failed", id); 
            return StatusCode(500, "Internal Server Error");
        }

     
     
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            var booking = await _bookingRepository.GetById(id); 

             //Check Booking belongs to correct user
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking != null && booking.UserId != userId) return Forbid();



            var returnOk = await _bookingRepository.DeleteAsync(id);  
            if (returnOk) return Ok("Booking Deleted!"); 


            return StatusCode(500, "Internal Server Error");
        }
    }

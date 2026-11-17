using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
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

        //private readonly UserManager<IdentityUser> _userManager; 


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
       



        [Authorize]
        [HttpPost("create")]
        //[Authorize]
        //[ValidateAntiForgeryToken]
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
                EndTime = end 
            };
            bool returnOk = await _bookingRepository.CreateAsync(newBooking); 
            if (returnOk)
                return CreatedAtAction(nameof(BookingList), newBooking); 

            _logger.LogWarning("[BookingAPIController] new Booking failed ", newBooking);

            return StatusCode(500, "Internal Server Error"); 
        
        }
/*
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,RoomId,StartTime,EndTime")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError(
                    nameof(Booking.EndTime),
                    "Sluttid må være etter starttid.");
            }

            var overlap = await _context.Bookings.AnyAsync(b =>
                b.Id != booking.Id &&
                b.RoomId == booking.RoomId &&
                b.StartTime < booking.EndTime &&
                booking.StartTime < b.EndTime);

            if (overlap)
            {
                ModelState.AddModelError(
                    nameof(Booking.RoomId),
                    "Rommet er allerede booket i dette tidsrommet.");
            }

            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            var existingBooking = await _context.Bookings.FindAsync(id);

            if (existingBooking == null)
            {
                return NotFound();
            }

            existingBooking.RoomId = booking.RoomId;
            existingBooking.StartTime = booking.StartTime;
            existingBooking.EndTime = booking.EndTime;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }*/
    }

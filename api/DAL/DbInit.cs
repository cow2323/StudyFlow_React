using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using StudyFlow.Models;
using System; 



namespace StudyFlow.DAL;


    public static class DbInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            
            using var serviceScope = app.ApplicationServices.CreateScope(); 
            StudyFlowDbContext context = serviceScope.ServiceProvider.GetRequiredService<StudyFlowDbContext>();
            context.Database.EnsureCreated();


            //--------Seed Users (AuthDbContext) ----------


var authContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>(); 
//authContext.Database.Migrate(); //Migrates existing DB / and creates AuthDB if missing

authContext.Database.EnsureCreated();

var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AuthUser>>(); 
    {
            var seedUsers = new List<AuthUser> {
                new AuthUser { UserName = "user1", Email = "user1@users.com"},
                new AuthUser { UserName = "user2", Email = "user2@users.com"},
                new AuthUser { UserName = "user3", Email = "user3@users.com"} 
                
                }; 
                
                for (int i = 0; i < seedUsers.Count; i++)
                {

                    AuthUser user = seedUsers[i];

                    //If user Doesnt exist we want to create SEED user
                    if(userManager.FindByNameAsync(user.UserName!).Result == null){

                    var result = userManager.CreateAsync(user, "Password123!").Result;
                    
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                    }
                
                }



        if (!context.Rooms.Any())
            {
                
            
                var rooms = new List<Room>(); 

                for (int i = 1; i <= 10; i++)
                {
                    
                    int roomSize = RandomNumberGenerator.GetInt32(4);
                    int size = 0;
                    string sizeDescription = string.Empty; 

                    switch (roomSize)

                    {

                        case 0: 
                        size = 4;
                        sizeDescription = "Small Study Room";
                        break; 

                        case 1: 
                        size = 8; 
                        sizeDescription = "Medium Study Room";
                        break;

                        case 2:
                        size = 16;
                        sizeDescription = "Large Study Room";
                        break;

                        case 3: 
                        size = 32;
                        sizeDescription = "Classroom";
                        break;
                        

                    }


                    rooms.Add(new Room{
                        Name = $"Room {i}",
                        Capacity = size,
                        Location = "Pilestredet 32, Oslo",
                        Description = sizeDescription

                    });
                    
                    }

                    context.Rooms.AddRange(rooms);
                    context.SaveChanges();




                }

            
    
            }


        context.Bookings.Add(new Booking
        {
            UserId = "7be159ee-ae72-4e8e-ac45-c4a288deded6",
            RoomId = 1,
            StartTime = Convert.ToDateTime("2026-11-12 09:00:00"),
            EndTime = Convert.ToDateTime("2026-11-12 10:00:00")


            
        }); 

        context.SaveChanges();

        }
        
        
        
        }
    
    
    
    

        
        
        
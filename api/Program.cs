using Microsoft.EntityFrameworkCore; 
using StudyFlow.DAL;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using Microsoft.AspNetCore.Identity; 
using StudyFlow.Models; 
using System.IdentityModel.Tokens.Jwt; 



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
   options.JsonSerializerOptions.ReferenceHandler =  // Prevents circular reference exceptions
   System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; 
});


builder.Services.AddOpenApi();

builder.Services.AddDbContext<StudyFlowDbContext>(options => //DB CONTEXT Declaration
{
    options.UseSqlite(builder.Configuration.GetConnectionString("StudyFlowDbContextConnection"));   //ADD DB

});

//Authentication DB 
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("AuthDbContextConnection"));
}); 

builder.Services.AddIdentity<AuthUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders(); 


builder.Services.AddAuthorization(); 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
     
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false; 
    options.SaveToken = true; 
     

     // OBS: set to true in production
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {   
        RoleClaimType = "role", //Add Role Based Authorization
        NameClaimType = JwtRegisteredClaimNames.Sub,


        ValidateIssuer = true, //validate token issuer is correct
        ValidateAudience = true,//validate token recipent correect
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //Validate JWT Signature
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //reading issuer  of token
        ValidAudience = builder.Configuration["Jwt:Audience"], // Reading audience of token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not found in configuration ")
        ))
    }; 
    //  ==================================================================
    //                    Login Authentication  Debug Options
    // ===================================================================
    // options.Events = new JwtBearerEvents
    // {
    //     OnMessageReceived = context =>
    //     {
    //         var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
    //         Console.WriteLine($"OnMessageReceived - Token: {token}");
    //         return Task.CompletedTask;
    //     },
    //     OnTokenValidated = context =>
    //     {
    //         Console.WriteLine("OnTokenValidated: SUCCESS");
    //         return Task.CompletedTask;
    //     },
    //     OnAuthenticationFailed = context =>
    //     {
    //         Console.WriteLine($"OnAuthenticationFailed: {context.Exception.Message}");
    //         Console.WriteLine($"Exception Type: {context.Exception.GetType().Name}");
    //         if (context.Exception.InnerException != null)
    //         {
    //             Console.WriteLine($"Inner Exception: {context.Exception.InnerException.Message}");
    //         }
    //         return Task.CompletedTask;
    //     },
    //     OnChallenge = context =>
    //     {
    //         Console.WriteLine($"OnChallenge: {context.Error} - {context.ErrorDescription}");
    //         return Task.CompletedTask;
    //     }
    // };    


}); 










builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
       policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
       .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
}); 






builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IStudySessionRepository, StudySessionRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

// Fresh StudyFlow DB on every dev start. Must run before any DbContext opens the file.
if (builder.Environment.IsDevelopment())
{
    foreach (var f in new[] { "StudyFlowDatabase.db", "StudyFlowDatabase.db-wal", "StudyFlowDatabase.db-shm" })
        File.Delete(f);
}

var loggerConfiguration = new LoggerConfiguration()
   .MinimumLevel.Warning()
   .WriteTo.File($"Logs/api_{DateTime.Now:yyyyMMdd_HHmmss}.log");

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    DbInit.Seed(app);
    app.MapOpenApi(); //Serve modern Open API document endpoint
    app.MapScalarApiReference(); //Serve scalar Interactive APi documentation
}


app.MapStaticAssets();
app.UseCors("CorsPolicy"); 


//  ==================================================================
    //                    UseAuthentication()  Debug Options
    // ===================================================================
// app.Use(async (context, next) =>
// {
//     if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
//     {
//         var headerValue = authHeader.FirstOrDefault();
//         if (headerValue?.StartsWith("Bearer ") == true)
//         {
//             var token = headerValue.Substring("Bearer ".Length).Trim();

//             // Decode the token to see its contents (without verification)
//             var handler = new JwtSecurityTokenHandler();
//             var jsonToken = handler.ReadJwtToken(token);

//             Console.WriteLine($"--> Token Issuer: {jsonToken.Issuer}");
//             Console.WriteLine($"--> Token Audience: {jsonToken.Audiences.FirstOrDefault()}");
//             Console.WriteLine($"--> Token Expiry: {jsonToken.ValidTo}");
//             Console.WriteLine($"--> Current Time: {DateTime.UtcNow}");
//             Console.WriteLine($"--> Config Issuer: {builder.Configuration["Jwt:Issuer"]}");
//             Console.WriteLine($"--> Config Audience: {builder.Configuration["Jwt:Audience"]}");
//         }
//     }
//     await next.Invoke();
// });


app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers();

app.Run();
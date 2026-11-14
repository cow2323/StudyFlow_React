using Microsoft.EntityFrameworkCore; 
using StudyFlow.DAL;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;


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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
       policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
       .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
}); 

//builder.Services.AddScoped<IItemRepository, ItemRepository>();

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


app.UseCors("CorsPolicy"); 
app.MapStaticAssets();
app.MapControllers();

app.Run();
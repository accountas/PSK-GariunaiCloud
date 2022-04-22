

using System.Configuration;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//add context
builder.Services.AddDbContext<GariunaiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GariunaiDatabase")));

//add services
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IUserService, UserService>();

//other random shit
builder.Services.AddAutoMapper(typeof(Profiles));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//init database
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<GariunaiDbContext>())
{
    context.Database.SetCommandTimeout(TimeSpan.FromMinutes(3));
    context.Database.Migrate();
}
       

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
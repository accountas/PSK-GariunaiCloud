using System.Reflection;
using System.Text;
using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

//add database context
builder.Services.AddDbContext<GariunaiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GariunaiDatabase")));

//add services
builder.Services.AddScoped<IAccessLogger, AccessLogger>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.Decorate<IListingService, LoggedListingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Decorate<IUserService, LoggedUserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.Decorate<IOrderService, LoggedOrderService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<IPasswordHashingStrategy, Hmacsha512PasswordHashingStrategy>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

//add jwt Authentication schema
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

//add swagger with authorisation support
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
    
    //to show xml docs in swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//other random shit
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

//build app
var app = builder.Build();

//init database
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetRequiredService<GariunaiDbContext>())
{
    // context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
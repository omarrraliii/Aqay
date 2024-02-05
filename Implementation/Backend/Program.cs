using Aqay_v2.Helpers;
using Microsoft.OpenApi.Models;
using Aqay_v2.Context;
using Microsoft.EntityFrameworkCore;
using Aqay_v2.Models;
using Microsoft.AspNetCore.Identity;
using Aqay_v2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Define connection string 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Introduce DB Context
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseSqlServer(connectionString)
);

//Introduce Identity User for custom user managment
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Introduce Auth Service 
builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
var configuration = builder.Configuration;
builder.Services.Configure<JWT>(configuration.GetSection("JWT"));

//Emailing system configuration
builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

//Register Services
builder.Services.AddTransient<ICategoryService,CategoryService>();


//Introduce a default schema Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(bearerOptions =>
{
    bearerOptions.RequireHttpsMetadata = false;
    bearerOptions.SaveToken = false;
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = configuration["JWT:Issuer"],
        ValidAudience = configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
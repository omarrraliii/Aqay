using aqay_apis.Helpers;
using aqay_apis.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using aqay_apis.Context;
using aqay_apis.Models;
using aqay_apis;

var builder = WebApplication.CreateBuilder(args);

// Define connection string 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Introduce DB Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Introduce global variable service
builder.Services.AddSingleton<GlobalVariables>();

// Introduce Identity User for custom user management
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Introduce Auth Service 
builder.Services.AddScoped<IAuthService, AuthService>();

// Introduce MailingService
builder.Services.AddTransient<IMailingService, MailingService>();
//add httpClient
builder.Services.AddHttpClient();

// Introduce system services
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService,BrandService>();
builder.Services.AddScoped<ITagService,TagService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFAQService,FAQService>();

// Introduce Azure Blob Service 
builder.Services.AddSingleton<IAzureBlobService, AzureBlobService>();

// Add controllers and endpoints
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

// Configure MailService
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Configure JWT authentication
var configuration = builder.Configuration;
builder.Services.Configure<JWT>(configuration.GetSection("JWT"));

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

// Initialize global variables
var globalVariables = app.Services.GetRequiredService<GlobalVariables>();
globalVariables.PageSize = 10;

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

// Activating the Auth Services
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

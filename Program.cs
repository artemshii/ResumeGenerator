using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResumeGenerator.Data;
using ResumeGenerator.Data.Models;
using System.IdentityModel.Tokens;
using Stripe;
using PdfSharp.Fonts;
using Newtonsoft.Json.Linq;
using ResumeGenerator.Data.Interfaces;
using ResumeGenerator.Data.ResumeStyles;



GlobalFontSettings.UseWindowsFontsUnderWindows = true;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration["StripeToken"];

builder.Services.AddHttpClient();

// Add services

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Your services
builder.Services.AddScoped<IPdfGenerator, ClassicPdfGenerator>();
builder.Services.AddScoped<IPdfGenerator, ProjectsPdfGenetor>();
builder.Services.AddScoped<IPdfGenerator, ColoredPdfGenerator>();
builder.Services.AddScoped<IOpenAiIntegration, OpenAiIntegration>();
builder.Services.AddScoped<IInvoiceGenerator, InvoiceGenerator>();
builder.Services.AddScoped<IJWTTokenCreator, JWTtokenCreator>();
builder.Services.AddScoped<CompleteData>();
builder.Services.AddScoped<KeyWordsForAi>();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddScoped<PaymentId>();

// JWT Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtToken"] ?? " ")),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero

        };




    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Demo API", Version = "v1" });

    // Add JWT bearer authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: {your JWT token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
};


    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();

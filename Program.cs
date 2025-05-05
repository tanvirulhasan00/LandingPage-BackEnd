using System.Text;
using LandingPage.Data;
using LandingPage.Models;
using LandingPage.Repositories;
using LandingPage.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 1234asdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "LandingPage v1",
        Description = "Api to manage asset",
        TermsOfService = new Uri("https://cookiessoftwaresolution.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Cookies Software Solution Ltd.",
            Url = new Uri("https://cookiessoftwaresolution.com")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://cookiessoftwaresolution.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "LandingPage v2",
        Description = "Api to manage asset",
        TermsOfService = new Uri("https://cookiessoftwaresolution.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Cookies Software Solution Ltd.",
            Url = new Uri("https://cookiessoftwaresolution.com")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://cookiessoftwaresolution.com/license")
        }
    });
});
builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LandingPageDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Db connect
builder.Services.AddDbContext<LandingPageDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LiveDatabase")));

// Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var key = builder.Configuration.GetValue<string>("TokenSetting:SecretKey") ?? "";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
});

// Add Cors.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
{
    policy.WithOrigins(
        "http://localhost:3000",
        "http://localhost:3001",
        "http://localhost:5000",
        "http://localhost:5001",
        "http://localhost:5173",
        "http://localhost:5274",
        "http://httpool-001-site1.anytempurl.com",
        "https://www.travello.agency",
        "https://travello.agency",
        "https://verdant-brioche-d5af3b.netlify.app"
    )
    .AllowAnyHeader()
    .AllowAnyMethod();
});

    // options.AddPolicy("AllowAll", policy =>
    // {
    //     policy.AllowAnyOrigin()
    //           .AllowAnyHeader()
    //           .AllowAnyMethod();
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LandingPage Api v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "LandingPage Api v2");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LandingPage Api v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "LandingPage Api v2");
    });
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.UseCors("AllowSpecificOrigins");
// Enable serving static files from wwwroot
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

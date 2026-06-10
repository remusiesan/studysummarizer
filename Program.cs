using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StudySummarizer;
using StudySummarizer.Application.Interfaces;
using StudySummarizer.Application.Repositories;
using StudySummarizer.Application.Settings;
using StudySummarizer.Data;
using StudySummarizer.DTOs;
using StudySummarizer.DTOs.Auth;
using StudySummarizer.DTOs.Documents;
using StudySummarizer.Infrastructure.Services;
using StudySummarizer.Middleware;
using StudySummarizer.Repository;
using StudySummarizer.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(Constants.Jwt.SchemeId, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = Constants.Jwt.SchemeName,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = Constants.Jwt.Scheme,
        BearerFormat = Constants.Jwt.BearerFormat,
        Description = Constants.Jwt.Description
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = Constants.Jwt.SchemeId
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(Constants.Database.SqliteConnection));

// Infrastructure
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();
builder.Services.AddSingleton<IIdGeneratorService, IdGeneratorService>();

// Infrastructure services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Validators
builder.Services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
builder.Services.AddScoped<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
builder.Services.AddScoped<IValidator<DocumentUploadRequest>, DocumentUploadRequestValidator>();
builder.Services.AddScoped<IValidator<PaginationRequest>, PaginationRequestValidator>();

var jwtSecret = builder.Configuration[Constants.Jwt.ConfigKeySecret] ?? Constants.Jwt.DefaultSecret;
var jwtIssuer = builder.Configuration[Constants.Jwt.ConfigKeyIssuer] ?? Constants.Jwt.DefaultIssuer;
var jwtAudience = builder.Configuration[Constants.Jwt.ConfigKeyAudience] ?? Constants.Jwt.DefaultAudience;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy(Constants.Cors.AllowAllPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Authorization", "Content-Type");
    });
});

builder.Services.AddLogging(config => config.AddSerilog());
builder.Services.AddHealthChecks();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(Constants.Logging.LogFilePattern, rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors(Constants.Cors.AllowAllPolicy);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(Constants.Swagger.JsonEndpoint, Constants.Swagger.Title);
    c.RoutePrefix = Constants.Swagger.RoutePrefix;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run("http://localhost:5000");

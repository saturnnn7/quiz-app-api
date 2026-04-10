using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using quiz.app.api.Data;
using quiz.app.api.DTOs.Account;
using quiz.app.api.DTOs.Quiz;
using quiz.app.api.DTOs.Result;
using quiz.app.api.Repositories.Implementations;
using quiz.app.api.Repositories.Interfaces;
using quiz.app.api.Services.Implementations;
using quiz.app.api.Services.Interfaces;
using quiz.app.api.Validators;

var builder = WebApplication.CreateBuilder(args);

// ===== Database =====
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<AppDbContext>());

// ===== Repositories =====
builder.Services.AddScoped<IUserRepository,   UserRepository>();
builder.Services.AddScoped<IQuizRepository,   QuizRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();

// ===== Services =====
builder.Services.AddScoped<ITokenService,  TokenService>();
builder.Services.AddScoped<IAuthService,   AuthService>();
builder.Services.AddScoped<IQuizService,   QuizService>();
builder.Services.AddScoped<IResultService, ResultService>();

// ===== Validators =====
builder.Services.AddScoped<IValidator<RegisterDto>,         RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>,            LoginDtoValidator>();
builder.Services.AddScoped<IValidator<CreateQuizDto>,       CreateQuizDtoValidator>();
builder.Services.AddScoped<IValidator<SubmitResultDto>,     SubmitResultDtoValidator>();

// ===== Authentication =====
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = builder.Configuration["Jwt:Issuer"],
        ValidAudience            = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew                = TimeSpan.Zero,
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = """{"success":false,"message":"Unauthorized. Token is missing or invalid.","errors":[]}""";
            return context.Response.WriteAsync(response);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode  = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = """{"success":false,"message":"Forbidden. You do not have permission to access this resource.","errors":[]}""";
            return context.Response.WriteAsync(response);
        },
    };
});

builder.Services.AddAuthorization();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// ===== Controllers & Swagger =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "QuizApi", Version = "v1" });

    // Enable JWT auth in Swagger UI
    options.AddSecurityDefinition("Bearer", new()
    {
        Name         = "Authorization",
        Type         = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description  = "Enter your JWT token. Example: eyJhbGci...",
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer",
                }
            },
            []
        }
    });
});

// ===== Build =====
var app = builder.Build();

// ===== Middleware pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ===== Apply migrations on startup =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Only run migrations for relational databases (not InMemory used in tests)
    if (db.Database.IsRelational())
        db.Database.Migrate();
}

app.Run();

public partial class Program { }
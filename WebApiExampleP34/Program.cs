using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using WebApiExampleP34.Application;
using WebApiExampleP34.Application.Services;
using WebApiExampleP34.Application.Swagger;
using WebApiExampleP34.Infrastructure;
using WebApiExampleP34.Infrastructure.Services;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DAL;
using WebApiExampleP34.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Введіть JWT-токен",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Применяем эту схему глобально ко всем методам
    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] { }
    //    }
    //});

    options.OperationFilter<AuthorizeCheckOperationFilter>();

});

builder.Services.AddDbContext<TodoListContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ITodoListService, TodoListService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // Allow requests from any origin
               .AllowAnyMethod()   // Allow any HTTP method (GET, POST, PUT, etc.)
               .AllowAnyHeader();  // Allow any header
    });
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
builder.Services
    .AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.FromMinutes(5)
        };

        // прервірка токена
        // якщо немає OnChallenge 
        // якщо є але невірний чи прострочений токен  OnAuthenticationFailed - OnChallenge 

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Подія виконується, коли токен відсутній або недійсний
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                    OperationResult.Fail("Invalid token"))
                    );
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                // Подія виконується, коли автентифікація не вдалася
                Console.WriteLine("JWT Auth failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
        };

    });

builder.Services.AddIdentityCore<User>(options => {
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<TodoListContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");


// Використання інших заголовків для авторизації
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Auth-Token", out var token))
    {
        context.Request.Headers["Authorization"] = $"Bearer {token}";
    }
    await next();
});

app.MapFallbackToFile("index.html");

app.Run();

/*
 * Якщо не авторизований користувач - показуємо форму входу
 * Модно переключитися на форму реєстрації
 * На формах показуємо помилки входу/реєстрації
 * 
 * Якщо зареєстрований користувач - показуємо профіль користувача та кнопку вийти
 * 
 * 
 * 
 */ 
using Core;
using Core.ApiInterface;
using Core.Middleware;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;

using Infrastructure;
using Microsoft.OpenApi.Models;
using Services;
using Services.NotificationServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

#region Injection

builder.Services

    .AddDBInjection(builder.Configuration)
    .AddAuthServices(builder.Configuration)
    .AddHangfirejection(builder.Configuration)
    .AddMailSetting(builder.Configuration)
    .AddNotificationjection()
    .AddDepInjection()
    .AddServiesInjection()
    .AddSessionDep()
    .AddCoredDependencies();

#endregion Injection

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://127.0.0.1:5500") // السماح فقط لهذا الأصل
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter JWT token with 'Bearer ' prefix.",
    });

    // Add Security Requirement for Bearer token
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/Dashboard");
// تشغيل الوظيفة المجدولة
//RecurringJob.AddOrUpdate<INotificationServices>(
//    "weekly_notification",
//    x => x.SendWeeklyNotification(),
//     "*/2 * * * *"
//     //"0 10 * * 0" // كل يوم أحد الساعة 10 صباحًا
//);

app.UseCors("AllowSpecificOrigin"); // تأكد من إنه مفعّل

app.UseSession(); // لازم يكون قبل Authorization

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
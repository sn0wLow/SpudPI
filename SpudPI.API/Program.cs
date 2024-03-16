using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpudPI;
using SpudPI.API;
using SpudPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddSingleton<IVoicemodService, VoicemodService>();
builder.Services.AddScoped<IPongService, PongService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddHostedService<PasswordInitialization>();
builder.Services.AddHostedService<VoicemodConnectionInitializer>();
builder.Services.AddHostedService<VoicemodConnectionMonitorService>();

builder.Services.AddHealthChecks().AddCheck<VoicemodConnectionHealthService>("voicemod_connection");


builder.Services.AddControllers();
builder.Services.AddResponseCaching();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"] ??
        throw new InvalidOperationException("Token not found")))
    };
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        ValidateAudience = false,
//        ValidateIssuer = false,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                builder.Configuration.GetSection("AppSettings:Token").Value!)),
//        RequireExpirationTime = false,
//        ValidateLifetime = true
//    };
//});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer [jwt]'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
});

builder.Services.AddHttpContextAccessor();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAnyOrigin",
//        policy => policy.AllowAnyOrigin()
//                        .AllowAnyMethod()
//                        .AllowAnyHeader());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin",
        policy => policy.WithOrigins("http://localhost:5059")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<VoicemodConnectionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();

if (app.Environment.IsDevelopment())
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();

app.MapHealthChecks("/health");

//app.UseCors("AllowAnyOrigin");
app.UseCors("AllowBlazorOrigin");

app.Run();

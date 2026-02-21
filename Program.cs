using AuthorizationIntegration.Application.Cache;
using AuthorizationIntegration.Application.Mappings;
using AuthorizationIntegration.Application.Services;
using AuthorizationIntegration.Core.Database;
using AuthorizationIntegration.Core.Helper;
using AuthorizationIntegration.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<EmailService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAuthorization();
builder.Services.AddDbContext<UserDbContext>(i =>
{
  i.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://ltdluka.ge/",
        ValidAudience = "https://ltdluka.ge/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperDuperSecretKeyNoOneCanBreakAndFindCuz5+5=10")),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Management", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
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

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "User_Authentiation_Project_V1");
  });
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

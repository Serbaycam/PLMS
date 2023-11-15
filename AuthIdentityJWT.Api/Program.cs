using AuthIdentityJWT.Core.Configuration;
using AuthIdentityJWT.Core.Repositories;
using AuthIdentityJWT.Core.Services;
using AuthIdentityJWT.Core.UnitOfWork;
using AuthIdentityJWT.Data;
using AuthIdentityJWT.Data.Repositories;
using AuthIdentityJWT.Data.UnitOfWork;
using AuthIdentityJWT.Service.Services;
using AuthIdentityJWT.SharedLibrary.Configurations;
using AuthIdentityJWT.SharedLibrary.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DI register
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("AuthServer.Data");
    });
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(op =>
{
    op.User.RequireUniqueEmail = true;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireDigit = false;
    op.Password.RequireUppercase = false;
    op.Password.RequireLowercase = false;
    op.Password.RequiredLength = 1;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();




// Add services to the container.
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
builder.Services.AddCustomTokenAuth(tokenOptions);


builder.Services.AddControllers().AddFluentValidation(op =>
{
    op.RegisterValidatorsFromAssemblyContaining<Program>();
});
builder.Services.UseCustomValidationResponse();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCustomException();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

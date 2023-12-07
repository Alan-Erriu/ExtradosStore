using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Configuration.JWTConfiguration;
using ExtradosStore.Data.DAOs.Implementations;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Implementations;
using ExtradosStore.Services.Interfaces;
using ExtradosStore.Services.Validations;

var builder = WebApplication.CreateBuilder(args);

//******************* start dependecies and options *********************

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRoleDAO, RoleDAO>();
builder.Services.AddScoped<IAuthDAO, AuthDAO>();
builder.Services.AddScoped<IJWTDAO, JWTDAO>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IValidations, Validations>();
builder.Services.AddScoped<IHasherService, BcryptHasher>();
builder.Services.Configure<SQLServerConfig>(builder.Configuration.GetSection("DBTestConnection"));
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JwtSettings"));
//******************* end dependecies and options **********************
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

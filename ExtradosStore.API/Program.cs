using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Implementations;
using ExtradosStore.Data.DAOs.Intefaces;
using ExtradosStore.Services.Implementations;
using ExtradosStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//******************* start dependecies and options *********************

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRoleDAO, RoleDAO>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.Configure<SQLServerConfig>(builder.Configuration.GetSection("DBTestConnection"));
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

using ExtradosStore.API.Middlewares;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Configuration.JWTConfiguration;
using ExtradosStore.Data.DAOs.Implementations;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Implementations;
using ExtradosStore.Services.Interfaces;
using ExtradosStore.Services.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//******************* start dependecies and options *********************

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//*********DAOS*******************************
builder.Services.AddScoped<IRoleDAO, RoleDAO>();
builder.Services.AddScoped<IAuthDAO, AuthDAO>();
builder.Services.AddScoped<IJWTDAO, JWTDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<ICarDAO, CarDAO>();
builder.Services.AddScoped<IPostDAO, PostDAO>();
builder.Services.AddScoped<IBrandDAO, BrandDAO>();
builder.Services.AddScoped<ICategoryDAO, CategoryDAO>();
builder.Services.AddScoped<IOfferDAO, OfferDAO>();
builder.Services.AddScoped<IOfferPostDAO, OfferPostDAO>();
builder.Services.AddScoped<IPostStatusDAO, PostStatusDAO>();
builder.Services.AddScoped<ISalesDAO, SalesDAO>();
builder.Services.AddScoped<ISalesDetailDAO, SalesDetailDAO>();
//*************Services*********************************
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISalesHistoryService, SalesHistoryService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IPostSearchService, PostSearchService>();
builder.Services.AddScoped<IOfferPostService, OfferPostService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IValidations, Validations>();
builder.Services.AddScoped<IHasherService, BcryptHasher>();
builder.Services.Configure<SQLServerConfig>(builder.Configuration.GetSection("DBTestConnection"));
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JwtSettings"));
//******************* end dependecies and options **********************


//*****************config jwt**************************************
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});



//*******cors****************************
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


//**********
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.MapControllers();

app.Run();

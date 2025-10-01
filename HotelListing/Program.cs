using HotelListing.Configurations;
using HotelListing.Contracts;
using HotelListing.Data;
using HotelListing.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<HotelDBContext>(options => options.UseSqlServer(connectString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().
AllowAnyMethod().
AllowAnyOrigin());
});
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddAutoMapper(cfg=>cfg.AddProfile<MapperConfig>() );
// Replace this line:
// builder.Services.AddAutoMapper(typeof(MapperConfig));
// With this line: add the type,the implementation no need to specify the type explicitly

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<IGenericRepository<Country>, GenericRepository<Country>>();

//Add the type/ interfacr and implementation of repository
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();

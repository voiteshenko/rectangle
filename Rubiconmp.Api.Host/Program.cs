using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Rubiconmp.Api.DataLayer;
using Rubiconmp.Api.Application;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RubiconmpConnection");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<ShapeContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ShapeContext>());

builder.Services.AddProblemDetails(options => { options.IncludeExceptionDetails = (context, exception) => false; })
    .AddRubiconmpProblemDetailsFactory();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseProblemDetails();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
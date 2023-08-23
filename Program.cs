using PruebaDapper.EXCELGenerator;
using PruebaDapper.PDFGenerator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7040", "*").WithMethods("POST", "PUT", "DELETE", "GET");
    });
});

builder.Services.AddScoped<PDFCliente>();
builder.Services.AddScoped<PDFVehiculo>();
builder.Services.AddScoped<EXCELCliente>();
builder.Services.AddScoped<EXCELVehiculo>();
builder.Services.AddScoped<EjemploPdf>();


var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;
        var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (contextFeature != null) await context.Response.WriteAsync(contextFeature.Error.Message);

    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors("AllowWebApp");

app.UseHttpsRedirection();

app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());  

app.UseAuthorization();

app.MapControllers();

app.Run();

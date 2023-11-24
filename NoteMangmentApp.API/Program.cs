using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NoteMangmentApp.API.Extensions;
using NoteMangmentApp.API.Middelware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
                           .AddJsonOptions(options =>
                           {
                               options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                               options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                           })
                           .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowAnyOrigin());
});

//===================End Services======================

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/Errors/{0}");

app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();


app.Run();

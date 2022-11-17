using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using SESDemo.Data;
using SESDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SESDemo.Interfaces;
using SESDemo.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Amazon.SimpleEmail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<AWSAccount>(builder.Configuration.GetSection("AWS"));
builder.Services.AddScoped<IEmailService, EmailServices>();
builder.Services.AddSweetAlert2();
builder.Services.AddAWSService<IAmazonSimpleEmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

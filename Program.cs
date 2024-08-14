//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using IntegratedMobileApp.Extensions;
using IntegratedMobileApp.Helpers;
using IntegratedMobileApp.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using tusdotnet;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
AppConfiguration.LoadConfiguration(configuration);


// Add services to the container.
services.AddControllersWithViews();

services.AddHttpContextAccessor();
services.AddMemoryCache();
services.AddSession();

services.AddDbContext<ImobappContext>();

services.AddScoped<IClaimsTransformation, UserClaimsTransform>();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
options =>
{
	options.LoginPath = "/Login";
	options.LogoutPath = "/Login/Logout";
	options.AccessDeniedPath = "/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();


//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	ServeUnknownFileTypes = true,
	DefaultContentType = "application/octet-stream"
});

app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider(AppConfiguration.UploadDocFolder!),
	RequestPath = "/UploadDocs"
});

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseTus(httpContext => new TusConfig());

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

var cookiePolicyOptions = new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict
};
app.UseCookiePolicy(cookiePolicyOptions);

app.Run();

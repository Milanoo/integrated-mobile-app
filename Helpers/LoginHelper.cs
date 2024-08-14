//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IntegratedMobileApp.Models.Database;
using System.Security.Claims;

namespace IntegratedMobileApp.Helpers
{
	public  class LoginHelper
	{
		public static async Task SignInUser(HttpContext context, TabUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, user.Umail!),
			};

			var claimsIdentity = new ClaimsIdentity(
				claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				IsPersistent = false,
				RedirectUri = "/Login"
			};

			await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
		}

		public static async Task SignOutUser(HttpContext context)
		{
			await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}
	}
}

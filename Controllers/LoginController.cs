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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegratedMobileApp.Controllers
{
	public class LoginController : Controller
	{
		ImobappContext DbContext;
		
		public LoginController(ImobappContext dbContext)
		{
			DbContext = dbContext;			
		}

		public IActionResult Index(string ReturnUrl)
		{
			return View("Index", ReturnUrl);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckLogin(string email, string pwd, string returnUrl)
		{
			IActionResult InvalidResponse(string msg)
			{
				ViewData["email"] = email;
				ViewData["Message"] = msg;
				return View("Index", returnUrl);
			}

			var user = DbContext.TabUsers.FirstOrDefault(u => EF.Functions.ILike(u.Umail!, email));
			if (user == null) return InvalidResponse("Incorrect username or password");

			if (user.IsPublic()) return InvalidResponse("Public user cannot login.");			

			bool verified = PasswordHelper.VerifyHashedPassword(pwd, user.Password!); 
			if (verified)
			{
				if (user.Utype![1] == '0')
					return InvalidResponse("User not activated. Please check your email for instructions.");


				await LoginHelper.SignInUser(this.HttpContext, user);
				
				DbContext.TabUsers.Update(user);
				await DbContext.SaveChangesAsync();

				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}
				else
				{
					return RedirectToAction("Index", "AdminPanel");
				}
			}
			else
			{
				return InvalidResponse("Incorrect username or password");
			}
		}

		public async Task<IActionResult> Logout()
		{
			await LoginHelper.SignOutUser(this.HttpContext);
			return RedirectToAction("Index", "Home");
		}
	}
}

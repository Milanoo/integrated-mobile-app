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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntegratedMobileApp.Controllers
{
	[Authorize]
	public class AdminPanelController : Controller
	{

		ImobappContext DbContext;
		public AdminPanelController(ImobappContext dbContext)
		{
			DbContext = dbContext;
		}

		private Dictionary<string, int> MunCodeWardCount = new Dictionary<string, int>() {
			{"504",23 } ,
			{"508",10 } ,
			{"601",13 } ,
			{"618",13 } ,
			{"725",12 } ,
			{"720",19 }
		};

		public IActionResult Index(string? alertMessage = "")
		{
			var pm = User.GetProvinceMunicipality();
			var users = DbContext.TabUsers.Where(it => it.Upmw!.StartsWith(pm)).OrderBy(it => it.Id).ToList();
			if (User.IsAdmin())
			{
				users = users.Where(it => !it.IsAdmin()).ToList();
			}
			else if (User.IsMunicipality())
			{
				users = users.Where(it => !it.IsAdmin()).ToList();
				users = users.Where(it => !it.IsMunicipality()).ToList();
			}
			else if (User.IsWard())
			{
				var pmw = User.GetProvinceMunicipalityWard();
				users = users.Where(it => it.Upmw!.StartsWith(pmw)).ToList();
				users = users.Where(it => !it.IsAdmin()).ToList();
				users = users.Where(it => !it.IsMunicipality()).ToList();
				users = users.Where(it => !it.IsWard()).ToList();
			}
			ViewData["Users"] = users;
			ViewData["MunCodeWardCount"] = MunCodeWardCount;
			if(alertMessage!= null && alertMessage != "")
			{
				if (alertMessage.StartsWith("Success"))
				{
					TempData["SuccessMessage"] = alertMessage;
				}
				else if (alertMessage.StartsWith("Error"))
				{
					TempData["ErrorMessage"] = alertMessage;
				}
				else if (alertMessage.StartsWith("Info"))
				{
					TempData["InfoMessage"] = alertMessage;
				}
				else if (alertMessage.StartsWith("Warning"))
				{
					TempData["WarningMessage"] = alertMessage;
				}
			}
			
			return View();
		}


		public PartialViewResult GetUserToEdit(int userId)
		{
			var user = DbContext.TabUsers.FirstOrDefault(it => it.Id == userId);
			ViewData["User"] = user;
			ViewData["MunCodeWardCount"] = MunCodeWardCount;
			return PartialView("EditUser");
		}


		[HttpPost]
		public IActionResult EditUser([FromForm] TabUser user)
		{
			var msg = "";
			try
			{
				var existingUser = DbContext.TabUsers.FirstOrDefault(it => it.Id == user.Id);				
				if ((user.Umail!.IsValidEmail() || user.Umail!.IsValidUserName()) && user.Uname!.IsValidUserName())
				{
					existingUser!.Uname = user.Uname;
					existingUser!.Umail = user.Umail;
					existingUser!.Uphone = user.Uphone;
					existingUser!.Upmw = user.Upmw;
					existingUser!.Utype = user.Utype;
					if (user.Password != null && user.Password != "") existingUser!.Password = PasswordHelper.CreateHashedPwd(user.Password);
					DbContext.TabUsers.Update(existingUser);
					DbContext.SaveChanges();
					msg = "Success: User Edited.";
				}
				else
				{
					msg = "Warning: Invalid Email or UserName.";
				}				
			}
			catch (Exception ex)
			{
				msg = $"Error: {ex.Message}";
			}
			return RedirectToAction("Index", new { alertMessage = msg });			
			
		}

		[HttpPost]
		public IActionResult CreateUser([FromForm] TabUser user)
		{
			var msg = "";
			try
			{
				var existingUser = DbContext.TabUsers.FirstOrDefault(it => it.Umail == user.Umail);
				if (existingUser == null)
				{
					if ((user.Umail!.IsValidEmail() || user.Umail!.IsValidUserName()) && user.Uname!.IsValidUserName())
					{

						if (user.Password != null && user.Password != "")
						{
							user.Password = PasswordHelper.CreateHashedPwd(user.Password);
							user.CreatedOn = DateTime.Now;
							DbContext.TabUsers.Add(user);
							DbContext.SaveChanges();
							msg = "Success: User Created.";
						}
						else
						{
							msg = "Warning: Empty Passowrd.";
						}
					}
					else
					{
						msg = "Warning: Invalid Email or UserName.";
					}
				}
				else
				{
					msg = "Warning: User already exists.";
				}
			}
			catch(Exception ex) { 
				msg = $"Error: {ex.Message}"; 
			}			
			return RedirectToAction("Index", new { alertMessage = msg });
		}

	}
}


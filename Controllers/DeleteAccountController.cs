//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using IntegratedMobileApp.Helpers;
using IntegratedMobileApp.Models.Database;
using Microsoft.AspNetCore.Mvc;

namespace IntegratedMobileApp.Controllers
{
    public class DeleteAccountController : Controller
    {
        ImobappContext DbContext;

        public DeleteAccountController(ImobappContext dbContext)
        {
            DbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

    
        [HttpPost]
        public async Task<IActionResult> DeleteAccount([FromForm] string email,[FromForm] string pwd)
        {
            IActionResult InvalidResponse(string msg)
            {
                ViewData["email"] = email;
                ViewData["Message"] = msg;
                return View("Index");
            }

            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == email);
                if (user == null)
                {
                    return InvalidResponse("User not found");
                }
                else
                {

                    if (!PasswordHelper.VerifyHashedPassword(pwd, user.Password!))
                    {
                        return InvalidResponse("Incorrect password");
                    }
                    else
                    {
                        DbContext.TabUsers.Remove(user);
                        await DbContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                return InvalidResponse($"Error!{Environment.NewLine}{ex.Message}");
            }

            
        }
    }
}

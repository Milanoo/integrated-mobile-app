//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using IntegratedMobileApp.Controllers.API;
using IntegratedMobileApp.Extensions;
using IntegratedMobileApp.Helpers;
using IntegratedMobileApp.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegratedMobileApp.Controllers
{
	[Authorize]

	public class ServiceAdditionalController : Controller
	{

		ImobappContext DbContext;
		public ServiceAdditionalController(ImobappContext dbContext)
		{
			DbContext = dbContext;
		}

		public IActionResult Index()
		{
			var pm = User.GetProvinceMunicipality();

			var links = DbContext.TabLinks.Where(it => EF.Functions.ILike(it.Lkid!, $"{pm}%")).OrderBy(it => it.Id).ToList();
			ViewData["Links"] = links;

			var polls = DbContext.TabPolls.Where(it => EF.Functions.ILike(it.MunCode!, $"{pm}%")).OrderBy(it => it.Id).ToList();
			ViewData["Polls"] = polls;

			var preTexts = new List<TabPretext>();
			if (User.IsWard())
			{
				preTexts = DbContext.TabPretexts.Where(it => it.Usertype == 2).OrderBy(it => it.Id).ToList();
			} else if (User.IsMunicipality())
			{
				preTexts = DbContext.TabPretexts.Where(it => it.Usertype == 3).OrderBy(it => it.Id).ToList();
			}
			ViewData["PreTexts"] = preTexts;
			return View();

		}

		[HttpPost]
		public IActionResult AddLink([FromForm] int linkCount, [FromForm] string linkNameNep, [FromForm] string linkNameEng, [FromForm] string link)
		{
			if (linkCount == 0) linkCount = Convert.ToInt32($"{User.GetProvinceMunicipality()}00");
			if (linkNameNep != null && linkNameNep != "" && linkNameEng != null && linkNameEng != "" && link != null && link != "")
			{
				if (linkNameEng.IsValiLinkName())
				{
					var lnk = new TabLink()
					{
						Lkid = $"{(linkCount + 1):00}",
						Lnamenep = linkNameNep,
						Lnameeng = linkNameEng,
						Linkdetail = link,
					};
					DbContext.TabLinks.Add(lnk);
					DbContext.SaveChanges();
				}

			}
			return RedirectToAction("Index");
		}

		public IActionResult EditLink(string linkId, string linkNameNep, string linkNameEng, string link)
		{
			var lnk = DbContext.TabLinks.FirstOrDefault(it => it.Lkid == linkId);
			if (lnk != null && linkNameNep != null && linkNameNep != "" && linkNameEng != null && linkNameEng != "" && link != null && link != "")
			{
				if (linkNameEng.IsValiLinkName())
				{
					lnk.Lnamenep = linkNameNep;
					lnk.Lnameeng = linkNameEng;
					lnk.Linkdetail = link;
					DbContext.TabLinks.Update(lnk);
					DbContext.SaveChanges();
				}
			}
			return RedirectToAction("Index");
		}

		public IActionResult DeleteLink(string linkId)
		{
			var lnk = DbContext.TabLinks.FirstOrDefault(it => it.Lkid == linkId);
			if (lnk != null)
			{
				DbContext.TabLinks.Remove(lnk);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult AddPoll([FromForm] string qText, [FromForm] string[] oTexts)
		{
			if (qText != null && oTexts != null && oTexts!.Length > 0)
			{
				var polls = new List<TabPoll>();
				var munCode = User.GetProvinceMunicipality();
				var qUuid = Guid.NewGuid().ToString();
				foreach (var oText in oTexts)
				{
					var poll = new TabPoll()
					{
						MunCode = munCode,
						QUuid = qUuid,
						QText = qText,
						OUuid = Guid.NewGuid().ToString(),
						OText = oText
					};
					polls.Add(poll);
				}
				DbContext.TabPolls.AddRange(polls);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult EditPoll([FromForm] string qUuid, [FromForm] string qText, [FromForm] string[] oTexts)
		{
			if (qText != null && oTexts != null && oTexts!.Length > 0)
			{
				var polls = DbContext.TabPolls.Where(it=>it.QUuid == qUuid).OrderBy(it=>it.Id).ToList();
				if(polls != null && polls.Count > 0)
				{
					DbContext.TabPolls.RemoveRange(polls);
					polls = new List<TabPoll>();
					var munCode = User.GetProvinceMunicipality();
					foreach (var oText in oTexts)
					{
						var poll = new TabPoll()
						{
							MunCode = munCode,
							QUuid = qUuid,
							QText = qText,
							OUuid = Guid.NewGuid().ToString(),
							OText = oText
						};
						polls.Add(poll);
					}
					DbContext.TabPolls.AddRange(polls);
					DbContext.SaveChanges();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult DeletePoll([FromForm] string qUuid)
		{
			if (qUuid != null)
			{
				var polls = DbContext.TabPolls.Where(it => it.QUuid == qUuid).OrderBy(it => it.Id).ToList();
				if (polls != null && polls.Count > 0)
				{
					DbContext.TabPolls.RemoveRange(polls);					
					DbContext.SaveChanges();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult EditPollOption([FromForm] string oUuid, [FromForm] string oText)
		{
			if (oUuid != null && oText != null)
			{
				var poll = DbContext.TabPolls.FirstOrDefault(it => it.OUuid == oUuid);
				if (poll != null)
				{
					poll.OText = oText;
					DbContext.TabPolls.Update(poll);
					DbContext.SaveChanges();
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult DeletePollOption([FromForm] string oUuid)
		{
			if (oUuid != null)
			{
				var poll = DbContext.TabPolls.FirstOrDefault(it => it.OUuid == oUuid);
				if (poll != null )
				{															
					DbContext.TabPolls.Remove(poll);
					DbContext.SaveChanges();					
				}
			}
			return RedirectToAction("Index");
		}


		public IActionResult AddPreText(string nepPreText, string engPreText)
		{
			if (nepPreText != null && nepPreText != "" && engPreText != null && engPreText != "")
			{
				var userType = User.IsWard() ? 2 : User.IsMunicipality() ? 3 : User.IsAdmin() ? 4 : 0;
				var preText = new TabPretext()
				{				
					Neptext = nepPreText,
					Engtext = engPreText,
					Usertype = userType
				};
				DbContext.TabPretexts.Add(preText);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public IActionResult EditPreText(int Id, string nepPreText, string engPreText)
		{
			var preText  = DbContext.TabPretexts.FirstOrDefault(it => it.Id == Id);
			if (preText != null &&  nepPreText != null && nepPreText != "" && engPreText != null && engPreText != "")
			{				
				preText.Neptext = nepPreText;
				preText.Engtext = engPreText;
				DbContext.TabPretexts.Update(preText);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public IActionResult DeletePreText(int Id)
		{
			var preText = DbContext.TabPretexts.FirstOrDefault(it => it.Id == Id);
			if (preText != null)
			{			
				DbContext.TabPretexts.Remove(preText);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}
	}
}

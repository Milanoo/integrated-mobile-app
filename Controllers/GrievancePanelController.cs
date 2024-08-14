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
using Microsoft.AspNetCore.Mvc;

namespace IntegratedMobileApp.Controllers
{
	[Authorize]
	public class GrievancePanelController : Controller
	{
		ImobappContext DbContext;
		public GrievancePanelController(ImobappContext dbContext)
		{
			DbContext = dbContext;
		}

		public IActionResult Index(int partialViewId = 1)
		{
			ViewData["PartialViewId"] = partialViewId;
			return View();
		}

		public PartialViewResult PostGrievanceView()
		{			
			return PartialView("PostGrievance");
		}

		public PartialViewResult PendingGrievanceView()
		{
			var pmwd = User.GetProvinceMunicipalityWard();
			var grievances = DbContext.TabUsergrivs.Where(it => it.Grivid!.StartsWith(pmwd) && (it.Replyid == null || it.Replyid == "")).OrderByDescending(it => it.Id).ToList();
			if (User.IsWard())
			{
				pmwd = User.GetLocality();
			}
			else if (User.IsMunicipality())
			{
				pmwd = User.GetLocality();
			}
			if (!User.IsAdmin())
			{
				grievances = grievances.Where(it => it.Grivid!.StartsWith(pmwd) && it.Grivreceiver == pmwd && (it.Replyid == null || it.Replyid == "")).OrderByDescending(it => it.Id).ToList();
			}
			ViewData["Grievs"] = grievances;
			return PartialView("PendingGrievance");
		}

		public PartialViewResult RepliedGrievanceView()
		{
			var pmwd = User.GetProvinceMunicipalityWard();
			var grievances = DbContext.TabUsergrivs.Where(it => it.Grivid!.StartsWith(pmwd) && (it.Replyid != null && it.Replyid != "")).OrderByDescending(it => it.Id).ToList();
			if (User.IsWard())
			{
				pmwd = User.GetLocality();
			}
			else if (User.IsMunicipality())
			{
				pmwd = User.GetLocality();
			}
			if (!User.IsAdmin())
			{
				grievances = grievances.Where(it => it.Grivid!.StartsWith(pmwd) && it.Grivreceiver == pmwd && (it.Replyid != null && it.Replyid != "")).OrderByDescending(it => it.Id).ToList();
			}
			ViewData["Grievs"] = grievances;
			return PartialView("RepliedGrievance");
		}

		public IActionResult PostGrievance(string grievTxt, string wardCode, string deptCode)
		{
			var uPMWD = User.GetLocality();
			var uMail = User.GetEmail();
			var uName = User.GetFullName();
			var dt = $"{DateTime.Now:yyyyMMddHHmm}";
			var grievId = $"{uPMWD}-{dt}-{uMail}";
			var grievSender = $"Ward-{uPMWD.Substring(3, 2)}({uName})";
			var grievReceiver = $"{uPMWD[..3]}{wardCode}{deptCode}";
			var griev = new TabUsergriv()
			{
				Grivid = grievId,
				Grivtext = grievTxt,
				Grivsender = grievSender,
				Grivreceiver = grievReceiver,
				Gpic = "-",
				Replyid = $"",
				Replytext = "",
				Rpic = "-"
			};
			DbContext.TabUsergrivs.Add(griev);
			DbContext.SaveChanges();
			return RedirectToAction("Index", new { partialViewId = 1 });
		}

		public IActionResult ForwardGrievance(int Id, string PMWD, string WardCode, string DeptCode)
		{
			var newRecieverId = $"{PMWD[..3]}{WardCode}{DeptCode}";
			var departmentName = WardCode != "00" ? DepartmentHelper.WardDepartmentNep[WardCode] : DepartmentHelper.MunicipalityDepartmentNep[DeptCode];
			var grievance = DbContext.TabUsergrivs.FirstOrDefault(it => it.Id == Id);
			if (grievance != null && DeptCode != "00")
			{
				var grivId = grievance.Grivid!;
				var grivTxt = grievance.Grivtext!;
				var grivSender = grievance.Grivsender!;
				var dt = $"{DateTime.Now:yyyyMMddHHmm}";
				grievance.Replyid = $"{User.GetLocality()}-{dt}-{User.GetEmail()}";
				grievance.Replytext = $"तपाईको गुनासोलाई {departmentName}मा पठाइएको छ ।";
				grievance.Rpic = "-";
				DbContext.TabUsergrivs.Update(grievance);

				var nGrievance = new TabUsergriv()
				{
					Grivid = $"{grivId.Split('-')[0]}-{dt}-{grivId.Split('-')[2]}",
					Grivtext = grivTxt,
					Grivsender = grivSender,
					Grivreceiver = $"{PMWD[..3]}{WardCode}{DeptCode}",
					Gpic = "-",
					Replyid = "",
					Replytext = "",
					Rpic = "-"
				};
				DbContext.TabUsergrivs.Add(nGrievance);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index", new { partialViewId = 2 });
		}

		public IActionResult Reply2Griev(int Id, string ReplyTxt, string uPMWD, string uMail)
		{
			var griev = DbContext.TabUsergrivs.FirstOrDefault(it => it.Id == Id);
			if (griev != null && (griev.Replyid == null || griev.Replyid == ""))
			{
				var dt = $"{DateTime.Now:yyyyMMddHHmm}";
				griev.Replyid = $"{uPMWD}-{dt}-{uMail}";
				griev.Replytext = ReplyTxt;
				griev.Rpic = "-";
				DbContext.TabUsergrivs.Update(griev);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index", new { partialViewId = 2 });
		}

		public IActionResult GrievanceStatusChange(int grievId)
		{
			var griev = DbContext.TabUsergrivs.FirstOrDefault(it => it.Id == grievId);
			if (griev != null)
			{
				griev.Status = griev.Status == null || griev.Status == false ? true : false;
				DbContext.TabUsergrivs.Update(griev);
				DbContext.SaveChanges();
			}
			return RedirectToAction("Index", new { partialViewId = 2 });
		}

	}
}

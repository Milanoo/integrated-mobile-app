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
using IntegratedMobileApp.Models;
using IntegratedMobileApp.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntegratedMobileApp.Controllers
{

	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        ImobappContext DbContext;

		public HomeController(ILogger<HomeController> logger, ImobappContext DbContext)
		{
			_logger = logger;
			this.DbContext = DbContext;
		}

		public IActionResult Index()
		{
			var pm = User.GetProvinceMunicipality();
			ViewData["WardUserCounts"] = WardUserCounts(pm);
			ViewData["WardGrievanceCounts"] = WardGrievanceCounts(pm);
			ViewData["DepartmentGrievanceCounts"] = DepartmentGrievanceCounts(pm);
			ViewData["WardInformationCounts"] = WardInformationCounts(pm);
			ViewData["DepartmentInformationCounts"] = DepartmentInformationCounts(pm);			
			return View();
		}
		
		private Dictionary<string,int> WardUserCounts(string pm)
		{			
			var users = DbContext.TabUsers.Where(it => it.Upmw!.StartsWith(pm)).OrderBy(it => it.Id).ToList();
			var wardCodes = users.Select(it => it.GetWard()).ToList();
			var d_wardCodes = wardCodes.Distinct().ToList();
			d_wardCodes.Sort();
			var WardUserCounts = new Dictionary<string, int>();
            foreach (var wardCode in d_wardCodes)
            {
				if (wardCode == "00") continue;
				var count = wardCodes.Count(it => it == wardCode);
				WardUserCounts.Add($"Ward {wardCode}", count);
            }
			return WardUserCounts;
        }


		private Dictionary<string, Tuple<int,int>> WardGrievanceCounts(string pm)
		{
			var userGrivs = DbContext.TabUsergrivs.Where(it => it.Grivid!.StartsWith(pm)).OrderBy(it => it.Id).ToList();
			userGrivs = userGrivs.Where(it => !it.IsGrievanceToMunicipality()).ToList();
			var wardCodes = userGrivs.Select(it => it.GetGrievanceSenderWard()).ToList();
			var d_wardCodes = wardCodes.Distinct().ToList();
			d_wardCodes.Sort();
			var WardGrievanceCounts = new Dictionary<string, Tuple<int, int>>();
			foreach (var wardCode in d_wardCodes)
			{
				if (wardCode == "00") continue;
				var grivCount = userGrivs.Count(it => it.Grivid!.StartsWith($"{pm}{wardCode}"));
				var replyCount = userGrivs.Count(it => it.Replyid != null && it.Replyid!.StartsWith($"{pm}{wardCode}"));
				WardGrievanceCounts.Add($"Ward {wardCode}", Tuple.Create(grivCount, replyCount));
			}
			return WardGrievanceCounts;
		}

		private Dictionary<string, Tuple<int, int>> DepartmentGrievanceCounts(string pm)
		{			
			var userGrivs = DbContext.TabUsergrivs.Where(it => it.Grivid!.StartsWith($"{pm}00")).OrderBy(it => it.Id).ToList();
			var deptCodes = userGrivs.Select(it => it.GetGrievanceSenderDepartment()).ToList().Distinct().ToList();								
			var DepartmentGrievanceCounts = new Dictionary<string, Tuple<int, int>>();
			foreach (var deptCode in deptCodes)
			{
				var deptName = "";
				if (deptCode == "00") continue;
				if(deptCode != "00") deptName = DepartmentHelper.MunicipalityDepartmentEng[deptCode];				
				var grivCount = userGrivs.Count(it => it.Grivid!.StartsWith($"{pm}00{deptCode}"));
				var replyCount = userGrivs.Count(it => it.Grivid!.StartsWith($"{pm}00{deptCode}") && it.Replyid != null && it.Replyid != "");
				DepartmentGrievanceCounts.Add(deptName, Tuple.Create(grivCount, replyCount));
			}
			return DepartmentGrievanceCounts;
		}

		private Dictionary<string, int> WardInformationCounts(string pm)
		{
			var info2Users = DbContext.TabInfotousers.Where(it => it.Infoid!.StartsWith(pm)).OrderBy(it => it.Id).ToList();
			info2Users = info2Users.Where(it => !it.IsInfoFromMunicipality()).ToList();
			var wardCodes = info2Users.Select(it => it.GetInfoSenderWard()).ToList();
			var d_wardCodes = wardCodes.Distinct().ToList();
			d_wardCodes.Sort();
			var WardInformationCounts = new Dictionary<string, int>();
			foreach (var wardCode in d_wardCodes)
			{
				if (wardCode == "00") continue;
				var count = info2Users.Count(it => it.Infoid!.StartsWith($"{pm}{wardCode}"));
				WardInformationCounts.Add($"Ward {wardCode}", count);
			}
			return WardInformationCounts;
		}

		private Dictionary<string, int> DepartmentInformationCounts(string pm)
		{          
            var info2Users = DbContext.TabInfotousers.Where(it => it.Infoid!.StartsWith($"{pm}00")).OrderBy(it => it.Id).ToList();			
			var deptCodes = info2Users.Select(it => it.GetInfoSenderDepartment()).ToList();
			var d_deptCodes = deptCodes.Distinct().ToList();
			d_deptCodes.Sort();
			var DepartmentInformationCounts = new Dictionary<string, int>();
			foreach (var deptCode in d_deptCodes)
			{
                var deptName = "";
				if (deptCode == "00") continue;
                if (deptCode != "00") deptName = DepartmentHelper.MunicipalityDepartmentEng[deptCode];
                var count = info2Users.Count(it => it.Infoid!.StartsWith($"{pm}00{deptCode}"));
				DepartmentInformationCounts.Add(deptName, count);
			}
			return DepartmentInformationCounts;
		}



		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using IntegratedMobileApp.Models.Database;
using System.Globalization;
using IntegratedMobileApp.Helpers;

namespace IntegratedMobileApp.Extensions
{
	public static class UserExtension
	{

		public static string[] ghos = new string[] { "nep_gho", "raja_gho", "dhan_gho", "ghoda_gho", "bheri_gho", "dullu_gho" };

		public static string GetClaim(this ClaimsPrincipal user, string claimType)
		{
			return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? "";
		}

		public static string GetFullName(this ClaimsPrincipal user)
		{
			return $"{user.GetClaim(ClaimTypes.Name)}";
		}

		public static string GetEmail(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Email);
		}

		public static long GetUserId(this ClaimsPrincipal user)
		{
			return Convert.ToInt64(user.GetClaim(ClaimTypes.NameIdentifier));
		}

		public static string GetLocality(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Locality);
		}

		public static string GetProvince(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(0, 1);
		}

		public static string GetMunicipality(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(1, 2);
		}

		public static string GetWard(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(3, 2);
		}

		public static string GetDepartment(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(5, 2);
		}

		public static string GetProvinceMunicipality(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(0, 3);
		}

		public static string GetProvinceMunicipalityWard(this ClaimsPrincipal user)
		{
			return user.GetLocality().Substring(0, 5);
		}

		public static string GetProvinceMunicipalityWardDept(this ClaimsPrincipal user)
		{
			return user.GetLocality();
		}

		public static string GetMunicipalityName(this ClaimsPrincipal user)
		{
			var upm = user.GetProvinceMunicipality();
			var name = "";
			switch (upm)
			{
				case "504":
					name = "Nepalganj";
					break;
				case "508":
					name = "Rajapur";
					break;
				case "601":
					name = "Dullu";
					break;
				case "618":
					name = "Bheriganga";
					break;
				case "720":
					name = "Dhangadi";
					break;
				case "725":
					name = "Ghodaghodi";
					break;
				default:
					name = "";
					break;
			}
			return name;
		}

		public static bool IsActive(this ClaimsPrincipal user)
		{
			var expired = user.GetClaim(ClaimTypes.Expired);
			return (expired != "0");
		}

		public static string Role(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Role);
		}

		public static bool IsAdmin(this ClaimsPrincipal user)
		{
			return user.IsInRole("Admin");
		}

		public static bool IsMunicipality(this ClaimsPrincipal user)
		{
			return user.IsInRole("Municipality");
		}

		public static bool IsWard(this ClaimsPrincipal user)
		{
			return user.IsInRole("Ward");
		}

		public static bool IsPublic(this ClaimsPrincipal user)
		{
			return user.IsInRole("Public");
		}

		public static bool IsMunicipalityAdmin(this ClaimsPrincipal user)
		{
			return user.GetWard() == "00" && user.IsAdmin();
		}

		public static bool IsWardAdmin(this ClaimsPrincipal user)
		{
			return user.GetWard() != "00" && user.IsAdmin();
		}

		public static bool IsGHO(this ClaimsPrincipal user)
		{
			return ghos.Contains(user.GetEmail());
		}

		public static bool CanReplyFromWard(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Sid)[2] == '1';
		}
		public static bool CanReplyFromMunicipality(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Sid)[3] == '1';
		}
		public static bool CanPostFromWard(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Sid)[4] == '1';
		}
		public static bool CanPostFromMunicipality(this ClaimsPrincipal user)
		{
			return user.GetClaim(ClaimTypes.Sid)[5] == '1';
		}

		internal static string GetProvince(this TabUser user)
		{
			return user.Upmw!.Substring(0, 1);
		}
		internal static string GetMunicipality(this TabUser user)
		{
			return user.Upmw!.Substring(1, 2);
		}

		internal static string GetWard(this TabUser user)
		{
			return user.Upmw!.Substring(3, 2);
		}

		internal static string GetDepartment(this TabUser user)
		{
			return user.Upmw!.Substring(5, 2);
		}

		internal static string GetProvinceMunicipality(this TabUser user)
		{
			return user.Upmw!.Substring(0, 3);
		}

		internal static bool IsPublic(this TabUser user)
		{
			return user.Utype!.StartsWith("1");
		}

		internal static bool IsWard(this TabUser user)
		{
			return user.Utype!.StartsWith("2");
		}

		internal static bool IsMunicipality(this TabUser user)
		{
			return user.Utype!.StartsWith("3");
		}

		internal static bool IsAdmin(this TabUser user)
		{
			return user.Utype!.StartsWith("4");
		}


		internal static bool IsGrievanceToMunicipality(this TabUsergriv userGriv)
		{
			return userGriv.Grivid!.Split('-')[0].Substring(3, 2) == "00";
		}
		internal static bool IsGrievanceReplyFromDepartment(this TabUsergriv userGriv)
		{
			return !userGriv.Replyid!.Split('-')[0].EndsWith("00");
		}
		internal static string GetGrievanceSenderWard(this TabUsergriv userGriv)
		{
			return userGriv.Grivid!.Split('-')[0].Substring(3, 2);
		}
		internal static string GetGrievanceSenderDepartment(this TabUsergriv userGriv)
		{
			return userGriv.Grivid!.Split('-')[0].Substring(5, 2);
		}
		internal static string GetGrievanceSenderName(this TabUsergriv userGriv)
		{
			var i1 = userGriv.Grivsender!.IndexOf("(");
			var i2 = userGriv.Grivsender!.IndexOf(")");
			return userGriv.Grivsender.Substring(i1 + 1, i2-i1-1);
		}
		internal static string GetGrievanceReceiverDepartment(this TabUsergriv userGriv)
		{
			var deptId = "";
			if(userGriv.Grivreceiver!.Length == 7)
			{
				deptId = userGriv.Grivreceiver!.Substring(5, 2);
			}
			return deptId;
		}
		internal static string GetGrievanceReceiverDepartmentName(this TabUsergriv userGriv)
		{
			var deptId = "";
			var deptName = "";
			if (userGriv.Grivreceiver!.Length == 7)
			{
				deptId = userGriv.Grivreceiver!.Substring(5, 2);
				deptName = DepartmentHelper.MunicipalityDepartmentEng.ContainsKey(deptId) ? DepartmentHelper.MunicipalityDepartmentEng[deptId] : "";
			}
			return deptName;			
		}
		internal static string GetGrievancePostedBy(this TabUsergriv userGriv)
		{
			return userGriv.Grivid != null && userGriv.Grivid != "" ? userGriv.Grivid!.Split('-')[2] : "";
		}
		internal static string GetGrievancePostTime(this TabUsergriv userGriv)
		{
			return userGriv.Grivid != null && userGriv.Grivid != "" ? DateTime.ParseExact(userGriv.Grivid!.Split('-')[1], "yyyyMMddHHmm", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd HH:mm") : "";
		}
		internal static string GetGrievancePicPath(this TabUsergriv userGriv)
		{
			return userGriv.Gpic != null && userGriv.Gpic != "" && userGriv.Gpic != "-" ? userGriv.Gpic.Split("-")[2] : "";
		}
		internal static string GetReplyPostedBy(this TabUsergriv userGriv)
		{
			return userGriv.Replyid != null && userGriv.Replyid != "" ? userGriv.Replyid!.Split('-')[2] : "";
		}
		internal static string GetReplyPostTime(this TabUsergriv userGriv)
		{
			return userGriv.Replyid != null && userGriv.Replyid != "" ? DateTime.ParseExact(userGriv.Replyid!.Split('-')[1], "yyyyMMddHHmm", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd HH:mm") : "";
		}
		internal static string GetReplyPicPath(this TabUsergriv userGriv)
		{
			return userGriv.Rpic != null && userGriv.Rpic != "" && userGriv.Gpic != "-" ? userGriv.Rpic.Split("-")[2] : "";
		}

		internal static bool IsInfoFromMunicipality(this TabInfotouser infoUser)
		{
			return infoUser.Infoid!.Split('-')[0].Substring(3, 2) == "00";
		}
		internal static string GetInfoSenderWard(this TabInfotouser infoUser)
		{
			return infoUser.Infoid!.Split('-')[0].Substring(3, 2);
		}
		internal static string GetInfoSenderDepartment(this TabInfotouser infoUser)
		{
			return infoUser.Infoid!.Split('-')[0].Substring(5, 2);
		}

		internal static int CompareInfoPostDate(this TabInfotouser infouser, string date)
		{
			DateTime aDt1 = DateTime.ParseExact(infouser.Infoid!.Substring(8, 12), "yyyyMMddHHmm", CultureInfo.InvariantCulture);
			DateTime aDt2 = DateTime.ParseExact(date, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
			if (aDt1 < aDt2)
			{
				return -1;
			}
			else if (aDt1 == aDt2)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}
	}

	public class UserClaimsTransform : IClaimsTransformation
	{
		ImobappContext DbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserClaimsTransform(ImobappContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			DbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			var clone = principal.Clone();
			if (clone == null) return clone!;

			var newIdentity = (ClaimsIdentity)clone!.Identity!;

			var email = newIdentity!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";
			//try getting user by Email
			var user = await DbContext.TabUsers.FirstOrDefaultAsync(u => EF.Functions.ILike(u.Umail, email));

			if (user == null)
			{
				await _httpContextAccessor!.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}

			var claims = newIdentity.Claims.ToList();
			foreach (var c in claims)
			{
				newIdentity.RemoveClaim(c);
			}

			newIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()));
			newIdentity.AddClaim(new Claim(ClaimTypes.Email, user!.Umail!));
			newIdentity.AddClaim(new Claim(ClaimTypes.Name, user!.Uname! ?? ""));
			newIdentity.AddClaim(new Claim(ClaimTypes.Locality, user!.Upmw!));
			newIdentity.AddClaim(new Claim(ClaimTypes.Role, user!.Utype!.StartsWith("1") ? "Public" : user!.Utype!.StartsWith("2") ? "Ward" : user!.Utype!.StartsWith("3") ? "Municipality" : "Admin"));
			newIdentity.AddClaim(new Claim(ClaimTypes.Expired, user!.Utype![1] == '1' ? "1" : "0"));
			newIdentity.AddClaim(new Claim(ClaimTypes.Sid, user!.Utype!));
			return clone;

		}
	}
}
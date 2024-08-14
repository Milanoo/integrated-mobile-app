//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using System.Text.RegularExpressions;

namespace IntegratedMobileApp.Helpers
{
	internal static class SecurityHelper
	{

		internal static bool IsValidEmail(this string email)
		{
			if (email == null) return false;
			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
			return Regex.IsMatch(email, pattern);
		}

		internal static bool IsValidUserName(this string userName) {
			if (userName == null) return false;
			//string pattern = @"^[a-zA-Z@_ ]*$";	
			string pattern = @"^[a-zA-Z\d@_\s]*$";	
			return Regex.IsMatch(userName, pattern);
		}

		internal static bool IsValiLinkName(this string linkName)
		{
			if (linkName == null) return false;
			string pattern = @"^[a-zA-Z\s]*$";
			return Regex.IsMatch(linkName, pattern);
		}

	}
}

//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

namespace IntegratedMobileApp.Helpers
{
	public class DepartmentHelper
	{
		public static Dictionary<string, string> WardDepartmentEng = new Dictionary<string, string> {
			{ "01","Chairperson"},
			{ "02","Elected Member"},
			{ "03","Ward Secretary"},
			{ "04","Ward Staff"}
		};

		public static Dictionary<string, string> WardDepartmentNep = new Dictionary<string, string> {
			{ "01","अध्यक्ष"},
			{ "02","निर्वाचित सदस्य"},
			{ "03","वडा सचिव"},
			{ "04","वडा कर्मचारी"}
		};		

		public static Dictionary<string, string> MunicipalityDepartmentEng = new Dictionary<string, string> {
			{ "01","Mayor"},
			{ "02","Deputy Mayor"},
			{ "03","Administration Division"},		
			{ "04","Urban Development"},
			{ "05","Infrastructure Division"},
			{ "06","Health Division"},
			{ "07","Social Development"},
			{ "08","Planning and Budgeting"},
			{ "09","IT Division"},
			{ "10","Education Division"},
			{ "11","Youth and Sports Division"},
			{ "12","Economic Development Division"},
			{ "13","Financial Administration Division"},
			{ "14","Internal Audit Division"}
		};

		public static Dictionary<string, string> MunicipalityDepartmentNep = new Dictionary<string, string> {
			{"01","मेयर" },
			{"02","उपमेयर" },
			{"03","प्रशासन शाखा" },
			{"04","शहरी विकास शाखा" },
			{"05","पूर्वाधार शाखा" },
			{"06","स्वास्थ्य शाखा" },
			{"07","सामाजिक विकास शाखा" },
			{"08","योजना र बजेट शाखा" },
			{"09","आईटी शाखा" },
			{"10","शिक्षा शाखा" },
			{"11","युवा तथा खेलकुद शाखा" },
			{"12","आर्थिक विकास शाखा" },
			{"13","वित्तीय प्रशासन शाखा" },
			{"14","आन्तरिक परिक्षण शाखा" }
		};

				


	}
}

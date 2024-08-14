//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using System;
using System.Collections.Generic;

namespace IntegratedMobileApp.Models.Database;

public partial class TabInfoUrl
{
    public int Id { get; set; }

    public string? MunCode { get; set; }

    public string? ArticleUrl { get; set; }

    public string? IntroUrl { get; set; }

    public string? ElectedUrl { get; set; }

    public string? StaffUrl { get; set; }

    public string? SifarisUrl { get; set; }

    public string? PmUrl { get; set; }

    public string? HnUrl { get; set; }

    public string? NbUrl { get; set; }

    public string? ImaUrl { get; set; }

    public string? NbYr { get; set; }
}

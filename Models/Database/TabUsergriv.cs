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

public partial class TabUsergriv
{
    public int Id { get; set; }

    public string? Grivid { get; set; }

    public string? Grivtext { get; set; }

    public string? Grivsender { get; set; }

    public string? Grivreceiver { get; set; }

    public string? Replyid { get; set; }

    public string? Replytext { get; set; }

    public string? Gpic { get; set; }

    public string? Rpic { get; set; }

    public bool? Status { get; set; }
}

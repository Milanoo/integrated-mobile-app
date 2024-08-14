//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using tusdotnet.Models;
using tusdotnet.Stores;
using tusdotnet.Models.Configuration;
using tusdotnet.Interfaces;
using System.Text;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore;
using IntegratedMobileApp.Models.Database;
using IntegratedMobileApp.Helpers;
using IntegratedMobileApp.Extensions;

/// <summary>
/// Configuration for Tus resumable uploads
/// 
/// Avinab Malla 
/// avinabmalla@yahoo.com
/// 25 October 2021
///USAGE: app.UseTus(httpContext => new TusConfig());
/// </summary>
public class TusConfig : DefaultTusConfiguration
{
	ImobappContext DbContext = new ImobappContext();
	string UploadDir = $"{AppConfiguration.UploadDocFolder}";
	string TempUploadDir = $"{AppConfiguration.UploadDocFolder}\\Temp";

	public TusConfig()
	{

		this.MaxAllowedUploadSizeInBytesLong = 8L * 1024 * 1024 * 1024;
		this.Store = new TusDiskStore(TempUploadDir);
		this.UrlPath = "/Uploads";

		if (!Directory.Exists(TempUploadDir)) Directory.CreateDirectory(TempUploadDir);

		this.Events = new Events()
		{
			OnAuthorizeAsync = OnAuthorizeAsync,
			OnBeforeCreateAsync = OnBeforeCreateAsync,
			OnFileCompleteAsync = OnFileCompleteAsync
		};
	}

	private Task OnAuthorizeAsync(AuthorizeContext ctx)
	{
		if (!ctx.HttpContext.User.Identity!.IsAuthenticated)
		{
			ctx.FailRequest(HttpStatusCode.Unauthorized, "Unauthorized!");
		}
		return Task.CompletedTask;
	}

	private async Task OnBeforeCreateAsync(BeforeCreateContext ctx)
	{
		var metadata = ctx.Metadata;
		//var userId = ctx.HttpContext.User.GetUserId();
		//var folderName = metadata["folder"].GetString(Encoding.UTF8);

		// Check if user has folder access here
		// ctx.FailRequest(HttpStatusCode.Unauthorized, "Access denied");
	}

	private async Task OnFileCompleteAsync(FileCompleteContext ctx)
	{
		ITusFile file = await ctx.GetFileAsync();
		var terminationStore = (ITusTerminationStore)ctx.Store;

		var metadata = await file.GetMetadataAsync(ctx.CancellationToken);

		var stream = await file.GetContentAsync(ctx.CancellationToken);
		var task = metadata["task"].GetString(Encoding.UTF8);
		if(task == "PostGrievance")
		{			
			var uPMWD = metadata["userPMWD"].GetString(Encoding.UTF8);
			var uMail = metadata["userMail"].GetString(Encoding.UTF8);
			var uName = metadata["userName"].GetString(Encoding.UTF8);
			var grievTxt = metadata["grievText"].GetString(Encoding.UTF8);
			var wardCode = metadata["wardCode"].GetString(Encoding.UTF8);
			var deptCode = metadata["departmentCode"].GetString(Encoding.UTF8);
			var fileName = metadata["filename"].GetString(Encoding.UTF8);

			var destinationFile = Path.Combine(TempUploadDir, $"{fileName}");
			using (var fileStream = File.Create(destinationFile))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(fileStream);
			}

			var dt = $"{DateTime.Now:yyyyMMddHHmm}";
			var grievId = $"{uPMWD}-{dt}-{uMail}";
			var grievSender = $"Ward-{uPMWD.Substring(3, 2)}({uName})";
			var grievReceiver = $"{uPMWD[..3]}{wardCode}{deptCode}";
			var gPic = $"{uPMWD}-{dt}-{uMail}_img{Path.GetExtension(fileName)}";			

			var griev = new TabUsergriv()
			{
				Grivid = grievId,
				Grivtext = grievTxt,
				Grivsender = grievSender,
				Grivreceiver = grievReceiver,
				Gpic = gPic,
				Replyid = $"",
				Replytext = "",
				Rpic = "-"
			};
			DbContext.TabUsergrivs.Add(griev);
			DbContext.SaveChanges();

			File.Copy(destinationFile, Path.Combine(UploadDir, gPic));
			File.Delete(destinationFile);
		}
		else if(task == "ReplyGrievance")
		{
			var id = metadata["id"].GetString(Encoding.UTF8);
			var replyText = metadata["replyText"].GetString(Encoding.UTF8);
			var userPMWD = metadata["userPMWD"].GetString(Encoding.UTF8);
			var userMail = metadata["userMail"].GetString(Encoding.UTF8);
			var fileName = metadata["filename"].GetString(Encoding.UTF8);

			var rPic = "";

			var destinationFile = Path.Combine(TempUploadDir, $"{fileName}");
						
			using (var fileStream = File.Create(destinationFile))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(fileStream);
			}
			
			//Add upload record to database here	
			var griev = DbContext.TabUsergrivs.FirstOrDefault(it => it.Id == Convert.ToInt64(id));
			if (griev != null)
			{
				var dt = $"{DateTime.Now:yyyyMMddHHmm}";
				rPic = $"{userPMWD}-{dt}-{userMail}_img{Path.GetExtension(fileName)}";
				griev.Replyid = $"{userPMWD}-{dt}-{userMail}";
				griev.Replytext = replyText;
				griev.Rpic = rPic;
				DbContext.TabUsergrivs.Update(griev);
				DbContext.SaveChanges();

				File.Copy(destinationFile, Path.Combine(UploadDir, rPic));
			}

			File.Delete(destinationFile);
		}
		
		//Delete the uploaded temp files
		await stream.DisposeAsync();
		await terminationStore.DeleteFileAsync(file.Id, ctx.CancellationToken);
	}
}

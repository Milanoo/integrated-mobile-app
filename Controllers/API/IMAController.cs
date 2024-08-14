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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders.Physical;
using System.Numerics;
using System.Xml.Linq;

namespace IntegratedMobileApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IMAController : ControllerBase
    {

        ImobappContext DbContext;
        public IMAController(ImobappContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("addattachment")]
        public async Task<IActionResult> AddAttachment(string attachmentId, string? attachmentDescription = "")
        {
            try
            {
                var attachment = DbContext.TabAttachments.FirstOrDefault(it => it.AId == attachmentId);
                if (attachment == null)
                {
                    var nAttachment = new TabAttachment() { AId = attachmentId, Descr = attachmentDescription };
                    await DbContext.TabAttachments.AddAsync(nAttachment);
                    await DbContext.SaveChangesAsync();
                    return Ok("Exist");
                }
                else
                {
                    attachment.Descr = attachmentDescription;
                    DbContext.TabAttachments.Update(attachment);
                    await DbContext.SaveChangesAsync();
                    return Ok("Added");
                }
               
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpGet("addmunlink")]
        public async Task<IActionResult> AddMunLink(string linkId, string link, string lNep, string lEng)
        {
            try
            {
                var lnk = DbContext.TabLinks.FirstOrDefault(it => it.Lkid == linkId);
                if (lnk == null)
                {
                    var nLnk = new TabLink() {Lkid = linkId, Linkdetail = link, Lnamenep = lNep, Lnameeng = lEng };
                    await DbContext.TabLinks.AddAsync(nLnk);
                    await DbContext.SaveChangesAsync();
                    return Ok("Added");
                }
                else
                {
                    lnk.Linkdetail = link;
                    lnk.Lnamenep = lNep;
                    lnk.Lnameeng = lEng;
                    DbContext.TabLinks.Update(lnk);
                    await DbContext.SaveChangesAsync();
                    return Ok("Exist");
                }                
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }
               
        [HttpGet("adduser")]
        public async Task<IActionResult> AddUser(string mail, string? name="", string? phone = "", string? ctz = "", string? fname = "", string? fctz = "", string? mname = "", string? mctz = "", string? hno = "", string? pmw = "", string? type = "", string? password = "")
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user == null)
                {
                    var nUser = new TabUser()
                    {
                        Umail = mail,
                        Uname = name != null ? name : "",
                        Uphone = phone != null ? phone : "",
                        Uctz = ctz != null ? ctz : "" ,
                        Fname = fname != null ? fname : "",
                        Fctz = fctz != null ? fctz : "",
                        Mname = mname != null ? mname : "",
                        Mctz = mctz != null ? mctz : "",
                        Uhno = hno != null ? hno : "",
                        Upmw = pmw != null ? pmw : "",
                        Utype = type != null ? type : "",
                        Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "",
                        CreatedOn = DateTime.Now
                    };
                    await DbContext.TabUsers.AddAsync(nUser);
					await DbContext.SaveChangesAsync();
					return Ok("User added");
				}
                else
                {
                    user.Umail = mail;
                    user.Uname = name != null ? name : "";
                    user.Uphone = phone != null ? phone : "";
                    user.Uctz = ctz != null ? ctz : "";
                    user.Fname = fname != null ? fname : ""  ;
                    user.Fctz = fctz != null ? fctz : "";
                    user.Mname = mname != null ? mname : "";
                    user.Mctz = mctz != null ? mctz : "";
                    user.Uhno = hno != null ? hno : "";
                    user.Upmw = pmw != null ? pmw : "";
                    user.Utype = type!= null ? type : "";
                    user.Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "";
                    DbContext.TabUsers.Update(user);
					await DbContext.SaveChangesAsync();
					return Ok("User Exist");
				}
                
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpGet("changepassword")]
        public async Task<IActionResult> ChangePassword(string mail, string password)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user != null)
                {

                    user.Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "";
                    DbContext.TabUsers.Update(user);
                    await DbContext.SaveChangesAsync();
                    return Ok("success");
                }
                else
                {
                    return Ok("fail");
                }
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpGet("createuser")]
        public async Task<IActionResult> CreateUser(string mail, string pmw, string type, string password)
        {
            try
            {   
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user == null)
                {
                    var nUser = new TabUser()
                    {
                        Umail = mail,
                        Upmw = pmw,
                        Utype = type,
                        Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "",
                        CreatedOn = DateTime.Now
                    };
                    await DbContext.TabUsers.AddAsync(nUser);
                    await DbContext.SaveChangesAsync();
                    return Ok("User added");
                }
                else
                {
                    user.Umail = mail;
                    user.Upmw = pmw;
                    user.Utype = type;
                    user.Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "";
                    DbContext.TabUsers.Update(user);
                    await DbContext.SaveChangesAsync();
                    return Ok("User Exist");
                }
                
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpGet("deletedocument")]
        public async Task<IActionResult> DeleteDocument(string fileName)
        {
            try
            {
                var filePath = Path.Combine(AppConfiguration.UploadDocFolder!, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return Ok("not exist");
                }
                else
                {
                    var attachment = DbContext.TabAttachments.FirstOrDefault(it => it.AId == fileName);
                    if (attachment == null)
                    {
                        return Ok("not exist");
                    }
                    else
                    {
                        System.IO.File.Delete(filePath);
                        DbContext.TabAttachments.Remove(attachment);
                        await DbContext.SaveChangesAsync();
                        return Ok("deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok($"fail");
            }
        }

        [HttpGet("deletemunlink")]
        public async Task<IActionResult> DeleteMunLink(string linkId)
        {
            try
            {
                var lnk = DbContext.TabLinks.FirstOrDefault(it => it.Lkid == linkId);
                if (lnk != null)
                {
                    DbContext.TabLinks.Remove(lnk);
                    await DbContext.SaveChangesAsync();
                    return Ok("Deleted");
                }
                else
                {
                    return Ok("Error");
                }
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser([FromForm] string mail, [FromForm] string password)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user == null)
                {                    
                    return Ok("User not found");
                }
                else
                {

                    if (!PasswordHelper.VerifyHashedPassword(password, user.Password!))
                    {
                        return Ok("Incorrect password");
                    }
                    else
                    {
                        DbContext.TabUsers.Remove(user);
                        await DbContext.SaveChangesAsync();
                        return Ok("User account deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok($"Error!{Environment.NewLine}{ex.Message}");
            }
        }

        [HttpGet("getusertype")]
        public IActionResult GetUserType(string mail, string password)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail && it.Password == password);
                if (user != null)
                {
                    if (PasswordHelper.VerifyHashedPassword(password,user.Password!))
                    {
                        return Ok($"{user.Utype}");
                    }
                    else
                    {
						return Ok("loginfail");
					}
                }
                else
                {
                    return Ok("loginfail");
                }
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpGet("login")]
        public IActionResult Login(string mail, string password)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);          
                if (user != null)
                {
                    if(PasswordHelper.VerifyHashedPassword(password, user.Password!))
                    {
						return Ok($"{user.Upmw}:{user.Utype}");
                    }
                    else
                    {
						return Ok("Login fail");
					}                   
                }
                else
                {
                    return Ok("Login fail");
                }
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpGet("reply2griev")]
        public async Task<IActionResult> Reply2Grievance(string greivId, string replyId, string replyTxt, string replyPic)
        {
            try
            {
                var greiv = DbContext.TabUsergrivs.FirstOrDefault(it => it.Grivid == greivId);
                if (greiv != null)
                {
                    greiv.Replyid = replyId;
                    greiv.Replytext = replyTxt;
                    greiv.Rpic = replyPic;
                    DbContext.TabUsergrivs.Update(greiv);
                    await DbContext.SaveChangesAsync();
                    return Ok("success");
                }
                else
                {
                    return Ok("fail");
                }
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. Error!");
            }
        }

        [HttpGet("savegriev")]
        public async Task<IActionResult> SaveGrievance(string greivId, string grievTxt, string grievSender, string grievReceiver, string grievPic)
        {
            try
            {
                var greiv = DbContext.TabUsergrivs.FirstOrDefault(it => it.Grivid == greivId);
                if (greiv == null)
                {
                    var nGreiv = new TabUsergriv()
                    {
                        Grivid = greivId,
                        Grivtext = grievTxt,
                        Grivsender = grievSender,
                        Grivreceiver = grievReceiver,
                        Gpic = grievPic,
                        Replyid = "",
                        Replytext = "",
                        Rpic = "-"
                    };
                    await DbContext.TabUsergrivs.AddAsync(nGreiv);
                    await DbContext.SaveChangesAsync();
                    return Ok("Info created");
                }
                else
                {
                    greiv.Grivtext = grievTxt;
                    greiv.Grivsender = grievSender;
                    greiv.Grivreceiver = grievReceiver;
                    greiv.Gpic = grievPic;
                    DbContext.TabUsergrivs.Update(greiv);
                    await DbContext.SaveChangesAsync();
                    return Ok("Info update");
                }
                
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }

        [HttpGet("saveinfo")]
        public async Task<IActionResult> SaveInfo(string infoId, string infoTxt, string infoSender, string infoReceiver)
        {
            try
            {
                var info = DbContext.TabInfotousers.FirstOrDefault(it => it.Infoid == infoId);
                if (info == null)
                {
                    var nInfo = new TabInfotouser()
                    {
                        Infoid = infoId,
                        Infotext = infoTxt,
                        Infosender = infoSender,
                        Inforeceiver = infoReceiver,
                    };
                    await DbContext.TabInfotousers.AddAsync(nInfo);
                    await DbContext.SaveChangesAsync();
                    return Ok("Info created");
                }
                else
                {
                    info.Infotext = infoTxt;
                    info.Infosender = infoSender;
                    info.Inforeceiver = infoReceiver;
                    DbContext.TabInfotousers.Update(info);
                    await DbContext.SaveChangesAsync();
                    return Ok("Info update");
                }
                
            }
            catch (Exception ex)
            {
                return Ok($"Error");
            }
        }
              
        [HttpGet("syncgriev")]
        public IActionResult SyncGrievance(string date, string code, string mail, string type)
        {
            try
            {
                var grievs = new List<TabUsergriv>();
                if (type == "1")
                {
                    grievs = DbContext.TabUsergrivs.Where(it => 
                    (it.Grivid != null && Convert.ToDouble(it.Grivid!.Substring(8,12)) > Convert.ToDouble(date)   ||
					(it.Replyid!= "" && Convert.ToDouble(it.Replyid!.Substring(8,12)) > Convert.ToDouble(date))) && 
                    EF.Functions.ILike(it.Grivid!,$"%{mail}")).OrderBy(it => it.Id).ToList();
                }
                else
                {
                    grievs = DbContext.TabUsergrivs.Where(it =>
					(it.Grivid != null && Convert.ToDouble(it.Grivid!.Substring(8, 12)) > Convert.ToDouble(date) ||
					(it.Replyid != "" && Convert.ToDouble(it.Replyid!.Substring(8, 12)) > Convert.ToDouble(date))) && 
                    it.Grivreceiver == code).OrderBy(it => it.Id).ToList();
                }
                var grievTxts = new List<string>();
                foreach (var griev in grievs)
                {
                    grievTxts.Add($"{griev.Grivid}/ww/{griev.Grivtext}/ww/{griev.Grivsender}/ww/{griev.Grivreceiver}/ww/{griev.Replyid}/ww/{griev.Replytext}/ww/{griev.Gpic}/ww/{griev.Rpic}");
                }
                var msg = string.Join("/::/", grievTxts);
                if (msg == "") msg = "nodata";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. Greivance Error!");
            }
        }

        [HttpGet("syncinfo")]
        public IActionResult SyncInfo(string type, string date, string code)
        {
            try
            {
                var munCode = code.Substring(0, 3);
                var wardCode = code.Substring(0, 5);
                var deptCode = code.Substring(5, 2);
                var infos = new List<TabInfotouser>();
                if (type == "1")
                {
                    infos = DbContext.TabInfotousers.Where(it => 
                    (Convert.ToDouble(it.Infoid!.Substring(8, 12)) > Convert.ToDouble(date)) &&
					((it.Inforeceiver == "All" && it.Infoid!.Substring(0,3) == munCode) || 
                    (it.Inforeceiver == "Public_ward" && it.Infoid!.Substring(0,5) == wardCode) ||
                    (it.Inforeceiver == "Public" && it.Infoid.Substring(0, 3) == munCode))).OrderBy(it => it.Id).ToList();
                }
                else if(type == "2")
                {
                    infos = DbContext.TabInfotousers.Where(it =>
					(Convert.ToDouble(it.Infoid!.Substring(8, 12)) > Convert.ToDouble(date)) &&
					((it.Inforeceiver == "All" && it.Infoid!.Substring(0, 3) == munCode) ||
                    (it.Inforeceiver == "Ward**" && it.Infoid!.Substring(0, 3) == munCode && it.Infoid!.Substring(5,2) == deptCode) ||
                    (it.Inforeceiver == "Ward**" && it.Infoid!.Substring(0, 5) == wardCode && it.Infoid!.Substring(5,2) == "00") ||
                    (it.Inforeceiver == code))).OrderBy(it => it.Id).ToList();
                }
                else if(type == "3")
                {
                    infos = DbContext.TabInfotousers.Where(it =>
					(Convert.ToDouble(it.Infoid!.Substring(8, 12)) > Convert.ToDouble(date)) &&
					((it.Inforeceiver == "All" && it.Infoid!.Substring(0, 3) == munCode) ||
                    (it.Inforeceiver == "Mun**" && it.Infoid!.Substring(0, 3) == munCode) ||
                    (it.Inforeceiver == code))).OrderBy(it => it.Id).ToList();

                }
                else if(type == "4")
                {
                    infos = DbContext.TabInfotousers.Where(it =>
					(Convert.ToDouble(it.Infoid!.Substring(8, 12)) > Convert.ToDouble(date)) &&
					((it.Inforeceiver == "All" && it.Infoid!.Substring(0, 3) == munCode) ||
                    (it.Inforeceiver == "Admin" && it.Infoid!.Substring(0, 3) == munCode) ||
                    (it.Inforeceiver == code))).OrderBy(it => it.Id).ToList();
                }
                var infoTxts = new List<string>();
                foreach (var info in infos)
                {
                    infoTxts.Add($"{info.Infoid}/ww/{info.Infotext}/ww/{info.Infosender}/ww/{info.Inforeceiver}");
                }
                var msg = string.Join("/::/", infoTxts);
                if (msg == "") msg = "nodata";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. Info Error!");
            }
        }

        [HttpGet("synclink")]
        public IActionResult SyncLink(string linkId)
        {
            try
            {
                var lnks = DbContext.TabLinks.Where(it => EF.Functions.ILike(it.Lkid!,$"{linkId}%")).OrderBy(it=>it.Id).ToList();
                var lnkTxts = new List<string>();
                foreach (var lnk in lnks)
                {
                    lnkTxts.Add($"{lnk.Lkid}/ww/{lnk.Lnameeng}/ww/{lnk.Lnamenep}/ww/{lnk.Linkdetail}");
                }
                var msg = string.Join("/::/", lnkTxts);
                if (msg == "") msg = "nodata";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. Link Error!");
            }
        }

        [HttpGet("syncuploaddoc")]
        public IActionResult SyncUploadDoc(string type)
        {
            try
            {
                var attachments = DbContext.TabAttachments.Where(it => EF.Functions.ILike(it.AId!, $"{type}%")).OrderBy(it=>it.Id).ToList();
                var attachmentsTxts = new List<string>();
                foreach (var attachment in attachments)
                {
                    attachmentsTxts.Add($"{attachment.AId}/ww/{attachment.Descr}");
                }
                var msg = string.Join("/::/", attachmentsTxts);
                if (msg == "") msg = "nodata";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. Error!");
            }
        }

		[HttpGet("syncuser")]
		public IActionResult SyncUser(string mail, string password)
		{
			try
			{
				var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                var msg = "User not found";
                if (user != null) {
					if (PasswordHelper.VerifyHashedPassword(password, user.Password!))
					{
						msg = $"{user.Umail}/ww/{user.Uname}/ww/{user.Uphone}/ww/{user.Uctz}/ww/{user.Fname}/ww/{user.Fctz}/ww/{user.Mname}/ww/{user.Mctz}/ww/{user.Uhno}/ww/{user.Upmw}/ww/{user.Utype}/ww/{user.Password}";
					}
					else
					{
                        msg = $"Incorrect Password";
					}					
				} 
				return Ok(msg);
			}
			catch (Exception ex)
			{
				return Ok($"Error");
			}
		}

		[HttpGet("updateuser")]
        public async Task<IActionResult> UpdateUser(string mail, string? name = "", string? phone = "", string? ctz = "", string? fname = "", string? fctz = "", string? mname = "", string? mctz = "", string? hno ="", string? pmw= "", string? type ="", string? password ="")
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user != null)     
                {
                    user.Umail = mail;
                    user.Uname = name;
                    user.Uphone = phone;
                    user.Uctz = ctz;
                    user.Fname = fname;
                    user.Fctz = fctz;
                    user.Mname = mname;
                    user.Mctz = mctz;
                    user.Uhno = hno;
                    user.Upmw = pmw;
                    user.Utype = type;
                    user.Password = password != null && password != "" ? PasswordHelper.CreateHashedPwd(password) : "";
                    DbContext.TabUsers.Update(user);
                    await DbContext.SaveChangesAsync();
                    return Ok("success");
                }
                else
                {
                    return Ok("fail");
                }              
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpGet("updateusertype")]
        public async Task<IActionResult> UpdateUserType(string mail, string type, string pmw)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => it.Umail == mail);
                if (user != null)  
                {                   
                    user.Upmw = pmw;
                    user.Utype = type; 
                    DbContext.TabUsers.Update(user);
                    await DbContext.SaveChangesAsync();
                    return Ok("success");
                }
                else
                {
                    return Ok("fail");
                }
               
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpPost("uploaddocs")]
        public async Task<IActionResult> UploadDocs(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0) return Ok("File not found.");
                   
                if (!Directory.Exists(AppConfiguration.UploadDocFolder)) Directory.CreateDirectory(AppConfiguration.UploadDocFolder!);

                var fileName = file.FileName;
                var filePath = $"{AppConfiguration.UploadDocFolder}\\{fileName}";
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    //var count = Directory.GetFiles(AppConfiguration.UploadDocFolder!).Length;
                    //filePath = $"{AppConfiguration.UploadDocFolder}\\{Path.GetFileNameWithoutExtension(fileName)}_{count + 1}{Path.GetExtension(fileName)}";
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return Ok("success");
            }
            catch(Exception ex)
            {
                return Ok("fail");
            }
        }

        [HttpGet("usersearch")]
        public IActionResult UserSearch(string mail)
        {
            try
            {
                var user = DbContext.TabUsers.FirstOrDefault(it => EF.Functions.ILike( it.Umail!, $"{mail}%"));
                if (user != null)
                {
                    var msg = $"{user.Upmw}:{user.Utype}";                   
                    return Ok(msg);
                }
                else
                {
                    return Ok("Not Found");
                }

            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpGet("usersearchlist")]
        public IActionResult UserSearchList(string? mail = "", string? type = "", string? pmw= "")
        {
            try
            {
                var users = new List<TabUser>();
                if (pmw == "Admin")
                {
                    users = DbContext.TabUsers.Where(it=> EF.Functions.ILike(it.Umail!,$"{mail}%") && EF.Functions.ILike(it.Utype!,$"{type}%")).OrderBy(it=>it.Id).Take(20).ToList();   
                }
                else
                {
                    users = DbContext.TabUsers.Where(it => EF.Functions.ILike(it.Umail!, $"{mail}%") && EF.Functions.ILike(it.Utype!, $"{type}%") && EF.Functions.ILike(it.Upmw!, $"{pmw}%")).OrderBy(it => it.Id).Take(20).ToList();
                }
                var userTxts = new List<string>();
                foreach (var user in users)
                {
                    userTxts.Add($"{user.Upmw}:{user.Utype}:{user.Umail}");
                }
                var msg = string.Join(",", userTxts);
                if (msg == "") msg = "nodata";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

        [HttpGet("usersearchlistdms")]
        public IActionResult UserSearchListForDMS(string? pmw = "")
        {
            try
            {               
                var users = DbContext.TabUsers.Where(it => EF.Functions.ILike(it.Upmw!, $"{pmw}%")).OrderBy(it => it.Id).ToList();
                var userTxts = new List<string>();
                foreach (var user in users)
                {
                    userTxts.Add($"{user.Upmw}:{user.Utype}:{user.Umail}");
                }
                var msg = string.Join(",", userTxts);
                if (msg == "") msg = "No Data.";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return Ok($"{ex.Message}. User Error!");
            }
        }

		[HttpGet("usersearchwithdocs")]
		public IActionResult UserSearchWithDocs(string? pmw = "")
		{
			try
			{
				var users = DbContext.TabUsers.Where(it => EF.Functions.ILike(it.Upmw!, $"{pmw}%")).OrderBy(it => it.Id).ToList();
                var docs = DbContext.TabAttachments.ToList();
                var usersWithDocs = new List<object>();
				foreach (var user in users)
				{
                    var docName = user.Umail!.Contains("@gmail.com") ? user.Umail.Replace("@gmail.com","_gm") : user.Umail;
                    var userDocs = docs.Where(it => it.AId!.StartsWith(docName)).ToList();
                    var userDocsJsonObj = new List<object>();
                    if (userDocs != null && userDocs.Count > 0)
                    {
                        foreach (var ud in userDocs)
                        {
                            userDocsJsonObj.Add(new { DocId = ud.AId! });
						}
						usersWithDocs.Add(new { Mail = user.Umail!, DocIds = userDocsJsonObj });
					}					
				}						
				return Ok(usersWithDocs);
			}
			catch (Exception ex)
			{
				return Ok($"{ex.Message}. User Error!");
			}
		}

		[HttpGet("infourl")]
        public IActionResult InfoURL(string muncode)
        {
            var infoUrl = DbContext.TabInfoUrls.FirstOrDefault(it=>it.MunCode == muncode);						
            if(infoUrl != null)
            {
                var infoUrlJson = new { 
                    MunCode = infoUrl.MunCode,
                    ArticleUrl = infoUrl.ArticleUrl,
                    IntroUrl= infoUrl.IntroUrl,
                    ElectedUrl = infoUrl.ElectedUrl,
                    StaffUrl = infoUrl.StaffUrl,
                    SifarisUrl = infoUrl.SifarisUrl,
                    PMUrl = infoUrl.PmUrl,
                    HNUrl = infoUrl.HnUrl,
                    NBUrl = infoUrl.NbUrl,
                    NbYr = infoUrl.NbYr,
                    IMAUrl = infoUrl.ImaUrl
                };
                return Ok(infoUrlJson);
            }
            else
            {
				return Ok("No Data");
			}		
		}

        [HttpGet("pretext")]
        public IActionResult Pretext(int usertype)
        {
            var pretexts = DbContext.TabPretexts.Where(it => it.Usertype == usertype).ToList();
            if (pretexts != null && pretexts.Count > 0)
            {

                var pretextJsonObj = new List<Object>();
                foreach (var pretext in pretexts)
                {
                    pretextJsonObj.Add(new { 
                        NepText = pretext.Neptext,
                        EngText = pretext.Engtext
                    });
                }     
                return Ok(pretextJsonObj);
            }
            else
            {
                return Ok("No Data");
            }
        }
               

        [HttpPost("vote")]
        public IActionResult Vote([FromForm]string ouuid,[FromForm] string umail)
        {
            var poll = DbContext.TabPolls.First(it => it.OUuid == ouuid);
            if (poll != null)
            {
                var pollVotes = DbContext.TabPollVotes.Where(it => it.OUuid == ouuid && it.Umail == umail).OrderBy(it=>it.Id).ToList();
                if (pollVotes == null || pollVotes.Count == 0)
                {
                    var pollVote = new TabPollVote()
                    {
                        OUuid = ouuid,
                        Umail = umail,
                        VotedOn = DateTime.Now
                    };
                    DbContext.TabPolls.Add(poll);
                    DbContext.SaveChanges();
                    return Ok("Voted");
                }
                else
                {
                    var voteOn = DateTime.Now;
                    if(voteOn.Subtract(pollVotes.Last().VotedOn!.Value).TotalHours > 24)
                    {
						var pollVote = new TabPollVote()
						{
							OUuid = ouuid,
							Umail = umail,
							VotedOn = DateTime.Now
						};
						DbContext.TabPolls.Add(poll);
						DbContext.SaveChanges();
						return Ok("Voted");
					}
                    else
                    {
						return Ok("Cannot vote again within 24 hours");
					}
                }
            }
            else
            {
                return Ok("No Data");
            }
        }


		[HttpGet("pollresult")]
        public IActionResult PollResult(string muncode)
        {
			var polls = DbContext.TabPolls.Where(it => it.MunCode == muncode).OrderBy(it => it.Id).ToList();
			if (polls != null && polls.Count > 0)
			{
                var pollUuids = polls.Select(it => it.QUuid).Distinct().ToList();
                var pollResults = new List<object>();
                foreach (var qUuid in pollUuids)
                {                    
                    var options = polls.Where(it => it.QUuid == qUuid).OrderBy(it=>it.Id).ToList();
                    var pollQuestion = "";
                    var optResults  = new List<object>(); 
                    foreach (var option in options)
                    {
                        pollQuestion = option.QText;
						//var optionCount = DbContext.TabPollVotes.Count(it => it.OUuid == option.OUuid);
						var optionCount = new Random().NextInt64(1,10);
                        var optResult = new { OUuid = option.OUuid!, OText =option.OText!, Count = optionCount } ;
                        optResults.Add(optResult);
					}
                    var pollResult = new { QUuid = qUuid, QText = pollQuestion, PollResult = optResults };
                    pollResults.Add(pollResult);
				}
                return Ok(pollResults);
			}
			else
			{
				return Ok("No Data");
			}
		}
	}
}

﻿using ApiSleepingPatener.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;


namespace ApiSleepingPatener.Controllers
{

    public class AccountController : ApiController
    {

     //   [Authorize]
        [HttpGet]
        [Route("api/account/getuser/{id}")]
        public IHttpActionResult GetUFirstUser(int id)
        {
            // Get user from dummy list

            //var userList = new List<User>(){
            //     new User(   2,
            //       "twt",
            //        "t@g.com",
            //        "test",
            //        "test") {

            //    },
            //      new User(  1,
            //       "twt",
            //        "t@g.com",
            //        "test",
            //        "test") {

            //    }

            ////};
            //List = db.VideoPackTbls.Where(a => a.VideoPackCatId == cat_id && a.VideoPackPkgId == userpackageid)
            //  .Select(x => new VideoPackTblModelCopy
            //  {
            //      VideoPackId = x.VideoPackId,
            //      VideoPackCatId = x.VideoPackCatId,
            //      VideoPackName = x.VideoPackName,
            //      VideoPackDesc = x.VideoPackDesc,
            //      VideoPackVideos = x.VideoPackVideos
            //  }).ToList();
            ////ViewBag.VideoList = List;
            //NewUserRegistration newuser = new NewUserRegistration();
            //  List<NewUserRegistration> list = new List<NewUserRegistration>();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["remote"].ConnectionString);

            if (connect.State != ConnectionState.Open)
                connect.Open();
            SqlCommand cmd = new SqlCommand("select * from NewUserRegistration where UserId = '" + id + "'", connect);
            try
            {
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                int uid = Convert.ToInt32(sdr["UserId"].ToString());
                string uname = sdr["Username"].ToString();
                string pass = sdr["Password"].ToString();
                string Email = sdr["Email"].ToString();
                //var  client = new WebClient();
                //client.Encoding = System.Text.Encoding.UTF8;
                //client.DownloadString("http://binarymlm.epizy.com/messageNotifyApi.php?send_notification&sname=acccepted_your_status&uid=1&sid=2&message=Verification&token=crFqGndflQA:APA91bE9mEQjjerEp1GGOalbPehcZqXg8goxyeUBG__l1WhF_51nrPsGRmWa-GFVwDlDetN-vbpKVgGvu1mam5D7GjDFAodX-uB98RS7tc-A1U_t4MYcRf_Kfp3mfmVGfzLYCFZM_5PE&i=1");

                //Uri myUri = new Uri("http://binarymlm.epizy.com/messageNotifyApi.php?send_notification&sname=acccepted_your_status&uid=1&sid=2&message=Verification&token=crFqGndflQA:APA91bE9mEQjjerEp1GGOalbPehcZqXg8goxyeUBG__l1WhF_51nrPsGRmWa-GFVwDlDetN-vbpKVgGvu1mam5D7GjDFAodX-uB98RS7tc-A1U_t4MYcRf_Kfp3mfmVGfzLYCFZM_5PE&i=1");
                //// Create a new request to the above mentioned URL.	
                //WebRequest myWebRequest = WebRequest.Create(myUri);
                //// Assign the response object of 'WebRequest' to a 'WebResponse' variable.
                //WebResponse myWebResponse = myWebRequest.GetResponse();

                //string rep = myWebResponse.ResponseUri.UserInfo;

                //var request = (HttpWebRequest)WebRequest.Create("https://sleepingpartnertesting.royalcryptoexchange.com/messageNotifyApi.php?send_notification&sname=acccepted_your_status&uid=1&sid=2&message=Verification&token=crFqGndflQA:APA91bE9mEQjjerEp1GGOalbPehcZqXg8goxyeUBG__l1WhF_51nrPsGRmWa-GFVwDlDetN-vbpKVgGvu1mam5D7GjDFAodX-uB98RS7tc-A1U_t4MYcRf_Kfp3mfmVGfzLYCFZM_5PE&i=1");
                //request.Method = "HEAD";
                //var response = request.GetResponse();
                //var location = response.Headers[HttpResponseHeader.Location];

                WebClient client = new WebClient();
                string content = client.DownloadString("https://sleepingpartnertesting.royalcryptoexchange.com/messageNotifyApi.php?send_notification&sname=acccepted_your_status&uid=1&sid=2&message=Verification&token=crFqGndflQA:APA91bE9mEQjjerEp1GGOalbPehcZqXg8goxyeUBG__l1WhF_51nrPsGRmWa-GFVwDlDetN-vbpKVgGvu1mam5D7GjDFAodX-uB98RS7tc-A1U_t4MYcRf_Kfp3mfmVGfzLYCFZM_5PE&i=1");

                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.yoursite.com/resource/file.htm");

                //using (StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream(), Encoding.UTF8))
                //{
                //    streamWriter.Write(requestData);
                //}

                //string responseData = string.Empty;
                //HttpWebResponse httpResponse = (HttpWebResponse)webRequest.GetResponse();
                //using (StreamReader responseReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    responseData = responseReader.ReadToEnd();
                //}

                sdr.Close();
                connect.Close();
               
                return Ok(0);
            }
            catch (Exception n)
            {
                return Ok(0);
            }


        }
        //Profile setup
        [HttpPost]
        [Route("addprofilesetup/{userId}")]
        public IHttpActionResult ProfileSetup(ProfileSetup model, int userId)
        {


            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
            NewUserRegistration newuser = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
            if (newuser != null)
            {
                newuser.Name = model.Name;
                //newuser.Username = model.UserName;
                newuser.Password = model.Password;
                newuser.Country = model.Country;
                newuser.Address = model.Address;
                newuser.Phone = model.Phone;
                newuser.Email = model.Email;
                newuser.IsBlock = true;             
                newuser.AccountTitle = model.AccountTitle;
                newuser.AccountNumber = model.AccountNumber;
                newuser.BankName = model.BankName;
                newuser.CNIC = model.CNICNumber;
                //if (userId == Common.Enum.UserType.User.ToString())
                //{
                //    newuser.IsBlock = model.IsBlock = true;
                //}
                //else
                //{
                //    newuser.IsBlock = model.IsBlock = false;
                //}
                var fileImage1 = model.NICImage;
                var fileImage2 = model.ProfileImage;
                var fileImage3 = model.NICImage1;
                var fileImage4 = model.DocumentImage;
                if (fileImage1 != null && fileImage2 != null && fileImage3 != null && fileImage4 != null)
                {
                    newuser.NICImage = fileImage1;
                    newuser.ProfileImage = fileImage2;
                    newuser.NICImage1 = fileImage3;
                    newuser.DocumentImage = fileImage4;
                }
                dc.SaveChanges();
                //dbTree.update_tree_name(userId, model.UserName);

                ModelState.Clear();
                return Ok(new { success = true, message = "Update Successfully" });

            }
            return Ok(new { success = true, message = "Update Successfully" });
            //this.AddNotification("Your profile has bees saved", NotificationType.SUCCESS);

        }
        [HttpPost]
        [Route("newusername/{username}/{userId}")]
        public IHttpActionResult MakeNewUserName(string username,int userId)
        {
          //  var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();

            NewUserRegistration newuser = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
            List<EWalletWithdrawalFund> userEWF = dc.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<ReceiveAdminMessage> userRAM = dc.ReceiveAdminMessages.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<ReceiveUserMessage> userRUM = dc.ReceiveUserMessages.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<SentAdminMessage> userSAM = dc.SentAdminMessages.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<SentUserMessage> userSUM = dc.SentUserMessages.Where(a => a.UserId.Value.Equals(userId)).ToList();

            List<UserGenealogyTable> userUGT = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableLeft> userUGTL = dc.UserGenealogyTableLefts.Where(a => a.UserId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableRight> userUGTR = dc.UserGenealogyTableRights.Where(a => a.UserId.Value.Equals(userId)).ToList();

            List<UserGenealogyTable> userUGTS = dc.UserGenealogyTables.Where(a => a.SponsorId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableLeft> userUGTLS = dc.UserGenealogyTableLefts.Where(a => a.SponsorId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableRight> userUGTRS = dc.UserGenealogyTableRights.Where(a => a.SponsorId.Value.Equals(userId)).ToList();

            List<UserGenealogyTable> userUGTD = dc.UserGenealogyTables.Where(a => a.DownlineMemberId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableLeft> userUGTLD = dc.UserGenealogyTableLefts.Where(a => a.DownlineMemberId.Value.Equals(userId)).ToList();
            List<UserGenealogyTableRight> userUGTRD = dc.UserGenealogyTableRights.Where(a => a.DownlineMemberId.Value.Equals(userId)).ToList();

            UserTableLevel userUTL = dc.UserTableLevels.Where(a => a.UserId.Value.Equals(userId)).FirstOrDefault();

            if (newuser != null)
            {
                NewUserRegistration usernamecheck = dc.NewUserRegistrations.Where(a => a.Username.Equals(username)).FirstOrDefault();
                if (usernamecheck == null)
                {
                    newuser.Username = username;
                    dbTree.update_tree_name(userId, username);
                    if (userEWF != null)
                    {
                        foreach (var item in userEWF)
                        {
                            item.UserName = username;
                        }
                    }
                    if (userRAM != null)
                    {
                        foreach (var item in userRAM)
                        {
                            item.Sender_Name = username;
                        }
                    }
                    if (userRUM != null)
                    {
                        foreach (var item in userRUM)
                        {
                            item.Sender_Name = username;
                        }
                    }
                    if (userSAM != null)
                    {
                        foreach (var item in userSAM)
                        {
                            item.Sender_Name = username;
                        }
                    }
                    if (userSUM != null)
                    {
                        foreach (var item in userSUM)
                        {
                            item.Sender_Name = username;
                        }
                    }
                    //
                    if (userUGT != null)
                    {
                        foreach (var item in userUGT)
                        {
                            item.UserName = username;
                        }
                    }
                    if (userUGTL != null)
                    {
                        foreach (var item in userUGTL)
                        {
                            item.UserName = username;
                        }
                    }
                    if (userUGTR != null)
                    {
                        foreach (var item in userUGTR)
                        {
                            item.UserName = username;
                        }
                    }
                    //
                    if (userUGTS != null)
                    {
                        foreach (var item in userUGTS)
                        {
                            item.SponsorName = username;
                        }
                    }
                    if (userUGTLS != null)
                    {
                        foreach (var item in userUGTLS)
                        {
                            item.SponsorName = username;
                        }
                    }
                    if (userUGTRS != null)
                    {
                        foreach (var item in userUGTRS)
                        {
                            item.SponsorName = username;
                        }
                    }
                    //
                    if (userUGTD != null)
                    {
                        foreach (var item in userUGTD)
                        {
                            item.DownlineMemberName = username;
                        }
                    }
                    if (userUGTLD != null)
                    {
                        foreach (var item in userUGTLD)
                        {
                            item.DownlineMemberName = username;
                        }

                    }
                    if (userUGTRD != null)
                    {
                        foreach (var item in userUGTRD)
                        {
                            item.DownlineMemberName = username;
                        }
                    }

                    if (userUTL != null)
                    {
                        userUTL.UserName = username;
                    }

                    dc.SaveChanges();
                    ModelState.Clear();
                    return Ok(new { success = true, message = "Update Successfully" });
                }
                else
                {
                    return Ok(new { success = false, message = "This user name is exist. Kindly change your username" });
                }

            }

            return Ok(new { success = true, message = "Update Successfully" });

            //this.AddNotification("Your profile has bees saved", NotificationType.SUCCESS);
            //return RedirectToAction("ProfileSetup");
            //return RedirectToAction("Index", "Dashboard", model);
        }

        //Fcm update
        [HttpPost]
        [Route("updateFcm")]
        public IHttpActionResult FcmUpdate(FcmModel model)
        {

            SleepingPartnermanagementTestingEntities dce = new SleepingPartnermanagementTestingEntities();
            NewUserRegistration newuser = dce.NewUserRegistrations.SingleOrDefault(x => x.UserId == model.UserId);
            if (newuser != null)
            {
                newuser.fcm = model.Fcm;
                dce.SaveChanges();
                return Ok(new { success = true, message = "Update Successfully" });

            }
            return Ok(new { success = false, message = "user not found" });

        }


        //Fcm update
        [HttpPost]
        [Route("updatePassword/{passord}/{uid}")]
        public IHttpActionResult updatePassword(string passord,int uid)
        {

            SleepingPartnermanagementTestingEntities dce = new SleepingPartnermanagementTestingEntities();
            NewUserRegistration newuser = dce.NewUserRegistrations.SingleOrDefault(x => x.UserId == uid);
            if (newuser != null)
            {
                newuser.Password = passord;
                dce.SaveChanges();
                return Ok(new { success = true, message = "Update Successfully" });

            }
            return Ok(new { success = false, message = "user not found" });

        }


        [HttpGet]
        [Route("getprofileimage/{userId}")]
        public IHttpActionResult profileimage( int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            var image = db.NewUserRegistrations.Where(x => x.UserId == userId).Select(x => x.ProfileImage).FirstOrDefault();
            
                return Ok(new { success = true, message = image});
            
        }
    }
}
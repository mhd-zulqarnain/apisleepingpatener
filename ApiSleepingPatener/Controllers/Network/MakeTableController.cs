﻿using System.Collections.Generic;
using ApiSleepingPatener.Models;
using System.Web.Http;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ApiSleepingPatener.Models.Account;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using ApiSleepingPatener.Models.Reports;

namespace ApiSleepingPatener.Controllers
{

    public class DashboardController : ApiController
    {

        string SendSMSAccountSid = System.Configuration.ConfigurationManager.AppSettings["SendSMSAccountSid"];
        string SendSMSAuthToken = System.Configuration.ConfigurationManager.AppSettings["SendSMSAuthToken"];
        string SendSMSFromNumber = System.Configuration.ConfigurationManager.AppSettings["SendSMSFromNumber"];
        //[Authorize]
        [HttpGet]
        [Route("maketabledetails/{userId}")]
        public IHttpActionResult getMaketableData(int userId)
        {
            MakeTableData obj = new MakeTableData();
            string TotalLeftUsers = GetAllTotalLeftUsers(userId);
            string TotalRightUsers = GetAllTotalRightUsers(userId);

            string TotalAmountLeftUsers = GetAllTotalAmountLeftUsers(userId);
            string TotalAmountRightUsers = GetAllTotalAmountRightUsers(userId);
            string leftRemaingAmount = GetAllLeftRemaingAmount(userId);
            string rightremaingamount = GetAllRightRemaingAmount(userId);
            string getalltotalearningamount = GetAllTotalEarningAmount(userId);
            obj.totalLeftUsers = TotalLeftUsers;
            obj.totalRightUsers = TotalRightUsers;
            obj.totalAmountLeftUsers = TotalAmountLeftUsers;
            obj.totalAmountRightUsers = TotalAmountRightUsers;
            obj.leftRemaingAmount = leftRemaingAmount;
            obj.rightRemaingAmount = rightremaingamount;
            obj.getalltotalearningamount = getalltotalearningamount;
            
            return Ok(obj);

        }

        /// [Authorize]
        [HttpGet]
        [Route("getAllDownlineMembersLeft/{userId}")]
        public IHttpActionResult AllGetUserDownlineMembersLeft(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            List<GetParentChildsSP_Result> List = new List<GetParentChildsSP_Result>();
            //List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
            //    && a.DownlineMemberId.Equals(userId))
            usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsUserActive.Value == true
                        //&& n.IsPaidMember.Value == true
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();
            return Ok(usrmodel);

        }
        [HttpGet]
        [Route("getAllDownlineMembersRight/{userId}")]
        public IHttpActionResult AllGetUserDownlineMembersRight(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            List<GetParentChildsSP_Result> List = new List<GetParentChildsSP_Result>();

            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsUserActive.Value == true
                        //&& n.IsPaidMember.Value == true
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();
            return Ok(usrmodel);
        }



        //UserAllDownlineMembers Page dashboar info
        //[HttpGet]
        //[Route("getalltotaleftusers/{userId}")]
        public string GetAllTotalLeftUsers(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalLeftUsers = dc.GetParentChildsLeftSP(userId).ToList();
                int TotalLeftUsersShow = TotalLeftUsers.Count();
                
                if (TotalLeftUsersShow != 0)
                {
                    return TotalLeftUsersShow.ToString();
                    //return TotalLeftUsersShow.ToString();
                }
                else
                {
                    return TotalLeftUsersShow.ToString();
                    //return TotalLeftUsersShow.ToString();
                }
            }
            //return View();

        }
        [HttpGet]
        [Route("getalltotalrightusers/{userId}")]
        public string GetAllTotalRightUsers(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalRightUsers = dc.GetParentChildsRightSP(userId).ToList();
                int TotalRightUsersShow = TotalRightUsers.Count();

                if (TotalRightUsersShow != 0)
                {
                    return TotalRightUsersShow.ToString();
                }
                else
                {
                    return TotalRightUsersShow.ToString();
                }
            }
            //return View();

        }
        //[HttpGet]
        //[Route("getalltotalamountleftusers/{userId}")]
        public string GetAllTotalAmountLeftUsers(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalAmountLeftUsers = dc.GetParentChildsLeftSP(userId).ToList();
                decimal TotalAmountLeftUsersShow = TotalAmountLeftUsers.Sum(x => x.PaidAmount.Value);

                if (TotalAmountLeftUsersShow != 0)
                {
                    return TotalAmountLeftUsersShow.ToString();
                }
                else
                {
                    return TotalAmountLeftUsersShow.ToString();
                }
            }
            //return View();

        }
        //[HttpGet]
        //[Route("getalltotalamountrightusers/{userId}")]
        public string GetAllTotalAmountRightUsers(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var TotalAmountRightUsers = dc.GetParentChildsRightSP(userId).ToList();
                decimal TotalAmountRightUsersShow = TotalAmountRightUsers.Sum(x => x.PaidAmount.Value);

                if (TotalAmountRightUsersShow != 0)
                {
                    return TotalAmountRightUsersShow.ToString();
                    
                }
                else
                {
                    return TotalAmountRightUsersShow.ToString();
                }
            }
            //return View();

        }
        public string GetAllLeftRemaingAmount(int userId)
        {
           
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);

                //decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - LeftPaidAmountShow;

                if (showAmount != 0)
                {
                    return showAmount.ToString();
                }
                else
                {
                    return showAmount.ToString();
                }
            }
            //return View();

        }

    

        public string GetAllRightRemaingAmount(int userId)
        {
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);

                decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - RightPaidAmountShow;

                if (showAmount != 0)
                {
                    return showAmount.ToString();
                    
                }
                else
                {
                    return showAmount.ToString();
                }
            }
            //return View();

        }


        public string GetAllTotalEarningAmount(int userId)
        {
            string UserTypeUser =Common.Enum.UserType.User.ToString();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId
                           && eWallTr.IsMatchingBonus == true
                           select eWallTr).ToList();
                decimal query = CGP.Sum(a => a.Amount.Value);
              
                if (query != 0)
                {
                    return query.ToString();
                }
                else { return query.ToString(); }
            }
            

        }

      


        //add left members in network page
        [HttpPost]
        [Route("addleftmembers/{userId}")]
        public IHttpActionResult AddNewMemeberLeft(UserModel model, int userId)
        {
            try
            {
                SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
                using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
                {
                    var usercheckEmail = dc.NewUserRegistrations.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                    var usercheckPhone = dc.NewUserRegistrations.Where(a => a.Phone.Equals(model.Phone)).FirstOrDefault();
                   if (usercheckEmail != null)
                    {
                        return Ok(new { error = true, message = "User email already exist" });
                    }
                    else if (usercheckPhone != null)
                    {
                        return Ok(new { error = true, message = "User phone number already exist" });
                    }
                  else
                    {



                        decimal pckgePrice = 0;

                        Package package = dc.Packages.Where(a => a.PackagePrice.Value.Equals(pckgePrice)).FirstOrDefault();
                        UserPackage userpackage = new UserPackage();
                        UserTableLevel userTableLevel = new UserTableLevel();
                        NewUserRegistration newuser = new NewUserRegistration();

                        newuser.Name = model.Name;
                        newuser.Username = model.Username;
                        newuser.Password = model.Password;
                        newuser.Country = model.Country;
                        newuser.Phone = model.Phone;
                        newuser.Email = model.Email;
                        newuser.IsThisFirstUser = model.IsThisFirstUser;
                        if (model.DownlineMemberId == 0 || model.DownlineMemberId == null)
                        {
                            newuser.DownlineMemberId = userId;
                        }
                        else
                        {
                            newuser.DownlineMemberId = model.DownlineMemberId.Value;
                        }
                        newuser.UserPosition = Common.Enum.UserPosition.Left;
                        newuser.IsUserActive = false;
                        newuser.IsNewRequest = true;
                        newuser.SponsorId = userId;
                        newuser.UpperId = model.UpperId;
                        newuser.PaidAmount = package.PackagePrice.Value;
                        newuser.CreateDate = DateTime.Now;
                        newuser.UserCode = Common.Enum.UserType.User.ToString();
                        newuser.IsPaidMember = false;
                        newuser.UserPackage = package.PackageId;
                       
                        var fileImage = model.DocumentImage;
                        if (fileImage != null)
                        {
                            byte[] img = fileImage;
                            newuser.DocumentImage = img;
                        }
                        newuser.IsSleepingPartner = false;
                        newuser.IsSalesExecutive = false;
                        newuser.IsWithdrawalOpen = false;
                        newuser.IsBlock = false;
                        newuser.IsVerify = false;
                        newuser.IsReject = false;
                        dc.NewUserRegistrations.Add(newuser);
                        


                        userpackage.PackageId = package.PackageId;
                        userpackage.PackageName = package.PackageName;
                        userpackage.PackagePercent = package.PackagePercent;
                        userpackage.PackagePrice = package.PackagePrice;
                        userpackage.PackageValidity = package.PackageValidity;
                        userpackage.PackageMinWithdrawalAmount = package.PackageMinWithdrawalAmount;
                        userpackage.PackageMaxWithdrawalAmount = package.PackageMaxWithdrawalAmount;
                        userpackage.UserId = newuser.UserId;
                        userpackage.IsInCurrentUse = true;
                        userpackage.PurchaseDate = DateTime.Now;
                        userpackage.IsBuyFromEWallet = false;
                        userpackage.IsBuyFromBankAcnt = false;
                        userpackage.IsRequestedForBuy = false;
                        userpackage.IsApprovedForBuy = false;
                        userpackage.IsRejectedForBuy = false;

                        dc.UserPackages.Add(userpackage);


                        userTableLevel.UserName = model.Username;
                        userTableLevel.TableLevel = 1;
                        userTableLevel.NoOfUsers = 0;
                        userTableLevel.RightUsers = 0;
                        userTableLevel.LeftUsers = 0;
                        userTableLevel.TableLevelLimit = 2;
                        userTableLevel.UserId = newuser.UserId;
                        userTableLevel.LastModifiedDate = DateTime.Now;
                        dc.UserTableLevels.Add(userTableLevel);


                        //decimal pckgePrice = 0;

                        //Package package = dc.Packages.Where(a => a.PackagePrice.Value.Equals(pckgePrice)).FirstOrDefault();
                        //UserPackage userpackage = new UserPackage();
                        //UserTableLevel userTableLevel = new UserTableLevel();
                        //NewUserRegistration newuser = new NewUserRegistration();

                        //newuser.Name = model.Name;
                        //newuser.Username = model.Username;
                        //newuser.Password = model.Password;
                        //newuser.Country = model.Country;
                        ////newuser.Address = model.Address;
                        //newuser.Phone = model.Phone;
                        //newuser.Email = model.Email;
                        ////newuser.AccountNumber = model.AccountNumber;
                        ////newuser.BankName = model.BankName;
                        ////newuser.CNIC = model.CNICNumber;
                        //newuser.IsThisFirstUser = model.IsThisFirstUser;
                        //if (model.DownlineMemberId == 0 || model.DownlineMemberId == null)
                        //{
                        //    newuser.DownlineMemberId = userId;
                        //}
                        //else
                        //{
                        //    newuser.DownlineMemberId = model.DownlineMemberId.Value;
                        //}
                        //newuser.UserPosition = Common.Enum.UserPosition.Left;
                        //newuser.IsUserActive = false;
                        //newuser.IsNewRequest = true;
                        //newuser.SponsorId = userId;
                        //newuser.UpperId = model.UpperId;
                        //newuser.PaidAmount = package.PackagePrice.Value;
                        //newuser.CreateDate = DateTime.Now;
                        //newuser.UserCode = Common.Enum.UserType.User.ToString();
                        //newuser.IsPaidMember = false;
                        ////newuser.UserPackage = model.UserPackage;
                        //newuser.UserPackage = package.PackageId;
                        ////file = Request.Files["AddNewMemberLeftImageData"];
                        //var fileImage = model.DocumentImage;
                        //if (fileImage != null)
                        //{
                        //    byte[] img = fileImage;
                        //    newuser.DocumentImage = img;
                        //}
                        //newuser.IsSleepingPartner = false;
                        //newuser.IsSalesExecutive = false;
                        //newuser.IsWithdrawalOpen = false;
                        //newuser.IsBlock = false;
                        //newuser.IsVerify = false;
                        //newuser.IsReject = false;
                        //dc.NewUserRegistrations.Add(newuser);
                        #region send sms


                        //TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);

                        //var message = MessageResource.Create(
                        //    body: "Welcome to Sleeping partner portal. "
                        //    + " Please make sure to pay your amount with in 5 bussiness days"
                        //    + " to avoid your account deactivation. "
                        //    + " Your username is : " + model.Username
                        //    + " and password is : " + model.Password + "."
                        //    + " Click on http://sleepingpartnermanagementportalrct.com ",
                        //    from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                        //    to: new Twilio.Types.PhoneNumber(model.Phone)
                        //);


                        #endregion
                        #region user email

                        System.Net.Mail.MailMessage mail1 = new System.Net.Mail.MailMessage();
                        mail1.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                        mail1.To.Add(model.Email);
                        mail1.Subject = "Sleeping partner management portal";
                        mail1.Body = "User accept by admin. " +
                            " Your username is " + model.Username + " and password : " + model.Password + "</br></br>" +
                            "<table style='font-family:Verdana, Helvetica, sans-serif;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; border-right:2px solid #BD272D; padding-right:15px; text-align: right; vertical-align:top; ' valign='top'><table style='font-family:Verdana; margin-right:0; margin-left:auto;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; height:55px; vertical-align:top; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:14pt; font-weight:bold'>Sleeping partner management<span><br></span></span></td></tr><tr><td style='font-family:Verdana; height:40px; vertical-align:top; padding:0; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:10pt;'>phone: 123456<span><br></span></span><span style='font-family:Verdana; font-size:10pt;'>mobile: 0123456</span></td></tr><tr><td><a href='http://sleepingpartnermanagementportalrct.com'>sleepingpartnermanagementportal</a></td></tr></tbody></table></td><td style='padding-left:15px;font-size:1pt; vertical-align:top; font-family:Verdana;' valign='top'><table style='font-family:Verdana;' cellpadding='0' cellspacing='0'><tbody><tr><td style='height:55px; font-family:Verdana; vertical-align:top;' valign='top'><a href='{Logo URL}' target='_blank'><img alt='Logo' style='height:40px; width:auto; border:0; ' height='40' border='0'  src='~/Content/images/newsleepinglogo.png'></a></td></tr><tr><td style='height:40px; font-family:Verdana; vertical-align:top; padding:0;' valign='top'><span style='font-family:Verdana; font-size:10pt;'>{Address 1}<span><br></span></span> <span style='font-family:Verdana; font-size:10pt;'>{Address 2}</span> </td></tr><tr><td style='height:20px; font-family:Verdana; vertical-align:middle;' valign='middle'><a href='http://{Web page}' target='_blank' style='color:#BD272D; font-size:10pt; font-family:Verdana;'>{Web page}</a></td></tr></tbody></table></td></tr></tbody></table>";
                        mail1.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "sleepingpartnermanagementportalrct.com";
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Port = 25;
                        ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate,
                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
                        smtp.Send(mail1);

                        //  await SendEmailToSponsor(newuserdata.Email, "sleeping patners", Body);

                        #endregion
                        dc.SaveChanges();


                        userpackage.PackageId = package.PackageId;
                        userpackage.PackageName = package.PackageName;
                        userpackage.PackagePercent = package.PackagePercent;
                        userpackage.PackagePrice = package.PackagePrice;
                        userpackage.PackageValidity = package.PackageValidity;
                        userpackage.PackageMinWithdrawalAmount = package.PackageMinWithdrawalAmount;
                        userpackage.PackageMaxWithdrawalAmount = package.PackageMaxWithdrawalAmount;
                        userpackage.UserId = newuser.UserId;
                        userpackage.IsInCurrentUse = true;
                        userpackage.PurchaseDate = DateTime.Now;
                        userpackage.IsBuyFromEWallet = false;
                        userpackage.IsBuyFromBankAcnt = false;
                        userpackage.IsRequestedForBuy = false;
                        userpackage.IsApprovedForBuy = false;
                        userpackage.IsRejectedForBuy = false;

                        dc.UserPackages.Add(userpackage);


                        userTableLevel.UserName = model.Username;
                        userTableLevel.TableLevel = 1;
                        userTableLevel.NoOfUsers = 0;
                        userTableLevel.RightUsers = 0;
                        userTableLevel.LeftUsers = 0;
                        userTableLevel.TableLevelLimit = 2;
                        userTableLevel.UserId = newuser.UserId;
                        userTableLevel.LastModifiedDate = DateTime.Now;
                        dc.UserTableLevels.Add(userTableLevel);

                        dc.SaveChanges();

                        #region creating first user tree

                        TreeDataTbl userTree = dbTree.TreeDataTbls.Where(a => a.UserId.Value.Equals(newuser.DownlineMemberId)).FirstOrDefault();

                        if (userTree == null)
                        {
                            if (newuser.IsThisFirstUser == true)
                            {
                                dbTree.insert_tree_node(newuser.Username, 0, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                            else
                            {
                                dbTree.insert_tree_node(newuser.Username, userTree.Tree_ID, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                        }



                        #endregion


                        ModelState.Clear();
                        model = null;
                      //  ViewBag.MessageAddNewMemeberLeft = "Successfully Registration Done";
                    }

                }
                return Ok(new { success = true, message = "User has been saved" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }

        }
        //add right members in network page 
        [HttpPost]
        [Route("addrightmembers/{userId}")]
        public IHttpActionResult AddNewMemeberRight(UserModel model,int userId)
        {
            try
            {
                SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
                using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
                {
                    var usercheckEmail = dc.NewUserRegistrations.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                    var usercheckPhone = dc.NewUserRegistrations.Where(a => a.Phone.Equals(model.Phone)).FirstOrDefault();
                    //var usercheckAccountNumber = dc.NewUserRegistrations.Where(a => a.AccountNumber.Equals(model.AccountNumber)).FirstOrDefault();
                    //var usercheckCNIC = dc.NewUserRegistrations.Where(a => a.CNIC.Equals(model.CNICNumber)).FirstOrDefault();
                    if (usercheckEmail != null)
                    {
                        return Ok(new { error = true, message = "User email already exist" });
                    }
                    else if (usercheckPhone != null)
                    {
                        return Ok(new { error = true, message = "User phone number already exist" });
                    }
                    //else if (usercheckAccountNumber != null)
                    //{
                    //    return Json(new { error = true, message = "User Account Number already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else if (usercheckCNIC != null)
                    //{
                    //    return Json(new { error = true, message = "User CNIC already exist" }, JsonRequestBehavior.AllowGet);
                    //}
                    else
                    {
                        decimal pckgePrice = 0;

                        Package package = dc.Packages.Where(a => a.PackagePrice.Value.Equals(pckgePrice)).FirstOrDefault();
                        //Package package = dc.Packages.Where(a => a.PackageId.Equals(model.UserPackage)).FirstOrDefault();
                        UserPackage userpackage = new UserPackage();
                        UserTableLevel userTableLevel = new UserTableLevel();
                        NewUserRegistration newuser = new NewUserRegistration();

                        newuser.Name = model.Name;
                        newuser.Username = model.Username;
                        newuser.Password = model.Password;
                        newuser.Country = model.Country;
                        //newuser.Address = model.Address;
                        newuser.Phone = model.Phone;
                        newuser.Email = model.Email;
                        //newuser.AccountNumber = model.AccountNumber;
                        //newuser.BankName = model.BankName;
                        //newuser.CNIC = model.CNICNumber;
                        newuser.IsThisFirstUser = model.IsThisFirstUser;
                        if (model.DownlineMemberId == 0 || model.DownlineMemberId == null)
                        {
                            newuser.DownlineMemberId = userId;
                        }
                        else
                        {
                            newuser.DownlineMemberId = model.DownlineMemberId.Value;
                        }
                        newuser.UserPosition = Common.Enum.UserPosition.Right;
                        newuser.IsUserActive = false;
                        newuser.IsNewRequest = true;
                        newuser.SponsorId = userId;
                        newuser.UpperId = model.UpperId;
                        newuser.PaidAmount = package.PackagePrice;
                        newuser.CreateDate = DateTime.Now;
                        newuser.UserCode = Common.Enum.UserType.User.ToString();
                        newuser.IsPaidMember = false;
                        //newuser.UserPackage = model.UserPackage;
                        newuser.UserPackage = package.PackageId;
                        //file = Request.Files["AddNewMemberRightImageData"];
                        var fileImage = model.DocumentImage;
                        if (fileImage != null)
                        {
                            byte[] img = fileImage;
                            newuser.DocumentImage = img;
                        }
                        newuser.IsSleepingPartner = false;
                        newuser.IsSalesExecutive = false;
                        newuser.IsWithdrawalOpen = false;
                        newuser.IsBlock = false;
                        newuser.IsVerify = false;
                        newuser.IsReject = false;
                        dc.NewUserRegistrations.Add(newuser);
                        #region send sms


                        //TwilioClient.Init(SendSMSAccountSid, SendSMSAuthToken);

                        //var message = MessageResource.Create(
                        //    body: "Welcome to Sleeping partner portal. "
                        //    + " Please make sure to pay your amount with in 5 bussiness days"
                        //    + " to avoid your account deactivation. "
                        //    + " Your username is : " + model.Username
                        //    + " and password is : " + model.Password + "."
                        //    + " Click on http://sleepingpartnermanagementportalrct.com ",
                        //    from: new Twilio.Types.PhoneNumber(SendSMSFromNumber),
                        //    to: new Twilio.Types.PhoneNumber(model.Phone)
                        //);
                        #region user email

                        System.Net.Mail.MailMessage mail1 = new System.Net.Mail.MailMessage();
                        mail1.From = new MailAddress("noreply@sleepingpartnermanagementportalrct.com");
                        mail1.To.Add(model.Email);
                        mail1.Subject = "Sleeping partner management portal";
                        mail1.Body = "User accept by admin. " +
                            " Your username is " + model.Username + " and password : " + model.Password + "</br></br>" +
                            "<table style='font-family:Verdana, Helvetica, sans-serif;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; border-right:2px solid #BD272D; padding-right:15px; text-align: right; vertical-align:top; ' valign='top'><table style='font-family:Verdana; margin-right:0; margin-left:auto;' cellpadding='0' cellspacing='0'><tbody><tr><td style='font-family:Verdana; height:55px; vertical-align:top; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:14pt; font-weight:bold'>Sleeping partner management<span><br></span></span></td></tr><tr><td style='font-family:Verdana; height:40px; vertical-align:top; padding:0; text-align:right;' valign='top' align='right'><span style='font-family:Verdana; font-size:10pt;'>phone: 123456<span><br></span></span><span style='font-family:Verdana; font-size:10pt;'>mobile: 0123456</span></td></tr><tr><td><a href='http://sleepingpartnermanagementportalrct.com'>sleepingpartnermanagementportal</a></td></tr></tbody></table></td><td style='padding-left:15px;font-size:1pt; vertical-align:top; font-family:Verdana;' valign='top'><table style='font-family:Verdana;' cellpadding='0' cellspacing='0'><tbody><tr><td style='height:55px; font-family:Verdana; vertical-align:top;' valign='top'><a href='{Logo URL}' target='_blank'><img alt='Logo' style='height:40px; width:auto; border:0; ' height='40' border='0'  src='~/Content/images/newsleepinglogo.png'></a></td></tr><tr><td style='height:40px; font-family:Verdana; vertical-align:top; padding:0;' valign='top'><span style='font-family:Verdana; font-size:10pt;'>{Address 1}<span><br></span></span> <span style='font-family:Verdana; font-size:10pt;'>{Address 2}</span> </td></tr><tr><td style='height:20px; font-family:Verdana; vertical-align:middle;' valign='middle'><a href='http://{Web page}' target='_blank' style='color:#BD272D; font-size:10pt; font-family:Verdana;'>{Web page}</a></td></tr></tbody></table></td></tr></tbody></table>";
                        mail1.IsBodyHtml = true;
                        SmtpClient smtp1 = new SmtpClient();
                        smtp1.Host = "sleepingpartnermanagementportalrct.com";
                        smtp1.EnableSsl = true;
                        smtp1.UseDefaultCredentials = false;
                        smtp1.Credentials = new NetworkCredential("noreply@sleepingpartnermanagementportalrct.com", "Yly21#p8");
                        smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp1.Port = 25;
                        ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object s, X509Certificate certificate,
                                 X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
                        smtp1.Send(mail1);
                        //await SendEmailToSponsor(newuserdata.Email, "sleeping patners", Body);

                        #endregion

                        #endregion
                        dc.SaveChanges();


                        userpackage.PackageId = package.PackageId;
                        userpackage.PackageName = package.PackageName;
                        userpackage.PackagePercent = package.PackagePercent;
                        userpackage.PackagePrice = package.PackagePrice;
                        userpackage.PackageValidity = package.PackageValidity;
                        userpackage.PackageMinWithdrawalAmount = package.PackageMinWithdrawalAmount;
                        userpackage.PackageMaxWithdrawalAmount = package.PackageMaxWithdrawalAmount;
                        userpackage.UserId = newuser.UserId;
                        userpackage.IsInCurrentUse = true;
                        userpackage.PurchaseDate = DateTime.Now;
                        userpackage.IsBuyFromEWallet = false;
                        userpackage.IsBuyFromBankAcnt = false;
                        userpackage.IsRequestedForBuy = false;
                        userpackage.IsApprovedForBuy = false;
                        userpackage.IsRejectedForBuy = false;

                        dc.UserPackages.Add(userpackage);


                        userTableLevel.UserName = model.Username;
                        userTableLevel.TableLevel = 1;
                        userTableLevel.NoOfUsers = 0;
                        userTableLevel.RightUsers = 0;
                        userTableLevel.LeftUsers = 0;
                        userTableLevel.TableLevelLimit = 2;
                        userTableLevel.UserId = newuser.UserId;
                        userTableLevel.LastModifiedDate = DateTime.Now;
                        dc.UserTableLevels.Add(userTableLevel);

                        dc.SaveChanges();

                        #region creating first user tree

                        TreeDataTbl userTree = dbTree.TreeDataTbls.Where(a => a.UserId.Value.Equals(newuser.DownlineMemberId)).FirstOrDefault();

                        if (userTree == null)
                        {
                            if (newuser.IsThisFirstUser == true)
                            {
                                dbTree.insert_tree_node(newuser.Username, 0, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                            else
                            {
                                dbTree.insert_tree_node(newuser.Username, userTree.Tree_ID, newuser.UserId, newuser.DownlineMemberId, newuser.UserPosition);
                            }
                        }



                        #endregion

                     

                   

                        ModelState.Clear();
                        model = null;
                       // ViewBag.MessageAddNewMemeberRight = "Successfully Registration Done";

                    }
                }
                return Ok(new { success = true, message = "User has been saved" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }

        }
        [HttpGet]
        [Route("checkifnewmemeberleft/{userId}")]
        public IHttpActionResult CheckIfNewMemeberLeft(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var usercheck = dc.NewUserRegistrations.Where(a => a.DownlineMemberId.Equals(userId)
                    && a.IsUserActive.Value.Equals(false) && a.IsNewRequest.Value.Equals(true)
                    && a.UserPosition.Equals(Common.Enum.UserPosition.Left)).FirstOrDefault();
                if (usercheck != null)
                {
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Left.ToString())
                    {
                        return Ok(new { success = false, message = "Left" });
                    }
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Right.ToString())
                    {
                        return Ok(new { success = true, message = "Right" });
                    }
                }
                if (usercheck == null)
                {
                    return Ok(new { success = true, message = "none" });
                }
            }
            //return Json(new { success = true, message = "User has been saved" }, JsonRequestBehavior.AllowGet);
            return Ok(new { success = true, message = "Success" });
        }
        [HttpGet]
        [Route("checkIfnewmemeberleftchild/{userId}")]
        public IHttpActionResult CheckIfNewMemeberLeftChild(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var usercheck = dc.NewUserRegistrations.Where(a => a.DownlineMemberId.Equals(userId)
                    && a.IsUserActive.Value.Equals(false) && a.IsNewRequest.Value.Equals(true)
                    && a.UserPosition.Equals(Common.Enum.UserPosition.Left)).FirstOrDefault();
                if (usercheck != null)
                {
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Left.ToString())
                    {
                        return Ok(new { success = false, message = "Left" });
                    }
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Right.ToString())
                    {
                        return Ok(new { success = true, message = "Right" });
                    }
                }
                if (usercheck == null)
                {
                    return Ok(new { success = true, message = "none" });
                }
            }
            //return Json(new { success = true, message = "User has been saved" }, JsonRequestBehavior.AllowGet);
            return Ok(new { success = true, message = "Success" });
        }
       
        [HttpGet]
        [Route("checkifnewmemeberright/{userId}")]
        public IHttpActionResult CheckIfNewMemeberRight(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var usercheck = dc.NewUserRegistrations.Where(a => a.DownlineMemberId.Equals(userId)
                    && a.IsUserActive.Value.Equals(false) && a.IsNewRequest.Value.Equals(true)
                    && a.UserPosition.Equals(Common.Enum.UserPosition.Right)).FirstOrDefault();
                if (usercheck != null)
                {
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Left.ToString())
                    {
                        return Ok(new { success = false, message = "Left" });
                    }
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Right.ToString())
                    {
                        return Ok(new { success = true, message = "Right" });
                    }
                }
                if (usercheck == null)
                {
                    return Ok(new { success = true, message = "none" });
                }
            }
            //return Json(new { success = true, message = "User has been saved" }, JsonRequestBehavior.AllowGet);
            return Ok(new { success = true, message = "Success" });
        }
        [HttpGet]
        [Route("checkifnewmemeberrightchild/{userId}")]
        public IHttpActionResult CheckIfNewMemeberRightChild(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var usercheck = dc.NewUserRegistrations.Where(a => a.DownlineMemberId.Equals(userId)
                    && a.IsUserActive.Value.Equals(false) && a.IsNewRequest.Value.Equals(true)
                    && a.UserPosition.Equals(Common.Enum.UserPosition.Right)).FirstOrDefault();
                if (usercheck != null)
                {
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Left.ToString())
                    {
                        return Ok(new { success = true, message= "Left" });
                    }
                    if (usercheck.UserPosition == Common.Enum.UserPosition.Right.ToString())
                    {
                        return Ok(new { success = false, position = "Right" });
                    }
                }
                if (usercheck == null)
                {
                    return Ok(new { success = true, position = "none" });
                }
            }
            //return Json(new { success = true, message = "User has been saved" }, JsonRequestBehavior.AllowGet);
            return Ok(new { success = false, message = "Success" });
        }
        //dropdown for left downliner memebers
        //  [Authorize]
        [HttpGet]
        [Route("dropdownleft/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyLeft(int userId)
        {
           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<GetParentChildsOuterLeftSP_Result> List = new List<GetParentChildsOuterLeftSP_Result>();
            List = db.GetParentChildsOuterLeftSP(userId).ToList();

            List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();

            UserGenealogyTableLeft leftUsers = new UserGenealogyTableLeft();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    leftUsers = db.UserGenealogyTableLefts.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    if (leftUsers == null)
                    {
                        listDownlineMember.Add(new NewUserRegistration() { UserId = item.UserId.Value, Username = item.Username, UserPosition = null });
                    }
                }
            }
            return Ok(listDownlineMember);           

            // ViewBag.DownlineMemberList = listDownlineMember;
        }
        // dropdown for right downliner memebers
       // [Authorize]
        [HttpGet]
        [Route("dropdownright/{userId}")]
        public IHttpActionResult GetUserForDownlineMemberByUserOnlyRight(int userId)
        {

         //   var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<GetParentChildsOuterRightSP_Result> List = new List<GetParentChildsOuterRightSP_Result>();
            List = db.GetParentChildsOuterRightSP(userId).ToList();

            List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();

            UserGenealogyTableRight rightUsers = new UserGenealogyTableRight();

            foreach (var item in List)
            {
                var userIdChild = Convert.ToInt32(item.UserId);
                if (item.UserCode == Common.Enum.UserType.User)
                {
                    rightUsers = db.UserGenealogyTableRights.Where(a => a.DownlineMemberId.Value.Equals(userIdChild)).FirstOrDefault();
                    if (rightUsers == null)
                    {
                        listDownlineMember.Add(new NewUserRegistration() { UserId = item.UserId.Value, Username = item.Username, UserPosition = null });
                    }
                }
            }
            return Ok(listDownlineMember);
        }
        // [Authorize]
        [HttpGet]
        [Route("maketablemembersleft/{userId}")]
        public IHttpActionResult GetUserDownlineMembersLeft(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SleepingPartnermanagementTreeTestingEntities dbTree = new SleepingPartnermanagementTreeTestingEntities();
            NewMembers usrmodel = new NewMembers();
            string UserTypeUser =Common.Enum.UserType.User.ToString();

            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();        
                List = db.GetParentChildsLeftSP(userId).ToList();

                List<NewMembers> listDownlineMember = new List<NewMembers>();
                UserGenealogyTable usersLeft = new UserGenealogyTable();
                List<UserCommission> userCommissionLeft = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    if (item.UserCode == Common.Enum.UserType.User)
                    {
                        usersLeft = db.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                        if (usersLeft != null)
                        {
                            userCommissionLeft = db.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(usersLeft.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMember.Add(new NewMembers()
                                {
                                    UserId = item.UserId.Value,
                                    Username = item.Username,
                                    Country = item.Country,
                                    Phone = item.Phone,
                                    AccountNumber = item.AccountNumber,
                                    BankName = item.BankName,
                                    SponsorId = item.SponsorId.Value,
                                    PaidAmount = item.PaidAmount.Value,
                                });
                            }


                        }
                    }
                }
                //return Json(new { data = listDownlineMember }, JsonRequestBehavior.AllowGet);            
            return Ok(listDownlineMember);

            // return Json(new { data = listDownlineMember }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("maketablemembersright/{userId}")]
        public IHttpActionResult GetUserDownlineMembersRight(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            UserModel usrmodel = new UserModel();
            string UserTypeUser = Common.Enum.UserType.User.ToString();

            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();          
              List = db.GetParentChildsRightSP(userId).ToList();

                List<NewMembers> listDownlineMember = new List<NewMembers>();
                UserGenealogyTable usersRight = new UserGenealogyTable();
                List<UserCommission> userCommissionRight = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    if (item.UserCode == Common.Enum.UserType.User)
                    {
                        usersRight = db.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                        if (usersRight != null) //both null
                        {
                            userCommissionRight = db.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionRight.Exists(x => x.MatchingCommUserId.Value.Equals(usersRight.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMember.Add(new NewMembers()
                                {
                                    UserId = item.UserId.Value,
                                    Username = item.Username,
                                    Country = item.Country,
                                    Phone = item.Phone,
                                    AccountNumber = item.AccountNumber,
                                    BankName = item.BankName,
                                    SponsorId = item.SponsorId.Value,
                                    PaidAmount = item.PaidAmount.Value,
                                });
                            }
                        }

                    }                
                
            }
            return Ok(listDownlineMember);
        }
        [HttpGet]
        [Route("getuserpaidmembersleftlist/{userId}")]
        public IHttpActionResult GetUserPaidMembersLeftList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();

            return Ok(usrmodel);
        }

        //get paid and unpaid memebers on nework page
        [HttpGet]
        [Route("getuserunpaidmembersleftlist/{userId}")]
        public IHttpActionResult GetUserUnPaidMembersLeftList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            usrmodel = (from n in db.GetParentChildsLeftSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == false
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();

            return Ok(usrmodel);
        }

        //get paid members right list
        [HttpGet]
        [Route("getuserPaidmembersrightlist/{userId}")]
        public IHttpActionResult GetUserPaidMembersRightList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == true
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();

            return Ok(usrmodel);
        }
        //get Unpaid members right list
        [HttpGet]
        [Route("getuserunpaidmembersrightlist/{userId}")]
        public IHttpActionResult GetUserUnPaidMembersRightList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            IEnumerable<NewMembers> usrmodel = new List<NewMembers>();
            usrmodel = (from n in db.GetParentChildsRightSP(userId)
                        join c in db.NewUserRegistrations on n.SponsorId equals c.UserId
                        where n.IsPaidMember.Value == false
                        select new NewMembers
                        {
                            UserId = n.UserId.Value,
                            Username = n.Username,
                            Country = n.Country,
                            Phone = n.Phone,
                            AccountNumber = n.AccountNumber,
                            BankName = n.BankName,
                            SponsorId = n.SponsorId,
                            PaidAmount = n.PaidAmount.Value,
                            SponsorName = c.Username
                        }).ToList();

            return Ok(usrmodel);
        }
        //refered members
        [HttpGet]
        [Route("GetUserReferedMembers/{userId}")]
        public IHttpActionResult GetUserReferedMembers(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            UserModel usrmodel = new UserModel();
            List<NewMembers> List = new List<NewMembers>();
            List = db.NewUserRegistrations.Where(a => a.UserCode.Equals(UserTypeUser)
                && a.SponsorId.Equals(userId))
                .Select(x => new NewMembers
                //List = db.NewUserRegistrations.Select(x => new UserModel
                {
                    UserId = x.UserId,
                    Username = x.Username,
                    Country = x.Country,
                    Phone = x.Phone,
                    AccountNumber = x.AccountNumber,
                    BankName = x.BankName,
                    SponsorId = x.SponsorId,
                    PaidAmount = x.PaidAmount.Value
                }).ToList();
            return Ok(List);
        }



        //userdownlinemembers object on add new member 
        [HttpGet]
        [Route("getuserdownlinmembers/{userId}")]
        public IHttpActionResult GetUserDownlinMembers(int userId)
        {
            MakeTableData obj = new MakeTableData();
            string gettotalleftusers = GetTotalLeftUsers(userId);
            string gettotalamountleftusers = GetTotalAmountLeftUsers(userId);          
            string getleftremaingamount = GetLeftRemaingAmount(userId);
            string getrightremaingamount = GetRightRemaingAmount(userId);
            string gettotalrightusers = GetTotalRightUsers(userId);
            string gettotalamountrightusers = GetTotalAmountRightUsers(userId);
            //string getalltotalearningamount = GetAllTotalEarningAmount(userId);
            string getusertablebalance = GetUserTableBalance(userId);


            obj.totalLeftUsers = gettotalleftusers;
            obj.totalAmountLeftUsers = gettotalamountleftusers;
            obj.leftRemaingAmount = getleftremaingamount;
            obj.rightRemaingAmount = getrightremaingamount;
            obj.totalRightUsers = gettotalrightusers;
            obj.totalAmountRightUsers = gettotalamountrightusers;
            obj.usertablebalance = getusertablebalance;



            return Ok(obj);

        }
        [HttpGet]
        [Route("getusertablebalance/{userId}")]
        public string GetUserTableBalance(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                UserCommissionBalance commBalance = new UserCommissionBalance();
                commBalance = dc.UserCommissionBalances.Where(a => a.UserId.Value.Equals(userId)).FirstOrDefault();
                if (commBalance != null)
                {
                    var Position = commBalance.UserCommPosition;
                    var Balance = commBalance.UserCommBalance;

                    if (Position != "")
                    {
                        return   Balance.ToString();
                        //return Ok(new { success = true, position = Position, balance = Balance });
                    }
                    else
                    {
                        return Balance.ToString();
                        //return Json(new { warning = true, position = "", balance = 0 }, JsonRequestBehavior.AllowGet);
                    }
                  
                }
                return "";
                //else
                //{
                //    r;
                //    return Json(new { warning = true, position = "", balance = 0 }, JsonRequestBehavior.AllowGet);
                //}

            }
            //return View();

        }
        public string GetTotalLeftUsers(int userId)
        {
            int TotalLeftUsersShow = 0;
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                List = dc.GetParentChildsLeftSP(userId).ToList();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                UserGenealogyTable usersLeft = new UserGenealogyTable();
                List<UserCommission> userCommissionLeft = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersLeft = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                    if (usersLeft != null)
                    {
                        userCommissionLeft = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                        bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(usersLeft.UserId.Value));
                        if (checkIfExists == false)
                        {
                            listDownlineMember.Add(new NewUserRegistration()
                            {
                                UserId = item.UserId.Value,
                                Username = item.Username,
                                Country = item.Country,
                                Phone = item.Phone,
                                AccountNumber = item.AccountNumber,
                                BankName = item.BankName,
                                SponsorId = item.SponsorId.Value,
                                PaidAmount = item.PaidAmount.Value,
                                UserCode = item.UserCode
                            });
                        }

                        TotalLeftUsersShow = listDownlineMember.Count();
                    }

                }

                if (TotalLeftUsersShow != 0)
                {
                    return TotalLeftUsersShow.ToString();
                }
                else
                {
                    return TotalLeftUsersShow.ToString();
                }
            }
            //return View();

        }
        public string GetTotalAmountLeftUsers(int userId)
        {
            decimal TotalAmountLeftUsersShow = 0;
            List<GetParentChildsLeftSP_Result> List = new List<GetParentChildsLeftSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                List = dc.GetParentChildsLeftSP(userId).ToList();
                List<UserGenealogyTable> usersLeft = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                List<UserCommission> userCommissionLeft = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersLeft = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            userCommissionLeft = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(item.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMember.Add(new NewUserRegistration()
                                {
                                    UserId = item.UserId.Value,
                                    Username = item.Username,
                                    Country = item.Country,
                                    Phone = item.Phone,
                                    AccountNumber = item.AccountNumber,
                                    BankName = item.BankName,
                                    SponsorId = item.SponsorId.Value,
                                    PaidAmount = item.PaidAmount.Value,
                                    UserCode = item.UserCode
                                });
                            }

                            TotalAmountLeftUsersShow = listDownlineMember.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }

                if (TotalAmountLeftUsersShow != 0)
                {
                    return TotalAmountLeftUsersShow.ToString();
                }
                else
                {
                    return TotalAmountLeftUsersShow.ToString();
                }
            }
            //return View();

        }
        public string GetLeftRemaingAmount(int userId)
        {
            decimal LeftPaidAmountShow = 0;
            decimal RightPaidAmountShow = 0;
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            List<GetParentChildsLeftSP_Result> ListLeft = new List<GetParentChildsLeftSP_Result>();
            List<GetParentChildsRightSP_Result> ListRight = new List<GetParentChildsRightSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                ListLeft = dc.GetParentChildsLeftSP(userId).ToList();
                ListRight = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTable> usersLeft = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<UserGenealogyTable> usersRight = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();

                List<NewUserRegistration> listDownlineMemberLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMemberRight = new List<NewUserRegistration>();
                List<UserCommission> userCommissionLeft = new List<UserCommission>();
                List<UserCommission> userCommissionRight = new List<UserCommission>();

                foreach (var itemLeft in ListLeft)
                {
                    var userIdChild = Convert.ToInt32(itemLeft.UserId);
                    usersLeft = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            userCommissionLeft = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(itemLeft.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMemberLeft.Add(new NewUserRegistration()
                                {
                                    UserId = itemLeft.UserId.Value,
                                    Username = itemLeft.Username,
                                    Country = itemLeft.Country,
                                    Phone = itemLeft.Phone,
                                    PaidAmount = itemLeft.PaidAmount.Value
                                });
                            }

                            LeftPaidAmountShow = listDownlineMemberLeft.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                foreach (var itemRight in ListRight)
                {
                    var userIdChild = Convert.ToInt32(itemRight.UserId);
                    usersRight = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            userCommissionRight = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(itemRight.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMemberRight.Add(new NewUserRegistration()
                                {
                                    UserId = itemRight.UserId.Value,
                                    Username = itemRight.Username,
                                    Country = itemRight.Country,
                                    Phone = itemRight.Phone,
                                    PaidAmount = itemRight.PaidAmount.Value
                                });
                            }

                            RightPaidAmountShow = listDownlineMemberRight.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                //var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                //var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                //decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                //decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);



                //decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - LeftPaidAmountShow;

                if (showAmount != 0)
                {
                    return showAmount.ToString();
                    
                }
                else
                {
                    return showAmount.ToString();
                }
            }
            //return View();

        }
        public string GetRightRemaingAmount(int userId)
        {
            decimal LeftPaidAmountShow = 0;
            decimal RightPaidAmountShow = 0;
            string UserTypeUser = Common.Enum.UserType.User.ToString();
            List<GetParentChildsLeftSP_Result> ListLeft = new List<GetParentChildsLeftSP_Result>();
            List<GetParentChildsRightSP_Result> ListRight = new List<GetParentChildsRightSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                ListLeft = dc.GetParentChildsLeftSP(userId).ToList();
                ListRight = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTable> usersLeft = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersLeft = new List<NewUserRegistration>();
                List<UserGenealogyTable> usersRight = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();

                List<NewUserRegistration> listDownlineMemberLeft = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMemberRight = new List<NewUserRegistration>();
                List<UserCommission> userCommissionLeft = new List<UserCommission>();
                List<UserCommission> userCommissionRight = new List<UserCommission>();

                foreach (var itemLeft in ListLeft)
                {
                    var userIdChild = Convert.ToInt32(itemLeft.UserId);
                    usersLeft = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersLeft)
                    {
                        var userIdChildLeft = Convert.ToInt32(itemUser.UserId);
                        newUsersLeft = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildLeft)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersLeft != null)
                        {
                            userCommissionLeft = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(itemLeft.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMemberLeft.Add(new NewUserRegistration()
                                {
                                    UserId = itemLeft.UserId.Value,
                                    Username = itemLeft.Username,
                                    Country = itemLeft.Country,
                                    Phone = itemLeft.Phone,
                                    PaidAmount = itemLeft.PaidAmount.Value
                                });
                            }

                            LeftPaidAmountShow = listDownlineMemberLeft.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                foreach (var itemRight in ListRight)
                {
                    var userIdChild = Convert.ToInt32(itemRight.UserId);
                    usersRight = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            userCommissionRight = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionLeft.Exists(x => x.MatchingCommUserId.Value.Equals(itemRight.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMemberRight.Add(new NewUserRegistration()
                                {
                                    UserId = itemRight.UserId.Value,
                                    Username = itemRight.Username,
                                    Country = itemRight.Country,
                                    Phone = itemRight.Phone,
                                    PaidAmount = itemRight.PaidAmount.Value
                                });
                            }

                            RightPaidAmountShow = listDownlineMemberRight.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }
                //var LeftPaidAmount = dc.GetParentChildsLeftSP(userId).ToList();
                //var RightPaidAmount = dc.GetParentChildsRightSP(userId).ToList();
                //decimal LeftPaidAmountShow = LeftPaidAmount.Sum(x => x.PaidAmount.Value);
                //decimal RightPaidAmountShow = RightPaidAmount.Sum(x => x.PaidAmount.Value);



                //decimal minimumAmount = Math.Min(LeftPaidAmountShow, RightPaidAmountShow);
                decimal maximumAmount = Math.Max(LeftPaidAmountShow, RightPaidAmountShow);
                decimal showAmount = maximumAmount - RightPaidAmountShow;

                if (showAmount != 0)
                {
                    return showAmount.ToString();
                }
                else
                {
                    return showAmount.ToString();
                }
            }
            //return View();

        }
        public string GetTotalRightUsers(int userId)
        {
            int TotalRightUsersShow = 0;
            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                List = dc.GetParentChildsRightSP(userId).ToList();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                UserGenealogyTable usersRight = new UserGenealogyTable();
                List<UserCommission> userCommissionRight = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersRight = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).FirstOrDefault();
                    if (usersRight != null)
                    {
                        userCommissionRight = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                        bool? checkIfExists = userCommissionRight.Exists(x => x.MatchingCommUserId.Value.Equals(usersRight.UserId.Value));
                        if (checkIfExists == false)
                        {
                            listDownlineMember.Add(new NewUserRegistration()
                            {
                                UserId = item.UserId.Value,
                                Username = item.Username,
                                Country = item.Country,
                                Phone = item.Phone,
                                AccountNumber = item.AccountNumber,
                                BankName = item.BankName,
                                SponsorId = item.SponsorId.Value,
                                PaidAmount = item.PaidAmount.Value,
                                UserCode = item.UserCode
                            });
                        }

                        TotalRightUsersShow = listDownlineMember.Count();
                    }

                }

                if (TotalRightUsersShow != 0)
                {
                    return TotalRightUsersShow.ToString();
                }
                else
                {
                    return TotalRightUsersShow.ToString();
                }
            }
            //return View();

        }
        public string GetTotalAmountRightUsers(int userId)
        {
            decimal TotalAmountRightUsersShow = 0;
            List<GetParentChildsRightSP_Result> List = new List<GetParentChildsRightSP_Result>();
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                List = dc.GetParentChildsRightSP(userId).ToList();
                List<UserGenealogyTable> usersRight = new List<UserGenealogyTable>();
                List<NewUserRegistration> newUsersRight = new List<NewUserRegistration>();
                List<NewUserRegistration> listDownlineMember = new List<NewUserRegistration>();
                List<UserCommission> userCommissionRight = new List<UserCommission>();

                foreach (var item in List)
                {
                    var userIdChild = Convert.ToInt32(item.UserId);
                    usersRight = dc.UserGenealogyTables.Where(a => a.UserId.Value.Equals(userIdChild)
                            && a.MatchingCommision.Value.Equals(false)).ToList();
                    foreach (var itemUser in usersRight)
                    {
                        var userIdChildRight = Convert.ToInt32(itemUser.UserId);
                        newUsersRight = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userIdChildRight)
                            && a.IsUserActive.Value.Equals(true)).ToList();
                        if (newUsersRight != null)
                        {
                            userCommissionRight = dc.UserCommissions.Where(a => a.UserId.Value.Equals(userId)).ToList();
                            bool? checkIfExists = userCommissionRight.Exists(x => x.MatchingCommUserId.Value.Equals(item.UserId.Value));
                            if (checkIfExists == false)
                            {
                                listDownlineMember.Add(new NewUserRegistration()
                                {
                                    UserId = item.UserId.Value,
                                    Username = item.Username,
                                    Country = item.Country,
                                    Phone = item.Phone,
                                    AccountNumber = item.AccountNumber,
                                    BankName = item.BankName,
                                    SponsorId = item.SponsorId.Value,
                                    PaidAmount = item.PaidAmount.Value,
                                    UserCode = item.UserCode
                                });
                            }

                            TotalAmountRightUsersShow = listDownlineMember.Sum(x => x.PaidAmount.Value);
                        }

                    }

                }

                if (TotalAmountRightUsersShow != 0)
                {
                    return TotalAmountRightUsersShow.ToString();
                }
                else
                {
                    return TotalAmountRightUsersShow.ToString();
                }
            }
            //return View();

        }

    }
}





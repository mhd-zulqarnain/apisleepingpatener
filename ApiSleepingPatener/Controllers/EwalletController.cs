﻿using ApiSleepingPatener.Models;
using ApiSleepingPatener.Models.Account;
using ApiSleepingPatener.Models.GenealogyTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ApiSleepingPatener.Controllers
{
    public class EwalletController : ApiController
    {
        //summary
        [HttpGet]
        [Route("ewalletsummary/summary/{userId}")]
        public IHttpActionResult getbonusandwithdrawsummary(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletSummarySponsorBonus(userId);
            obj.witdraw = GetEWalletSummaryWithdrawal(userId);
                return Ok(obj);

        }

        //montlhy
        [HttpGet]
        [Route("ewalletsummary/summarymonthly/{userId}")]
        public IHttpActionResult getbonusandwithdrawmontlhy(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletThisMonthSponsorBonus(userId);
            obj.witdraw = GetEWalletThisMonthWithdrawal(userId);
            return Ok(obj);

        }
        //yearly
        [HttpGet]
        [Route("ewalletsummary/summaryyearly/{userId}")]
        public IHttpActionResult getbonusandwithdrawyearly(int userId)
        {
            EwalletModel obj = new EwalletModel();
            obj.bonus = GetEWalletThisYearSponsorBonus(userId);
            obj.witdraw = GetEWalletThisYearWithdrawal(userId);
            return Ok(obj);

        }
        //E wallet Transacion
        //Overall transaction
        [HttpGet]
        [Route("wallet/overalllist/{userId}")] 
        public IHttpActionResult GetEWalletTransactionsOverAllList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();           
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId))
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();

         
            //return Json(new { data = List }, JsonRequestBehavior.AllowGet);
            return Ok(List);

        }

        //This Months transaction
        [HttpGet] 
        [Route("wallet/thismonth/{userId}")]
        public IHttpActionResult GetEWalletTransactionsThisMonthList(int userId)
        {            
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          

                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month))
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);
                 

        }
        [HttpGet]
        [Route("ewalletcredit/overall/{userId}")]
        public IHttpActionResult GetEWalletCreditsOverAllList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                && a.Credit == true && a.Debit == false)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("ewalletcredit/thismonth/{userId}")]
        public IHttpActionResult GetEWalletCreditsThisMonthList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month)
                    && a.Credit == true && a.Debit == false)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }
        //debit
        [HttpGet]
        [Route("ewalletdebit/overall/{userId}")]
        public IHttpActionResult GetEWalletDebitsOverAllList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();          
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.Credit == false && a.Debit == true)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();

            return Ok(List);

        }
        [HttpGet]
        [Route("ewalletdebit/thismonth/{userId}")]
        public IHttpActionResult GetEWalletDebitsThisMonthList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletTransactionModel> List = new List<EWalletTransactionModel>();      
                List = db.EWalletTransactions.Where(a => a.UserId.Value.Equals(userId)
                    && a.TransactionDate.Value.Year.Equals(DateTime.Now.Year)
                    && a.TransactionDate.Value.Month.Equals(DateTime.Now.Month)
                    && a.Credit == false && a.Debit == true)
                    .Select(x => new EWalletTransactionModel
                    {
                        TransactionId = x.TransactionId,
                        TransactionSource = x.TransactionSource,
                        TransactionName = x.TransactionName,
                        AsscociatedMember = x.AsscociatedMember.Value,
                        Amount = x.Amount.Value,
                        TransactionDate = x.TransactionDate.Value
                    }).ToList();
            return Ok(List);

        }

        //

        //Overall transaction
        [HttpPost]
        [Route("addAds")]
        public IHttpActionResult postAdd(Advertisement model)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
          
            db.Advertisements.Add(model);
            db.SaveChanges();

            return Ok("success");

        }
        
        public string GetEWalletSummarySponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;

                return Sum.ToString();
            }
           // return View();
        }
        
        public string GetEWalletSummaryWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);

                return query.ToString();
            }
            //return View();
        }

        //ewallet motnhly
        
        public string GetEWalletThisMonthSponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                             && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                              && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;
                return Sum.ToString();
            }
                   }


        public string GetEWalletThisMonthWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                           && eWallTr.TransactionDate.Value.Month == DateTime.Now.Month
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);
                return query.ToString();
            }
        }

        //ewallet bonus and withdraw yearly

        
        public string GetEWalletThisYearSponsorBonus(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var Debit = (from eWallTr in dc.EWalletTransactions
                             where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                             && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                             select eWallTr).ToList();
                var DebitValue = Debit.Sum(x => x.Amount);

                var Credit = (from eWallTr in dc.EWalletTransactions
                              where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                              && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                              select eWallTr).ToList();
                var CreditValue = Credit.Sum(x => x.Amount);

                var Sum = DebitValue - CreditValue;
                return Sum.ToString();
            }
        }

   
        public string GetEWalletThisYearWithdrawal(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from eWallTr in dc.EWalletTransactions
                           where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                           && eWallTr.TransactionDate.Value.Year == DateTime.Now.Year
                           select eWallTr).ToList();
                var query = CGP.Sum(x => x.Amount);
               return query.ToString();
            }
        }

        //Ewallet my withdrawl request
        [HttpGet]
        [Route("GetEWalletPendingWithdrawalRequests/{userId}")]
        public IHttpActionResult GetEWalletPendingWithdrawalRequestsList(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();  
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == true && a.IsPending.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        Username = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        RequestedDate = x.RequestedDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletApprovedRequestPendingPayment/{userId}")]
        public IHttpActionResult GetEWalletApprovedRequestPendingPaymentList(int userId)
        {
           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();  
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == true && a.IsApproved.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        Username = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        ApprovedDate = x.ApprovedDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletApprovedRequestPaidPayment/{userId}")]
        public IHttpActionResult GetEWalletApprovedRequestPaidPaymentList(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();     
                List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == false && a.IsApproved.Value == true && a.IsPaid.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        Username = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        PaidDate = x.PaidDate.Value
                    }).ToList();
            return Ok(List);

        }
        [HttpGet]
        [Route("GetEWalletRejectedWithdrawalRequests/{userId}")]
        public IHttpActionResult GetEWalletRejectedWithdrawalRequestsList(int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<EWalletWithdrawalFundModel> List = new List<EWalletWithdrawalFundModel>();
            List = db.EWalletWithdrawalFunds.Where(a => a.UserId.Value.Equals(userId)
                && a.IsActive.Value == false && a.IsRejected.Value == true)
                    .Select(x => new EWalletWithdrawalFundModel
                    {
                        Username = x.UserName,
                        WithdrawalFundMethod = x.WithdrawalFundMethod,
                        AmountPayble = x.AmountPayble.Value,
                        WithdrawalFundCharge = x.WithdrawalFundCharge.Value,
                        RejectedDate = x.RejectedDate.Value
                    }).ToList();
            return Ok(List);

        }



        //E wallet withdraw fund page  Object return

        [HttpGet]
        [Route("getalluserwithdrawfund/{userId}")]
        public IHttpActionResult EWalletWithDrawFund(int userId)
        {
            SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities();
            UserPackage userPackages = dc.UserPackages.Where(a => a.UserId.Value.Equals(userId)).FirstOrDefault();

            Models.EWallet.EWalletPayoutConfigModel obj = EWalletPayoutConfigDetail();
            EwalletWithDrawObjectModel obj_ewdf = new EwalletWithDrawObjectModel();
            string getuserpackagecommissionAmount = GetUserPackageCommissionAmount(userId);//2: Your total current package commision
            string getuserpackageamountLimitforwithdrawal = GetUserPackageAmountLimitForWithdrawal(userId);//3:Package amount limit for withdrawal
            string getuserewalletamountinprocess = GetUserEWalletAmountInProcess(userId);//4:E-wallet amount already in payout process
            string PayoutChargesPercent = obj.PayoutChargesPercent.ToString();//6:Processing Charges - Bank Account
            string MinimumPayout = obj.MinimumPayout.ToString();//7:Minimum withdrawal Amount
            string packagename = userPackages.PackageName;

            obj_ewdf.PackageName = packagename;
            obj_ewdf.GetUserPackageCommissionAmount = getuserpackagecommissionAmount;
            obj_ewdf.GetUserPackageAmountLimitForWithdrawal = getuserpackageamountLimitforwithdrawal;
            obj_ewdf.GetUserEWalletAmountInProcess = getuserewalletamountinprocess;
            obj_ewdf.PayoutChargesPercent = PayoutChargesPercent;
            obj_ewdf.MinimumPayout = MinimumPayout;


            return Ok(obj_ewdf);
        }

        //[HttpGet]
        //[Route("getuserpackagecommission/{userId}")]
        public string GetUserPackageCommissionAmount(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                EwalletModel obj = new EwalletModel();
                UserPackage userPackages = dc.UserPackages.Where(a => a.UserId.Value.Equals(userId)).FirstOrDefault();

                var EWalletTransactionAmount = (from eWallTr in dc.EWalletTransactions
                                                where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                                                && eWallTr.IsPackageBonus == true && eWallTr.PackageId == userPackages.PackageId.Value
                                                select eWallTr).ToList();
                var EWalletTransactionAmountValue = EWalletTransactionAmount.Sum(x => x.Amount);
                obj.bonus = EWalletTransactionAmountValue.ToString();
                

                return EWalletTransactionAmountValue.ToString();
            }
        }
            //return View(
        //[HttpGet]
        //[Route("getuserpackageamountlimitforwithdrawal/{userId}")]
        public string GetUserPackageAmountLimitForWithdrawal(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                UserPackage userPackages = dc.UserPackages.Where(a => a.UserId.Value.Equals(userId)).FirstOrDefault();
                var userPackagesAmountLimitValue = userPackages.PackagePrice.Value;

                return userPackagesAmountLimitValue.ToString();
            }
            //return View();
        }
        //[HttpGet]
        //[Route("getuserewalletamountinprocess/{userId}")]
        public string GetUserEWalletAmountInProcess(int userId)
        {
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                var CGP = (from eWallWDF in dc.EWalletWithdrawalFunds
                           where eWallWDF.UserId == userId && eWallWDF.IsActive == true
                           && eWallWDF.IsPending == false && eWallWDF.IsApproved == true
                           && eWallWDF.IsPaid == false && eWallWDF.IsRejected == false
                           select eWallWDF).ToList();
                var query = CGP.Sum(x => x.AmountPayble);

                if (query != null)
                {
                    return query.ToString();
                }
                else
                {
                    return query.ToString();
                }
            }
            //return View();
        }
        //[HttpGet]
        //[Route("ewalletpayoutconfigdetail/{userId}")]
        public Models.EWallet.EWalletPayoutConfigModel EWalletPayoutConfigDetail()
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            Models.EWallet.EWalletPayoutConfigModel obj = new Models.EWallet.EWalletPayoutConfigModel();
            obj = db.EWalletPayoutConfigs
                .Select(x => new Models.EWallet.EWalletPayoutConfigModel
                {
                    TimePeriod = x.TimePeriod,
                    MinimumPayout = x.MinimumPayout.Value,
                    PayoutChargesPercent = x.PayoutChargesPercent.Value
                }).FirstOrDefault();
            return obj;
        }
        [HttpPost]
        [Route("ewalletwithdrawalfund/{userId}")]
        public IHttpActionResult EWalletWithdrawalFund(EWalletWithdrawalFundModel model, int userId)
        {
            try
            {
                //  var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
                using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
                {
                    decimal amountcheck = model.AmountPayble;
                    decimal withdrawalFundCharges = 0;
                    int payoutChargesPercent = 0;
                    decimal payoutChargesPercentValue = 0;
                    decimal minimumPayout = 0;
                    decimal amountAfterChargesDeduct = 0;

                    var Debit = (from eWallTr in dc.EWalletTransactions
                                 where eWallTr.UserId == userId && eWallTr.Credit == false
                                 && eWallTr.Debit == true && eWallTr.IsPackageBonus == true
                                 select eWallTr).ToList();
                    decimal DebitValue = Debit.Sum(x => x.Amount.Value);

                    var Credit = (from eWallTr in dc.EWalletTransactions
                                  where eWallTr.UserId == userId && eWallTr.Credit == true
                                  && eWallTr.Debit == false && eWallTr.IsPackageBonus == true
                                  select eWallTr).ToList();
                    decimal CreditValue = Credit.Sum(x => x.Amount.Value);

                    decimal minVal = Math.Min(DebitValue, CreditValue);
                    decimal maxVal = Math.Max(DebitValue, CreditValue);

                    decimal Sum = maxVal - minVal;

                    EWalletWithdrawalFund newpckg = dc.EWalletWithdrawalFunds.Where(a => a.WithdrawalFundId.Equals(model.WithdrawalFundId)).FirstOrDefault();
                    NewUserRegistration user = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();
                    if (user.IsPaidMember.Value == true)
                    {
                        EWalletPayoutConfig payoutconfig = dc.EWalletPayoutConfigs.FirstOrDefault();
                        minimumPayout = payoutconfig.MinimumPayout.Value;
                        if (model.AmountPayble >= minimumPayout)
                        {

                            if (model.AmountPayble <= Sum)
                            {
                                if (newpckg == null)
                                {

                                    payoutChargesPercent = payoutconfig.PayoutChargesPercent.Value;
                                    payoutChargesPercentValue = (decimal)payoutChargesPercent / 100; //0.1

                                    withdrawalFundCharges = model.AmountPayble * payoutChargesPercentValue;
                                    amountAfterChargesDeduct = model.AmountPayble - withdrawalFundCharges;

                                    EWalletWithdrawalFund newpckgadd = new EWalletWithdrawalFund();
                                    newpckgadd.UserName = user.Username;
                                    newpckgadd.AccountNumber = user.AccountNumber; //auto
                                    newpckgadd.BankName = user.BankName; //auto
                                    newpckgadd.WithdrawalFundMethod = Common.Enum.PaymentSource.BankAccount.ToString();
                                    newpckgadd.AmountPayble = amountAfterChargesDeduct;
                                    newpckgadd.WithdrawalFundDescription = model.WithdrawalFundDescription;
                                    newpckgadd.WithdrawalFundCharge = withdrawalFundCharges; //auto
                                    newpckgadd.Country = user.Country;
                                    newpckgadd.RequestedDate = DateTime.Now;
                                    newpckgadd.ApprovedDate = null;
                                    newpckgadd.PaidDate = null;
                                    newpckgadd.RejectedDate = null;
                                    newpckgadd.IsActive = true;
                                    newpckgadd.IsPending = true;
                                    newpckgadd.IsApproved = false;
                                    newpckgadd.IsPaid = false;
                                    newpckgadd.IsRejected = false;
                                    newpckgadd.UserId = user.UserId;
                                    dc.EWalletWithdrawalFunds.Add(newpckgadd);
                                    dc.SaveChanges();
                                    ModelState.Clear();
                                    //    ViewBag.Message = "Successfully Done";
                                      return Ok(new { success = true, message = "Successfully Done" });

                                }
                                //else
                                //{
                                //    newpckg.TimePeriod = model.TimePeriod;
                                //    newpckg.MinimumPayout = model.MinimumPayout;
                                //    newpckg.PayoutChargesPercent = model.PayoutChargesPercent;
                                //    dc.SaveChanges();
                                //    model = null;
                                //    ViewBag.Message = "Successfully Edit";
                                //}
                            }
                            else
                            {
                                return Ok(new { success =false, message = "Amount must be smaller than the eligible amount" });
                            }
                        }
                        else
                        {
                            //this.AddNotification("Amount payable is less then minimum range", NotificationType.WARNING);
                            return Ok(new { success = false, message = "Amount must be greater than the processing charges" });
                            //return RedirectToAction("EWalletWithdrawalFund");
                        }
                    }
                    else
                    {
                        return Ok(new { success = false, message = "You are not eligible for withdrawal. Kindly purchase your package." });
                    }

                }
                //this.AddNotification("Value has bees saved", NotificationType.SUCCESS);
                return Ok(new { success = true, message = "Successfully Done" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "exception" });
            }
            //return View();
        }
        [HttpGet]
        [Route("getusercurrentpackageslist/{userId}")]
        public IHttpActionResult GetUserCurrentPackagesList(int userId)
        {
          //  var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<UserPackageModel> List = new List<UserPackageModel>();  
                List = db.UserPackages.Where(a => a.UserId.Value.Equals(userId))
                    .Select(x => new UserPackageModel
                    {
                        UserPackageId = x.UserPackageId,
                        PackageName = x.PackageName,
                        PackagePercent = x.PackagePercent.Value,
                        PackagePrice = x.PackagePrice.Value,
                        PackageValidity = x.PackageValidity,
                        PackageMinWithdrawalAmount = x.PackageMinWithdrawalAmount.Value,
                        PackageMaxWithdrawalAmount = x.PackageMaxWithdrawalAmount.Value,
                        PackageId = x.PackageId.Value,
                        UserId = x.UserId.Value,
                        IsInCurrentUse = x.IsInCurrentUse.Value,
                        PurchaseDate = x.PurchaseDate.Value,
                        IsBuyFromEWallet = x.IsBuyFromEWallet.Value,
                        IsBuyFromBankAcnt = x.IsBuyFromBankAcnt.Value,
                        IsRequestedForBuy = x.IsRequestedForBuy.Value,
                        IsApprovedForBuy = x.IsApprovedForBuy.Value,
                        IsRejectedForBuy = x.IsRejectedForBuy.Value

                    }).ToList();
            return Ok(List);

        }

        [HttpPost]
        [Route("ewalletupgradeinvestment/{userId}")]
        public IHttpActionResult EWalletUpgradeInvestment(UserPackageModel model,int userId)
        {
           // var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            using (SleepingPartnermanagementTestingEntities dc = new SleepingPartnermanagementTestingEntities())
            {
                NewUserRegistration user = dc.NewUserRegistrations.Where(a => a.UserId.Equals(userId)).FirstOrDefault();

                if (user.IsVerify.Value.Equals(true))
                {
                    Package newpckg = dc.Packages.Where(a => a.PackageId.Equals(model.PackageId)).FirstOrDefault();

                    if (model.PackagePurchaseMethod == Common.Enum.UpgradeInvestmentPaymentSource.EWalletBalance)
                    {
                        var Debit = (from eWallTr in dc.EWalletTransactions
                                     where eWallTr.UserId == userId && eWallTr.Credit == false && eWallTr.Debit == true
                                     select eWallTr).ToList();
                        var DebitValue = Debit.Sum(x => x.Amount);

                        var Credit = (from eWallTr in dc.EWalletTransactions
                                      where eWallTr.UserId == userId && eWallTr.Credit == true && eWallTr.Debit == false
                                      select eWallTr).ToList();
                        var CreditValue = Credit.Sum(x => x.Amount);

                        decimal MinVal = Math.Min(DebitValue.Value, CreditValue.Value);
                        decimal MaxVal = Math.Max(DebitValue.Value, CreditValue.Value);

                        var Sum = MaxVal - MinVal;

                        if (Sum >= newpckg.PackagePrice)
                        {
                            if (newpckg != null)
                            {
                                UserPackage userpackage = new UserPackage();
                                userpackage.PackageId = newpckg.PackageId;
                                userpackage.PackageName = newpckg.PackageName;
                                userpackage.PackagePercent = newpckg.PackagePercent;
                                userpackage.PackagePrice = newpckg.PackagePrice;
                                userpackage.PackageValidity = newpckg.PackageValidity;
                                userpackage.PackageMinWithdrawalAmount = newpckg.PackageMinWithdrawalAmount;
                                userpackage.PackageMaxWithdrawalAmount = newpckg.PackageMaxWithdrawalAmount;
                                userpackage.PackagePurchaseMethod = model.PackagePurchaseMethod;
                                userpackage.UserId = userId;
                                userpackage.IsInCurrentUse = false;
                                userpackage.PurchaseDate = DateTime.Now;
                                userpackage.IsBuyFromEWallet = true;
                                userpackage.IsBuyFromBankAcnt = false;
                                userpackage.IsRequestedForBuy = true;
                                userpackage.IsApprovedForBuy = false;
                                userpackage.IsRejectedForBuy = false;

                                dc.UserPackages.Add(userpackage);
                                dc.SaveChanges();
                                ModelState.Clear();
                                model = null;
                                return Ok(new { success = true, message = "Package save successfully" });
                            }
                            else
                            {
                                return Ok(new { error = true, message = "Unsuccessfull" });
                            }
                        }
                        else
                        {
                            return Ok(new { info = true, message = "Your E-wallet balance is not enough to buy package" });
                        }
                    }
                    if (model.PackagePurchaseMethod == Common.Enum.UpgradeInvestmentPaymentSource.BankAccount)
                    {
                        if (newpckg != null)
                        {
                            UserPackage userpackage = new UserPackage();
                            userpackage.PackageId = newpckg.PackageId;
                            userpackage.PackageName = newpckg.PackageName;
                            userpackage.PackagePercent = newpckg.PackagePercent;
                            userpackage.PackagePrice = newpckg.PackagePrice;
                            userpackage.PackageValidity = newpckg.PackageValidity;
                            userpackage.PackageMinWithdrawalAmount = newpckg.PackageMinWithdrawalAmount;
                            userpackage.PackageMaxWithdrawalAmount = newpckg.PackageMaxWithdrawalAmount;
                            userpackage.PackagePurchaseMethod = model.PackagePurchaseMethod;
                            userpackage.UserId = userId;
                            userpackage.IsInCurrentUse = false;
                            userpackage.PurchaseDate = DateTime.Now;
                            userpackage.IsBuyFromEWallet = false;
                            userpackage.IsBuyFromBankAcnt = true;
                            userpackage.IsRequestedForBuy = true;
                            userpackage.IsApprovedForBuy = false;
                            userpackage.IsRejectedForBuy = false;

                            var bankSlipImage = model.BankSlipImage;
                            if (bankSlipImage != null)
                            {
                                byte[] bankSlipImageSave = bankSlipImage;
                                userpackage.BankSlipImage = bankSlipImageSave;
                            }
                            //file = Request.Files["AddBankSlipImage"];
                            //userpackage.BankSlipImage = ConvertToBytes(file);

                            dc.UserPackages.Add(userpackage);
                            dc.SaveChanges();
                            ModelState.Clear();
                            model = null;
                            return Ok(new { success = true, message = "Package save successfully" });
                        }
                        else
                        {
                            return Ok(new { error = true, message = "Unsuccessfull" });
                        }
                    }
                }
                else
                {
                    return Ok(new { error = true, message = "You are not allowed to purchase package. Please verify your profile" });
                }
            }
            return Ok(new { success = true, message = "Package save successfully" });
            //return RedirectToAction("EWalletUpgradeInvestment");
        }
    }
}

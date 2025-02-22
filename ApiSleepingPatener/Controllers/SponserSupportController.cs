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

namespace ApiSleepingPatener.Controllers
{
    public class SponserSupportController : ApiController
    {
        [HttpPost]
        [Route("inboxsponsersupport")]
        public IHttpActionResult Inbox(SentUserMessageModel sentmodel)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            //string username = Session["LogedUserFullname"].ToString();
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SentUserMessage sent_msg = new SentUserMessage();
            sent_msg.Sender = sentmodel.Sender = sentmodel.UserId;
            sent_msg.UserId = sentmodel.UserId = sentmodel.UserId;
            sent_msg.SponserId = sentmodel.SponserId;
            sent_msg.Sender_Name = sentmodel.Sender_Name;
            sent_msg.Message = sentmodel.Message;
            sent_msg.IsRead = sentmodel.IsRead = true;
            sent_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            db.SentUserMessages.Add(sent_msg);
            ReceiveUserMessage Recive_msg = new ReceiveUserMessage();
            Recive_msg.Sender = sentmodel.Sender = sentmodel.UserId;
            Recive_msg.UserId = sentmodel.UserId = sentmodel.UserId;
            Recive_msg.SponserId = sentmodel.SponserId;
            Recive_msg.Sender_Name = sentmodel.Sender_Name;
            Recive_msg.Message = sentmodel.Message;
            Recive_msg.IsRead = sentmodel.IsRead = false;
            Recive_msg.CreateDate = sentmodel.CreateDate = DateTime.Today;
            db.ReceiveUserMessages.Add(Recive_msg);
            db.SaveChanges();      
            var fcm = db.NewUserRegistrations.Where(x => x.UserId == sentmodel.SponserId).Select(x => x.fcm).FirstOrDefault();
            if (fcm != null)
            {
                WebClient client = new WebClient();
                client.DownloadString("https://sleepingpartnertesting.royalcryptoexchange.com/messageNotifyApi.php?send_notification&sname=" +
                    sentmodel.Sender_Name + "&uid=" + sentmodel.UserId + "&sid=" + sentmodel.SponserId + "&message=" + sentmodel.Message
                   + "&token=" + fcm);

            }
          
            return Ok(new { success = true, message = "messsage sent successfully" });
        }
        [HttpGet]
        [Route("getnewmessages/{userId}")]
        public IHttpActionResult GetNewMessages(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();

            List<ReciveUserMessageModel> List = new List<ReciveUserMessageModel>();
            List = db.ReceiveUserMessages.Where(a => a.IsRead == true && a.SponserId == userId)
                .Select(x => new ReciveUserMessageModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Message = x.Message,
                    SponserId = x.SponserId,
                    Sender_Name = x.Sender_Name,
                    IsRead = x.IsRead,
                    CreateDate = x.CreateDate
                }).ToList();
            return Ok(new { data = List });
        }
        [HttpGet]
        [Route("viewallreadmessage/{userId}")]
        public IHttpActionResult ViewAllReadMessage(SentUserMessageModel UMM,int userId)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<ReciveUserMessageModel> List = new List<ReciveUserMessageModel>();
            List = db.ReceiveUserMessages.OrderByDescending(id => id.Id).Where(a => a.SponserId == userId)
                .Select(x => new ReciveUserMessageModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Message = x.Message,
                    Sender_Name = x.Sender_Name,
                    SponserId = x.SponserId,
                    IsRead = x.IsRead,
                    CreateDate = x.CreateDate
                }).ToList();
            return Ok(List);
        }
        [HttpPost]
        [Route("replymessagesponsorsupport/{u_id}/{msg}/{userId}/{username}")]
        public IHttpActionResult ReplyMessage(int u_id, string msg,int userId,string username)
        {
            //var userId = Convert.ToInt32(Session["LogedUserID"].ToString());
            //string username = Session["LogedUserFullname"].ToString();
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            SentUserMessage sent_msg = new SentUserMessage();
            sent_msg.Sender = userId;
            sent_msg.UserId = userId;
            sent_msg.SponserId = u_id;
            sent_msg.Sender_Name = username;
            sent_msg.Message = msg;
            sent_msg.IsRead = true;
            sent_msg.CreateDate = DateTime.Today;
            db.SentUserMessages.Add(sent_msg);
            ReceiveUserMessage Recive_msg = new ReceiveUserMessage();
            Recive_msg.Sender = userId;
            Recive_msg.UserId = userId;
            Recive_msg.SponserId = u_id;
            Recive_msg.Sender_Name = username;
            Recive_msg.Message = msg;
            Recive_msg.IsRead = true;
            Recive_msg.CreateDate = DateTime.Today;
            db.ReceiveUserMessages.Add(Recive_msg);
            db.SaveChanges();
            var fcm = db.NewUserRegistrations.Where(x => x.UserId == u_id).Select(x => x.fcm).FirstOrDefault();
            if (fcm != null)
            {
                WebClient client = new WebClient();
                client.DownloadString("https://sleepingpartnertesting.royalcryptoexchange.com/messageNotifyApi.php?send_notification&sname=" +
                    username + "&uid=" + userId + "&sid=" + u_id + "&message=" + msg
                   + "&token=" + fcm);

            }
            return Ok(new { success = true, message = "messsage sent successfully" });
        }
        [HttpPost]
        [Route("deleteinboxmsg/{Id}")]
        public IHttpActionResult DeleteInboxMsg(int Id)
        {
            try
            {
                SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
                ReceiveUserMessage vd1 = db.ReceiveUserMessages.Where(x => x.Id == Id).FirstOrDefault<ReceiveUserMessage>();
                db.ReceiveUserMessages.Remove(vd1);
                db.SaveChanges();
                return Ok(new { success = true, message = "message delete successfully" });

            }
            catch (Exception ex)
            {
                return Ok(new { success = true, message = "unable to delete this field", ex.Message });
            }


        }
        [HttpGet]
        [Route("getsentmessagessponsorsupport/{userId}")]
        public IHttpActionResult GetSentMessages(int userId)
        {
            SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
            List<SentUserMessageModel> List = new List<SentUserMessageModel>();
            List = db.SentUserMessages.Where(a => a.UserId == userId)
                .Select(x => new SentUserMessageModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Message = x.Message,
                    Sender_Name = x.Sender_Name,
                    SponserId = x.SponserId,
                    IsRead = x.IsRead,
                    CreateDate = x.CreateDate
                }).ToList();
            return Ok(List);
            
        }
        //[HttpGet]
        //[Route("deletereadmessage/{userId}")]
        //public IHttpActionResult DeleteReadMessage(int Id)
        //{
        //    try
        //    {
        //        SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
        //        SentUserMessage d = new SentUserMessage();
        //        d.Id = Id;
        //        db.Entry(d).State = ;
        //        db.SaveChanges();
        //        return Ok(new { success = true, message = "message delete successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { success = true, message = "unable to delete this field", ex.Message });
        //    }
        //}
        [HttpPost]
        [Route("updatemessagestatus/{Id}")]
        public IHttpActionResult updatemessagestatus(int Id)
        {
            try
            {
                SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
                ReceiveUserMessage obj = db.ReceiveUserMessages.SingleOrDefault(x => x.Id == Id);

                if (obj != null)
                {
                    obj.IsRead = true;
                    db.SaveChanges();
                    return Ok(new { success = true, message = "successfully" });
                }
                else
                {
                    return Ok(new { success = true, message = "Failed" });
                }                

            }
            catch (Exception ex)
            {
                return Ok(new { success = true, message = "failed", ex.Message });
            }


        }

        [HttpPost]
        [Route("deletereadmessagesponsorsupport/{Id}")]
        public IHttpActionResult DeleteReadMessage(int Id)
        {
            try
            {
                SleepingPartnermanagementTestingEntities db = new SleepingPartnermanagementTestingEntities();
                SentUserMessage sum = db.SentUserMessages.Where(x => x.Id ==Id).FirstOrDefault();
                db.SentUserMessages.Remove(sum);
                db.SaveChanges();
                return Ok(new { success = true, message = "message delete successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = true, message = "unable to delete this field", ex.Message });
            }
        }
    }
}




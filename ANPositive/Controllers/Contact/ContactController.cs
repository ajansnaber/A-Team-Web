using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using ANPositive.Models;
using ANPositive.Models.Shared;

namespace ANPositive.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Contact";
            return View();
        }

        public JsonResult Send(ContactMessageEmail model)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Mailing/Mailing.html")))
            {
                body = reader.ReadToEnd();
            }

            var builder = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port);

            string webSiteUrl = builder.Uri.AbsoluteUri;
            string logoSrc = $"{webSiteUrl}images/Site/Logo.png";
            int logoWidth = 200;
            int logoHeight = 75;
            string webSiteTitle = "A-Team Ship Repair Technical Services";
            string messageTitle = model.emailSubject;
            string emailMessage =
                $"{model.emailMessage}<br /><br />Gönderen: {model.firstNameLastName} - {model.emailAddress}";
            int startYear = 2003;
            int endYear = DateTime.Now.Year;
            string facebookAddress = "#";
            string twitterAddress = "#";
            string instagramAddress = "#";

            body = body.Replace("{webSiteUrl}", webSiteUrl);
            body = body.Replace("{logoSrc}", logoSrc);
            body = body.Replace("{logoWidth}", logoWidth.ToString());
            body = body.Replace("{logoHeight}", logoHeight.ToString());
            body = body.Replace("{webSiteTitle}", webSiteTitle);
            body = body.Replace("{messageTitle}", messageTitle);
            body = body.Replace("{emailMessage}", emailMessage);
            body = body.Replace("{startYear}", startYear.ToString());
            body = body.Replace("{endYear}", endYear.ToString());
            body = body.Replace("{facebookAddress}", facebookAddress);
            body = body.Replace("{twitterAddress}", twitterAddress);
            body = body.Replace("{instagramAddress}", instagramAddress);

            try
            {
                var fromAddress = new MailAddress("mailing@ajansnaber.com", "Ajans Naber? Yaratıcı Çözümler");
                var toAddress = new MailAddress("evren@ajansnaber.com", webSiteTitle);

                var smtp = new SmtpClient
                {
                    Host = "smtp.yandex.ru",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, "Password")
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Sitenizden Mesajınız Var",
                    Body = body,
                    IsBodyHtml = true,
                    ReplyToList = { new MailAddress(model.emailAddress, model.firstNameLastName)}
                })
                {
                    smtp.Send(message);
                }

                Result result = new Result
                {
                    success = true,
                    title = "Mail Gönderildi",
                    message = $"Sayın {model.firstNameLastName}. Mesajınız bize ulaştı. En kısa sürede sizinle iletişime geçeceğiz."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {

                Result result = new Result
                {
                    success = false,
                    title = "Mail Gönderildi",
                    message = $"Hata oluştu: {e.ToString()}."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
    }
}

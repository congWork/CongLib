using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace CongLib.Helpers
{
    public class EmailException : Exception
    {
        public string myMessage { get; set; }
        public EmailException()
        {
            this.myMessage = "Email Error";
        }
        public EmailException(string message)
        {
            this.myMessage = message;
        }
    }
    public class EmailHelper
    {

        public static void SendEmail(string subjectMessage, string bodyMessage, string toEmail = "", bool debugMode = false, bool isWebApp = false, bool allowHtmlBody = false)
        {
            try
            {
                bool enableSsl = true;
                string fromAddress, toAddress, password, smtpHost, smtpPort, smtpSslEnableStr;

                if (isWebApp)
                {
                    fromAddress = CommonHelper.WebConfig_GetAppSetting("FromEmail");
                    toAddress = string.IsNullOrEmpty(toEmail) ? CommonHelper.WebConfig_GetAppSetting("ToEmail") : toEmail;
                    password = CommonHelper.WebConfig_GetAppSetting("FromEmailPassword");
                    smtpHost = CommonHelper.WebConfig_GetAppSetting("SmtpHost");
                    smtpPort = CommonHelper.WebConfig_GetAppSetting("SmtpPort");
                    smtpSslEnableStr = CommonHelper.WebConfig_GetAppSetting("SmtpSslEnable");
                }
                else
                {
                    fromAddress = CommonHelper.Config_GetAppSetting("FromEmail");
                    toAddress = string.IsNullOrEmpty(toEmail) ? CommonHelper.Config_GetAppSetting("ToEmail") : toEmail;
                    password = CommonHelper.Config_GetAppSetting("FromEmailPassword");
                    smtpHost = CommonHelper.Config_GetAppSetting("SmtpHost");
                    smtpPort = CommonHelper.Config_GetAppSetting("SmtpPort");
                    smtpSslEnableStr = CommonHelper.Config_GetAppSetting("SmtpSslEnable");
                }

                //message subject line and body
                string subjectLine = subjectMessage;
                string body = "";
                if (debugMode)
                {
                    body = "From: " + fromAddress + "\n";
                    body += "Email: " + toAddress + "\n";
                    body += "Message: " + bodyMessage + "\n";
                    subjectLine = "Alert(Debuging Mode) Testing 1,2,3...Please ignore this message.";
                    body += "P.S *This is a test message. IGNORE AND DO NOT REPLY.";

                }
                else
                {
                    body = bodyMessage;
                }

                // smtp settings
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpHost;
                int smtpPortNum = 25;
                if (int.TryParse(smtpPort, out smtpPortNum))
                {
                    //do nothing
                }
                smtp.Port = smtpPortNum;
                bool.TryParse(smtpSslEnableStr, out enableSsl);
                smtp.EnableSsl = enableSsl;
                if (!string.IsNullOrEmpty(password) || !string.IsNullOrWhiteSpace(password))
                {
                    smtp.Credentials = new NetworkCredential(fromAddress, password);
                }

                // send email to a adminstrator
                MailMessage msg = new MailMessage(fromAddress, toAddress);
                msg.Subject = subjectLine.Replace("\r\n", " ");
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = allowHtmlBody;
                msg.Body =allowHtmlBody ?  body : WebUtility.HtmlEncode(body);
                msg.Priority = MailPriority.High;

                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }


    }
}

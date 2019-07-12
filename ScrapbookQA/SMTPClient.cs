using System;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace ScrapbookQA
{
    class SMTPClient
    {
        string MailTo;
        string MailFrom;
        string Password;
        string SMTPHost;
        string EmailSubjectLine;
        string MsgFooter;
        UInt16 Port;

        MailMessage MailMsg;
        SmtpClient Client;

        public SMTPClient () { }

        //Attempts to send mail; returns success or failure; requires call to BuildMail first
        public bool SendMail()
        {
            bool MailSent = true;

            try
            {
                Client.Send(MailMsg);
            }
            catch
            {
                MailSent = false;
            }

            return MailSent;
        }

        //Get config settings and builds the mail message and smtp settings
        public void BuildMail(StringBuilder EmailMsg)
        {
            //Config settings
            try
            {
                //Mail message settings
                EmailSubjectLine = ConfigurationManager.AppSettings["MailSubject"];
                MsgFooter = ConfigurationManager.AppSettings["MailFooter"];

                MailMsg = new MailMessage(MailFrom, MailTo);
                MailMsg.Subject = EmailSubjectLine;
                MailMsg.Body = EmailMsg.Append(MsgFooter).ToString();

                //Mail backend settings
                MailTo = ConfigurationManager.AppSettings["MailTo"];
                MailFrom = ConfigurationManager.AppSettings["MailFrom"];
                Password = ConfigurationManager.AppSettings["Password"];
                SMTPHost = ConfigurationManager.AppSettings["SMTPHost"];
                Port = Convert.ToUInt16(ConfigurationManager.AppSettings["Port"]);

                //SMTP settings
                Client = new SmtpClient();
                Client.Port = Port;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.EnableSsl = true;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new System.Net.NetworkCredential(MailFrom, Password);
                Client.Host = SMTPHost;
            }
            catch (ConfigurationErrorsException e)
            {
                throw new ConfigurationErrorsException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

using System;
using System.Net.Mail;

namespace Web.Utilities.AppCode
{

    public static class MailSender
    {
        // settings
        private static string host { get { return "webmail.pha.co.ir"; } }
        private static string from { get { return "noreply@pha.co.ir"; } }
        private static string password { get { return "D?.@!E#L$_h&X%Q"; } }
        private static bool useSsl { get { return true; } }

                
        public static bool Send(string title, string to, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from, title, System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = true;

            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            if (useSsl)
            {
                smtp.Port = 587;
            }
            else
            {
                smtp.Port = 25;
            }
            smtp.Host = host;
            smtp.EnableSsl = useSsl;     // for yahoo set false                                           

            try
            {

                smtp.Send(mail);

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }              

        // with cc and Bcc
        public static bool Send(string title, string to, string[] cc,string[] bcc, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            
            // add cc
            if (cc.Length > 0)
                for (int i = 0; i < cc.Length; i++)
                    mail.CC.Add(cc[i]);

            // add bcc
            if (bcc.Length > 0)
                for (int i = 0; i < bcc.Length; i++)
                    mail.Bcc.Add(cc[i]);


            mail.From = new MailAddress(from, title, System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = true;

            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            if (useSsl)
            {
                smtp.Port = 587;
            }
            else
            {
                smtp.Port = 25;
            }
            smtp.Host = host;
            smtp.EnableSsl = useSsl;     // for yahoo set false                                           

            try
            {
                smtp.Send(mail);

                return true;
            }
            catch
            {
                return false;
            }

        }

        // with cc and Bcc and attachment
        public static bool Send(string title, string to, string[] cc, string[] bcc,string[] attachmentsPhysicalAddress, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(to);

            // add cc
            for (int i = 0; i < cc.Length; i++)
            {
                mail.CC.Add(cc[i]);
            }

            // add bcc
            for (int i = 0; i < bcc.Length; i++)
            {
                mail.Bcc.Add(bcc[i]);
            }

            mail.From = new MailAddress(from, title, System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.IsBodyHtml = true;

            // add attachment
            for (int i = 0; i < attachmentsPhysicalAddress.Length; i++)
            {
                mail.Attachments.Add(new Attachment(attachmentsPhysicalAddress[i]));
            }

            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(from, password);
            if (useSsl)
            {
                smtp.Port = 587;
            }
            else
            {
                smtp.Port = 25;
            }
            smtp.Host = host;
            smtp.EnableSsl = useSsl;     // for yahoo set false                                           

            try
            {
                smtp.Send(mail);

                return true;
            }
            catch
            {
                return false;
            }

        }
        
    }

}
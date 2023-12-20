using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace URF.Core.Helper.Helpers
{
    public class EmailHelper
    {
        public static bool SendEmail(EmailEntity entity)
        {
            var smtpAccount = entity.SmtpAccount;
            foreach (var email in entity.Contacts)
            {
                MailMessage mailMessage = null;
                try
                {
                    mailMessage = new MailMessage
                    {
                        IsBodyHtml = true,
                        Body = entity.Content,
                        Subject = entity.Subject,
                        Priority = MailPriority.High,
                        BodyEncoding = Encoding.UTF8,
                        HeadersEncoding = Encoding.UTF8,
                        From = new MailAddress(smtpAccount.Username, smtpAccount.EmailFrom),
                    };
                    mailMessage.To.Add(email);
                    foreach (var attachment in entity.Attachments)
                    {
                        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(attachment.File))
                        {
                            Position = 0
                        };
                        ContentType contentType = new ContentType
                        {
                            Name = attachment.Name,
                            MediaType = "application/octet-stream",
                        };
                        Attachment attachment1 = new Attachment(memoryStream, contentType);
                        attachment1.ContentDisposition.FileName = attachment.Name;
                        mailMessage.Attachments.Add(attachment1);
                    }
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                    SmtpClient smtpClient = new SmtpClient
                    {
                        Host = smtpAccount.Host,
                        UseDefaultCredentials = true,
                        Port = Convert.ToInt32(smtpAccount.Port),
                        EnableSsl = Convert.ToBoolean(smtpAccount.EnableSsl),
                        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(smtpAccount.Username, smtpAccount.Password),
                    };
                    smtpClient.Send(mailMessage);
                    smtpClient.Dispose();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class EmailEntity
    {
        public EmailEntity()
        {
            Contacts = new List<string>();
            Attachments = new List<AttachmentEntity>();
        }

        public string Subject { get; set; }

        public string Content { get; set; }

        public List<string> Contacts { get; set; }

        public SmtpAccountEntity SmtpAccount { get; set; }

        public List<AttachmentEntity> Attachments { get; set; }
    }

    public class AttachementInfo
    {
        Stream stream;
        string mimeType;
        string contentId;

        public AttachementInfo(Stream stream, string mimeType, string contentId)
        {
            this.stream = stream;
            this.mimeType = mimeType;
            this.contentId = contentId;
        }

        public Stream Stream { get { return stream; } }
        public string MimeType { get { return mimeType; } }
        public string ContentId { get { return contentId; } }
    }

    public class AttachmentEntity
    {
        public string Name { get; set; }

        public string File { get; set; }
    }

    public class SmtpAccountEntity
    {
        public int? Port { get; set; }
        public string Host { get; set; }
        public bool? EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailFrom { get; set; }
    }
}

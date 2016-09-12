using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Contact
{
    using Core.Domain;

    public class ContactService : IContactService
    {
        private readonly IRepository<User> _useRepository;

        public ContactService(IRepository<User> useRepository )
        {
            _useRepository = useRepository;
        }

        public IEnumerable<User> GetStaffProfiles()
        {
            var query = _useRepository.Table;
            var users = query.Where(a => (bool)a.IsAdmin).ToList();
            return users;
        }

        public bool SendToAdmin(Email email)
        {
            var fromAddress = new MailAddress("rebdev20@gmail.com", "PremiumStress Users");
            var toAddress = new MailAddress("rebdev20@gmail.com", "PremiumStress Users");
            const string fromPassword = "6408172619";
            const string subject = "Customer's opinion";

            var bodyOfEmail = new StringBuilder();

            bodyOfEmail.AppendFormat("Name: {0}{1}", email.Name, Environment.NewLine);
            bodyOfEmail.AppendFormat("Email: {0}{1}{2}", email.EmailAddress, Environment.NewLine, Environment.NewLine);
            bodyOfEmail.AppendFormat("Message: {0}", email.Body);

            var body = bodyOfEmail.ToString();

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            return true;
        }
    }
}

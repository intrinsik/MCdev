using System;
using System.Net.Mail;

namespace EmailForm.Services
{
	public class MailService : IMailService
	{
		public bool SendMail(string from, string to, string subject, string body)
		{
			try
			{
				var msg = new MailMessage(from, to, subject, body);

				var client = new SmtpClient();
				client.Send(msg);
			}
			catch(Exception ex)
			{
				//Add logging
				return false;
			}
			return true;
		}
	}
}
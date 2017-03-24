using System;
namespace EmailForm.Services
{
	public interface IMailService
	{
		bool SendMail(string from, string to, string subject, string body);
	}
}
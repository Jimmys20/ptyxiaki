using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using ptyxiaki.Data;
using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Services
{
  public interface IEmailService
  {
    Task sendEmailAsync(IEnumerable<EmailAddress> addresses, string subject, string text);
  }

  public class EmailService : IEmailService
  {
    private readonly EmailServiceOptions options;

    public EmailService(IOptions<EmailServiceOptions> options)
    {
      this.options = options.Value;
    }

    public async Task sendEmailAsync(IEnumerable<EmailAddress> addresses, string subject, string text)
    {
      if (addresses == null || !addresses.Any())
        return;

      var message = new MimeMessage
      {
        Subject = subject,
        Body = new TextPart("html")
        {
          Text = text
        }
      };

      var sender = new MailboxAddress(options.senderName, options.senderAddress);

      var recipients = addresses.Select(a => new MailboxAddress(a.name, a.address)).ToList();

      using (var client = new SmtpClient())
      {
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(options.host, options.port, options.useSsl);


        if (client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
        {
          try
          {
            await client.AuthenticateAsync(options.userName, options.password);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }

        await client.SendAsync(message, sender, recipients);

        await client.DisconnectAsync(true);
      }
    }
  }

  public class EmailServiceOptions
  {
    public string host { get; set; }
    public int port { get; set; }
    public bool useSsl { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public string senderName { get; set; }
    public string senderAddress { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using JetBrains.Annotations;
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using MimeKit;
using PH.Core3.AspNetCoreApi.Services;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Test.WebApp.HostedService
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(string to);
    }

    public class MailSenderService : IDisposable , IHostedService , IMailSenderService
    {

        private readonly BufferBlock<MimeMessage> _messages;
        private MailKit.Net.Smtp.SmtpClient _smtpClient;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IViewRenderService _viewRenderService;

        private Task _sendTask;

        /// <summary>
        /// Initialize a new instance of <see cref="CoreDisposable"/>
        /// </summary>
        public MailSenderService(IViewRenderService viewRenderService) 

        {
            _viewRenderService = viewRenderService;
            _messages = new BufferBlock<MimeMessage>();
          
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected void Dispose(bool disposing)
        {
            _smtpClient?.Dispose();
            _cancellationTokenSource?.Dispose();
            _sendTask?.Dispose();
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        [NotNull]
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            //The StartAsync method just needs to start a background task (or a timer)
            _sendTask = Send(_cancellationTokenSource.Token);
            return _sendTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                //Let's cancel the e-mail delivery
                CancelSendTask();

                if(null != _smtpClient && _smtpClient.IsConnected)
                {
                    await _smtpClient.DisconnectAsync(true);
                }


                //Next, we wait for sendTask to end, but no longer than what the web host allows
                if(null != _sendTask)
                {
                    await Task.WhenAny(_sendTask, Task.Delay(Timeout.Infinite, cancellationToken));
                }
            }
            catch 
            {
                //
                if(null != _smtpClient && _smtpClient.IsConnected)
                {
                    await _smtpClient.DisconnectAsync(true);
                }
            }
        }
        
        private void CancelSendTask()
        {
            try
            {
                if (_cancellationTokenSource != null)
                {
                    
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = null;
                }
            }
            catch
            {
            }
        }
        public async Task Send(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                MimeMessage message = null;

                try
                {
                    message = await _messages.ReceiveAsync(token);
                    await PerformSmtpSend(message);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    //We need to terminate the delivery, so we'll just break the while loop
                    break;
                }
            }
        }

        






        public async Task SendAsync(MimeMessage mail)
        {
            await _messages.SendAsync(mail);
        }
         private async Task PerformSmtpSend(MimeMessage m)
        {

            try
            {
                if(null == _smtpClient)
                {
                    _smtpClient = new MailKit.Net.Smtp.SmtpClient();
                }


                if (!_smtpClient.IsConnected)
                {
                    try
                    {
                        //var opt = SecureSocketOptions.Auto;

                        //if (_settings.SmtpConfig.EnableSsl)
                        //    opt = SecureSocketOptions.SslOnConnect;

                        //if (_settings.SmtpConfig.StartTls)
                        //    opt = SecureSocketOptions.StartTls;

                        /*
                         *
                         * "SmtpConfig": {
      "DefaultSender": "noreply@estrobit.com",
      "Host": "smtp.gmail.com",
      "Port": 587,
      "EnableSsl": true,
      "StartTls":  true, 
      "UseDefaultCredentials": false,
      "SmtpConfigCredentials": {
        "Username": "noreply@estrobit.com",
        "Password": "EstrobitSPA2016!"
      }

                         */


                        await _smtpClient.ConnectAsync("smtp.gmail.com", 587,SecureSocketOptions.StartTls);

                        await _smtpClient.AuthenticateAsync("noreply@estrobit.com","EstrobitSPA2016!");
                    }
                    catch 
                    {

                        throw;
                    }
                
                }

                //if (!string.IsNullOrEmpty(_settings.SmtpConfig.SmtpConfigCredentials.Username) &&
                //    !string.IsNullOrEmpty(_settings.SmtpConfig.SmtpConfigCredentials.Password))
                //{
                //    if (!_smtpClient.IsAuthenticated)
                //    {
                //        try
                //        {
                //            await _smtpClient.AuthenticateAsync(_settings.SmtpConfig.SmtpConfigCredentials.Username,
                //                                                _settings.SmtpConfig.SmtpConfigCredentials.Password);
                //        }
                //        catch (Exception e)
                //        {
                //            _logger.Fatal(e,$"Error Authenticating to SMTP ");
                //            throw;
                //        }
                    
                //    }
                //}


                //var mailId = m.Headers.FirstOrDefault(x => x.Field == EmailContants.XEmailId);
                
                await _smtpClient.SendAsync(m);

                //_logger.Debug($"Sent Email with {EmailContants.XEmailId}: '{mailId?.Value}'");
            }
            catch 
            {
                //_logger.Fatal(e, $"Unable to send email");
            }

           

        }


        public async Task SendEmailAsync(string to)
        {
            
            var title = $"Test Email ";

            var emailBody = await  _viewRenderService.RenderToStringAsync("~/Views/Email/MyMail.cshtml", null);

            MimeMessage m = new MimeMessage();
                    
            var builder = new BodyBuilder {HtmlBody = emailBody};


            m.Subject = title;


            m.Body = builder.ToMessageBody();

            var sender = new MailboxAddress("noreply@estrobit.com");
            m.From.Add(sender);
            m.To.Add(new MailboxAddress("paolo.innocenti@estrobit.com"));
            m.Bcc.Add(new MailboxAddress("paolo.innocenti78@gmail.com"));

            await SendAsync(m);

        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

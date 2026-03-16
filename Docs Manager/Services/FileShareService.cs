using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Docs_Manager.Services
{
    public class FileShareService
    {
        public enum ShareMethod
        {
            Email,
            WhatsApp,
            Telegram,
            LocalShare
        }

        public async Task<bool> ShareFileAsync(string filePath, ShareMethod method)
        {
            try
            {
                switch (method)
                {
                    case ShareMethod.Email:
                        return await ShareViaEmailAsync(filePath);
                    case ShareMethod.WhatsApp:
                        return await ShareViaWhatsAppAsync(filePath);
                    case ShareMethod.Telegram:
                        return await ShareViaTelegramAsync(filePath);
                    case ShareMethod.LocalShare:
                        return await ShareLocalAsync(filePath);
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Share error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ShareViaEmailAsync(string filePath)
        {
            try
            {
                await Email.ComposeAsync(new EmailMessage
                {
                    Subject = "File Share",
                    Body = "Please find the attached file",
                    Attachments = new List<EmailAttachment>
                    {
                        new EmailAttachment(filePath)
                    }
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> ShareViaWhatsAppAsync(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                await Launcher.OpenAsync(new Uri($"https://wa.me/?text=Check%20this%20file:%20{fileName}"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> ShareViaTelegramAsync(string filePath)
        {
            try
            {
                await Launcher.OpenAsync(new Uri("https://t.me/"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> ShareLocalAsync(string filePath)
        {
            try
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share File",
                    File = new ShareFile(filePath)
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
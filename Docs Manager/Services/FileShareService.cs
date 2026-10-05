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

        public async Task<bool> EmailFilesAsync(IEnumerable<string> filePaths)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Documents",
                    Body = "Please find the attached files"
                };
                foreach (var path in filePaths)
                    message.Attachments.Add(new EmailAttachment(path));

                await Email.ComposeAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ShareFilesAsync(IEnumerable<string> filePaths, string title)
        {
            try
            {
                await Share.RequestAsync(new ShareMultipleFilesRequest
                {
                    Title = title,
                    Files = filePaths.Select(p => new ShareFile(p)).ToList()
                });
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Share error: {ex.Message}");
                return false;
            }
        }

        public Task<string?> CreateArchiveAsync(IEnumerable<string> filePaths)
        {
            return Task.Run<string?>(() =>
            {
                try
                {
                    var archivePath = Path.Combine(FileSystem.CacheDirectory, $"Documents_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                    using var archive = System.IO.Compression.ZipFile.Open(archivePath, System.IO.Compression.ZipArchiveMode.Create);
                    var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var path in filePaths)
                    {
                        var name = Path.GetFileName(path);
                        var baseName = Path.GetFileNameWithoutExtension(name);
                        var ext = Path.GetExtension(name);
                        var counter = 1;
                        while (!usedNames.Add(name))
                            name = $"{baseName}_{counter++}{ext}";

                        archive.CreateEntryFromFile(path, name);
                    }
                    return archivePath;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Archive error: {ex.Message}");
                    return null;
                }
            });
        }
    }
}
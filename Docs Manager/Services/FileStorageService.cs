using Docs_Manager.Models;
using Docs_Manager.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Docs_Manager.Services
{
    public class FileStorageService
    {
        private readonly DatabaseService _database;
        private readonly string _filesDirectory;

        public FileStorageService(DatabaseService database)
        {
            _database = database;
            _filesDirectory = Path.Combine(FileSystem.AppDataDirectory, "Files");

            if (!Directory.Exists(_filesDirectory))
                Directory.CreateDirectory(_filesDirectory);
        }

        public async Task<StoredFile?> UploadFileAsync(string sourceFilePath)
        {
            try
            {
                var fileName = Path.GetFileName(sourceFilePath);
                var destPath = Path.Combine(_filesDirectory, fileName);

                if (File.Exists(destPath))
                {
                    var baseName = Path.GetFileNameWithoutExtension(fileName);
                    var extension = Path.GetExtension(fileName);
                    var counter = 1;

                    while (File.Exists(destPath))
                    {
                        fileName = $"{baseName}_{counter}{extension}";
                        destPath = Path.Combine(_filesDirectory, fileName);
                        counter++;
                    }
                }

                File.Copy(sourceFilePath, destPath, true);

                var fileInfo = new FileInfo(destPath);
                var storedFile = new StoredFile
                {
                    FileName = fileName,
                    FilePath = destPath,
                    FileSizeBytes = fileInfo.Length,
                    UploadDate = DateTime.Now,
                    FileType = Path.GetExtension(fileName).ToLower().TrimStart('.')
                };

                await _database.SaveStoredFileAsync(storedFile);
                return storedFile;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Upload error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<StoredFile>> GetAllFilesAsync()
        {
            return await _database.GetStoredFilesAsync();
        }

        public async Task<bool> DeleteFileAsync(StoredFile file)
        {
            try
            {
                if (File.Exists(file.FilePath))
                    File.Delete(file.FilePath);

                await _database.DeleteStoredFileAsync(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<StoredFile?> GetFileAsync(int fileId)
        {
            return await _database.GetStoredFileAsync(fileId);
        }
    }
}
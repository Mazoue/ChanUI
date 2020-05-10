using System;
using System.Threading.Tasks;

namespace Framework.Interfaces.Services
{
    public interface IImageService
    {
        string GenerateFilePath(string baseFolder, string boardName, string threadName, string fileName,
            string fileExtension);
        Task DownloadFile(string fileUrl, string destination);
        bool FileNameInUseExists(string path);
        bool FolderExists(string path);
        int CalculateFileSizeInKiloBytes(int bytes);
    }
}

using System;

namespace Framework.Datamodels.Downloads
{
    public class Download
    {
        public Uri DownloadUri { get; set; }
        public int PostNumber { get; set; }
        public string PostName { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
    }
}

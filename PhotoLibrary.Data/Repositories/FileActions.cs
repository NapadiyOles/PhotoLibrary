using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PhotoLibrary.Data.Repositories
{
    public abstract class FileActions
    {
        private readonly string _directoryPath;

        protected FileActions()
        {
            var fullPath = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var projectPath = new StringBuilder();

            foreach (var folder in fullPath)
            {
                projectPath.Append($@"{folder}\");
                if(folder == "PhotoLibrary") break;
            }

            projectPath.Append($@"PhotoLibrary.Data\Pictures");

            _directoryPath = projectPath.ToString();

            if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);
        }
        protected void SaveImage(Image image, string uniqueId) => 
            image.Save($@"{_directoryPath}\{uniqueId}.jpg", ImageFormat.Jpeg);

        protected Image LoadImage(string uniqueId) => new Bitmap($@"{_directoryPath}\{uniqueId}.jpg");

        protected void DeleteImage(string uniqueId) => File.Delete($@"{_directoryPath}\{uniqueId}.jpg");
    }
}
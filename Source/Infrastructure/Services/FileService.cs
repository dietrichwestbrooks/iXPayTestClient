using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    [Export(typeof(IFileService))]
    public class FileService : IFileService
    {
        public string OpenScriptFileDialog()
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Open Script File",
                    Multiselect = false,
                    Filter = "Script files (*.py)|*.py",
                    InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Scripts")
                };

            if (openFileDialog.ShowDialog() == true)
                filePath = openFileDialog.FileNames.FirstOrDefault() ?? string.Empty;

            return filePath;
        }

        public void SaveTextFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public string OpenTextFile(string filePath, bool create = false)
        {
            if (!File.Exists(filePath))
            {
                if (!create)
                    throw new FileNotFoundException(filePath);

                SaveTextFile(filePath, string.Empty);
            }

            return File.ReadAllText(filePath);
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public string SaveScriptFileDialog(string filePath = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Script files (*.py)|*.py",
                    Title = "Save Script File",
                    InitialDirectory =
                        (string.IsNullOrWhiteSpace(filePath)
                            ? Directory.GetCurrentDirectory()
                            : Path.GetDirectoryName(filePath)),
                    FileName = (string.IsNullOrWhiteSpace(filePath) ? string.Empty : Path.GetFileName(filePath))
                };

            var returnVal = saveFileDialog.ShowDialog();

            if (!returnVal.HasValue || !returnVal.Value)
                return null;

            return saveFileDialog.FileName;
        }
    }
}

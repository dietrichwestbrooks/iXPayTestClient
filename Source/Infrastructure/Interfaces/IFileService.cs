using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface IFileService
    {
        string OpenScriptFileDialog();

        void SaveTextFile(string filePath, string text);

        string OpenTextFile(string filePath, bool create = false);

        void DeleteFile(string filePath);

        string SaveScriptFileDialog(string filePath = null);
    }
}

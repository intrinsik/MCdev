using System;
using System.IO;
using System.Configuration;
using System.IO.Compression;
using System.Text;

namespace ZipInOut
{
    static class Program
    {
        static void Main(string[] args)
        {
            var _reader = new AppSettingsReader();
            var extractPath = _reader.GetValue("ExtractPath", typeof(string)).ToString();
            var zipped = _reader.GetValue("Zipped", typeof(string)).ToString();

            if (Directory.Exists(extractPath) && File.Exists(zipped))
            {
                using (var archive = ZipFile.OpenRead(zipped))
                {
                    foreach (var entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName), true);
                    }
                }
            }
            else
                using (var fs = File.Create(extractPath + "error.log"))
                {
                    var info = new UTF8Encoding(true).GetBytes("There was an error because the file and/or file path does not exist");
                    fs.Write(info,0,info.Length);
                }

            using (var sr = File.OpenText(extractPath + "error.log"))
            {
                var s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
}

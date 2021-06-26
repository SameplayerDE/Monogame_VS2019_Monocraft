using Monocraft_Protocol_NET.Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Monocraft_Core_NET
{
    class Program
    {

        public static void Main(string[] args)
        {

            Dictionary<string, string> models = new Dictionary<string, string>();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $".monocraft/assets.zip");
            if (File.Exists(path))
            {

                using (var file = File.OpenRead(path))
                {
                    using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            using (var stream = entry.Open())
                            {

                                string filepath = Path.Combine(path, entry.FullName);

                                string extension;

                                extension = Path.GetExtension(filepath);
                                if (extension == ".json" && filepath.Contains("assets/minecraft/models/block"))
                                {
                                    using (StreamReader sr = new StreamReader(stream))
                                    {
                                        StringBuilder builder = new StringBuilder();
                                        while (sr.Peek() >= 0)
                                        {
                                            builder.Append(sr.ReadLine());
                                        }
                                        models.Add(entry.Name.Replace(".json", ""), builder.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, string> keyValuePair in models)
            {
                //Console.WriteLine($"{keyValuePair.Key} : {0}");
            }
            Console.ReadKey();
        }


    }
}

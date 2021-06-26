using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Protocol_NET.Data.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Monocraft_Client_NET
{
    public class TextureManager
    {

        public bool Loaded = false;
        public Dictionary<string, Texture2D> Textures { get; private set; } = new Dictionary<string, Texture2D>();

        public void LoadContent(ContentManager Content)
        {

            Textures = LoadZipAssets();
            Textures.Add("Not_Found", Content.Load<Texture2D>("bin/Textures/not_found"));

            Loaded = true;
        }

        public Dictionary<string, Texture2D> LoadAssets()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            
            foreach (Material material in Enum.GetValues(typeof(Material)))
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $".monocraft/blocks/{material.ToString().ToLower()}.png");
                if (!File.Exists(path))
                {
                    //Console.WriteLine("Not Found");
                    continue;
                }
                FileStream fileStream = new FileStream(path, FileMode.Open);
                Texture2D texture = Texture2D.FromStream(MonocraftGame.Instance.GraphicsDevice, fileStream);
                textures.Add($"{material}", texture);
                fileStream.Dispose();
            }


            return textures;
        }

        public Dictionary<string, Texture2D> LoadZipAssets()
        {
            
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

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
                                if (extension == ".png" && filepath.Contains("assets/minecraft/textures/"))
                                {
                                    Texture2D texture = Texture2D.FromStream(MonocraftGame.Instance.GraphicsDevice, stream);
                                    textures.Add($"{filepath.Replace(path, "").Replace("\\assets/minecraft/textures/", "").Replace(".png", "")}", texture);
                                    stream.Dispose();
                                }
                            }
                        }
                    }
                }
            }

            return textures;

        }

    }
}

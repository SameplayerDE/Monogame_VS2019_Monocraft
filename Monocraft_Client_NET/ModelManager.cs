using Microsoft.Xna.Framework.Content;
using Monocraft_Core_NET;
using Monocraft_Core_NET.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Monocraft_Client_NET
{
    public class ModelManager
    {
        public bool Loaded = false;
        public Dictionary<string, ResourceModel> Models { get; private set; } = new Dictionary<string, ResourceModel>();

        public void LoadContent(ContentManager Content)
        {
            Models = LoadZipAssets();
            Loaded = true;
        }

        public ResourceModel GetModel(string name)
        {
            Dictionary<string, ResourceModel> copy = new Dictionary<string, ResourceModel>(Models);
            if (copy.ContainsKey(name))
            {
                ResourceModel model = (ResourceModel)copy[name].Clone();
                if (string.IsNullOrEmpty(model.Parent))
                {
                    return model;
                }
                else
                {
                    if (model.Textures != null)
                    {
                        return GetModel(model.Parent.Replace("minecraft:", "").Replace("block/", ""), model.Textures);
                    }
                    else
                    {
                        return GetModel(model.Parent.Replace("minecraft:", "").Replace("block/", ""));
                    }
                }
            }
            return null;
        }

        public ResourceModel GetModel(string name, Dictionary<string, string> textures)
        {
            Dictionary<string, ResourceModel> copy = new Dictionary<string, ResourceModel>(Models);
            if (copy.ContainsKey(name))
            {
                ResourceModel model = (ResourceModel)copy[name].Clone();
                if (string.IsNullOrEmpty(model.Parent))
                {
                    model.Textures = textures;
                    return model;
                }
                else
                {

                    if (model.Textures != null)
                    {
                        Dictionary<string, string> combinedTextures = new Dictionary<string, string>();
                        foreach(KeyValuePair<string, string> parentTextures in model.Textures)
                        {
                            foreach (KeyValuePair<string, string> childTextures in textures)
                            {
                                if (parentTextures.Value.Replace("#", "") == childTextures.Key)
                                {
                                    combinedTextures.Add(parentTextures.Key, childTextures.Value);
                                }
                            }
                        }
                        textures = combinedTextures;
                    }

                    string parent = model.Parent.Replace("minecraft:", "").Replace("block/", "");
                    if (parent == "block")
                    {
                        model.Textures = textures;
                        return model;
                    }

                    return GetModel(parent, textures);
                }
            }
            return null;
        }

        public Dictionary<string, ResourceModel> LoadZipAssets()
        {
            int count = 0;
            Dictionary<string, ResourceModel> models = new Dictionary<string, ResourceModel>();

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
                                if (extension == ".json" && filepath.Contains("assets/minecraft/models/block/")) {
                                    
                                    using (StreamReader sr = new StreamReader(stream))
                                    {
                                        StringBuilder builder = new StringBuilder();
                                        while (sr.Peek() >= 0)
                                        {
                                            builder.Append(sr.ReadLine());
                                        }
                                        models.Add(entry.Name.Replace(".json", ""), JsonConvert.DeserializeObject<ResourceModel>(builder.ToString()));
                                        //Console.WriteLine($"{++count}");
                                        if (filepath.Contains("tnt"))
                                        {
                                            Console.WriteLine(builder.ToString());
                                            Console.WriteLine(models["tnt"].Parent);
                                        }
                                    }
                                    stream.Dispose();
                                }
                            }
                        }
                    }
                }
            }

            return models;

        }
    }
}

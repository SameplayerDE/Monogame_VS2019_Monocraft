using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.Model
{
    public class Face
    {
        [JsonProperty(PropertyName = "uv")]
        public float[] UV;

        [JsonProperty(PropertyName = "texture")]
        public string Texture;
    }
}

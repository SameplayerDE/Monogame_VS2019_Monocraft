using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.Model
{
    public class ResourceModel : ICloneable
    {
        [JsonProperty(PropertyName = "parent")]
        public string Parent;

        [JsonProperty(PropertyName = "elements")]
        public Element[] Elements;

        [JsonProperty(PropertyName = "textures")]
        public Dictionary<string, string> Textures;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

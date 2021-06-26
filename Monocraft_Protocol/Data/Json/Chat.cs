using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Data.Json
{
    public class Chat
    {
        [JsonProperty(PropertyName = "bold")]
        public bool Bold;
        [JsonProperty(PropertyName = "italic")]
        public bool Italic;
        [JsonProperty(PropertyName = "underlined")]
        public bool Underlined;
        [JsonProperty(PropertyName = "strikethrough")]
        public bool Strikethrough;
        [JsonProperty(PropertyName = "obfuscated")]
        public bool Obfuscated;
        [JsonProperty(PropertyName = "text")]
        public string Text;
        [JsonProperty(PropertyName = "extra")]
        public Chat[] Extra;
    }
}

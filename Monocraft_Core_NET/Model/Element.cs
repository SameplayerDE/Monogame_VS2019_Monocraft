using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocraft_Core_NET.Model
{
    public class Element
    {
        [JsonProperty(PropertyName = "from")]
        public float[] From;

        [JsonProperty(PropertyName = "to")]
        public float[] To;

        [JsonProperty(PropertyName = "faces")]
        public Dictionary<string, Face> Faces;

        public ResourceModel Parent;
    }
}

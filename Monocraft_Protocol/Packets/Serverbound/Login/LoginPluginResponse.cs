using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Login
{
    public class LoginPluginResponse : ServerboundPacket
    {
        public LoginPluginResponse() : base(0x02) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            return writer.Data;
        }
    }
}

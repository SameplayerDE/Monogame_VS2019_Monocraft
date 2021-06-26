using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Login
{
    public class LoginSuccess : ClientboundPacket
    {

        public readonly Guid UUID;
        public readonly string Username;

        public LoginSuccess() : base(0x02) { }
        public LoginSuccess(DataReader reader) : base(0x02) 
        {
            UUID = reader.ReadUUID();
            Username = reader.ReadString();
        }
        public LoginSuccess(byte[] data) : base(0x02) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            return writer.Data;
        }
    }
}

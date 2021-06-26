using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Play
{
    public class TimeUpdate : ClientboundPacket
    {

        public readonly long WorldAge;
        public readonly long TimeOfDay;

        public TimeUpdate() : base(0x4E) { }
        public TimeUpdate(DataReader reader) : base(0x4E) 
        {
            WorldAge = reader.ReadLong();
            TimeOfDay = reader.ReadLong();
        }
        public TimeUpdate(byte[] data) : base(0x4E) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            throw new NotImplementedException();
        }
    }
}

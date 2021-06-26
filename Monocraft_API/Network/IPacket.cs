using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_API.Network
{
    public interface IPacket
    {
        byte ID { get; }
        void ReadPacket(IMinecraftStream stream);
        void WritePacket(IMinecraftStream stream);
    }
}

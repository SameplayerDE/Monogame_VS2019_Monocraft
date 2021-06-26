namespace Monocraft_Protocol_NET.Packets.Clientbound
{
    public abstract class ClientboundPacket : Packet
    {
        public ClientboundPacket(byte packetID) : base(packetID) { }
    }
}
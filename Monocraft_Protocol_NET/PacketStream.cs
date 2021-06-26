using fNbt;
using Monocraft_Protocol_NET.Packets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET
{
    public class PacketStream : Stream
    {

        public Stream Stream { get; set; }
        public CancellationTokenSource CancelationToken { get; private set; }

        public PacketStream() : this(new MemoryStream()) { }

        public PacketStream(Stream stream)
        {
            Stream = stream;
            CancelationToken = new CancellationTokenSource();
        }

        public override bool CanRead => Stream.CanRead;

        public override bool CanSeek => Stream.CanSeek;

        public override bool CanWrite => Stream.CanWrite;

        public override long Length => Stream.Length;

        public override long Position { get => Stream.Position; set => Stream.Position = value; }

        public override void Flush()
        {
            Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            /*for (int i = 0; i < count; i++)
            {
                while (!Stream.DataAvailable)
                {
                    Thread.Sleep(10);
                }
                Stream.Read(buffer, offset + i, 1);
            }
            return 0;*/
            return Stream.Read(buffer, offset, count);
        }

        public long ReadVarLong()
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                read = (byte)ReadByte();
                int value = (read & 0x7f);
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 10)
                {
                    throw new Exception("VarLong is too big");
                }
            } while ((read & 0x80) != 0);

            return result;
        }

        public byte[] Read(int count)
        {
            if (Stream is MemoryStream)
            {
                byte[] data = new byte[count];
                Read(data, 0, count);
                return data;
            }

            int read = 0;

            byte[] buffer = new byte[count];
            while (read < buffer.Length && !CancelationToken.IsCancellationRequested)
            {
                int readBytes = Read(buffer, read, count - read);
                if (readBytes < 0) //No data read?
                {
                    break;
                }

                read += readBytes;

                if (CancelationToken.IsCancellationRequested)
                {
                    throw new ObjectDisposedException("");
                }
            }

            return buffer;
        }

        public sbyte ReadSignedByte()
        {
            sbyte value = (sbyte)Stream.ReadByte();
            return value;
        }


        public void WriteFloat(float data)
        {
            Write(HostToNetworkOrder(data));
        }

        public byte ReadUnsignedByte()
        {
            byte value = (byte)Stream.ReadByte();
            return value;
        }

        public int ReadInt()
        {
            byte[] data = new byte[4]; // 4 bytes because 32 bit value
            Read(data, 0, data.Length);
            int value = BitConverter.ToInt32(data, 0);
            return IPAddress.NetworkToHostOrder(value);
        }

        public long ReadLong()
        {
            byte[] data = new byte[8]; // 8 bytes because 64 bit value
            Read(data, 0, data.Length);
            long value = BitConverter.ToInt64(data, 0);
            return IPAddress.NetworkToHostOrder(value);
        }

        public string ReadString()
        {
            int length = ReadVarInt();
            byte[] chars = new byte[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = ReadUnsignedByte();
            }
            return Encoding.Default.GetString(chars);
        }

        public Guid ReadUUID()
        {
            int length = 16;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(Read(1)[0].ToString("X2"));
            }
            return new Guid(builder.ToString());
        }

        public int[] ReadPosition()
        {

            long var = ReadLong();

            long x = var >> 38;
            long y = var & 0xfff;
            long z = var << 26 >> 38;

            if (x >= Math.Pow(2, 25)) { x -= (long)Math.Pow(2, 26); }
            if (y >= Math.Pow(2, 11)) { y -= (long)Math.Pow(2, 12); }
            if (z >= Math.Pow(2, 25)) { z -= (long)Math.Pow(2, 26); }

            //Console.WriteLine($"x: {Convert.ToInt32(x)}/ y: {Convert.ToInt32(y)} / z: {Convert.ToInt32(z)}");
            return new int[] { Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(z) };
        }

        public short ReadSignedShort()
        {
            var da = Read(2);
            var d = BitConverter.ToInt16(da, 0);
            return IPAddress.NetworkToHostOrder(d);
        }

        public ushort ReadUnsignedShort()
        {
            var da = Read(2);
            return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
        }

        public double ReadDouble()
        {
            return NetworkToHostOrder(ReadUnsignedBytes(8));
        }

        public void WriteVarInt(int value)
        {
            do
            {
                byte temp = (byte)(value & 0b01111111);
                value >>= 7;
                if (value != 0)
                {
                    temp |= 0b10000000;
                }
                WriteByte(temp);
            } while (value != 0);
        }

        public void WriteString(string value)
        {
            WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                char @char = value[i];
                WriteByte(Convert.ToByte(@char));
            }
        }

        public void WriteSignedShort(short value)
        {
            value = IPAddress.HostToNetworkOrder(value);
            byte[] data = BitConverter.GetBytes(value);
            WriteBytes(data);
        }

        public void WriteAngle(byte value)
        {
            WriteByte(value);
        }

        public void WriteBoolean(bool value)
        {
            WriteByte((value) ? (byte)0x01 : (byte)0x00);
        }

        public void WriteDouble(double value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteUnsignedShort(ushort value)
        {
            value = (ushort)IPAddress.HostToNetworkOrder(value);
            byte[] data = BitConverter.GetBytes(value);
            WriteBytes(data);
        }

        public float ReadFloat()
        {
            float value = BitConverter.ToSingle(ReadUnsignedBytes(4), 0);
            return NetworkToHostOrder(value);
        }

        private ulong ReadUnsignedLong()
        {
            return Convert.ToUInt64(ReadLong());
        }

        private byte[] ReadUnsignedBytes(int count)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                bytes[i] = ReadUnsignedByte();
            }
            return bytes;
        }

        public void WriteBytes(byte[] value)
        {
            foreach (byte @byte in value)
            {
                Stream.WriteByte(@byte);
            }
        }

        public void WriteLong(long value)
        {
            value = IPAddress.HostToNetworkOrder(value);
            byte[] data = BitConverter.GetBytes(value);
            WriteBytes(data);
        }

        public int ReadVarInt()
        {
            int bytesRead = 0;
            return ReadVarInt(out bytesRead);
        }

        public int ReadVarInt(out int bytesRead)
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = (byte)Stream.ReadByte();
                int value = (read & 0x7f);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0x80) != 0);
            bytesRead = numRead;
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!Stream.CanSeek)
            {
                throw new NotSupportedException();
            }
            return Stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Stream.Write(buffer, offset, count);
        }

        public void WritePacket(Packet packet, int compressionThreshold)
        {
            WriteBytes(packet.ToBytes(compressionThreshold));
        }

        public void WritePacket(Packet packet)
        {
            WriteBytes(packet.ToBytes(-1));
        }

        public byte[] ToPacket()
        {
            if (Stream is MemoryStream)
            {
                Stream.Position = 0;
                MemoryStream stream = Stream as MemoryStream;
                byte[] data = stream.ToArray();
                int count = data.Length;
                WriteVarInt(count); // Write the length
                Write(data, 0, count);
                return stream.ToArray();
            }
            return null;
        }
        public byte[] ToArray()
        {
            if (Stream is MemoryStream)
            {
                MemoryStream stream = Stream as MemoryStream;
                return stream.ToArray();
            }
            return null;
        }

        private double NetworkToHostOrder(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            return BitConverter.ToDouble(data, 0);
        }

        private byte[] HostToNetworkOrder(float host)
        {
            var bytes = BitConverter.GetBytes(host);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
        private float NetworkToHostOrder(float network)
        {
            var bytes = BitConverter.GetBytes(network);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        private ushort[] NetworkToHostOrder(ushort[] network)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(network);
            return network;
        }

        private ushort NetworkToHostOrder(ushort network)
        {
            var net = BitConverter.GetBytes(network);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(net);
            return BitConverter.ToUInt16(net, 0);
        }
        private ulong NetworkToHostOrder(ulong network)
        {
            var net = BitConverter.GetBytes(network);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(net);
            return BitConverter.ToUInt64(net, 0);
        }

        public NbtCompound ReadNbtCompound()
        {
            NbtTagType type = (NbtTagType)ReadByte();
            if (type != NbtTagType.Compound)
            {
                return null;
            }
            Position--;

            NbtFile file = new NbtFile() { BigEndian = true };

            file.LoadFromStream(this, NbtCompression.AutoDetect);
            return (NbtCompound)file.RootTag;
        }

        public bool ReadBoolean()
        {
            return (ReadUnsignedByte() == 0x01) ? true : false;
        }
    }
}

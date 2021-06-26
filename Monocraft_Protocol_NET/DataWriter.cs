using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET
{
    public class DataWriter
    {

        private List<byte> _data = new List<byte>();

        public byte[] Data { get { return _data.ToArray(); } }

        public void WriteBytes(byte[] value)
        {
            _data.AddRange(value);
        }

        public void WriteByte(byte value)
        {
            _data.Add(value);
        }

        public void WriteAngle(byte value)
        {
            _data.Add(value);
        }

        public void WriteBoolean(bool value)
        {
            WriteByte((value)?(byte)0x01:(byte)0x00);
        }

        public void WriteShort(short value)
        {
            byte mostSignificantByte = (byte)(value >> 8);
            byte leastSignificantByte = (byte)(value & 0xFF);
            _data.Add(mostSignificantByte);
            _data.Add(leastSignificantByte);
        }

        public void WriteUShort(ushort value)
        {
            byte mostSignificantByte = (byte)(value >> 8);
            byte leastSignificantByte = (byte)(value & 0xFF);
            _data.Add(mostSignificantByte);
            _data.Add(leastSignificantByte);
        }

        public void WriteFloat(float value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteDouble(double value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteInt(int value)
        {
            WriteByte(Convert.ToByte(value));
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

        public void WriteLong(long value)
        {
            WriteBytes(BitConverter.GetBytes(value));
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

        public void Clear()
        {
            _data.Clear();
        }

        public byte[] ToArray()
        {
            int count = _data.Count;
            byte[] data = new byte[count];
            Array.Copy(_data.ToArray(), data, count);
            _data.Clear();
            WriteVarInt(count);
            _data.AddRange(data);
            return _data.ToArray();
        }
    }
}

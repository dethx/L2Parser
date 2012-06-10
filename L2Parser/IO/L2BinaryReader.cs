using System;
using System.IO;
using System.Text;

namespace L2Parser.IO
{
    public class L2BinaryReader : BinaryReader
    {
        public L2BinaryReader(Stream input)
            : base(input)
        {
        }
        public L2BinaryReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {
        }

        public override string ReadString()
        {
            Encoding encoding = Encoding.ASCII;
            byte b = ReadByte();
            if (b == 0)
            {
                return null;
            }
            int length = b & 0x3f;
            if ((b & 0x40) != 0) // 2 byte length
            {
                length = length + (ReadByte() << 0x06);
            }
            if ((b & 0x80) != 0) // unicode
            {
                encoding = Encoding.Unicode;
                length *= 2;
            }

            return encoding.GetString(ReadBytes(length)).TrimEnd('\0');
        }
        public string ReadUString()
        {
            int length = ReadInt32();
            if (length == 0)
            {
                return null;
            }
            return Encoding.Unicode.GetString(ReadBytes(length));
        }

        public T[] ReadArray<T>(Func<T> reader)
        {
            int count = ReadInt32();
            if (count == 0)
            {
                return null;
            }

            T[] arr = new T[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = reader();
            }

            return arr;
        }

        public T[][] ReadTable<T>(Func<T> reader)
        {
            int countX = ReadInt32();
            if (countX == 0)
            {
                return null;
            }

            T[][] table = new T[countX][];
            for (int i = 0; i < countX; i++)
            {
                int countY = ReadInt32();
                table[i] = new T[countY];
                for (int j = 0; j < countY; j++)
                {
                    table[i][j] = reader();
                }
            }

            return table;
        }
    }
}

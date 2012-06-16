using System;
using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class CommandName : BaseStructure
    {
        public static readonly string[] DataFiles = new string[] { "commandname-e" };

        public CommandData[] Data { get; set; }

        public class CommandData
        {
            [XmlAttribute]
            public uint Number { get; set; }
            [XmlAttribute]
            public uint Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
        }

        internal CommandName()
        {
        }
        public CommandName(string file)
        {
            try
            {
                using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
                {
                    Data = new CommandData[reader.ReadInt32()];
                    for (uint i = 0; i < Data.Length; i++)
                    {
                        CommandData data = new CommandData();

                        data.Number = reader.ReadUInt32();
                        data.Id = reader.ReadUInt32();
                        data.Name = reader.ReadString();

                        Data[i] = data;
                    }
                    reader.Validate();
                }
            }
            catch (Exception)
            {
                Data = null;
                throw new InvalidDataException(ParsingFailed);
            }
        }
    }
}

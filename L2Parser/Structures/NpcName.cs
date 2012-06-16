using System;
using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class NpcName : BaseStructure
    {
        public static readonly string[] DataFiles = new string[] { "npcname-e" };

        public NpcData[] Data { get; set; }

        public class NpcData
        {
            [XmlAttribute]
            public uint Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            public string Description { get; set; }
            [XmlAttribute]
            public byte[] RGB { get; set; }
            [XmlAttribute]
            public char Reserved { get; set; }

            // do not serialize default values
            public bool ShouldSerializeReserved() { return Reserved != '\xfffd'; }
        }

        internal NpcName()
        {
        }
        public NpcName(string file)
        {
            try
            {
                using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
                {
                    Data = new NpcData[reader.ReadInt32()];
                    for (uint i = 0; i < Data.Length; i++)
                    {
                        NpcData data = new NpcData();

                        data.Id = reader.ReadUInt32();
                        data.Name = reader.ReadString();
                        data.Description = reader.ReadString();
                        data.RGB = reader.ReadBytes(3);
                        data.Reserved = reader.ReadChar();

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

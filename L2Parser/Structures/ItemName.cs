using System;
using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class ItemName : BaseStructure
    {
        public static readonly string[] DataFiles = new string[] { "itemname-e" };

        public ItemData[] Data { get; set; }

        public class ItemData
        {
            [XmlAttribute]
            public uint Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public string AdditionalName { get; set; }
            public string Description { get; set; }
            public int Popup { get; set; }
            public uint[][] Class { get; set; }
            [XmlArray]
            [XmlArrayItem("SetEffect1")]
            public string[] SetId1 { get; set; }
            public uint[][] SetId2 { get; set; }
            [XmlArray]
            [XmlArrayItem("SetEffect3")]
            public string[] SetId3 { get; set; }
            public uint Unknown1 { get; set; }
            public uint Unknown2 { get; set; }
            public uint SetEnchantCount { get; set; }
            public string SetEnchantEffect { get; set; }
            public uint Color { get; set; }

            // do not serialize default values
            public bool ShouldSerializePopup() { return Popup != -1; }
            public bool ShouldSerializeUnknown1() { return Unknown1 != 0; }
            public bool ShouldSerializeUnknown2() { return Unknown2 != 0; }
            public bool ShouldSerializeSetEnchantCount() { return SetEnchantCount != 0; }
            public bool ShouldSerializeColor() { return Color != 0; }
        }

        internal ItemName()
        {
        }
        public ItemName(string file)
        {
            try
            {
                using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
                {
                    Data = new ItemData[reader.ReadInt32()];
                    for (uint i = 0; i < Data.Length; i++)
                    {
                        ItemData data = new ItemData();

                        data.Id = reader.ReadUInt32();
                        data.Name = reader.ReadUString();
                        data.AdditionalName = reader.ReadUString();
                        data.Description = reader.ReadString();
                        data.Popup = reader.ReadInt32();
                        data.Class = reader.ReadTable<uint>(reader.ReadUInt32);
                        data.SetId1 = reader.ReadArray<string>(reader.ReadString);
                        data.SetId2 = reader.ReadTable<uint>(reader.ReadUInt32);
                        data.SetId3 = reader.ReadArray<string>(reader.ReadString);
                        data.Unknown1 = reader.ReadUInt32();
                        data.Unknown2 = reader.ReadUInt32();
                        data.SetEnchantCount = reader.ReadUInt32();
                        data.SetEnchantEffect = reader.ReadString();
                        data.Color = reader.ReadUInt32();

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

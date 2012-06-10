using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class ItemName : IStructure
    {
        public List<ItemData> Data { get; set; } // foreach: uint data

        public class ItemData
        {
            [XmlAttribute]
            public uint Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public string AdditionalName { get; set; }
            public string Description { get; set; }
            [XmlIgnore]
            public int? Popup { get; set; }
            public uint[][] Class { get; set; }
            [XmlArray]
            [XmlArrayItem("SetEffect1")]
            public string[] SetId1 { get; set; }
            public uint[][] SetId2 { get; set; }
            [XmlArray]
            [XmlArrayItem("SetEffect3")]
            public string[] SetId3 { get; set; }
            [XmlIgnore]
            public uint? Unknown1 { get; set; }
            [XmlIgnore]
            public uint? Unknown2 { get; set; }
            [XmlIgnore]
            public uint? SetEnchantCount { get; set; }
            public string SetEnchantEffect { get; set; }
            [XmlIgnore]
            public uint? Color { get; set; }

            // used for serializing nullable types
            [XmlAttribute("Popup")]
            internal int PopupValue { get { return Popup.Value; } set { Popup = value; } }
            [XmlAttribute("Unknown1")]
            internal uint Unknown1Value { get { return Unknown1.Value; } set { Unknown1 = value; } }
            [XmlAttribute("Unknown2")]
            internal uint Unknown2Value { get { return Unknown2.Value; } set { Unknown2 = value; } }
            [XmlAttribute("SetEnchantCount")]
            internal uint SetEnchantCountValue { get { return SetEnchantCount.Value; } set { SetEnchantCount = value; } }
            [XmlAttribute("Color")]
            internal uint ColorValue { get { return Color.Value; } set { Color = value; } }
        }

        internal ItemName()
        {
        }
        public ItemName(string file)
        {
            using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
            {
                Data = new List<ItemData>(reader.ReadInt32());
                for (uint i = 0; i < Data.Capacity; i++)
                {
                    ItemData item = new ItemData();

                    item.Id = reader.ReadUInt32();
                    item.Name = reader.ReadUString();
                    item.AdditionalName = reader.ReadUString();
                    item.Description = reader.ReadString();
                    item.Popup = reader.ReadInt32();
                    item.Class = reader.ReadTable<uint>(reader.ReadUInt32);
                    item.SetId1 = reader.ReadArray<string>(reader.ReadString);
                    item.SetId2 = reader.ReadTable<uint>(reader.ReadUInt32);
                    item.SetId3 = reader.ReadArray<string>(reader.ReadString);
                    item.Unknown1 = reader.ReadUInt32();
                    item.Unknown2 = reader.ReadUInt32();
                    item.SetEnchantCount = reader.ReadUInt32();
                    item.SetEnchantEffect = reader.ReadString();
                    item.Color = reader.ReadUInt32();

                    // set default values to null
                    if (item.Popup == -1) { item.Popup = null; }

                    Data.Add(item);
                }
            }
        }
    }
}

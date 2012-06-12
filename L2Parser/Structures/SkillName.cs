using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class SkillName : IStructure
    {
        public static readonly string[] DataFiles = new string[] { "skillname-e" };

        public SkillData[] Data { get; set; }

        public class SkillData
        {
            [XmlAttribute]
            public uint Id { get; set; }
            [XmlAttribute]
            public uint Level { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            public string Description { get; set; }
            public string AdditionalDescription { get; set; }
            public string AdditionalDescription2 { get; set; }

            // do not serialize default values
            public bool ShouldSerializeAdditionalDescription() { return AdditionalDescription != "none"; }
            public bool ShouldSerializeAdditionalDescription2() { return AdditionalDescription2 != "none"; }
        }

        internal SkillName()
        {
        }
        public SkillName(string file)
        {
            using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
            {
                Data = new SkillData[reader.ReadInt32()];
                for (uint i = 0; i < Data.Length; i++)
                {
                    SkillData data = new SkillData();

                    data.Id = reader.ReadUInt32();
                    data.Level = reader.ReadUInt32();
                    data.Name = reader.ReadString();
                    data.Description = reader.ReadString();
                    data.AdditionalDescription = reader.ReadString();
                    data.AdditionalDescription2 = reader.ReadString();

                    Data[i] = data;
                }

                if (reader.ReadString() != "SafePackage")
                {
                    Data = null;
                    throw new InvalidDataException("Parsing failed.");
                }
            }
        }
    }
}

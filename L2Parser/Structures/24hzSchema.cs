using System.IO;
using System.Xml.Serialization;

using L2Parser.IO;

namespace L2Parser.Structures
{
    public class _24hzSchema : BaseStructure
    {
        public static readonly string[] DataFiles = new string[] { "24hzschema" };

        public string Xml { get; set; }

        internal _24hzSchema()
        {
        }
        public _24hzSchema(string file)
        {
            using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
            {
                Xml = reader.ReadString();

                if (reader.ReadString() != "SafePackage")
                {
                    throw new InvalidDataException("Parsing failed.");
                }
            }
        }

        public override string Output
        {
            get
            {
                return Xml;
            }
        }
    }
}

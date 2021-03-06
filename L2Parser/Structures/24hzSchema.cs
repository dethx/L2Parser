﻿using System;
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
            try
            {
                using (L2BinaryReader reader = new L2BinaryReader(File.OpenRead(file)))
                {
                    Xml = reader.ReadString();
                    reader.Validate();
                }
            }
            catch (Exception)
            {
                throw new InvalidDataException(ParsingFailed);
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

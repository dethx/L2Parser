using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using L2Parser.Structures;
using System.Xml.Serialization;

namespace L2Parser
{
    public static class Parser
    {
        public const string OriginalDirectory = "original";
        public const string DecryptedDirectory = "decrypted";
        public const string ParsedDirectory = "parsed";

        private static ProcessStartInfo _psi;

        static Parser()
        {
            _psi = new ProcessStartInfo();
            _psi.FileName = "l2encdec.exe";
            _psi.CreateNoWindow = true;
            _psi.UseShellExecute = false;
        }

        /// <summary>
        /// Gets all the data files in the original directory.
        /// </summary>
        /// <returns>An array of all found data files.</returns>
        public static string[] GetFiles()
        {
            if (!Directory.Exists(OriginalDirectory))
            {
                //throw new DirectoryNotFoundException("original/ directory does not exist.");
                Directory.CreateDirectory(OriginalDirectory);
            }

            return Directory.GetFiles(OriginalDirectory, "*.dat");
        }

        /// <summary>
        /// Decrypts a file and stores it in the decrypted directory.
        /// </summary>
        /// <param name="file">An original (encrypted) file path.</param>
        /// <returns>The decrypted file path.</returns>
        public static string DecryptFile(string file)
        {
            if (!File.Exists("l2encdec.exe"))
            {
                throw new FileNotFoundException("l2encdec.exe is missing, cannot decrypt data.");
            }
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("File does not exist.", file);
            }
            if (!Directory.Exists(DecryptedDirectory))
            {
                Directory.CreateDirectory(DecryptedDirectory);
            }

            string decryptFile = String.Format("{0}/{1}", DecryptedDirectory, Path.GetFileName(file));
            if (File.Exists(decryptFile))
            {
                File.Delete(decryptFile);
            }

            _psi.Arguments = String.Format("-g {0} {1}", file, decryptFile);
            Process.Start(_psi).WaitForExit();

            return decryptFile;
        }

        /// <summary>
        /// Parses a decrypted file and stores it in the parsed directory.
        /// </summary>
        /// <param name="file">A decrypted file path.</param>
        public static void ParseFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException();
            }
            if (!Directory.Exists(ParsedDirectory))
            {
                Directory.CreateDirectory(ParsedDirectory);
                //throw new DirectoryNotFoundException(String.Format("{0}/ directory does not exist.", DecryptedDirectory));
            }

            IStructure structure;
            string type = Path.GetFileNameWithoutExtension(file);
            switch (type.ToLower())
            {
                case "itemname-e":
                    structure = new ItemName(file);
                    break;
                default:
                    throw new NotSupportedException("File type is not supported.");
            }
            XmlSerializer xml = new XmlSerializer(structure.GetType());
            xml.Serialize(File.OpenWrite(String.Format("{0}/{1}.xml", ParsedDirectory, type)), structure);
        }
    }
}

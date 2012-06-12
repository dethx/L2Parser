using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace L2Parser
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Loading parser...");
            try
            {
                string[] files = Parser.GetFiles();
                Console.WriteLine("Found {0} files...", files.Length);
                foreach (string file in files)
                {
                    Console.WriteLine("Decrypting {0}...", Path.GetFileName(file));
                    try
                    {
                        string decryptedFile = Parser.DecryptFile(file);
                        Console.WriteLine("Parsing {0}...", decryptedFile);
                        try
                        {
                            Parser.ParseFile(decryptedFile);
                        }
                        catch (NotSupportedException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (FieldAccessException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

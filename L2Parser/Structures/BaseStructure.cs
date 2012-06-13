using System.IO;
using System.Xml.Serialization;

namespace L2Parser.Structures
{
    public abstract class BaseStructure
    {
        public virtual string Output
        {
            get
            {
                string output = null;
                using (MemoryStream stream = new MemoryStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    XmlSerializer xml = new XmlSerializer(this.GetType());
                    xml.Serialize(stream, this);
                    stream.Position = 0;
                    output = reader.ReadToEnd();
                }
                return output;
            }
        }
    }
}

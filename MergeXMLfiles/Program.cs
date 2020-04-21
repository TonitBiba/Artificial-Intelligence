using Heuristic_D4_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MergeXMLfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileStructureXml));
            FileStream fs;
            List<FileStructureXml> lists = new List<FileStructureXml>();
            foreach(string file in Directory.EnumerateFiles(@"C:\Users\tonit\Desktop\Rezultatet e testimit", "*.xml"))
            {
                try
                {
                    fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    lists.Add((FileStructureXml)xmlSerializer.Deserialize(fs));
                }
                catch { }
            }

            fs = new FileStream(@"C:\Users\tonit\Desktop\Rezultatet e testimit\complete.xml", FileMode.Create, FileAccess.Write);
            xmlSerializer = new XmlSerializer(typeof(List<FileStructureXml>));
            xmlSerializer.Serialize(fs, lists);
            fs.Flush();
            fs.Close();
        }
    }
}

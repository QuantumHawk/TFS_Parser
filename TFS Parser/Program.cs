using System;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TFS_Parser
{
    static class Program
    {
        //known issues: avoid [][] arrays in cs 
        //.net5 platform error
        //теги <OGRSOVMLIST /> и <ANCESTORLIST /> генерятся в начале, а не в нужных местах
        // <?xml version="1.0" encoding="windows-1251" standalone="no" ?> должно быть
        // <ROOT> должно быть пустым
        public static void Main(string[] args)
        {
            /*XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (var fileStream = File.OpenText("3_1.xml"))
            using(XmlReader reader = XmlReader.Create(fileStream, settings))
            {
                while(reader.Read())
                {
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Console.WriteLine($"Start Element: {reader.Name}. Has Attributes? : {reader.HasAttributes}");
                            break;
                        case XmlNodeType.Text:
                            Console.WriteLine($"Inner Text: {reader.Value}");
                            break;
                        case XmlNodeType.EndElement:
                            Console.WriteLine($"End Element: {reader.Name}");
                            break;
                        default:
                            Console.WriteLine($"Unknown: {reader.NodeType}");
                            break;
                    }
                }
            }*/
            
            // Load the Schema Into Memory. The Error handler is also presented here.
            StringReader sr = new StringReader(File.ReadAllText("3_1.xsd"));
            XmlSchema sch = XmlSchema.Read(sr,null);

            // Create the Reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(sch);

            // Create an XmlReader specifying the settings.
            StringReader xmlData = new StringReader(File.ReadAllText("3_1.xml"));
            XmlReader xr = XmlReader.Create(xmlData,settings);

            // Use the Native .NET Serializer (probably u cud substitute the Xsd2Code serializer here.
            XmlSerializer xs = new XmlSerializer(typeof(ROOT));
            var data = xs.Deserialize(xr);

            XmlWriterSettings settingsWriter = new XmlWriterSettings();
            settingsWriter.Indent = true;
            settingsWriter.IndentChars = ("\t");
            settingsWriter.OmitXmlDeclaration = true;
            
            XmlWriter writer = XmlWriter.Create("new.xml", settingsWriter);

            xs.Serialize(writer,data);
            
            Console.WriteLine();



        }
    }
}
using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using TFS_Parser.Entities;
using TFS_Parser.Enums;
using TFS_Parser.Models;

namespace TFS_Parser
{
    static class Program
    {
        //known issues:
        // avoid [][] arrays in ROOT.cs, replace to [] or list
        //.net5 platform error - use .net 3.1 instead
        //tags <OGRSOVMLIST /> и <ANCESTORLIST /> генерятся в начале, а не в нужных местах (fixed)
        // <?xml version="1.0" encoding="windows-1251" standalone="no" ?> (fixed)
        //todo - сохранять xml в папку с проектом каталог output
        //динамическая генерация схемы классов из любого xml файла - нужно только если xml часто меняется.
        //тут схема полная, но нужно добавить максимально значений чтобы и схема была полной
        public static async Task Main(string[] args)
        {
            int i = 1;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            // Load the Schema Into Memory. The Error handler is also presented here.
            var path_current = Environment.CurrentDirectory;
            
            var path_1 = "C:\\Users\\QW\\Desktop\\TFS_Parser\\TFS_Parser\\";
            var path_2 = "C:\\Users\\QW\\Desktop\\TFS_Parser\\TFS_Parser\\Files\\";
            var file = path_1 + "3_1.xsd";
            //1. test //var fileName = path + "3_begining.xml";
            //2. test var fileName = path + "3_1.xml";
            var fileName = path_2 + "5tfe_new_ogr.xml";

            StringReader sr = new StringReader(File.ReadAllText(file));
            XmlSchema sch = XmlSchema.Read(sr,null);

            // Create the Reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(sch);

            // Create an XmlReader specifying the settings.
            //StringReader xmlData = new StringReader(File.ReadAllText("3_1.xml"));
            StreamReader xmlData = new StreamReader(fileName,Encoding.GetEncoding("windows-1251"));
            XmlReader xr = XmlReader.Create(xmlData,settings);

            // Use the Native .NET Serializer (probably u cud substitute the Xsd2Code serializer here.
            XmlSerializer xs_1 = new XmlSerializer(typeof(ROOT));
            ROOT data = (ROOT)xs_1.Deserialize(xr);
            
            var tfsList = data.MAINLIST;
            var tfe = new ROOTMAINLISTTFSTFE();
            foreach (var tfs in tfsList)
            {
                var tf = tfs.TFE;
                foreach (var t in tf)
                {
                    var n = int.TryParse(t.TypeID, out int c) ? c : 0;
                    switch ((TFE_Type)c)
                    {
                        case TFE_Type.RAB_2par_OR:
                            var a = 1;
                            break;
                        default:
                            return;
                    }
                }
            }
            
            //todo перемешать раб операции местами и выгрузить в файл 

            XmlWriterSettings settingsWriter = new XmlWriterSettings();
            settingsWriter.Indent = true;
            settingsWriter.IndentChars = ("\t");
            settingsWriter.OmitXmlDeclaration = false;
            settingsWriter.Encoding = Encoding.GetEncoding("windows-1251");
            
            using (var writer = XmlWriter.Create($"new{i}.xml", settingsWriter))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Убираем xmlns:xsi и xmlns:xsd

                var xs = new XmlSerializer(typeof(ROOT));
                xs.Serialize(writer, data, ns);
            }
            
            //xs.Serialize(writer,data);

            //await SaveToDB(data);

            Console.WriteLine();
            
        }

        private static async Task SaveToDB(ROOT data)
        {
            using (var context = new PostgresContext())
            {
                try
                {
                    var id = 1;
                    var checkIfExist = context.TFSes.AnyAsync(x => x.ID == id).Result;

                    if (checkIfExist)
                    {
                        var entityToUpdate = context.TFSes.FirstAsync(x => x.ID == id).Result;
                        
                        var entity = new TFS()
                        {
                            ID = 1,
                            MAINLIST = data.MAINLIST,
                            TYPEPARAM = data.TYPEPARAM,
                            OGRSOVMLIST = data.OGRSOVMLIST,
                            ANCESTORLIST = data.ANCESTORLIST,
                            TYPEDECISION = data.TYPEDECISION,
                            ALTERNATELIST = data.ALTERNATELIST

                        };

                        entityToUpdate.MAINLIST = entity.MAINLIST;
                        entityToUpdate.TYPEPARAM = entity.TYPEPARAM;
                        entityToUpdate.OGRSOVMLIST = entity.OGRSOVMLIST;
                        entityToUpdate.ANCESTORLIST = entity.ANCESTORLIST;
                        entityToUpdate.TYPEDECISION = entity.TYPEDECISION;
                        entityToUpdate.ALTERNATELIST = entity.ALTERNATELIST;

                        var res = await context.SaveChangesAsync();
                    }
                    else
                    {
                        var entity = new TFS()
                        {
                            ID = 1,
                            MAINLIST = data.MAINLIST,
                            TYPEPARAM = data.TYPEPARAM,
                            OGRSOVMLIST = data.OGRSOVMLIST,
                            ANCESTORLIST = data.ANCESTORLIST,
                            TYPEDECISION = data.TYPEDECISION,
                            ALTERNATELIST = data.ALTERNATELIST

                        };

                        context.TFSes.Add(entity);

                        var res = await context.SaveChangesAsync();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
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
        private static readonly string path_xsd = "C:\\Users\\QW\\Desktop\\ДИПЛОМ\\TFS_Parser\\TFS_Parser\\";
        private static readonly string path_files = "C:\\Users\\QW\\Desktop\\ДИПЛОМ\\TFS_Parser\\TFS_Parser\\Files\\";
        
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

            var xsd = path_xsd + "3_1.xsd";
            //1. test //var fileName = path + "3_begining.xml";
            //2. test //var fileName = path + "3_1.xml";
            var fileName = path_files + "5tfe_new_ogr.xml";

            StringReader sr = new StringReader(File.ReadAllText(xsd));
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
            
            var main = data.MAINLIST;
            var k = 0;
            var temp2 = new List<ROOTMAINLISTTFSTFEPARAMSParamAlt>();
            var temp1 = new List<ROOTMAINLISTTFSTFEPARAMSParamAlt>();
            
            //tfes[0].PARAMS = temp2;
            //tfes[3].PARAMS = temp1;

            temp1 = main[0].TFE[0].PARAMS;
            temp2 = main[1].TFE[0].PARAMS;
            
            
            main[0].TFE[0].PARAMS = temp2;
            main[1].TFE[0].PARAMS = temp1;
            
            //todo попробовать поменять раб операции местами и выгрузить в файл -> новая TFS

            foreach (var tfs in main)
            {
                var tfes = tfs.TFE;
                foreach (var tfe in tfes)
                {
                    /*int.TryParse(tfe.TypeID, out int c);
                    switch ((TFE_Type)c)
                    {
                        case TFE_Type.RAB_2par_OR:
                            var a = 1;
                            break;
                        default:
                            continue;
                    }*/


                }
            }
            
            

            XmlWriterSettings settingsWriter = new XmlWriterSettings();
            settingsWriter.Indent = true;
            settingsWriter.IndentChars = ("\t");
            settingsWriter.OmitXmlDeclaration = false;
            settingsWriter.Encoding = Encoding.GetEncoding("windows-1251");

            var newName = $"new{i}.xml";
            var newFile = Path.Combine(path_files, newName);
            
            using (var writer = XmlWriter.Create(newFile, settingsWriter))
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
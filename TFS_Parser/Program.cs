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
        
        public static async Task Main(string[] args)
        {
            int i = 1;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            // Load the Schema Into Memory. The Error handler is also presented here.

            var xsd = path_xsd + "3_1.xsd";
            //1. test //var fileName = path + "3_begining.xml";
            //2. test //var fileName = path + "3_1.xml";
            var fileName = path_files + "test_geniat_zad.xml";

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
            
            await SaveToDB(data);
            
            var main = data.MAINLIST;
            
            //todo попробовать поменять раб операции местами и выгрузить в файл -> новая TFS
            //it works
            /*var temp2 = new List<ROOTMAINLISTTFSTFEPARAMSParamAlt>();
            var temp1 = new List<ROOTMAINLISTTFSTFEPARAMSParamAlt>();
            main[0].TFE[0].PARAMS = temp2;
            main[1].TFE[0].PARAMS = temp1;
            temp1 = main[0].TFE[0].PARAMS;
            temp2 = main[1].TFE[0].PARAMS;*/

           /* foreach (var tfs in main)
            {
                var tfes = tfs.TFE;
                foreach (var tfe in tfes)
                {
                    int.TryParse(tfe.TypeID, out int c);
                    switch ((TFE_Type)c)
                    {
                        case TFE_Type.RAB_2par_OR:
                            var a = 1;
                            break;
                        default:
                            continue;
                    
                }
            }
            }
            //create new xml
            var test_tfs = new List<ROOTMAINLISTTFS>();
            var main_1 = new ROOTMAINLISTTFS()
            {
                ID = "1",
                StartPointX = main[0].StartPointX,
                StartPointY = main[0].StartPointY,
                OffsetX = main[0].OffsetX,
                OffsetY = main[0].OffsetY,
                NextID = main[1].ID,
                PriorID = main[0].PriorID,
                TypeID = main[0].TypeID,
                TFE = new List<ROOTMAINLISTTFSTFE>()
                {
                    new ROOTMAINLISTTFSTFE()
                    {
                        ID = "1",
                        TypeID = "1",
                        PARAMS = new List<ROOTMAINLISTTFSTFEPARAMSParamAlt>()
                    }
                }
            };*/

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


            Console.WriteLine();
            
        }

        private static async Task SaveToDB(ROOT data)
        {
            using (var context = new PostgresContext())
            {
                try
                {
                    var id = 2;
                    var checkIfExist = context.TFSes.AnyAsync(x => x.ID == id).Result;
                    
                    var t = context.TFSes.FirstOrDefault(x => x.ID == 1);

                    List<TFS> tfs;

                    tfs = context.TFSes
                        .Include(x => x.ALTERNATELIST)
                        .ThenInclude(x => x.ITEM)
                        .Include(x => x.TYPEDECISION)
                        .ThenInclude(x => x.Params)
                        .Include(x => x.MAINLIST)
                        .ThenInclude(x => x.TFE)
                        .ThenInclude(x => x.PARAMS)
                        .Select(data => new TFS()
                        {
                            ID = data.ID,
                            MAINLIST = data.MAINLIST,
                            TYPEPARAM = data.TYPEPARAM,
                            OGRSOVMLIST = data.OGRSOVMLIST,
                            ANCESTORLIST = data.ANCESTORLIST,
                            TYPEDECISION = data.TYPEDECISION,
                            ALTERNATELIST = data.ALTERNATELIST
                            
                        }).ToList();

                    //code for create new file from old master         
                    XmlWriterSettings sw = new XmlWriterSettings();
                    sw.Indent = true;
                    sw.IndentChars = ("\t");
                    sw.OmitXmlDeclaration = true;
                    sw.Encoding = Encoding.GetEncoding("windows-1251");;

                    XmlWriter w = XmlWriter.Create($"Root6.xml", sw);
                    XmlSerializer s = new XmlSerializer(typeof(ROOT));
                    var root = new ROOT()
                    {
                        MAINLIST = tfs[0].MAINLIST,
                        TYPEPARAM = tfs[0].TYPEPARAM,
                        OGRSOVMLIST = tfs[0].OGRSOVMLIST,
                        ANCESTORLIST = tfs[0].ANCESTORLIST,
                        TYPEDECISION = tfs[0].TYPEDECISION,
                        ALTERNATELIST = tfs[0].ALTERNATELIST

                    };
                    s.Serialize(w, root);
                    
                    if (checkIfExist)
                    {
                        var entityToUpdate = context.TFSes.FirstAsync(x => x.ID == id).Result;
                        
                        var entity = new TFS()
                        {
                            ID = id,
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
                            ID = id,
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
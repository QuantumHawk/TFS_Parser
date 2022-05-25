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
using TFS_Parser.Models;

namespace TFS_Parser
{
    static class Program
    {
        //known issues: avoid [][] arrays in cs, replace to []
        //.net5 platform error
        //tags <OGRSOVMLIST /> и <ANCESTORLIST /> генерятся в начале, а не в нужных местах
        // <?xml version="1.0" encoding="windows-1251" standalone="no" ?>
        // <ROOT>
        //todo - сохранять xml в папку с проектом каталог output
        //todo динамическая генерация схемы классов из любого xml файла
        //доработать выгрузку (исправить айдишники чтобы были одинаковые)
        //добавить новую таблицу с характеристиками
        //интерфейс для обозначения характеристик
        //
        public static async Task Main(string[] args)
        {
            string fileName = "3_begining.xml";
            int i = 0;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            // Load the Schema Into Memory. The Error handler is also presented here.
            StringReader sr = new StringReader(File.ReadAllText("3_1.xsd"));
            XmlSchema sch = XmlSchema.Read(sr,null);

            // Create the Reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(sch);

            // Create an XmlReader specifying the settings.
            //StringReader xmlData = new StringReader(File.ReadAllText("3_1.xml"));
            StreamReader xmlData = new StreamReader(fileName,Encoding.GetEncoding("windows-1251"));
            XmlReader xr = XmlReader.Create(xmlData,settings);

            // Use the Native .NET Serializer (probably u cud substitute the Xsd2Code serializer here.
            XmlSerializer xs = new XmlSerializer(typeof(ROOT));
            ROOT data = (ROOT)xs.Deserialize(xr);
            
            // var tfsList = data.MAINLIST;
            // var tfe = new ROOTMAINLISTTFSTFE();
            // foreach (var tfs in tfsList)
            // {
            //     
            // }

            XmlWriterSettings settingsWriter = new XmlWriterSettings();
            settingsWriter.Indent = true;
            settingsWriter.IndentChars = ("\t");
            settingsWriter.OmitXmlDeclaration = true;
            settingsWriter.Encoding = Encoding.GetEncoding("windows-1251");;

            XmlWriter writer = XmlWriter.Create($"new_1{i}.xml", settingsWriter);

            xs.Serialize(writer,data);

            using (var context = new PostgresContext())
            {
                try
                {
                    var Tfs = context.TFSes.FirstOrDefault(x => x.ID ==1);

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


                        if (Tfs == null)
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

                        try
                        {
                            var res = await context.SaveChangesAsync();
                        }
                        catch (SystemException ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            Console.WriteLine();
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var file_id = 2;
            var data = await ParseFile();
            await SaveToDB(data, file_id);
 
            //var loadAll = await LoadAllFromDBAsync();
            
            var root = await LoadFromDBAsync(file_id);
          
            await CreateFile(file_id, root);
            
            Console.WriteLine();
            
        }

        private static async Task CreateFile(int file_id, ROOT root)
        {
            if (root != null)
            {
                XmlWriterSettings settingsWriter = new XmlWriterSettings();
                settingsWriter.Indent = true;
                settingsWriter.IndentChars = ("\t");
                settingsWriter.OmitXmlDeclaration = false;
                settingsWriter.Encoding = Encoding.GetEncoding("windows-1251");

                var newName = $"new{file_id}.xml";
                var newFile = Path.Combine(path_files, newName);
            
                using (var writer = XmlWriter.Create(newFile, settingsWriter))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", ""); // Убираем xmlns:xsi и xmlns:xsd

                    var xs = new XmlSerializer(typeof(ROOT));
                    xs.Serialize(writer, root, ns);
                }

            }
        }

        private static async Task<ROOT> ParseFile(int i = 1)
        {
            
            // Load the Schema Into Memory. The Error handler is also presented here.

            var xsd = path_xsd + "3_1.xsd";
            //1. test
            //var fileName = path_files + "3_begining.xml";
            //2. test
            var fileName = path_files + "3_1.xml";
            //var fileName = path_files + "test_geniat_zad.xml";

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
            
            return data;
        }

        private static async Task SaveToDB(ROOT data, int id_db)
        {
            using (var context = new PostgresContext())
            {
                try
                {
                    var id = id_db;
                    var checkIfExist = context.TFSes.AnyAsync(x => x.ID_DB == id).Result;
                    
                    var t = context.TFSes.FirstOrDefault(x => x.ID_DB == 1);
                    
                    if (checkIfExist)
                    {
                        var entityToUpdate = context.TFSes.FirstAsync(x => x.ID_DB == id).Result;
                        
                        var entity = new TFS()
                        {
                            ID_DB = id,
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
                            ID_DB = id,
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

        private static async Task<ROOT> LoadFromDBAsync(int id)
        {
            await using var context = new PostgresContext();

            // Грузим один TFS c нужными графами
            var tfs =  context.TFSes
                .AsNoTracking()
                .AsSplitQuery() // полезно при множественных Include
                .Where(x => x.ID_DB == id)
                .Include(x => x.ALTERNATELIST)
                .ThenInclude(x => x.ITEM)
                .Include(x => x.TYPEDECISION)
                .ThenInclude(x => x.Params)
                .Include(x => x.MAINLIST)
                .ThenInclude(x => x.TFE)
                .ThenInclude(x => x.PARAMS)
                .Include(x => x.OGRSOVMLIST)
                .Include(x => x.ANCESTORLIST)
                // Проецируем в TFS, чтобы не тащить лишние поля/трекер
                .Select(data => new TFS
                {
                    ID_DB            = data.ID_DB,
                    MAINLIST      = data.MAINLIST,
                    TYPEPARAM     = data.TYPEPARAM,
                    OGRSOVMLIST   = data.OGRSOVMLIST,
                    ANCESTORLIST  = data.ANCESTORLIST,
                    TYPEDECISION  = data.TYPEDECISION,
                    ALTERNATELIST = data.ALTERNATELIST
                })
                .SingleOrDefault();

            if (tfs is null)
                return null; // или бросьте исключение, если запись обязательна

            // Собираем ROOT из загруженного TFS (симметрично вашему SaveToDB)
            var root = new ROOT
            {
                MAINLIST      = tfs.MAINLIST,
                TYPEPARAM     = tfs.TYPEPARAM,
                OGRSOVMLIST   = tfs.OGRSOVMLIST,
                ANCESTORLIST  = tfs.ANCESTORLIST,
                TYPEDECISION  = tfs.TYPEDECISION,
                ALTERNATELIST = tfs.ALTERNATELIST
            };

            return root;
        }
        
        private static async Task<List<ROOT>> LoadAllFromDBAsync(CancellationToken ct = default)
        {
            await using var context = new PostgresContext();

            var tfsList = await context.TFSes
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.ALTERNATELIST).ThenInclude(x => x.ITEM)
                .Include(x => x.TYPEDECISION).ThenInclude(x => x.Params)
                .Include(x => x.MAINLIST).ThenInclude(x => x.TFE).ThenInclude(x => x.PARAMS)
                .Select(data => new TFS
                {
                    ID_DB            = data.ID_DB,
                    MAINLIST      = data.MAINLIST,
                    TYPEPARAM     = data.TYPEPARAM,
                    OGRSOVMLIST   = data.OGRSOVMLIST,
                    ANCESTORLIST  = data.ANCESTORLIST,
                    TYPEDECISION  = data.TYPEDECISION,
                    ALTERNATELIST = data.ALTERNATELIST
                })
                .ToListAsync(ct);

            return tfsList.Select(tfs => new ROOT
            {
                MAINLIST      = tfs.MAINLIST,
                TYPEPARAM     = tfs.TYPEPARAM,
                OGRSOVMLIST   = tfs.OGRSOVMLIST,
                ANCESTORLIST  = tfs.ANCESTORLIST,
                TYPEDECISION  = tfs.TYPEDECISION,
                ALTERNATELIST = tfs.ALTERNATELIST
            }).ToList();
        }
    }
}

#region changeTFS

//todo попробовать поменять раб операции местами и выгрузить в файл -> новая TFS
//it works
//var main = data.MAINLIST;
            
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

#endregion
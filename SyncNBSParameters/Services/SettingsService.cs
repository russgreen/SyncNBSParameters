using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using SyncNBSParameters.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncNBSParameters.Services;
internal class SettingsService : ISettingsService
{
    private const string _dataStorageElementName = "SyncNBSParametersSettings";
    private const string _schemaName = "SyncNBSParametersAppSettings";
    private const string _schemaGuid = "072BF62C-EF99-498F-B493-155620E4D47B"; 
    private const string _vendorID = "SyncNBSParameters";

    private const string _manNameParameterName = "ManNameParameterName";
    private const string _manNameParameterGuid = "ManNameParameterGuid";
    private const string _prodRefParameterName = "ProdRefParameterName";
    private const string _prodRefParameterGuid = "ProdRefParameterGuid";
    private const string _manProdURLParameterName = "ManProdURLParameterName";
    private const string _manProdURLParameterGuid = "ManProdURLParameterGuid";

    private const string _manNameMtrlParameterName = "ManNameMtrlParameterName";
    private const string _manNameMtrlParameterGuid = "ManNameMtrlParameterGuid";
    private const string _prodRefMtrlParameterName = "ProdRefMtrlParameterName";
    private const string _prodRefMtrlParameterGuid = "ProdRefMtrlParameterGuid";
    private const string _manProdURLMtrlParameterName = "ManProdURLMtrlParameterName";
    private const string _manProdURLMtrlParameterGuid = "ManProdURLMtrlParameterGuid";

    private readonly ILogger<SettingsService> _logger;

    private Autodesk.Revit.DB.ProjectInfo _projectInfo = null;

    private Schema _schema = null;

    public SettingsModel Settings { get; set; }

    public Dictionary<string, (string Source, string Target)> ObjectParameterGuids { get; }

    public Dictionary<string, (string Source, string Target)> MaterialParameterGuids { get; }

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;

        Settings = new SettingsModel();
        SetDefaults();

        ObjectParameterGuids = new Dictionary<string, (string, string)>
        {
            {"NBSChorusManName", ("911E28F9-12DD-40A8-B0D5-014F9522F86A", Settings.ManNameParameter.Guid) },
            {"NBSChorusProdRef", ("BE6F6DF3-763C-405A-9753-70306FF673D4", Settings.ProdRefParameter.Guid) },
            {"NBSChorusManProdURL", ("E12E541C-B092-4439-B9A3-9F7D070BC4C3", Settings.ManProdURLParameter.Guid) }
        };

        MaterialParameterGuids = new Dictionary<string, (string, string)>
        {
            {"NBSChorusManName_mtrl", ("69574448-B9B5-45D7-BC5F-38B193B320D7", Settings.ManNameMtrlParameter.Guid) },
            {"NBSChorusProdRef_mtrl", ("B1E5D0F4-44D8-4084-AFC7-4AE940E59B66", Settings.ProdRefMtrlParameter.Guid) },
            {"NBSChorusManProdURL_mtrl", ("8E182EF2-EBA8-43DF-8387-86106BE35563", Settings.ManProdURLMtrlParameter.Guid) }
        };
    }

    public bool GetSettings()
    {
        _projectInfo = App.RevitDocument.ProjectInformation; 

        _schema = null;

        if (SchemaExists(_schemaName))
        {
            _schema = GetSchema(_schemaName);
            _logger.LogDebug("Found schema");
        }

        if (_schema != null)
        {
            _logger.LogDebug("Schema exists. Getting settings from {schemeName}", _schema.SchemaName);
            GetSettingsFromSchema();
        }
        else
        {
            CreateAndSaveSchemaToRevit();
            SaveSettings();
        }
                
        return true;
    }

    private void CreateAndSaveSchemaToRevit()
    {
        try
        {
            _schema = CreateSchema();
            var dataStorageElement = FindDataStorageElement(_schema);

            using (Transaction createSchema = new Transaction(App.RevitDocument, "SyncNBSParametersSettings"))
            {
                createSchema.Start();

                if (dataStorageElement == null)
                {
                    dataStorageElement = DataStorage.Create(App.RevitDocument);
                    dataStorageElement.Name = _dataStorageElementName;
                }

                var entity = new Entity(_schema); // create an entity (object) for this schema (class)

                dataStorageElement.SetEntity(entity);

                createSchema.Commit();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schema");
        }
    }

    private Schema CreateSchema()
    {
        var schemaBuilder = new SchemaBuilder(new Guid(_schemaGuid));
        schemaBuilder.SetReadAccessLevel(AccessLevel.Public); // allow anyone to read the object
        schemaBuilder.SetWriteAccessLevel(AccessLevel.Public); // TODO why does it not work when we restrict writing to this vendor only
        schemaBuilder.SetVendorId(_vendorID); // required because of restricted write-access
        schemaBuilder.SetSchemaName(_schemaName);

        var fieldBuilder = schemaBuilder.AddSimpleField(_manNameParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Manufacturer Name value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manNameParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Manufacturer Name value");

        fieldBuilder = schemaBuilder.AddSimpleField(_prodRefParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Product Reference value");

        fieldBuilder = schemaBuilder.AddSimpleField(_prodRefParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Product Reference value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manProdURLParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Manufacturer Product URL value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manProdURLParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Manufacturer Product URL value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manNameMtrlParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Manufacturer Name value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manNameMtrlParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Manufacturer Name value");

        fieldBuilder = schemaBuilder.AddSimpleField(_prodRefMtrlParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Product Reference value");

        fieldBuilder = schemaBuilder.AddSimpleField(_prodRefMtrlParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Product Reference value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manProdURLMtrlParameterName, typeof(string));
        fieldBuilder.SetDocumentation("The parameter name used to store the Manufacturer Product URL value");

        fieldBuilder = schemaBuilder.AddSimpleField(_manProdURLMtrlParameterGuid, typeof(string));
        fieldBuilder.SetDocumentation("The parameter GUID used to store the Manufacturer Product URL value");

        return schemaBuilder.Finish();

    }

    public bool SaveSettings()
    {
        try
        {
            _schema = GetSchema(_schemaName);
            var dataStorageElement = FindDataStorageElement(_schema);

            using (Transaction storeData = new Transaction(App.RevitDocument, "SyncNBSParametersSettings"))
            {
                storeData.Start();

                Entity entity = dataStorageElement.GetEntity(_schema);

                entity.Set<string>(_schema.GetField(_manNameParameterName), Settings.ManNameParameter.Name);
                entity.Set<string>(_schema.GetField(_manNameParameterGuid), Settings.ManNameParameter.Guid);

                entity.Set<string>(_schema.GetField(_prodRefParameterName), Settings.ProdRefParameter.Name);
                entity.Set<string>(_schema.GetField(_prodRefParameterGuid), Settings.ProdRefParameter.Guid);

                entity.Set<string>(_schema.GetField(_manProdURLParameterName), Settings.ManProdURLParameter.Name);
                entity.Set<string>(_schema.GetField(_manProdURLParameterGuid), Settings.ManProdURLParameter.Guid);

                entity.Set<string>(_schema.GetField(_manNameMtrlParameterName), Settings.ManNameMtrlParameter.Name);
                entity.Set<string>(_schema.GetField(_manNameMtrlParameterGuid), Settings.ManNameMtrlParameter.Guid);

                entity.Set<string>(_schema.GetField(_prodRefMtrlParameterName), Settings.ProdRefMtrlParameter.Name);
                entity.Set<string>(_schema.GetField(_prodRefMtrlParameterGuid), Settings.ProdRefMtrlParameter.Guid);

                entity.Set<string>(_schema.GetField(_manProdURLMtrlParameterName), Settings.ManProdURLMtrlParameter.Name);
                entity.Set<string>(_schema.GetField(_manProdURLMtrlParameterGuid), Settings.ManProdURLMtrlParameter.Guid);

                dataStorageElement.SetEntity(entity);

                storeData.Commit();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving settings");
            return false;
        }

        return true;
    }

    private bool SchemaExists(string schemaName)
    {
        IList<Schema> schemas = Schema.ListSchemas();
        if (schemas.Count == 0)
        {
            return false;
        }
        else
        {
            foreach (Schema schema in schemas)
            {
                if (schema.SchemaName == schemaName)
                {
                    List<ElementId> ids = ElementsWithStorage(schema);
                    if (ids.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    private List<ElementId> ElementsWithStorage(Schema schema)
    {
        //List<ElementId> ids = new List<ElementId>();

        //FilteredElementCollector collector = new FilteredElementCollector(App.RevitDocument);
        //collector.WherePasses(new ExtensibleStorageFilter(schema.GUID));
        //ids.AddRange(collector.ToElementIds());

        //return ids;

        return App.RevitDocument.GetElements()
            .WherePasses(new ExtensibleStorageFilter(schema.GUID))
            .Select(q => q.Id)
            .ToList();
    }

    private Schema GetSchema(string schemaName)
    {
        Schema s = Schema.ListSchemas().FirstOrDefault(q => q.SchemaName == schemaName);
        _logger.LogDebug("Schema {schemaName} found: {s}", s.SchemaName, s);

        return s;
    }

    private DataStorage FindDataStorageElement(Schema schema)
    {
        //FilteredElementCollector collector = new FilteredElementCollector(App.RevitDocument);
        //collector.OfClass(typeof(DataStorage));
        //collector.WherePasses(new ExtensibleStorageFilter(schema.GUID));

        var collector = App.RevitDocument.GetElements()
            .OfClass(typeof(DataStorage))
            .WherePasses(new ExtensibleStorageFilter(schema.GUID));

        return collector.FirstElement() as DataStorage;
    }

    private void GetSettingsFromSchema()
    {
        try
        {
            var schema = GetSchema(_schemaName);
            var dataStorageElement = FindDataStorageElement(_schema);
            var entity = dataStorageElement.GetEntity(_schema);

            Settings.ManNameParameter.Name = entity.Get<string>(_schema.GetField(_manNameParameterName));
            Settings.ManNameParameter.Guid = entity.Get<string>(_schema.GetField(_manNameParameterGuid));

            Settings.ProdRefParameter.Name = entity.Get<string>(_schema.GetField(_prodRefParameterName));
            Settings.ProdRefParameter.Guid = entity.Get<string>(_schema.GetField(_prodRefParameterGuid));

            Settings.ManProdURLParameter.Name = entity.Get<string>(_schema.GetField(_manProdURLParameterName));
            Settings.ManProdURLParameter.Guid = entity.Get<string>(_schema.GetField(_manProdURLParameterGuid));

            Settings.ManNameMtrlParameter.Name = entity.Get<string>(_schema.GetField(_manNameMtrlParameterName));
            Settings.ManNameMtrlParameter.Guid = entity.Get<string>(_schema.GetField(_manNameMtrlParameterGuid));

            Settings.ProdRefMtrlParameter.Name = entity.Get<string>(_schema.GetField(_prodRefMtrlParameterName));
            Settings.ProdRefMtrlParameter.Guid = entity.Get<string>(_schema.GetField(_prodRefMtrlParameterGuid));

            Settings.ManProdURLMtrlParameter.Name = entity.Get<string>(_schema.GetField(_manProdURLMtrlParameterName));
            Settings.ManProdURLMtrlParameter.Guid = entity.Get<string>(_schema.GetField(_manProdURLMtrlParameterGuid));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting settings from schema");
        }

    }

    public void SetDefaults()
    {
        Settings.ManNameParameter = new ParameterDataModel
        {
            Name = "ManufacturerName",
            Guid = "8cbb36e4-83db-4c85-b24e-6d0f0362f6af"
        };

        Settings.ProdRefParameter = new ParameterDataModel
        {
            Name = "ModelReference",
            Guid = "3abb6113-6a66-4be1-bf5e-fc7ab310530f"
        };

        Settings.ManProdURLParameter = new ParameterDataModel
        {
            Name = "ProductInformation",
            Guid = "133c86a9-e00e-4550-937c-7220daed735b"
        };

        Settings.ManNameMtrlParameter = new ParameterDataModel
        {
            Name = "ManufacturerName_mtrl",
            Guid = "715dedbc-b126-4ecd-9ac8-c438782dd8c2"
        };

        Settings.ProdRefMtrlParameter = new ParameterDataModel
        {
            Name = "ModelReference_mtrl",
            Guid = "79b9aaae-be62-4459-ac43-ca48a7a36c18"
        };

        Settings.ManProdURLMtrlParameter = new ParameterDataModel
        {
            Name = "ProductInformation_mtrl",
            Guid = "a18cb312-db3c-4f3c-a90a-53e2d443c15b"
        };
    }
}

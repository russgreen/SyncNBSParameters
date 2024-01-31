using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using SyncNBSParameters.Extensions;
using SyncNBSParameters.Services;
using Nice3point.Revit.Extensions;
using ricaun.Revit.UI.StatusBar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncNBSParameters.Commands;

[Transaction(TransactionMode.Manual)]
public class CommandParameterSync : IExternalCommand
{
    private readonly ILogger<CommandParameterSync> _logger = Host.GetService<ILogger<CommandParameterSync>>();
    private readonly ISettingsService _settingsService = Host.GetService<ISettingsService>();

    private List<Element> _elementsNotUpdated = new List<Element>();

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        _logger.LogDebug("Command called");

        _settingsService.GetSettings();

        if (commandData.Application.ActiveUIDocument.Document is null)
        {
            throw new ArgumentException("activedoc");
        }
        else
        {
            App.RevitDocument = commandData.Application.ActiveUIDocument.Document;
        }

        var types = App.RevitDocument.GetTypes()
            .Where(x => x.HasNewChorusParameters(_settingsService.ObjectParameterGuids.Keys.ToList()));   
        _logger.LogDebug("Found {count} type(s) with new parameters", types.Count());

        var materials = App.RevitDocument.GetInstances(BuiltInCategory.OST_Materials)
            .Where(x => x.HasNewChorusParameters(_settingsService.MaterialParameterGuids.Keys.ToList()));       
        _logger.LogDebug("Found {count} material(s) with new parameters", materials.Count());

        if (!types.Any() && !materials.Any())
        {
            _logger.LogDebug("No new parameters found");
            return Result.Succeeded;
        }

        using (var revitProgressBar = new RevitProgressBar(true))
        {
            using (var transaction = new Transaction(App.RevitDocument, "Sync NBS parameter value"))
            {
                transaction.Start();
                _logger.LogDebug("Transaction started");

                revitProgressBar.Run("Syncing parameters values for type(s)", types, (element) =>
                {
                    _logger.LogDebug(element.Name); ;

                    if (revitProgressBar.IsCancelling())
                    {
                        return;
                    }

                    MapParameterValues(element, _settingsService.ObjectParameterGuids);
                });

                _logger.LogDebug("Syncing parameters values for material(s)");
                revitProgressBar.Run("Syncing parameters values for material(s)", materials, (element) =>
                {
                    _logger.LogDebug(element.Name); ;

                    if (revitProgressBar.IsCancelling())
                    {
                        return;
                    }

                    MapParameterValues(element, _settingsService.MaterialParameterGuids);
                });

                transaction.Commit();
                _logger.LogDebug("Transaction committed");
            } 
        }

        TaskDialog.Show("Sync NBS Parameters", 
            $"Synced {types.Count()} type(s) and {materials.Count()} material(s) with NBS Chorus parameters. {_elementsNotUpdated.Count()} could not be updated. This may be because the element is from the National BIM Library and contains some readonly parameters.");

        //TODO : show a list of elements that could not be updated

        return Result.Succeeded;
    }

    private void MapParameterValues(Element element, Dictionary<string, (string, string)> parameterGuids)
    {
        foreach (KeyValuePair<string, (string Source, string Target)> item in parameterGuids)
        {
            _logger.LogDebug("Key: {key}, Value: {value}", item.Key, item.Value);

            if(!MapParameterValues(element, item.Value.Source, item.Value.Target))
            {
                _elementsNotUpdated.Add(element);
            }
        }
    }

    private bool MapParameterValues(Element element, string source, string target)
    {
        var sourceParameter = element.get_Parameter(new Guid(source));
        var targetParameter = element.get_Parameter(new Guid(target));

        if (sourceParameter is null || sourceParameter.AsValueString() is null)
        {
            _logger.LogDebug("Source parameter {parameterName} not found or is null", source);
            return false;
        }

        if (targetParameter is null)
        {
            _logger.LogDebug("Target parameter {parameterName} not found", target);
            return false;
        }

        //some national bim library objects have readonly parameters so we need to check this
        if(targetParameter.IsReadOnly)
        {
            _logger.LogDebug("Target parameter {parameterName} is read only", target);
            return false;
        }

        if (targetParameter.StorageType != StorageType.String)
        {
            _logger.LogDebug("Target parameter {parameterName} is not a string", target);
            return false;
        }

        try
        {
            targetParameter.Set(sourceParameter.AsValueString());
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting parameter value");
        }

        return false;
    }


}

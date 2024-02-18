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
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        App.CachedUiApp = commandData.Application;
        App.RevitDocument = commandData.Application.ActiveUIDocument.Document;

        var newView = new Views.ParameterSyncView();
        newView.ShowDialog();

        return Result.Succeeded;
    }

}

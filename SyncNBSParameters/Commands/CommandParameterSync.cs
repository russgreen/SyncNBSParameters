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
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit;

namespace SyncNBSParameters.Commands;

[Transaction(TransactionMode.Manual)]
public class CommandParameterSync : ExternalCommand
{
    public override void Execute()
    {
        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.Document;

        var newView = new Views.ParameterSyncView();
        newView.ShowDialog();
    }

}

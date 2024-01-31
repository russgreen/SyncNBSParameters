using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SyncNBSParameters.Services;

namespace SyncNBSParameters.Commands;

[Transaction(TransactionMode.Manual)]
internal class CommandSettings : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        App.CachedUiApp = commandData.Application;
        App.RevitDocument = commandData.Application.ActiveUIDocument.Document;

        var newView = new Views.SettingsView();
        newView.ShowDialog();

        return Result.Succeeded;
    }
}
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SyncNBSParameters.Commands;
[Transaction(TransactionMode.Manual)]
internal class CommandAbout : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        App.CachedUiApp = commandData.Application;
        App.RevitDocument = commandData.Application.ActiveUIDocument.Document;

        var newView = new Views.AboutView();
        newView.ShowDialog();

        return Result.Succeeded;
    }
}

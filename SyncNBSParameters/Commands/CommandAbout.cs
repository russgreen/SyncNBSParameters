using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;

namespace SyncNBSParameters.Commands;
[Transaction(TransactionMode.Manual)]
internal class CommandAbout : ExternalCommand
{
    public override void Execute()
    {
        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.Document;

        var newView = new Views.AboutView();
        newView.ShowDialog();
    }
}

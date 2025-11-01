using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using Serilog.Context;

namespace SyncNBSParameters.Commands;
[Transaction(TransactionMode.Manual)]
internal class CommandAbout : ExternalCommand
{
    private readonly ILogger<CommandAbout> _logger = Host.GetService<ILogger<CommandAbout>>();

    public override void Execute()
    {
        using (LogContext.PushProperty("UsageTracking", true))
        {
            _logger.LogInformation("{command}", nameof(CommandAbout));
        }

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var newView = new Views.AboutView();
        newView.ShowDialog();
    }
}

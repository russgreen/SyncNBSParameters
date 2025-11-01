using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using Serilog.Context;

namespace SyncNBSParameters.Commands;

[Transaction(TransactionMode.Manual)]
public class CommandParameterSync : ExternalCommand
{
    private readonly ILogger<CommandParameterSync> _logger = Host.GetService<ILogger<CommandParameterSync>>();

    public override void Execute()
    {
        using (LogContext.PushProperty("UsageTracking", true))
        {
            _logger.LogInformation("{command}", nameof(CommandParameterSync));
        }

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var newView = new Views.ParameterSyncView();
        newView.ShowDialog();
    }

}

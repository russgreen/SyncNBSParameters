using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;

namespace NBSParameterSync.Commands;
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    private readonly ILogger<Command> _logger = Host.GetService<ILogger<Command>>();

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        _logger.LogDebug("Command called");

        if (commandData.Application.ActiveUIDocument.Document is null)
        {
            throw new ArgumentException("activedoc");
        }
        else
        {
            App.RevitDocument = commandData.Application.ActiveUIDocument.Document;
        }





        var mainWindowView = new Views.MainView();
        mainWindowView.ShowDialog();

        return Result.Succeeded;
    }


}

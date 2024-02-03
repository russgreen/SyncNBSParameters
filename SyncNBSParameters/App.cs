using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using SyncNBSParameters.Commands;
using SyncNBSParameters.Controllers;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SyncNBSParameters;
public class App : ExternalApplication
{
    // get the absolute path of this assembly
    public static readonly string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

    // class instance
    public static App ThisApp;

    public static UIControlledApplication CachedUiCtrApp;
    public static UIApplication CachedUiApp;
    public static ControlledApplication CtrApp;
    public static Autodesk.Revit.DB.Document RevitDocument;

    private readonly string _tabName = "NBS";

    private ILogger<App> _logger;

    public override void OnStartup()
    {
        ThisApp = this;
        CachedUiCtrApp = Application;
        CtrApp = Application.ControlledApplication;

        Host.StartHost();

        _logger = Host.GetService<ILogger<App>>();

        var panel = RibbonPanel(CachedUiCtrApp);
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        Host.StopHost();
        Serilog.Log.CloseAndFlush();

        return Result.Succeeded;
    }

    #region Ribbon Panel

    private RibbonPanel RibbonPanel(UIControlledApplication application)
    {
        RibbonPanel panel = null;

        //try and put the panel in the NBS tab
        try
        {
            panel = CachedUiCtrApp.CreateRibbonPanel(_tabName, "SyncNBSParameters_Panel");
        }
        catch 
        {
            panel = CachedUiCtrApp.CreateRibbonPanel(Tab.AddIns, "SyncNBSParameters_Panel");
        }
         
        panel.Title = "Sync Parameters";

        var splitButtonData = new SplitButtonData("SyncNBSParametersSplit", "Sync Parameters");
        var splitButton = panel.AddItem(splitButtonData) as SplitButton;
        splitButton.IsSynchronizedWithCurrentItem = false;

        splitButton.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "https://github.com/russgreen/SyncNBSParameters"));

        var buttonSync = splitButton.AddPushButton(new PushButtonData(
                       "CommandParameterSync",
                       "Sync Parameters",
                       ExecutingAssemblyPath,
                       $"{nameof(SyncNBSParameters)}.{nameof(Commands)}.{nameof(CommandParameterSync)}"));
        buttonSync.ToolTip = "Sync parameter values";
        buttonSync.LargeImage = PngImageSource("SyncNBSParameters.Resources.SyncData.png");
        buttonSync.SetAvailabilityController<CommandAvailabilityProject>();

        var buttonSettings = splitButton.AddPushButton(new PushButtonData(
            "CommandSettings",
            "Settings",
            ExecutingAssemblyPath,
            $"{nameof(SyncNBSParameters)}.{nameof(Commands)}.{nameof(CommandSettings)}"));
        buttonSettings.SetAvailabilityController<CommandAvailabilityProject>();

        var buttonAbout = splitButton.AddPushButton(new PushButtonData(
                       "CommandAbout",
                       "About",
                       ExecutingAssemblyPath,
                       $"{nameof(SyncNBSParameters)}.{nameof(Commands)}.{nameof(CommandAbout)}"));

        return panel;
    }

    private System.Windows.Media.ImageSource PngImageSource(string embeddedPath)
    {
        var stream = GetType().Assembly.GetManifestResourceStream(embeddedPath);
        System.Windows.Media.ImageSource imageSource;
        try
        {
            imageSource = BitmapFrame.Create(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading image from embedded resource");
            imageSource = null;
        }

        return imageSource;
    }

    #endregion
}

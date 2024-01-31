using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit.External;
using SyncNBSParameters.Commands;
using System;
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

    private AppDocEvents _appEvents;
    private readonly string _tabName = "Sync NBS Parameters";

    private ILogger<App> _logger;

    public override void OnStartup()
    {
        ThisApp = this;
        CachedUiCtrApp = Application;
        CtrApp = Application.ControlledApplication;

        Host.StartHost();

        _logger = Host.GetService<ILogger<App>>();

        var panel = RibbonPanel(CachedUiCtrApp);

        AddAppDocEvents();

    }

    public Result OnShutdown(UIControlledApplication application)
    {
        RemoveAppDocEvents();

        Host.StopHost();
        Serilog.Log.CloseAndFlush();

        return Result.Succeeded;
    }

    #region Event Handling
    private void AddAppDocEvents()
    {
        _appEvents = new AppDocEvents();
        _appEvents.EnableEvents();
    }
    private void RemoveAppDocEvents()
    {
        _appEvents.DisableEvents();
    }


    #endregion

    #region Ribbon Panel

    private RibbonPanel RibbonPanel(UIControlledApplication application)
    {

        try
        {
            CachedUiCtrApp.CreateRibbonTab(_tabName);
        }
        catch { }

        RibbonPanel panel = CachedUiCtrApp.CreateRibbonPanel(_tabName, "SyncNBSParameters_Panel");
        panel.Title = "Sync Parameters";

        PushButton buttonSync = (PushButton)panel.AddItem(
            new PushButtonData(
                "CommandParameterSync",
                "Sync Parameters",
                Assembly.GetExecutingAssembly().Location,
                $"{nameof(SyncNBSParameters)}.{nameof(Commands)}.{nameof(CommandParameterSync)}"));
        buttonSync.ToolTip = "Sync parameter values";
        buttonSync.LargeImage = PngImageSource("SyncNBSParameters.Resources.SyncData.png");

        PushButton buttonSettings = (PushButton)panel.AddItem(
    new PushButtonData(
        "CommandSettings",
        "Settings",
        Assembly.GetExecutingAssembly().Location,
        $"{nameof(SyncNBSParameters)}.{nameof(Commands)}.{nameof(CommandSettings)}"));
        buttonSettings.ToolTip = "Configure mapping settings";
        buttonSettings.LargeImage = PngImageSource("SyncNBSParameters.Resources.Settings.png");

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

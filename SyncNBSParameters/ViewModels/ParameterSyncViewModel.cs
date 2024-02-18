using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using ricaun.Revit.UI.StatusBar;
using SyncNBSParameters.Extensions;
using SyncNBSParameters.Models;
using SyncNBSParameters.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace SyncNBSParameters.ViewModels;
internal partial class ParameterSyncViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService = Host.GetService<ISettingsService>();
    private readonly ILogger<ParameterSyncViewModel> _logger = Host.GetService<ILogger<ParameterSyncViewModel>>();

    [ObservableProperty]
    private System.Windows.Visibility _isWindowVisible = System.Windows.Visibility.Visible;

    [ObservableProperty]
    private ObservableCollection<ElementDataModel> _elements = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasAnyErrors))]
    private ObservableCollection<object> _selectedElements = new();

    [ObservableProperty]
    private string _manNameHeader;

    [ObservableProperty]
    private string _prodRefHeader;

    [ObservableProperty]
    private string _manProdURLHeader;

    [ObservableProperty]
    private string _manNameMtrlHeader;

    [ObservableProperty]
    private string _prodRefMtrlHeader;

    [ObservableProperty]
    private string _manProdURLMtrlHeader;

    private List<Element> _elementsNotUpdated = new List<Element>();

    public bool HasAnyErrors => GetAnyErrors();


    public ParameterSyncViewModel()
    {
        _settingsService.GetSettings();

        ManNameHeader = _settingsService.Settings.ManNameParameter.Name;
        ProdRefHeader = _settingsService.Settings.ProdRefParameter.Name;
        ManProdURLHeader = _settingsService.Settings.ManProdURLParameter.Name;

        ManNameMtrlHeader = _settingsService.Settings.ManNameMtrlParameter.Name;
        ProdRefMtrlHeader = _settingsService.Settings.ProdRefMtrlParameter.Name;
        ManProdURLMtrlHeader = _settingsService.Settings.ManProdURLMtrlParameter.Name;

        LoadData();

        SelectedElements = new();
        SelectedElements.CollectionChanged += SelectedElements_CollectionChanged;
    }

    private bool GetAnyErrors()
    {
        if (GetErrors().Any())
        {
            return true;
        }

        if (!SelectedElements.Any())
        {
            return true;
        }

        return false;
    }

    private void LoadData()
    {
        Elements.Clear();

        GetElementsToSync();
        GetMaterialsToSync();
    }

    private void GetElementsToSync()
    {
        var types = App.RevitDocument.GetTypes()
            .Where(x => x.HasNewChorusParameters(_settingsService.ObjectParameterGuids.Keys.ToList()));
        _logger.LogDebug("Found {count} type(s) with new parameters", types.Count());

        foreach (var type in types)
        {
            var dataModel = new ElementDataModel
            {
                //TODO: get the family name of the element
                Element = type,
                CategoryName = type.Category.Name,
                IsMaterial = false,
                ChorusManName = type.get_Parameter(new Guid(_settingsService.Settings.NBSChorusManName)).AsValueString(),
                ChorusProdRef = type.get_Parameter(new Guid(_settingsService.Settings.NBSChorusProdRef)).AsValueString(), 
                ChorusManProdURL = type.get_Parameter(new Guid(_settingsService.Settings.NBSChorusManProdURL)).AsValueString(),
                ManName = type.get_Parameter(new Guid(_settingsService.Settings.ManNameParameter.Guid)).AsValueString(),
                ProdRef = type.get_Parameter(new Guid(_settingsService.Settings.ProdRefParameter.Guid)).AsValueString(),
                ManProdURL = type.get_Parameter(new Guid(_settingsService.Settings.ManProdURLParameter.Guid)).AsValueString(),
            };

            Elements.Add(dataModel);
        }
    }

    private void GetMaterialsToSync()
    {
        var materials = App.RevitDocument.GetInstances(BuiltInCategory.OST_Materials)
            .Where(x => x.HasNewChorusParameters(_settingsService.MaterialParameterGuids.Keys.ToList()));
        _logger.LogDebug("Found {count} material(s) with new parameters", materials.Count());

        foreach (var material in materials)
        {
            var dataModel = new ElementDataModel
            {
                Element = material,
                CategoryName = material.Category.Name,
                IsMaterial = true,
                ChorusManNameMtrl = material.get_Parameter(new Guid(_settingsService.Settings.NBSChorusManName_mtrl)).AsValueString(),
                ChorusProdRefMtrl = material.get_Parameter(new Guid(_settingsService.Settings.NBSChorusProdRef_mtrl)).AsValueString(),
                ChorusManProdURLMtrl = material.get_Parameter(new Guid(_settingsService.Settings.NBSChorusManProdURL_mtrl)).AsValueString(),
                ManNameMtrl = material.get_Parameter(new Guid(_settingsService.Settings.ManNameMtrlParameter.Guid)).AsValueString(),
                ProdRefMtrl = material.get_Parameter(new Guid(_settingsService.Settings.ProdRefMtrlParameter.Guid)).AsValueString(),
                ManProdURLMtrl = material.get_Parameter(new Guid(_settingsService.Settings.ManProdURLMtrlParameter.Guid)).AsValueString(),
            };

            Elements.Add(dataModel);
        }
    }


    [RelayCommand]
    private void SyncParameters()
    {
        IsWindowVisible = System.Windows.Visibility.Hidden;

        using (var revitProgressBar = new RevitProgressBar(true))
        {
            using (var transaction = new Transaction(App.RevitDocument, "Sync NBS parameter value"))
            {
                transaction.Start();
                _logger.LogDebug("Transaction started");

                revitProgressBar.Run("Syncing parameters values for type(s)",
                    SelectedElements
                        .Cast<ElementDataModel>()
                        .Where(x => x.IsMaterial == false), (element) =>
                {
                    _logger.LogDebug(element.Element.Name); ;

                    if (revitProgressBar.IsCancelling())
                    {
                        return;
                    }

                    MapParameterValues(element.Element, _settingsService.ObjectParameterGuids);
                });

                _logger.LogDebug("Syncing parameters values for material(s)");
                revitProgressBar.Run("Syncing parameters values for material(s)",
                    SelectedElements
                        .Cast<ElementDataModel>()
                        .Where(x => x.IsMaterial), (element) =>
                {
                    _logger.LogDebug(element.Element.Name); ;

                    if (revitProgressBar.IsCancelling())
                    {
                        return;
                    }

                    MapParameterValues(element.Element, _settingsService.MaterialParameterGuids);
                });

                transaction.Commit();
                _logger.LogDebug("Transaction committed");
            }
        }

        IsWindowVisible = System.Windows.Visibility.Visible;
        LoadData();

        if (_elementsNotUpdated.Any())
        {
            var elementListView = new Views.ElementListView(_elementsNotUpdated.Distinct().ToList());
            elementListView.Show();
        }
    }


    private void SelectedElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasAnyErrors));
    }

    private void MapParameterValues(Element element, Dictionary<string, (string, string)> parameterGuids)
    {
        //Thread.Sleep(2000); //slowing it down to demonstrate the progress bar

        foreach (KeyValuePair<string, (string Source, string Target)> item in parameterGuids)
        {
            _logger.LogDebug("Key: {key}, Value: {value}", item.Key, item.Value);

            if (!MapParameterValues(element, item.Value.Source, item.Value.Target))
            {
                _elementsNotUpdated.Add(element);
            }
        }
    }

    private bool MapParameterValues(Element element, string source, string target)
    {
        var sourceParameter = element.get_Parameter(new Guid(source));
        var targetParameter = element.get_Parameter(new Guid(target));

        if (sourceParameter is null || sourceParameter.AsValueString() is null)
        {
            _logger.LogDebug("Source parameter {parameterName} not found or is null", source);
            return false;
        }

        if (targetParameter is null)
        {
            _logger.LogDebug("Target parameter {parameterName} not found", target);
            return false;
        }

        //some national bim library objects have readonly parameters so we need to check this
        if (targetParameter.IsReadOnly)
        {
            _logger.LogDebug("Target parameter {parameterName} is read only", target);
            return false;
        }

        if (targetParameter.StorageType != StorageType.String)
        {
            _logger.LogDebug("Target parameter {parameterName} is not a string", target);
            return false;
        }

        try
        {
            targetParameter.Set(sourceParameter.AsValueString());
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting parameter value");
        }

        return false;
    }
}

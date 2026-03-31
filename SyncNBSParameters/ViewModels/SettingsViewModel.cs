using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SyncNBSParameters.Models;
using SyncNBSParameters.Requesters;
using SyncNBSParameters.Services;
using System.ComponentModel.DataAnnotations;

namespace SyncNBSParameters.ViewModels;
internal partial class SettingsViewModel : BaseViewModel, IParameterRequester
{
    private readonly ISettingsService _settingsService = null!;
    private readonly ILogger<SettingsViewModel> _logger = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _manNameParameter = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _prodRefParameter = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _manProdURLParameter = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _manNameMtrlParameter = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _prodRefMtrlParameter = null!;

    [ObservableProperty]
    [Required]
    private ParameterDataModel _manProdURLMtrlParameter = null!;

    public SettingsViewModel()
    {
        // design time constructor
    }

    public SettingsViewModel(ISettingsService settingsService,
        ILogger<SettingsViewModel> logger)
    {
        _settingsService = settingsService;
        _logger = logger;

        _settingsService.GetSettings();

        ManNameParameter = _settingsService.Settings.ManNameParameter;
        ProdRefParameter = _settingsService.Settings.ProdRefParameter;
        ManProdURLParameter = _settingsService.Settings.ManProdURLParameter;

        ManNameMtrlParameter = _settingsService.Settings.ManNameMtrlParameter;
        ProdRefMtrlParameter = _settingsService.Settings.ProdRefMtrlParameter;
        ManProdURLMtrlParameter = _settingsService.Settings.ManProdURLMtrlParameter;
    }

    public void ParameterComplete(string variableName, ParameterDataModel parameterDataModel)
    {
        switch(variableName)
        {
            case nameof(SettingsModel.ManNameParameter):
                ManNameParameter = parameterDataModel;
                break;

            case nameof(SettingsModel.ProdRefParameter):
                ProdRefParameter = parameterDataModel;
                break;

            case nameof(SettingsModel.ManProdURLParameter):
                ManProdURLParameter = parameterDataModel;
                break;

            case nameof(SettingsModel.ManNameMtrlParameter):
                ManNameMtrlParameter = parameterDataModel;
                break;
            
            case nameof(SettingsModel.ProdRefMtrlParameter):
                ProdRefMtrlParameter = parameterDataModel;
                break;

            case nameof(SettingsModel.ManProdURLMtrlParameter):
                ManProdURLMtrlParameter = parameterDataModel;
                break;

            default:
                break;
        }
    }



    [RelayCommand]
    private void SaveSettings()
    {
        _settingsService.Settings.ManNameParameter = ManNameParameter;
        _settingsService.Settings.ProdRefParameter = ProdRefParameter;
        _settingsService.Settings.ManProdURLParameter = ManProdURLParameter;

        _settingsService.Settings.ManNameMtrlParameter = ManNameMtrlParameter;
        _settingsService.Settings.ProdRefMtrlParameter = ProdRefMtrlParameter;
        _settingsService.Settings.ManProdURLMtrlParameter = ManProdURLMtrlParameter;

        _settingsService.SaveSettings();
        this.OnClosingRequest();
    }


}

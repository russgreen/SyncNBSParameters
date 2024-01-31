using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SyncNBSParameters.Models;
internal partial class SettingsModel : ObservableObject
{
    [ObservableProperty]
    private ParameterDataModel _manNameParameter;

    [ObservableProperty]
    private ParameterDataModel _prodRefParameter;

    [ObservableProperty]
    private ParameterDataModel _manProdURLParameter;

    [ObservableProperty]
    private ParameterDataModel _manNameMtrlParameter;

    [ObservableProperty]
    private ParameterDataModel _prodRefMtrlParameter;

    [ObservableProperty]
    private ParameterDataModel _manProdURLMtrlParameter;
}

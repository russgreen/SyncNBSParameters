using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SyncNBSParameters.Models;
internal partial class SettingsModel : ObservableObject
{

    public readonly string NBSChorusManName = "911E28F9-12DD-40A8-B0D5-014F9522F86A";
    public readonly string NBSChorusProdRef = "BE6F6DF3-763C-405A-9753-70306FF673D4";
    public readonly string NBSChorusManProdURL = "E12E541C-B092-4439-B9A3-9F7D070BC4C3";

    public readonly string NBSChorusManName_mtrl = "69574448-B9B5-45D7-BC5F-38B193B320D7";
    public readonly string NBSChorusProdRef_mtrl = "B1E5D0F4-44D8-4084-AFC7-4AE940E59B66";
    public readonly string NBSChorusManProdURL_mtrl = "8E182EF2-EBA8-43DF-8387-86106BE35563";

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

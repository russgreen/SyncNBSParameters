using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncNBSParameters.Models;
internal partial class ElementDataModel : ObservableObject
{
    [ObservableProperty]
    private Element _element = null!;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    [ObservableProperty] 
    private bool _isMaterial;

    [ObservableProperty]
    private string _chorusManName = string.Empty;

    [ObservableProperty]
    private string _chorusProdRef = string.Empty;

    [ObservableProperty]
    private string _chorusManProdURL = string.Empty;

    [ObservableProperty]
    private string _chorusManNameMtrl = string.Empty;

    [ObservableProperty]
    private string _chorusProdRefMtrl = string.Empty;

    [ObservableProperty]
    private string _chorusManProdURLMtrl = string.Empty;

    [ObservableProperty]
    private string _manName = string.Empty;

    [ObservableProperty]
    private string _prodRef = string.Empty;

    [ObservableProperty]
    private string _manProdURL = string.Empty;

    [ObservableProperty]
    private string _manNameMtrl = string.Empty;

    [ObservableProperty]
    private string _prodRefMtrl = string.Empty;

    [ObservableProperty]
    private string _manProdURLMtrl = string.Empty;
}

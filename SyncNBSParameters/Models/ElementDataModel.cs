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
    private Element _element;

    [ObservableProperty]
    private string _categoryName;

    [ObservableProperty] 
    private bool _isMaterial;

    [ObservableProperty]
    private string _chorusManName;

    [ObservableProperty]
    private string _chorusProdRef;

    [ObservableProperty]
    private string _chorusManProdURL;

    [ObservableProperty]
    private string _chorusManNameMtrl;

    [ObservableProperty]
    private string _chorusProdRefMtrl;

    [ObservableProperty]
    private string _chorusManProdURLMtrl;

    [ObservableProperty]
    private string _manName;

    [ObservableProperty]
    private string _prodRef;

    [ObservableProperty]
    private string _manProdURL;

    [ObservableProperty]
    private string _manNameMtrl;

    [ObservableProperty]
    private string _prodRefMtrl;

    [ObservableProperty]
    private string _manProdURLMtrl;
}

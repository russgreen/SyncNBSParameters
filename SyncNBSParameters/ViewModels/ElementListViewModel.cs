using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncNBSParameters.ViewModels;
internal partial class ElementListViewModel : BaseViewModel
{
    private readonly ILogger<ElementListViewModel> _logger = Host.GetService<ILogger<ElementListViewModel>>();

    [ObservableProperty]
    private ObservableCollection<Element> _elements = new();

    [ObservableProperty]
    private ObservableCollection<string> _elementNames = new();

    public ElementListViewModel(List<Element> elements)
    {
        foreach (var element in elements)
        {
            if(element.Category.Name == "Materials")
            {
                ElementNames.Add($"Material : {element.Id} : {element.Name}");
                return;
            }

            var familySymbol = element as FamilySymbol;

            ElementNames.Add($"{familySymbol.Category.Name} : {familySymbol.Id} : {familySymbol.FamilyName} : {familySymbol.Name}");
        }
        //ElementNames = new ObservableCollection<string>(elements.Select(e => e.Name));
    }



}

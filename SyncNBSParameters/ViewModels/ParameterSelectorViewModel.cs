using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyncNBSParameters.Models;
using SyncNBSParameters.Requesters;
using Nice3point.Revit.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SyncNBSParameters.ViewModels;
internal partial class ParameterSelectorViewModel : BaseViewModel
{
    private readonly IParameterRequester _callingViewModel;
    private readonly string _targetVariable;

    [ObservableProperty]
    private ObservableCollection<ParameterDataModel> _parameters = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasAnyErrors))]
    private ParameterDataModel _selectedParameter;

    public bool HasAnyErrors => GetAnyErrors();

    public ParameterSelectorViewModel(IParameterRequester caller, string targetVariable)
    {
        _callingViewModel = caller;
        _targetVariable = targetVariable;

    }

    [RelayCommand]
    private void PopulateParameterList(Enums.ParameterType parameterType)
    {
        Parameters = new ObservableCollection<ParameterDataModel>(LoadSharedParameters(parameterType));
    }
     
    [RelayCommand]
    private void SendParameter()
    {
        _callingViewModel.ParameterComplete(_targetVariable, SelectedParameter);
        this.OnClosingRequest();
    }

    private bool GetAnyErrors()
    {
        if (GetErrors().Any() == true)
        {
            return true;
        }

        if (SelectedParameter == null)
        {
            return true;
        }

        return false;
    }

    private List<ParameterDataModel> LoadSharedParameters(Enums.ParameterType parameterType)
    {
        var parameterDataModels = new List<ParameterDataModel>();

        var parameters = App.RevitDocument.GetElements()
            .WhereElementIsNotElementType()
            .OfClass(typeof(SharedParameterElement))
            .ToList();

        var map = App.RevitDocument.ParameterBindings;

        foreach (var element in parameters)
        {
            var sharedParameterElement = element as SharedParameterElement;
            var definition = sharedParameterElement.GetDefinition() as Definition;

            if (definition == null)
                continue;

            var parameterDataModel = new ParameterDataModel
            {
                ID = element.Id,
                Name = definition.Name,
                Guid = sharedParameterElement.GuidValue.ToString(),
                Binding = map.get_Item(definition) as ElementBinding
            };

            switch (parameterType)
            {
                case Enums.ParameterType.Object:
                    if (parameterDataModel.Binding != null && parameterDataModel.Binding is TypeBinding)
                        parameterDataModels.Add(parameterDataModel);
                    break;

                case Enums.ParameterType.Material:
                    if (parameterDataModel.Binding != null && 
                        parameterDataModel.Binding is InstanceBinding && 
                        parameterDataModel.Binding.Categories.Contains(Category.GetCategory(App.RevitDocument, BuiltInCategory.OST_Materials)))
                        parameterDataModels.Add(parameterDataModel);
                    break;

                default:
                    break;
            }
        }

        return parameterDataModels;
    }

}

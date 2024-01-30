using SyncNBSParameters.Requesters;
using System.Windows;

namespace SyncNBSParameters.Views;
/// <summary>
/// Interaction logic for ParameterSelectorView.xaml
/// </summary>
public partial class ParameterSelectorView : Window
{
    private readonly ViewModels.ParameterSelectorViewModel _viewModel;

    public ParameterSelectorView()
    {
        InitializeComponent();
    }

    public ParameterSelectorView(IParameterRequester caller, string targetVariable, Enums.ParameterType parameterType)
    {
        InitializeComponent();

        _viewModel = new ViewModels.ParameterSelectorViewModel(caller, targetVariable);

        this.DataContext = _viewModel;
        _viewModel.PopulateParameterListCommand.Execute(parameterType);
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }
}

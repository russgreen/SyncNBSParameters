using SyncNBSParameters.Models;
using SyncNBSParameters.ViewModels;
using System.Windows;

namespace SyncNBSParameters.Views;
/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : Window
{
    private readonly SettingsViewModel _viewModel;

    public SettingsView()
    {
        InitializeComponent();

        _viewModel = (ViewModels.SettingsViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void buttonNBSChorusManName_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ManNameParameter), Enums.ParameterType.Object);
    }

    private void buttonNBSChorusProdRef_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ProdRefParameter), Enums.ParameterType.Object);
    }

    private void buttonNBSChorusManProdURL_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ManProdURLParameter), Enums.ParameterType.Object);
    }

    private void buttonNBSChorusManName_mtrl_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ManNameMtrlParameter), Enums.ParameterType.Material);
    }

    private void buttonNBSChorusProdRef_mtrl_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ProdRefMtrlParameter), Enums.ParameterType.Material);
    }

    private void buttonNBSChorusManProdURL_mtrl_Click(object sender, RoutedEventArgs e)
    {
        ShowDialog(nameof(SettingsModel.ManProdURLMtrlParameter), Enums.ParameterType.Material);
    }

    private void buttonCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ShowDialog(string targetVariable, Enums.ParameterType parameterType)
    {
        var parameterSelectorView = new ParameterSelectorView(_viewModel, targetVariable, parameterType)
        {
            Owner = this
        };
        parameterSelectorView.ShowDialog();
    }
}

using Autodesk.Revit.UI;
using NBSParameterSync.ViewModels;
using System;
using System.Windows;

namespace NBSParameterSync.Views;
/// <summary>
/// Interaction logic for MainWindowView.xaml
/// </summary>
public partial class MainView : Window
{
    private readonly ViewModels.MainViewModel _viewModel;

    public MainView()
    {
        InitializeComponent();

        //var viewModel = new MainViewModel();
        //this.DataContext = viewModel;

        _viewModel = (ViewModels.MainViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();

    }

}

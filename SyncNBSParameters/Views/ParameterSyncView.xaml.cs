using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SyncNBSParameters.Views;
/// <summary>
/// Interaction logic for ParameterSyncView.xaml
/// </summary>
public partial class ParameterSyncView : Window
{
    private readonly ViewModels.ParameterSyncViewModel _viewModel;

    public ParameterSyncView()
    {
        InitializeComponent();

        _viewModel = Host.GetService<ViewModels.ParameterSyncViewModel>(); 
        DataContext = _viewModel;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void buttonCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
/// Interaction logic for AboutView.xaml
/// </summary>
public partial class AboutView : Window
{
    private readonly ViewModels.AboutViewModel _viewModel;

    public AboutView()
    {
        InitializeComponent();

        _viewModel = Host.GetService<ViewModels.AboutViewModel>();
        DataContext = _viewModel;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void OpenLink(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not Hyperlink link) return;
        Process.Start(link.NavigateUri.OriginalString);
    }
}

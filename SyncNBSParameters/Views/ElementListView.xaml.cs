using Autodesk.Revit.DB;
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
/// Interaction logic for ElementListView.xaml
/// </summary>
public partial class ElementListView : Window
{
    private readonly ViewModels.ElementListViewModel _viewModel;

    public ElementListView()
    {
        InitializeComponent();
    }

    public ElementListView(List<Element> elements)
    {
        InitializeComponent();

        _viewModel = new ViewModels.ElementListViewModel(elements);

        this.DataContext = _viewModel;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void buttonOK_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

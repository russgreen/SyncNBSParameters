using Syncfusion.UI.Xaml.Grid;
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
using SyncNBSParameters.Converters;

namespace SyncNBSParameters.Views;
/// <summary>
/// Interaction logic for ParameterSyncView.xaml
/// </summary>
public partial class ParameterSyncView : Window
{
    private readonly ViewModels.ParameterSyncViewModel _viewModel = null!;

    public ParameterSyncView()
    {
        InitializeComponent();

        _viewModel = Host.GetService<ViewModels.ParameterSyncViewModel>()!;
        DataContext = _viewModel;
        _viewModel.ClosingRequest += (sender, e) => this.Close();

        BuildDataGrid();
    }

    private void BuildDataGrid()
    {
        var parametersMatchConverter = (ParametersMatchConverter)FindResource(nameof(ParametersMatchConverter));

        var grid = new SfDataGrid
        {
            AutoGenerateColumns = false,
            AllowEditing = false,
            AllowGrouping = false,
            AllowSorting = true,
            AllowResizingColumns = true,
            AllowFiltering = true,
            NavigationMode = NavigationMode.Row,
            SelectionMode = Syncfusion.UI.Xaml.Grid.GridSelectionMode.Extended,
            ColumnSizer = GridLengthUnitType.AutoWithLastColumnFill,
            FrozenColumnCount = 3,
        };

        grid.SetBinding(SfDataGrid.ItemsSourceProperty, new Binding(nameof(ViewModels.ParameterSyncViewModel.Elements)));
        grid.SetBinding(SfDataGrid.SelectedItemsProperty, new Binding(nameof(ViewModels.ParameterSyncViewModel.SelectedElements)) { Mode = BindingMode.TwoWay });

        var stackedHeaderRow = new StackedHeaderRow();
        stackedHeaderRow.StackedColumns.Add(new StackedColumn
        {
            ChildColumns = "ChorusManName,ChorusProdRef,ChorusManProdURL,ManName,ProdRef,ManProdURL",
            HeaderText = "Family Parameters"
        });
        stackedHeaderRow.StackedColumns.Add(new StackedColumn
        {
            ChildColumns = "ChorusManNameMtrl,ChorusProdRefMtrl,ChorusManProdURLMtrl,ManNameMtrl,ProdRefMtrl,ManProdURLMtrl",
            HeaderText = "Material Parameters"
        });
        grid.StackedHeaderRows.Add(stackedHeaderRow);

        var selectColumn = new GridCheckBoxSelectorColumn
        {
            MappingName = "SelectorColumn",
            HeaderText = string.Empty,
            AllowCheckBoxOnHeader = true,
            Width = 34
        };
        grid.Columns.Add(selectColumn);

        var categoryColumn = new GridTextColumn { MappingName = "Element.Category.Name", HeaderText = "Category" };
        grid.Columns.Add(categoryColumn);

        var elementStyle = new Style(typeof(GridCell));
        elementStyle.Setters.Add(new Setter(GridCell.ForegroundProperty, new Binding { Converter = parametersMatchConverter }));
        var elementColumn = new GridTextColumn
        {
            MappingName = "Element.Name",
            HeaderText = "Element",
            CellStyle = elementStyle
        };
        grid.Columns.Add(elementColumn);

        grid.Columns.Add(new GridTextColumn { MappingName = "ChorusManName", HeaderText = "NBSChorusManName" });
        grid.Columns.Add(new GridTextColumn { MappingName = "ChorusProdRef", HeaderText = "NBSChorusProdRef" });
        grid.Columns.Add(new GridTextColumn { MappingName = "ChorusManProdURL", HeaderText = "NBSChorusManProdURL" });
        var manNameColumn = new GridTextColumn { MappingName = "ManName", HeaderText = _viewModel.ManNameHeader };
        var prodRefColumn = new GridTextColumn { MappingName = "ProdRef", HeaderText = _viewModel.ProdRefHeader };
        var manProdUrlColumn = new GridTextColumn { MappingName = "ManProdURL", HeaderText = _viewModel.ManProdURLHeader };
        grid.Columns.Add(manNameColumn);
        grid.Columns.Add(prodRefColumn);
        grid.Columns.Add(manProdUrlColumn);

        var yellowCellStyle = new Style(typeof(GridCell));
        yellowCellStyle.Setters.Add(new Setter(GridCell.BackgroundProperty, Brushes.LightYellow));

        var chorusManNameMtrlColumn = new GridTextColumn
        {
            MappingName = "ChorusManNameMtrl",
            HeaderText = "NBSChorusManName_mtrl",
            CellStyle = yellowCellStyle
        };
        var chorusProdRefMtrlColumn = new GridTextColumn
        {
            MappingName = "ChorusProdRefMtrl",
            HeaderText = "NBSChorusProdRef_mtrl",
            CellStyle = yellowCellStyle
        };
        var chorusManProdURLMtrlColumn = new GridTextColumn
        {
            MappingName = "ChorusManProdURLMtrl",
            HeaderText = "NBSChorusManProdURL_mtrl",
            CellStyle = yellowCellStyle
        };

        var manNameMtrlColumn = new GridTextColumn
        {
            MappingName = "ManNameMtrl",
            HeaderText = _viewModel.ManNameMtrlHeader,
            CellStyle = yellowCellStyle
        };
        var prodRefMtrlColumn = new GridTextColumn
        {
            MappingName = "ProdRefMtrl",
            HeaderText = _viewModel.ProdRefMtrlHeader,
            CellStyle = yellowCellStyle
        };
        var manProdURLMtrlColumn = new GridTextColumn
        {
            MappingName = "ManProdURLMtrl",
            HeaderText = _viewModel.ManProdURLMtrlHeader,
            CellStyle = yellowCellStyle
        };

        grid.Columns.Add(chorusManNameMtrlColumn);
        grid.Columns.Add(chorusProdRefMtrlColumn);
        grid.Columns.Add(chorusManProdURLMtrlColumn);
        grid.Columns.Add(manNameMtrlColumn);
        grid.Columns.Add(prodRefMtrlColumn);
        grid.Columns.Add(manProdURLMtrlColumn);

        _viewModel.PropertyChanged += (_, eventArgs) =>
        {
            if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ManNameHeader))
                manNameColumn.HeaderText = _viewModel.ManNameHeader;
            else if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ProdRefHeader))
                prodRefColumn.HeaderText = _viewModel.ProdRefHeader;
            else if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ManProdURLHeader))
                manProdUrlColumn.HeaderText = _viewModel.ManProdURLHeader;
            else if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ManNameMtrlHeader))
                manNameMtrlColumn.HeaderText = _viewModel.ManNameMtrlHeader;
            else if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ProdRefMtrlHeader))
                prodRefMtrlColumn.HeaderText = _viewModel.ProdRefMtrlHeader;
            else if (eventArgs.PropertyName == nameof(ViewModels.ParameterSyncViewModel.ManProdURLMtrlHeader))
                manProdURLMtrlColumn.HeaderText = _viewModel.ManProdURLMtrlHeader;
        };

        sfDataGridHost.Content = grid;
    }

    private void buttonCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}

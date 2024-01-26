using CommunityToolkit.Mvvm.ComponentModel;

namespace NBSParameterSync.Models;
internal partial class Model : ObservableObject
{
    [ObservableProperty]
    private string _parameter;
}

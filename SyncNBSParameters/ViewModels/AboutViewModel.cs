using CommunityToolkit.Mvvm.ComponentModel;
using SyncNBSParameters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncNBSParameters.ViewModels;
internal partial class AboutViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _copyright;

    [ObservableProperty]
    private List<OpenSourceSoftwareModel> _openSourceSoftwareModels = new List<OpenSourceSoftwareModel>();

public AboutViewModel()
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        var descriptionAttribute = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyDescriptionAttribute), false).FirstOrDefault() as System.Reflection.AssemblyDescriptionAttribute;
        Description = descriptionAttribute?.Description;

        var copyRightAttribute = assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false).FirstOrDefault() as System.Reflection.AssemblyCopyrightAttribute;
        Copyright = copyRightAttribute?.Copyright;

        BuildOpenSourceSoftwareList();
    }

    private void BuildOpenSourceSoftwareList()
    {
        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "CommunityToolkit.MVVM",
            SoftwareUri = "https://github.com/CommunityToolkit/dotnet",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/CommunityToolkit/dotnet/blob/main/License.md"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "Microsoft.Extensions.Hosting",
            SoftwareUri = "https://github.com/dotnet/runtime",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/dotnet/runtime/blob/main/LICENSE.TXT"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "Serilog",
            SoftwareUri = "https://serilog.net/",
            LicenseName = "Apache 2.0 License",
            LicenseUri = "https://github.com/serilog/serilog/blob/dev/LICENSE"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "Nice3point.Revit.Api",
            SoftwareUri = "https://github.com/Nice3point/RevitExtensions",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitExtensions/blob/main/License.md"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "Nice3point.Revit.Extensions",
            SoftwareUri = "https://github.com/Nice3point/RevitApi",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitApi/blob/main/License.md"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "Nice3point.Revit.Toolkit",
            SoftwareUri = "https://github.com/Nice3point/RevitToolkit",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitToolkit/blob/develop/License.md"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "ricaun.Revit.UI.StatusBar",
            SoftwareUri = "https://github.com/ricaun-io/ricaun.Revit.UI.StatusBar",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/ricaun-io/ricaun.Revit.UI.StatusBar?tab=MIT-1-ov-file#readme"
        });

        OpenSourceSoftwareModels.Add(new OpenSourceSoftwareModel()
        {
            SoftwareName = "ricaun.Revit.UI.StatusBar",
            SoftwareUri = "https://github.com/gluck/il-repack",
            LicenseName = "Apache 2.0 License",
            LicenseUri = "https://github.com/gluck/il-repack?tab=Apache-2.0-1-ov-file#readme"
        });
    }
}

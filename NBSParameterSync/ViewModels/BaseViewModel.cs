using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSParameterSync.ViewModels;
internal class BaseViewModel : ObservableValidator
{
    public event EventHandler ClosingRequest;

    protected void OnClosingRequest()
    {
        if (this.ClosingRequest != null)
        {
            this.ClosingRequest(this, EventArgs.Empty);
        }
    }
}

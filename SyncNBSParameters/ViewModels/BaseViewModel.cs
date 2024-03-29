﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SyncNBSParameters.ViewModels;
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

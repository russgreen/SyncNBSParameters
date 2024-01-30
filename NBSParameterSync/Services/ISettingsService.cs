using SyncNBSParameters.Models;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace SyncNBSParameters.Services;
internal interface ISettingsService
{
    Dictionary<string, (string Source, string Target)> ObjectParameterGuids { get; }

    Dictionary<string, (string Source, string Target)> MaterialParameterGuids { get; }

    SettingsModel Settings { get; set; }

    bool GetSettings();

    bool SaveSettings();

    void SetDefaults();
}

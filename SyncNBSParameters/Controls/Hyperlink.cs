using System;
using System.Windows;
using System.Windows.Controls;

namespace SyncNBSParameters.Controls;

public class Hyperlink : Button
{
    /// <summary>
    /// Property for <see cref="NavigateUri"/>.
    /// </summary>
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(nameof(NavigateUri),
        typeof(string), typeof(Hyperlink), new PropertyMetadata(String.Empty));

    /// <summary>
    /// The URL (or application shortcut) to open.
    /// </summary>
    public string NavigateUri
    {
        get => GetValue(NavigateUriProperty) as string ?? String.Empty;
        set => SetValue(NavigateUriProperty, value);
    }

    /// <summary>
    /// Action triggered when the button is clicked.
    /// </summary>
    public Hyperlink() => Click += RequestNavigate;

    private void RequestNavigate(object sender, RoutedEventArgs eventArgs)
    {
        if (String.IsNullOrEmpty(NavigateUri))
            return;

        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}


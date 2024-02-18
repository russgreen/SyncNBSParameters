using SyncNBSParameters.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SyncNBSParameters.Converters
{
    internal class ParametersMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as ElementDataModel;

            if(data.ChorusManName != data.ManName || 
                data.ChorusProdRef != data.ProdRef ||
                data.ChorusManProdURL != data.ManProdURL ||
                data.ChorusManNameMtrl != data.ManNameMtrl ||
                data.ChorusProdRefMtrl != data.ProdRefMtrl ||
                data.ChorusManProdURLMtrl != data.ManProdURLMtrl)
            {
                return Brushes.Red;
            }

            return Brushes.DarkGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

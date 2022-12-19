using System;
using System.Globalization;
using System.Windows.Data;

namespace book_manager.Helpers;

internal class Int2str : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if((long)value == 0)
            return "";
        return ((long)value).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value as string;
        if(str == "")
            return 0;
        return long.Parse(str!);
    }

}

internal class double2str : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if((double)value == 0.0)
            return "";
        return ((double)value).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value as string;
        if(str == "")
            return 0.0;
        return Double.Parse(str!);
    }
}


// item.is_free --> str
internal class item_statsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = value as Models.item;
        if(item!.is_free)
            return "空闲";
        else if(item!.loan_id != 0)
            return "借出";
        else if(item!.reservation_id != 0)
            return "已预约";
        return "丢失";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class itemDelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = value as Models.item;
        if(item!.is_free)
            return true;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


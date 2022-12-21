using System;
using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Common;

namespace book_manager.Helpers;

#region converter_nums
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

#endregion

#region converter_items

// item.is_free --> str
internal class item_statsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = value as Models.item;
        if(item!.is_free)
            return "空闲";
        else if(item!.loan_id != 0)
            return "已借出";
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

#endregion

#region converter_user
internal class UsertypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = (Models.User.userType)value;
        switch(item){
            case Models.User.userType.normal:
                return "普通用户";
            case Models.User.userType.book_manager:
                return "图书管理员";
            case Models.User.userType.system_manager:
                return "root";
        }
        return "unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class UsertypeiconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = (Models.User.userType)value;
        switch(item){
            case Models.User.userType.normal:
                return SymbolRegular.Person24;
            case Models.User.userType.book_manager:
                return SymbolRegular.PersonNote24;
            case Models.User.userType.system_manager:
                return SymbolRegular.PersonStar24;
        }
        return SymbolRegular.Person24;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class UserdelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = (Models.User.userType)value;
        if(item == Models.User.userType.system_manager)
            return false;
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region converter_confim
internal class ConfimtypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = (Models.waiting_solve.solve_type)value;
        switch(item){
            case Models.waiting_solve.solve_type.reservation_to_loan:
                return SymbolRegular.BookClock24;
            case Models.waiting_solve.solve_type.ext_loan:
                return SymbolRegular.BookArrowClockwise24;
            case Models.waiting_solve.solve_type.lose_solve:
                return SymbolRegular.BookQuestionMark24;
            case Models.waiting_solve.solve_type.loan_end:
                return SymbolRegular.ArrowReply24;
        }
        return SymbolRegular.Book24;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
internal class ConfimConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = (Models.waiting_solve.solve_type)value;
        switch(item){
            case Models.waiting_solve.solve_type.reservation_to_loan:
                return "预定兑现";
            case Models.waiting_solve.solve_type.ext_loan:
                return "延长确认";
            case Models.waiting_solve.solve_type.lose_solve:
                return "丢失处理";
            case Models.waiting_solve.solve_type.loan_end:
                return "归还确认";
        }
        return "unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region converter_borrowinfo
internal class info2titleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var item = value as Models.item;
        var res = dbhelper.Db.Queryable<Models.Title>().InSingle(item!.isbn);

        return res.name!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

#endregion
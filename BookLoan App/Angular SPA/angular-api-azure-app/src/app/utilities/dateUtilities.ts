export class DateUtilities
{
    public static GetBrowserLanguage()
    {
        return navigator.language;
    }

    public static GetDaysDifference(fromDate: Date, toDate: Date)
    {
        return Math.ceil((toDate.getTime().valueOf()-fromDate.getTime().valueOf())/86400/1000); 
    }
}
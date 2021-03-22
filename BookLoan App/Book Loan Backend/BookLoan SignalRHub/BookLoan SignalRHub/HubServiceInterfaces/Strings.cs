namespace HubServiceInterfaces
{
    public static class Strings
    {
        public static string HubUrl => "https://localhost:5001/hubs/bookloanchange";

        public static class Events
        {
            public static string BookDataChanges => nameof(IBookLoanChange.SendBookCatalogChanges);
        }
    }
}
namespace SmartBudget.Utilities
{
    public static class Constants
    {
        public static class Validation
        {
            public const int MinPasswordLength = 6;
            public const int MaxCategoryNameLength = 50;
            public const int MaxIncomeSourceLength = 100;
            public const int MaxDescriptionLength = 500;
            public const decimal MinAmount = 0.01m;
            public const decimal MaxAmount = decimal.MaxValue;
        }

        public static class Messages
        {
            public const string Success = "Operation completed successfully.";
            public const string CreatedSuccessfully = "Item created successfully.";
            public const string UpdatedSuccessfully = "Item updated successfully.";
            public const string DeletedSuccessfully = "Item deleted successfully.";
            public const string NotFound = "Item not found.";
            public const string UnauthorizedAccess = "You don't have permission to access this resource.";
            public const string InvalidInput = "The provided input is invalid.";
            public const string ServerError = "An unexpected server error occurred.";
        }

        public static class Routes
        {
            public const string ApiBase = "/api";
            public const string Income = ApiBase + "/income";
            public const string Expenses = ApiBase + "/expenses";
            public const string Categories = ApiBase + "/categories";
            public const string Budgets = ApiBase + "/budgets";
            public const string Analytics = ApiBase + "/analytics";
        }

        public static class DateFormats
        {
            public const string ShortDate = "MM/dd/yyyy";
            public const string LongDate = "dddd, MMMM dd, yyyy";
            public const string MonthYear = "MMMM yyyy";
        }
    }
}

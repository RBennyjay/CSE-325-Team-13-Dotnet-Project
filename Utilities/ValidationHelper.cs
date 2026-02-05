namespace SmartBudget.Utilities
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidAmount(decimal amount)
        {
            return amount > 0;
        }

        public static bool IsValidDate(DateTime date)
        {
            return date <= DateTime.Now;
        }

        public static bool IsValidBudgetMonth(int month)
        {
            return month >= 1 && month <= 12;
        }

        public static bool IsValidBudgetYear(int year)
        {
            return year >= DateTime.Now.Year;
        }

        public static List<string> ValidateIncomeInput(string source, decimal amount, DateTime date)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(source))
                errors.Add("Income source is required.");

            if (!IsValidAmount(amount))
                errors.Add("Amount must be greater than zero.");

            if (!IsValidDate(date))
                errors.Add("Date cannot be in the future.");

            return errors;
        }

        public static List<string> ValidateExpenseInput(int categoryId, decimal amount, DateTime date)
        {
            var errors = new List<string>();

            if (categoryId <= 0)
                errors.Add("Category is required.");

            if (!IsValidAmount(amount))
                errors.Add("Amount must be greater than zero.");

            if (!IsValidDate(date))
                errors.Add("Date cannot be in the future.");

            return errors;
        }

        public static List<string> ValidateBudgetInput(int categoryId, decimal limit, int month, int year)
        {
            var errors = new List<string>();

            if (categoryId <= 0)
                errors.Add("Category is required.");

            if (!IsValidAmount(limit))
                errors.Add("Budget limit must be greater than zero.");

            if (!IsValidBudgetMonth(month))
                errors.Add("Month must be between 1 and 12.");

            if (!IsValidBudgetYear(year))
                errors.Add("Year must be current year or later.");

            return errors;
        }
    }
}

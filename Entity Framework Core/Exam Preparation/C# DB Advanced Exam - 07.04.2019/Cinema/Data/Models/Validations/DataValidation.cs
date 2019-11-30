namespace Cinema.Data.Models.Validations
{
    public static class DataValidation
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 30;

        public const double MoneyMin = 0.01;
        public const double MoneyMax = double.MaxValue;

        public static class Customer
        {
            public const int AgeMin = 12;
            public const int AgeMax = 110;
            public const int NameMinLength = 3;
            public const int NameMaxLength = 20;
        }

        public static class Movie
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 20;
            public const double RatingMin = 1;
            public const double RatingMax = 10;
        }
    }
}
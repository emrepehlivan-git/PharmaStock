namespace PharmaStock.Modules.Product.Domain.Constants;

public static class ProductConstants
{
    public static class MaxLength
    {
        public const int Code = 50;
        public const int Name = 200;
        public const int Description = 2000;
        public const int Barcode = 100;
        public const int UnitOfMeasurement = 20;
        public const int Category = 100;
        public const int Manufacturer = 200;
        public const int Brand = 100;
    }

    public static class Messages
    {
        public const string ColdChainRequiresTemperatureLimits =
            "Cold chain products must define temperature limits.";

        public const string ProductCodeAlreadyExists = "Product with the same code already exists.";
        public const string InvalidTemperatureRange =
            "Minimum temperature ({0}°C) must be less than or equal to maximum temperature ({1}°C).";
    }
}

namespace ETA.InvoiceServices.Models
{
    public class InvoiceLines
    {
        public string internalCode { get; set; }

        public string description { get; set; }

        public string itemType { get; set; }

        public string itemCode { get; set; }

        public string unitType { get; set; }

        public double quantity { get; set; }

        public UnitValue unitValue { get; set; }

        public double salesTotal { get; set; }

        public double valueDifference { get; set; }

        public double totalTaxableFees { get; set; }

        public Discount discount { get; set; }

        public double netTotal { get; set; }

        public double itemsDiscount { get; set; }

        public TaxableItems[] taxableItems { get; set; }

        public double total { get; set; }
    }

    public class TaxableItems
    {
        public string taxType { get; set; }

        public string subType { get; set; }

        public double rate { get; set; }

        public double amount { get; set; }
    }

    public class Discount
    {
        public double rate { get; set; }

        public double amount { get; set; }
    }

    public class UnitValue
    {
        public string currencySold { get; set; }

        public double amountSold { get; set; }

        public double currencyExchangeRate { get; set; }

        public double amountEGP { get; set; }
    }
}
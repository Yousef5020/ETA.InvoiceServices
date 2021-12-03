namespace ETA.InvoiceServices.Models
{
    public class Delivery
    {
        public string approach { get; set; }

        public string packaging { get; set; }

        public string dateValidity { get; set; }

        public string exportPort { get; set; }

        public string countryOfOrigin { get; set; }

        public double grossWeight { get; set; }

        public double netWeight { get; set; }

        public string terms { get; set; }

    }
}
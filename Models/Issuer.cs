namespace ETA.InvoiceServices.Models
{
    public class Issuer
    {
        public Address address { get; set; }

        public string type { get; set; }

        public string id { get; set; }

        public string name { get; set; }
    }
    public class Address
    {
        public string branchID { get; set; }

        public string country { get; set; }

        public string governate { get; set; }

        public string regionCity { get; set; }

        public string street { get; set; }

        public string buildingNumber { get; set; }

        public string postalCode { get; set; }

        public string floor { get; set; }

        public string room { get; set; }

        public string landmark { get; set; }

        public string additionalInformation { get; set; }

    }
}

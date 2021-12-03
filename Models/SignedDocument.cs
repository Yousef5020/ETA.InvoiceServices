using System;

namespace ETA.InvoiceServices.Models
{
    public class SignedDocument
    {
        public Issuer issuer { get; set; }

        public Receiver receiver { get; set; }

        public string documentType { get; set; }

        public string documentTypeVersion { get; set; }

        public DateTime dateTimeIssued { get; set; }

        public string taxpayerActivityCode { get; set; }

        public string internalID { get; set; }

        public string purchaseOrderReference { get; set; }

        public string purchaseOrderDescription { get; set; }

        public string salesOrderReference { get; set; }

        public string salesOrderDescription { get; set; }

        public string proformaInvoiceNumber { get; set; }

        public Payment payment { get; set; }

        public Delivery delivery { get; set; }

        public InvoiceLines[] invoiceLines { get; set; }

        public double totalSalesAmount { get; set; }

        public double totalDiscountAmount { get; set; }

        public double netAmount { get; set; }

        public TaxTotals[] taxTotals { get; set; }

        public double extraDiscountAmount { get; set; }

        public double totalItemsDiscountAmount { get; set; }

        public double totalAmount { get; set; }
        public Signature[] signatures { get; set; }
    }
}

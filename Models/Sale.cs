using System;

namespace CarShowroom
{
    public class Sale
    {
        public int      Id           { get; set; }
        public Car      Car          { get; set; }
        public Customer Customer     { get; set; }
        public double   SalePrice    { get; set; }
        public DateTime SaleDate     { get; set; }
        public string   PaymentMode  { get; set; }
        public string   SalesRep     { get; set; }
    }
}

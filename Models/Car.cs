using System;

namespace CarShowroom
{
    public class Car
    {
        public int    Id          { get; set; }
        public string Brand       { get; set; }
        public string Model       { get; set; }
        public int    Year        { get; set; }
        public string Color       { get; set; }
        public string FuelType    { get; set; }
        public string Transmission{ get; set; }
        public double Price       { get; set; }
        public int    Mileage     { get; set; }
        public string Description { get; set; }
        public CarStatus Status   { get; set; }
        public DateTime AddedDate { get; set; }

        public override string ToString() =>
            $"{Year} {Brand} {Model} – {Color} – PKR {Price:N0}";
    }
}

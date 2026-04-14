using System;
using System.Collections.Generic;
using System.Linq;

namespace CarShowroom
{
    public static class DataStore
    {
        public static List<Car>      Cars      = new List<Car>();
        public static List<Customer> Customers = new List<Customer>();
        public static List<Sale>     Sales     = new List<Sale>();
        private static int _carId = 1, _custId = 1, _saleId = 1;

        public static int NextCarId()  => _carId++;
        public static int NextCustId() => _custId++;
        public static int NextSaleId() => _saleId++;

        public static void Seed()
        {
            // EXPANDED INVENTORY with 40+ vehicles
            var brands = new[]
            {
                // Toyota
                ("Toyota","Corolla","White","Petrol","Automatic",4200000,"Excellent condition, full options",0),
                ("Toyota","Corolla","Silver","Petrol","Manual",3800000,"2023 Model, low mileage",5000),
                ("Toyota","Fortuner","Black","Diesel","Automatic",12500000,"7-seater, 4x4 available",8000),
                ("Toyota","Fortuner","Grey","Diesel","Automatic",12200000,"SUV, leather interior",15000),
                ("Toyota","Yaris","Red","Petrol","Automatic",3200000,"Compact sedan, fuel efficient",20000),
                ("Toyota","Land Cruiser","Blue","Diesel","Automatic",18500000,"Premium SUV, immaculate",2000),
                
                // Honda
                ("Honda","Civic","Black","Petrol","Automatic",5800000,"Brand new 2024 model",0),
                ("Honda","Civic","Grey","Petrol","Automatic",5500000,"2023 Model with warranty",8000),
                ("Honda","BRV","White","Petrol","Automatic",5100000,"7-seater family car",15000),
                ("Honda","City","Gold","Petrol","Automatic",4100000,"Popular sedan model",22000),
                ("Honda","Accord","Black","Petrol","Automatic",9200000,"Premium sedan, fully loaded",5000),
                
                // Suzuki
                ("Suzuki","Swift","Red","Petrol","Manual",2750000,"Economy car, low mileage",12000),
                ("Suzuki","Swift","Blue","Petrol","Automatic",3100000,"Hatchback, modern features",8000),
                ("Suzuki","Cultus","Silver","Petrol","Manual",2200000,"Perfect city car",25000),
                ("Suzuki","Celerio","White","Petrol","Manual",2100000,"Budget-friendly, reliable",30000),
                ("Suzuki","Vitara","Red","Petrol","Automatic",6800000,"Compact SUV, all-wheel drive",10000),
                
                // KIA
                ("KIA","Sportage","Grey","Diesel","Automatic",7500000,"SUV with sunroof",5000),
                ("KIA","Sportage","Blue","Petrol","Automatic",7200000,"Modern SUV, tech-equipped",12000),
                ("KIA","Sorento","Black","Diesel","Automatic",9800000,"Premium 7-seater SUV",8000),
                ("KIA","Stonic","White","Petrol","Automatic",4800000,"Compact crossover, stylish",15000),
                
                // Hyundai
                ("Hyundai","Tucson","Blue","Hybrid","Automatic",8900000,"Hybrid engine, top variant",2000),
                ("Hyundai","Tucson","Silver","Petrol","Automatic",8100000,"SUV, excellent features",18000),
                ("Hyundai","Santa Fe","Black","Diesel","Automatic",11200000,"3-row SUV, spacious",6000),
                ("Hyundai","Elantra","White","Petrol","Automatic",4900000,"Sedan, fuel efficient",20000),
                ("Hyundai","Venue","Red","Petrol","Manual",3600000,"Compact SUV, affordable",25000),
                
                // MG
                ("MG","HS","Red","Petrol","Automatic",7800000,"Fully loaded, panoramic roof",1000),
                ("MG","ZS","Black","Petrol","Automatic",6200000,"Compact SUV, great value",12000),
                ("MG","RX5","Grey","Petrol","Automatic",8500000,"Luxury SUV, all features",5000),
                ("MG","Extender","White","Diesel","Automatic",4100000,"Pickup truck, dual cabin",22000),
                
                // Changan
                ("Changan","Alsvin","Gold","Petrol","Automatic",3400000,"Feature-packed budget sedan",3000),
                ("Changan","Alsvin","Silver","Petrol","Manual",3100000,"Compact sedan, popular",12000),
                ("Changan","CS35","Blue","Petrol","Automatic",5100000,"Compact SUV, modern design",8000),
                ("Changan","CS85","Black","Petrol","Automatic",8200000,"Coupe SUV, luxurious",4000),
                
                // Additional Premium Brands
                ("BMW","X5","Black","Diesel","Automatic",22500000,"Luxury SUV, fully equipped",10000),
                ("Mercedes","C-Class","Silver","Petrol","Automatic",25000000,"Premium sedan, performance",5000),
                ("Audi","Q5","White","Petrol","Automatic",24800000,"Luxury SUV, advanced tech",8000),
                ("Nissan","Altima","Blue","Petrol","Automatic",7100000,"Executive sedan, comfortable",15000),
                ("Mazda","CX-5","Red","Petrol","Automatic",8900000,"Premium crossover, stylish",12000),
                ("Volkswagen","Jetta","Gold","Petrol","Automatic",5900000,"Compact executive sedan",18000),
                ("Renault","Duster","Grey","Diesel","Automatic",3800000,"Budget SUV, rugged",35000),
                ("Peugeot","2008","Black","Petrol","Automatic",4200000,"Compact crossover, fun",28000),
            };

            foreach (var (brand, model, color, fuel, trans, price, desc, miles) in brands)
                Cars.Add(new Car
                {
                    Id = NextCarId(), Brand = brand, Model = model, Year = DateTime.Now.Year - Random.Shared.Next(0, 3),
                    Color = color, FuelType = fuel, Transmission = trans,
                    Price = price, Mileage = miles, Description = desc,
                    Status = CarStatus.Available, AddedDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 30))
                });

            // Add sample customers
            Customers.Add(new Customer { Id = NextCustId(), Name = "Ahmed Khan",    Phone = "03001234567", Email = "ahmed@email.com",    CNIC = "12345-1234567-1", Address = "Lahore" });
            Customers.Add(new Customer { Id = NextCustId(), Name = "Sara Ali",      Phone = "03111234567", Email = "sara@email.com",      CNIC = "12345-7654321-2", Address = "Karachi" });
            Customers.Add(new Customer { Id = NextCustId(), Name = "Usman Farooq",  Phone = "03211234567", Email = "usman@email.com",     CNIC = "12345-1111111-3", Address = "Islamabad" });
            Customers.Add(new Customer { Id = NextCustId(), Name = "Fatima Hassan", Phone = "03001111111", Email = "fatima@email.com",   CNIC = "12345-2222222-4", Address = "Peshawar" });
            Customers.Add(new Customer { Id = NextCustId(), Name = "Hassan Ali",    Phone = "03211111111", Email = "hassan@email.com",    CNIC = "12345-3333333-5", Address = "Rawalpindi" });
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Locations
{
    public class Pharma
    {
        public Locations[]? Pharmacies { get; set; }
        public Locations[]? Hospitals { get; set; }
    }

    public class Locations
    {
        public string? Name { get; set; }
        public string? Street { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Hotline { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
    }

    class Data
    {
        static void Main(string[] args)
        {
            string locationsJson = File.ReadAllText(@"/Users/engysaad/Projects/Locations/Locations/Locations.json");
            Pharma? pharmaData = JsonConvert.DeserializeObject<Pharma>(locationsJson);

            // Example: accessing the first pharmacy's region
            Console.WriteLine(pharmaData?.Pharmacies?[5]?.Region);

            Console.ReadLine();
        }
    }
}
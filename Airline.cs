using S10268022_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Name : Alluru Rishitha Reddy (S10268022)
// Partner Name : Faye Cheah Yi Fei (S10269175)
//==========================================================

namespace S10268022_PRG2Assignment
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; private set; }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
            Flights = new Dictionary<string, Flight>();
        }
        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights[flight.FlightNumber] = flight;
                return true;
            }
            return false;
        }

        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }

        public double CalculateFees()
        {
            double totalFees = 0.0;
            int totalFlights = Flights.Count;
            double discount = 0.0;

            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();

                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                { discount += 110; }

                else if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                { discount += 25; }

                else if (flight.Status == null)
                { discount += 50; }
            }

            discount += (totalFlights / 3) * 350;

            if (totalFlights > 5)
                discount += totalFees * 0.03;

            Console.WriteLine($"Subtotal:      ${totalFees.ToString("0.00")}");
            Console.WriteLine($"Discount:      $-{discount.ToString("0.00")}");

            return Math.Max(0, totalFees - discount);
        }

        public override string ToString()
        {
            return "Airline: " + Name + " Code: " + Code;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; private set; } = new Dictionary<string, Flight>();

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
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
            foreach (KeyValuePair<string, Flight> flight in Flights)
            {
                totalFees += flight.Value.CalculateFees();
            }
            return totalFees;
        }

        public override string ToString()
        {
            return "Airline: " + Name + " Code: " + Code;
        }
    }

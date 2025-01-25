using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class Terminal
    {
        private string TerminalName { get; set; }
        private Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
        private Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
        private Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();
        private Dictionary<string, double> gateFees = new Dictionary<string, double>();

        public Terminal(string name)
        {
            TerminalName = name;
        }
        public bool AddAirline(Airline airline)
        {
            if (!airlines.ContainsKey(airline.Code))
            {
                airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }
        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (!boardingGates.ContainsKey(boardingGate.GateNumber))
            {
                boardingGates[boardingGate.GateNumber] = boardingGate;
                return true;
            }
            return false;
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (KeyValuePair<string, Airline> airline in airlines)
            {
                if (airline.Value.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline.Value;
                }
            }
            return null;
        }
        public void PrintAirlineFees()
        {
            foreach (KeyValuePair<string, Airline> airline in airlines)
            {
                Console.WriteLine(airline.Value.Name + ": " + airline.Value.CalculateFees());
            }
        }
        public override string ToString()
        {
            return "Terminal Name: " + TerminalName;
        }
    }
}

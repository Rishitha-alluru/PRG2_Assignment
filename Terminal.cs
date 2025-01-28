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
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }

        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
        }

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (!BoardingGates.ContainsKey(boardingGate.GateNumber))
            {
                BoardingGates[boardingGate.GateNumber] = boardingGate;
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            return Airlines.Values.FirstOrDefault(airline => airline.Flights.ContainsKey(flight.FlightNumber));
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"Airline: {airline.Name}, Fees: {airline.CalculateFees():C}");
            }
        }

        public void CalculateGateFees()
        {
            foreach (var gate in BoardingGates.Values)
            {
                GateFees[gate.GateNumber] = gate.CalculateFees();
            }
        }

        public void PrintGateFees()
        {
            foreach (var gateFee in GateFees)
            {
                Console.WriteLine($"Gate: {gateFee.Key}, Fee: {gateFee.Value:C}");
            }
        }
        public override string ToString()
        {
            return "Terminal Name: " + TerminalName;
        }
    }
}
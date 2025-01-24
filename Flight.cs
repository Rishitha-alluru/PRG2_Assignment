using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
        }
        public abstract double CalculateFees();
        public override string ToString()
        { return "FlightNumber:" + FlightNumber + "\tOrigin:" + Origin + "\tDestination:" + Destination + "\tExpectedTime:" + ExpectedTime + "\tStatus:" + Status; }
    }
    public class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime): base(flightNumber, origin, destination, expectedTime){}
        public override double CalculateFees() { return 0.00; }
        public override string ToString()
        { return base.ToString(); }
    }

    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee): base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestFee;
        }
        public override double CalculateFees() => RequestFee;
        public override string ToString()
        { return base.ToString(); }
    }

    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee): base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestFee;
        }
        public override double CalculateFees() => RequestFee;
        public override string ToString()
        { return base.ToString(); }
    }

    public class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee): base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = requestFee;
        }
        public override double CalculateFees() => RequestFee;
        public override string ToString()
        { return base.ToString(); }
    }
}

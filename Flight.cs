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
            Status = status;
        }
        public virtual double CalculateFees()
        {
            double baseFee = 0;
            if (Destination == "Singapore (SIN)")
            {
                baseFee = 500;
            }
            else if (Origin == "Singapore (SIN)")
            {
                baseFee = 800;
            }

            return baseFee;
        }
        public override string ToString()
        { return "FlightNumber:" + FlightNumber + "\tOrigin:" + Origin + "\tDestination:" + Destination + "\tExpectedTime:" + ExpectedTime + "\tStatus:" + Status; }
    }
    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime, null) { }
        public override double CalculateFees()
        {
            return base.CalculateFees();
        }
        public override string ToString()
        { return base.ToString(); }
    }

    class CFFTFlight : Flight
    {
        private double RequestFee = 150;
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime, "CFFT") { }
        public override double CalculateFees()
        { return base.CalculateFees() + RequestFee; }
        public override string ToString()
        { return base.ToString(); }
    }

    class LWTTFlight : Flight
    {
        private double RequestFee = 500;
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime, "LWTT") { }
        public override double CalculateFees()
        { return base.CalculateFees() + RequestFee; }
        public override string ToString()
        { return base.ToString(); }
    }

    class DDJBFlight : Flight
    {
        private double RequestFee = 300;
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime, "DDJB") { }
        public override double CalculateFees()
        { return base.CalculateFees() + RequestFee; }
        public override string ToString()
        { return base.ToString(); }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10268022
// Student Name : Alluru Rishitha Reddy
// Partner Name : Faye Cheah Yi Fei
//==========================================================

namespace PRG2_Assignment
{
    class BoardingGate
    {
        public string GateNumber { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate(string gateNumber, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateNumber = gateNumber;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }
        public double CalculateFees()
        {
            double gateFee = 300;
            Flight.CalculateFees() += gateFee;
            return gateFee;
        }
        public override string ToString()
        {
            return "BoardingGate: " + GateNumber + " SupportsCFFT: " + SupportsCFFT + " SupportsDDJB " + SupportsDDJB + " SupportsLWTT: " + SupportsLWTT;
        }
    }
}

// See https://aka.ms/new-console-template for more information
using PRG2_Assignment;
using System.Runtime.CompilerServices;
using System.Text;

//==========================================================
// Student Number : S10268022
// Student Name : Alluru Rishitha Reddy
// Partner Name : Faye Cheah Yi Fei
//==========================================================

// 1) Load files (airlines and boarding gates)

StreamReader sr_Airlines = new StreamReader("airlines.csv",true);
StreamReader sr_BoardingGate = new StreamReader("boardinggates.csv", true);
Dictionary<string, Airline> dictAirline = new Dictionary<string, Airline>();
Dictionary<string, BoardingGate> dictBoardingGate = new Dictionary<string, BoardingGate>();
using (sr_Airlines)
{
    sr_Airlines.ReadLine();
    string line;
    while ((line = sr_Airlines.ReadLine()) != null)
    {
        string[] lineA = line.Split(",");
        Airline airline = new Airline(lineA[0], lineA[1]);
        dictAirline.Add(lineA[1], airline);
    }
}
using (sr_BoardingGate)
{
    sr_BoardingGate.ReadLine();
    string line;
    while ((line = sr_BoardingGate.ReadLine()) != null)
    {
        string[] lineB = line.Split(",");
        BoardingGate boardingGate = new BoardingGate(lineB[0], bool.Parse(lineB[1]), bool.Parse(lineB[2]), bool.Parse(lineB[3]));
        dictBoardingGate.Add(lineB[0], boardingGate);
    }
}

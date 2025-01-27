// See https://aka.ms/new-console-template for more information
using S10268022_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Text;

//==========================================================
// Student Number : S10268022
// Student Name : Alluru Rishitha Reddy
// Partner Name : Faye Cheah Yi Fei
//==========================================================

// 1) Load files (airlines and boarding gates)

StreamReader sr_Airlines = new StreamReader("airlines.csv", true);
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
        BoardingGate boardingGate = new BoardingGate(lineB[0], bool.Parse(lineB[1]), bool.Parse(lineB[2]), bool.Parse(lineB[3]), null);
        dictBoardingGate.Add(lineB[0], boardingGate);
    }
}

// 2) Load files (flights)



// 3) List all flights with their basic information



// 4) List all boarding gates



// 5) Assign a boarding gate to a flight



// 6) Create a new flight



// 7) Display full flight details from an airline



// 8) Modify flight details



// 9) Display scheduled flights in chronological order, with boarding gates assignments where applicable
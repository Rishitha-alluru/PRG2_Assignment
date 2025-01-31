// See https://aka.ms/new-console-template for more information
using S10268022_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Text;

//==========================================================
// Student Name : Alluru Rishitha Reddy (S10268022)
// Partner Name : Faye Cheah Yi Fei (S10269175)
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
        string[] lineA = line.Split(',');
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
        string[] lineB = line.Split(',');
        BoardingGate boardingGate = new BoardingGate(lineB[0], bool.Parse(lineB[1]), bool.Parse(lineB[2]), bool.Parse(lineB[3]), null);
        dictBoardingGate.Add(lineB[0], boardingGate);
    }
}
Console.WriteLine("Loading Airlines...");
Console.WriteLine($"{dictAirline.Count} Airlines Loaded!");
Console.WriteLine("Loading Boarding Gates...");
Console.WriteLine($"{dictBoardingGate.Count} Boarding Gates Loaded!");

// 2) Load files (flights)

StreamReader sr_Flights = new StreamReader("flights.csv", true);
Dictionary<string, Flight> dictFlights = new Dictionary<string, Flight>();
using (sr_Flights)
{
    sr_Flights.ReadLine();
    string line;
    while ((line = sr_Flights.ReadLine()) != null)
    {
        string[] lineC = line.Split(',');
        if (lineC.Length >= 5)
        {
            string flightNumber = lineC[0];
            string origin = lineC[1];
            string destination = lineC[2];
            DateTime expectedTime = DateTime.Parse(lineC[3]);
            string specialReqCode = lineC[4];
            string status = "";
            string airlineCode = flightNumber.Substring(0, 2);

            Flight flight;
            if (!string.IsNullOrEmpty(specialReqCode))
            {
                if (specialReqCode == "CFFT")
                { 
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, status);
                    dictFlights.Add(flightNumber, flight);
                    foreach (string code in dictAirline.Keys)
                    {
                        if (code == airlineCode)
                        { dictAirline[code].AddFlight(flight); }
                    }
                }
                else if (specialReqCode == "DDJB")
                { 
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, status);
                    dictFlights.Add(flightNumber, flight);
                    foreach (string code in dictAirline.Keys)
                    {
                        if (specialReqCode == airlineCode)
                        { dictAirline[code].AddFlight(flight); }
                    }
                }
                else if (specialReqCode == "LWTT")
                { 
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, status);
                    dictFlights.Add(flightNumber, flight);
                    foreach (string code in dictAirline.Keys)
                    {
                        if (code == airlineCode)
                        { dictAirline[code].AddFlight(flight); }
                    }
                }
            }
            else
            {
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
                dictFlights.Add(flightNumber, flight);
                foreach (string code in dictAirline.Keys)
                {
                    if (code == airlineCode)
                    { dictAirline[code].AddFlight(flight); }
                }
            }
        }
    }
    Console.WriteLine("Loading Flights...");
    Console.WriteLine($"{dictFlights.Count} Flights Loaded!");
}

// 3) List all flights with their basic information
void ListAllFlights(Dictionary<string, Flight> dictFlights)
{
    Console.WriteLine("==============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("==============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-21}{"Origin",-20}{"Destination",-19}{"Expected Departure/Arrival Time"}");
    foreach (Flight flight in dictFlights.Values)
    {
        string airlineCode = flight.FlightNumber.Substring(0,2);
        Airline airline;
        if (dictAirline.TryGetValue(airlineCode, out airline))
        {
            string airlineName = airline.Name;
            string expectedTime = flight.ExpectedTime.ToString("hh:mm:ss tt");
            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-21}{flight.Origin,-20}{flight.Destination,-19}{expectedTime}");
        }
    }
    Console.WriteLine("=============================================");

}
ListAllFlights(dictFlights);

// 4) List all boarding gates
// Since the requirement is to display all boarding gates with special requests (if any) and flight numbers (if any), the output will differ from the sample

void ListAllBoardingGates(Dictionary<string, BoardingGate> dictBoardingGates)
{
    Console.WriteLine("==============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("==============================================");
    Console.WriteLine($"{"Gate Number",-12} {"Special Requests",-16}  {"Flight"}");
    foreach (BoardingGate boardingGate in dictBoardingGates.Values)
    {
        string sRequest;
        string flightNumber;
        string bGate = boardingGate.GateNumber;
        if (boardingGate.SupportsCFFT != false)
        {
            sRequest = "CFFT";
        }
        else if (boardingGate.SupportsDDJB != false)
        {
            sRequest = "DDJB";
        }
        else if (boardingGate.SupportsLWTT != false)
        {
            sRequest = "LWTT";
        }
        else
        {
            sRequest = "None";
        }

        if (boardingGate.Flight == null)
        {
            flightNumber = "None";
        }
        else
        {
            flightNumber = boardingGate.Flight.FlightNumber;
        }
        Console.WriteLine($"{bGate,-12} {sRequest,-16}  {flightNumber}");
    }
    Console.WriteLine("==============================================");
}
ListAllBoardingGates(dictBoardingGate);

// 5) Assign a boarding gate to a flight
AssignBoardingGateToFlight();
void AssignBoardingGateToFlight()
{
    Console.Write("Enter Flight Number:\n");
    string flightNumber = Console.ReadLine();
    
    if (!dictFlights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight not found.");
        return;
    }
    Console.Write("Enter Boarding Gate Name:\n");
    string gateNumber = Console.ReadLine();

    if (!dictBoardingGate.ContainsKey(gateNumber))
    {
        Console.WriteLine("Boarding Gate not found.");
        return;
    }

    Flight flight = dictFlights[flightNumber]; 
    BoardingGate gate = dictBoardingGate[gateNumber];

    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
    Console.WriteLine($"Special Request Code: {flight.SpecialRequestCode}");

    Console.WriteLine($"Boarding Gate Name: {gateNumber}");
    Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}"); 
    Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");  
    Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");   

    if (gate.Flight != null)
    {
        Console.WriteLine($"Boarding Gate {gateNumber} is already assigned to flight {gate.Flight.FlightNumber}. Please choose another gate.");
        return;
    }

    gate.Flight = flight;  
    Console.Write("Would you like to update the status of the flight? (Y/N): ");
    string updateStatus = Console.ReadLine().Trim().ToUpper();

    if (updateStatus == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.Write("Please select the new status of the flight: ");
        string statusChoice = Console.ReadLine().Trim();
  
        if (statusChoice == "1")
        {
            flight.Status = "Delayed";
        }
        else if (statusChoice == "2")
        {
            flight.Status = "Boarding";
        }
        else
        {
            flight.Status = "On Time";
        }
    }
    else
    {
        flight.Status = "On Time";
    }

    Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {gateNumber}!");
}


// 6) Create a new flight
void CreateFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine();
        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();
        Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        DateTime expectedTime;
        while (!DateTime.TryParseExact(Console.ReadLine().Trim(), "dd/MM/yyyy HH:mm",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out expectedTime))
        {
            Console.Write("Invalid date format. Please enter the expected time again (dd/MM/yyyy hh:mm): ");
        }

        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string requestCode = Console.ReadLine().ToUpper();

        Flight newFlight = null;

        if (requestCode == "CFFT")
        {
            newFlight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
        }
        else if (requestCode == "LWTT")
        {
            newFlight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
        }
        else if (requestCode == "DDJB")
        {
            newFlight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
        }
        else if (requestCode == "NONE")
        {
            newFlight = new NORMFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
        }
        else
        {
            Console.WriteLine("Invalid request code. Flight not created.");
            return;
        }

        // store new flight in dictionary
        if (!dictFlights.ContainsKey(flightNumber))
        {
            dictFlights.Add(flightNumber, newFlight);
            Console.WriteLine($"Flight {flightNumber} has been added!");
        }
        else
        {
            Console.WriteLine("A flight with this number already exists.");
        }

        Console.Write("Would you like to add another flight? (Y/N): ");
        string addAnother = Console.ReadLine().ToUpper();

        if (addAnother != "Y")
        {
            break;
        }
    }
    Console.WriteLine("Flight creation process completed.");
}
CreateFlight();

// 7) Display full flight details from an airline

void DisplayFlightDetails(Dictionary<string, Airline> dictAirline)
{
    Console.WriteLine("==============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("==============================================");
    Console.WriteLine($"{"Code",-6}{"Name"}");
    foreach (Airline airline in dictAirline.Values)
    {
        Console.WriteLine($"{airline.Code,-6}{airline.Name}");
    }
    Console.WriteLine("==============================================");
    Console.WriteLine();
    Console.Write("Enter Airline Code: ");
    string code = Console.ReadLine();
    try
    {
        foreach (string aCode in dictAirline.Keys)
        {
            if (aCode == code)
            {
                Console.WriteLine($"Flights under {dictAirline[code].Name}:");
                Console.WriteLine($"{"Flight Number",-15}{"Origin",-20}{"Destination",-19}");
                foreach (Flight flight in dictAirline[aCode].Flights.Values)
                {
                    string expectedTime = flight.ExpectedTime.ToString("hh:mm:ss tt");
                    Console.WriteLine($"{flight.FlightNumber,-15}{flight.Origin,-20}{flight.Destination,-19}");
                }
                Console.WriteLine();
                Console.Write("Enter Flight Number: ");
                string flightNumber = Console.ReadLine();
                try
                {
                    foreach (Flight flight in dictAirline[aCode].Flights.Values)
                    {
                        if (flight.FlightNumber == flightNumber)
                        {
                            string airlineCode = flight.FlightNumber.Substring(0, 2);
                            Airline airline;
                            if (dictAirline.TryGetValue(airlineCode, out airline))
                            {
                                string airlineName = airline.Name;
                                string expectedTime = flight.ExpectedTime.ToString("hh:mm:ss tt");
                                Console.WriteLine($"Flight Number: {flightNumber}");
                                Console.WriteLine($"Airline Name: {airlineName}");
                                Console.WriteLine($"Origin: {flight.Origin}");
                                Console.WriteLine($"Destination: {flight.Destination}");
                                Console.WriteLine($"Expected Departure/Arrival Time: {expectedTime}");
                                if (flight.SpecialRequestCode != null)
                                { Console.WriteLine($"Special Request Code: {flight.SpecialRequestCode}"); }
                                else
                                { Console.WriteLine($"Special Request Code: None"); }
                                int i = 0;
                                foreach(BoardingGate bGate in dictBoardingGate.Values)
                                {
                                    i++;
                                    if (bGate.Flight.FlightNumber == flightNumber)
                                    {  Console.WriteLine($"Boarding Gate: {bGate.GateNumber}"); }
                                    else if (i == dictBoardingGate.Values.Count() && bGate.Flight.FlightNumber!=flightNumber)
                                    { throw new Exception("Boarding Gate unassigned"); }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
            else if (!dictAirline.Keys.Contains(code))
            {
                throw new Exception("Code not found, try again.");
            }
        }
    }
    catch (Exception e) 
    {
        Console.WriteLine("Error: "+e.Message);
    }
}

// 8) Modify flight details



// 9) Display scheduled flights in chronological order, with boarding gates assignments where applicable
void DisplayScheduledFlights(Dictionary<string, Flight> dictFlights, Dictionary<string, BoardingGate> dictBoardingGate)
{
    List<Flight> sortedFlights = dictFlights.Values.OrderBy(flight => flight.ExpectedTime).ToList();
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-21}{"Origin",-20}{"Destination",-19}{"Expected Departure/Arrival Time",-32}{"Status",-13}{"Boarding Gate",-8}");

    foreach (Flight flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Length >= 2 ? flight.FlightNumber.Substring(0, 2) : "Unknown";
        Airline airline = null;
        if (dictAirline.ContainsKey(airlineCode)) // checks if airline code exists
        {
            airline = dictAirline[airlineCode]; // get airline with airline code
        }

        if (airline != null)
        {
            string airlineName = airline.Name;
            string expectedTime = flight.ExpectedTime.ToString("dd/M/yyyy h:mm:ss tt");
            string status = flight.Status ?? "Scheduled"; // sets default to Scheduled if status is null

            string boardingGate = "Unassigned";
            foreach (BoardingGate gate in dictBoardingGate.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                {
                    boardingGate = gate.GateNumber;
                    break;
                }
            }
            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-21}{flight.Origin,-20}{flight.Destination,-19}{expectedTime,-32}{status,-13}{boardingGate,-8}");  
        }
    }
    Console.WriteLine("=============================================");
}
DisplayScheduledFlights(dictFlights, dictBoardingGate);

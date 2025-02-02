// See https://aka.ms/new-console-template for more information
using S10268022_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
            string status = "Scheduled";
            string airlineCode = flightNumber.Substring(0, 2);

            Flight flight;
            if (!string.IsNullOrEmpty(specialReqCode))
            {
                if (specialReqCode == "CFFT")
                { 
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, status);
                    flight.SpecialRequestCode = specialReqCode;
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
                    flight.SpecialRequestCode = specialReqCode;
                    dictFlights.Add(flightNumber, flight);
                    foreach (string code in dictAirline.Keys)
                    {
                        if (code == airlineCode)
                        { dictAirline[code].AddFlight(flight); }
                    }
                }
                else if (specialReqCode == "LWTT")
                { 
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, status);
                    flight.SpecialRequestCode = specialReqCode;
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
                specialReqCode = "NONE";
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
                flight.SpecialRequestCode = specialReqCode;
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

// Create Terminal object
Terminal terminal = new Terminal("Terminal 5");
foreach (Airline a in dictAirline.Values)
{
    terminal.AddAirline(a);
}
foreach (BoardingGate bG in dictBoardingGate.Values)
{
    terminal.AddBoardingGate(bG);
}
foreach(KeyValuePair<string, Flight> f in dictFlights)
{
    terminal.Flights.Add(f.Key, f.Value);
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
            string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-21}{flight.Origin,-20}{flight.Destination,-19}{expectedTime}");
        }
    }
    Console.WriteLine("=============================================");

}

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
            sRequest = "NONE";
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

// 5) Assign a boarding gate to a flight
void AssignBoardingGateToFlight()
{
    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();
    
    if (!dictFlights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight not found.");
        return;
    }
    Console.Write("Enter Boarding Gate Name: ");
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
        Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
        DateTime expectedTime;
        while (!DateTime.TryParseExact(Console.ReadLine().Trim(), "dd/MM/yyyy HH:mm",System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None, out expectedTime))
        {
            Console.Write("Invalid date format. Please enter the expected time again (dd/MM/yyyy HH:mm): ");
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

// 7) Display full flight details from an airline

void DisplayFlightDetails(Dictionary<string, Airline> dictAirline, Dictionary<string, BoardingGate> dictBoardingGate)
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
                Console.WriteLine($"{"Flight Number",-15}{"Origin",-20}{"Destination",-19}{"Expected Departure/Arrival Time"}");
                foreach (Flight flight in dictAirline[aCode].Flights.Values)
                {
                    string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
                    Console.WriteLine($"{flight.FlightNumber,-15}{flight.Origin,-20}{flight.Destination,-19}{expectedTime}");
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
                                string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
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
                                    if (bGate.Flight != null && bGate.Flight.FlightNumber == flightNumber)
                                    {
                                        Console.WriteLine($"Boarding Gate: {bGate.GateNumber}");
                                    }
                                    else if (i == dictBoardingGate.Values.Count() && (bGate.Flight == null || bGate.Flight.FlightNumber != flightNumber))
                                    {
                                        throw new Exception("Boarding Gate unassigned");
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: "+ ex.Message);
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

void ModifyFlightDetails(Dictionary<string, Airline> dictAirline, Dictionary<string, BoardingGate> dictBoardingGate)
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
                    string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
                    Console.WriteLine($"{flight.FlightNumber,-15}{flight.Origin,-20}{flight.Destination,-19}{expectedTime}");
                }
                Console.WriteLine();
                Console.WriteLine("[1] Modify flight details");
                Console.WriteLine("[2] Remove flight");
                Console.Write("Enter option: ");
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    if (option == 1)
                    {
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
                                        string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
                                        Console.WriteLine($"Flight Number: {flightNumber}");
                                        Console.WriteLine($"Airline Name: {airlineName}");
                                        Console.WriteLine($"Origin: {flight.Origin}");
                                        Console.WriteLine($"Destination: {flight.Destination}");
                                        Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt")}");
                                        if (flight.SpecialRequestCode != null)
                                        { Console.WriteLine($"Special Request Code: {flight.SpecialRequestCode}"); }
                                        else
                                        { Console.WriteLine($"Special Request Code: None"); }
                                        int i = 0;
                                        foreach (BoardingGate bGate in dictBoardingGate.Values)
                                        {
                                            i++;
                                            if (bGate.Flight != null && bGate.Flight.FlightNumber == flightNumber)
                                            {
                                                Console.WriteLine($"Boarding Gate: {bGate.GateNumber}");
                                            }
                                            else if (i == dictBoardingGate.Values.Count() && (bGate.Flight == null || bGate.Flight.FlightNumber != flightNumber))
                                            {
                                                throw new Exception("Boarding Gate unassigned");
                                            }
                                        }
                                    }
                                    Console.WriteLine("Aspects to modify:");
                                    Console.WriteLine("[1] Origin");
                                    Console.WriteLine("[2] Destination");
                                    Console.WriteLine("[3] Expected Departure/Arrival Time");
                                    Console.WriteLine("[4] Status");
                                    Console.WriteLine("[5] Special Request Code");
                                    Console.WriteLine("[6] Boarding Gate");
                                    Console.Write("Enter option: ");

                                    int modifyChoice = Convert.ToInt32(Console.ReadLine());

                                    switch (modifyChoice)
                                    {
                                        case 1:
                                            Console.Write("Enter new Origin: ");
                                            flight.Origin = Console.ReadLine();
                                            break;
                                        case 2:
                                            Console.Write("Enter new Destination: ");
                                            flight.Destination = Console.ReadLine();
                                            break;
                                        case 3:
                                            Console.Write("Enter new Expected Departure Time (yyyy-MM-dd): ");
                                            flight.ExpectedTime = DateTime.Parse(Console.ReadLine());
                                            break;
                                        case 4:
                                            Console.Write("Enter new Status: ");
                                            flight.Status = Console.ReadLine();
                                            break;
                                        case 5:
                                            Console.Write("Enter new Special Request Code: ");
                                            flight.SpecialRequestCode = Console.ReadLine();
                                            break;
                                        case 6:
                                            Console.Write("Enter new Boarding Gate: ");
                                            string gateNumber = Console.ReadLine();
                                            BoardingGate gate = dictBoardingGate[gateNumber];
                                            gate.Flight = flight;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid choice.");
                                            break;
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }

                    }
                    else if (option == 2)
                    {
                        Console.Write("Enter Flight Number: ");
                        string flightNumber = Console.ReadLine();
                        try
                        {
                            foreach (Flight flight in dictAirline[aCode].Flights.Values)
                            {
                                if (flight.FlightNumber == flightNumber)
                                {
                                    Console.Write("Confirm to delete? [Y/N]: ");
                                    string confirm = Console.ReadLine().ToUpper();
                                    if (confirm == "Y")
                                    {
                                        dictAirline[aCode].RemoveFlight(flight);
                                    }
                                    else { return; }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                    else { throw new IndexOutOfRangeException("Option not found"); }
                }
                catch (IndexOutOfRangeException iEx)
                { 
                    Console.WriteLine("Error: " + iEx.Message); 
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
        Console.WriteLine("Error: " + e.Message);
    }
}

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
        if (dictAirline.ContainsKey(airlineCode)) 
        {
            airline = dictAirline[airlineCode];
        }

        if (airline != null)
        {
            string airlineName = airline.Name;
            string expectedTime = flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm:ss tt");
            if (flight.Status == null)
            { flight.Status = "Scheduled"; }

            string boardingGate = "Unassigned";
            foreach (BoardingGate gate in dictBoardingGate.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                {
                    boardingGate = gate.GateNumber;
                }
            }
            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-21}{flight.Origin,-20}{flight.Destination,-19}{expectedTime,-32}{flight.Status,-13}{boardingGate,-8}");  
        }
    }
    Console.WriteLine("=============================================");
}

// Advanced : 1 Process all unassigned flights to boarding gates in bulk

void AssignGatesToUnassignedFlights(Dictionary<string, Flight> dictFlights, Dictionary<string, BoardingGate> dictBoardingGate)
{
    foreach (var flight in dictFlights.Values)
    {
        if (!dictBoardingGate.Values.Any(g => g.Flight == flight))
        {
            BoardingGate assignedGate = null;

            if (!string.IsNullOrEmpty(flight.SpecialRequestCode))
            {
                assignedGate = dictBoardingGate.Values.FirstOrDefault(g => g.Flight == null && ((flight.SpecialRequestCode == "CFFT" && g.SupportsCFFT) || (flight.SpecialRequestCode == "DDJB" && g.SupportsDDJB) || (flight.SpecialRequestCode == "LWTT" && g.SupportsLWTT)));
            }
            else
            {
                assignedGate = dictBoardingGate.Values.FirstOrDefault(g => g.Flight == null &&!g.SupportsCFFT && !g.SupportsDDJB && !g.SupportsLWTT);
            }
            if (flight.Status == null)
            {
                flight.Status = "On Time";
            }

            if (assignedGate != null)
            {
                assignedGate.Flight = flight;
            }
        }
    }
}

// Advanced 2: Display the total fee per airline for the day

Dictionary<string, double> feePerAirline = new Dictionary<string, double>();
void DisplayTotalFee(Terminal terminal)
{
    AssignGatesToUnassignedFlights(dictFlights, dictBoardingGate);
    double totalFee = 0;
    foreach (Airline airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code}  {airline.Name}");
        double total = airline.CalculateFees();
        double bGateFee = airline.Flights.Count * 300;
        Console.WriteLine($"Boarding Gate: ${bGateFee.ToString("0.00")}");
        total += bGateFee;
        Console.WriteLine($"Total        :${total.ToString("0.00")}");
        totalFee+= total;
        Console.WriteLine();
    }
    Console.WriteLine($"{terminal.TerminalName}");
    Console.WriteLine($"Total fee: {totalFee.ToString("0.00")}");
}

// Display Menu
int DisplayMenu()
{
    Console.WriteLine("==============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("==============================================");
    Console.WriteLine("[1] List All Flights");
    Console.WriteLine("[2] List Boarding Gates");
    Console.WriteLine("[3] Assign a Boarding Gate to a Flight");
    Console.WriteLine("[4] Create Flight");
    Console.WriteLine("[5] Display Airline Flights");
    Console.WriteLine("[6] Modify Flight Details");
    Console.WriteLine("[7] Display Flight Schedule");
    Console.WriteLine("[8] Assign Boarding Gates to all Flights unassaigned");
    Console.WriteLine("[9] Display Total Fee");
    Console.WriteLine("[0] Exit");
    Console.Write("Please select your option:\n");
    int opt = Convert.ToInt32(Console.ReadLine());
    return opt;
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
try
{
    int opt = DisplayMenu();
    while (opt != 0)
    {
        if (opt == 1)
        {
            ListAllFlights(dictFlights);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 2)
        {
            ListAllBoardingGates(dictBoardingGate);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 3)
        {
            AssignBoardingGateToFlight();
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 4)
        {
            CreateFlight();
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 5)
        {
            DisplayFlightDetails(dictAirline, dictBoardingGate);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 6)
        {
            ModifyFlightDetails(dictAirline, dictBoardingGate);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 7)
        {
            DisplayScheduledFlights(dictFlights, dictBoardingGate);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 8)
        {
            AssignGatesToUnassignedFlights(dictFlights, dictBoardingGate);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else if (opt == 9)
        {
            DisplayTotalFee(terminal);
            Console.WriteLine();
            opt = DisplayMenu();
        }
        else
        {
            Console.WriteLine("Invalid Option. Try Again.");
            Console.WriteLine();
            opt = DisplayMenu();
        }
    }
}
catch(Exception e)
{ Console.WriteLine("Error! Try again"); }

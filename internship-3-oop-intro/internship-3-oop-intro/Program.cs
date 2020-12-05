using System;
using System.Collections.Generic;

namespace internship_3_oop_intro
{
    class Program
    {
        static void Main()
        {
            var eventList = new Dictionary<Event, List<Person>>()
            {
                {new Event("Kava", new DateTime(2020, 12, 06, 10, 00, 00), new DateTime(2020, 12, 06, 13, 00, 00), 0), new List<Person>(){ } },
                {new Event("Ucenje", new DateTime(2020, 12, 06, 17, 00, 00), new DateTime(2020, 12, 06, 22, 00, 00), 3), new List<Person>(){ } },
                {new Event("Koncert", new DateTime(2020, 12, 23, 20, 00, 00), new DateTime(2020, 12, 23, 00 , 00 , 00), 2), new List<Person>(){ } }
            };
            var peopleList = new List<Person> { };
            peopleList.Add(new Person("Mate Miso", "Kovac", 79858656260, 955940863));
            peopleList.Add(new Person("Josif", "Staljin", 17845569522, 983087982));
            foreach (var list in eventList)
            {

                foreach (var item in peopleList)
                {
                    list.Value.Add(item);
                }
            }
            peopleList.Add(new Person("Kreso", "Bengalka", 54896754247, 975864731));
            // Ovaj person je dodan kasnije od ostalih tako da bi postojao netko tko nije na eventima, tako da se mogu testirati funkcije//
            while (true)
            {
                Console.WriteLine("Upisite redni broj operacije koju zelite izvrsiti:" +
                    "\n1. Dodavanje eventa" +
                    "\n2. Brisanje eventa" +
                    "\n3. Edit eventa" +
                    "\n4. Dodavanje osobe na event" +
                    "\n5. Uklanjanje osobe sa eventa" +
                    "\n6. Ispis detalja eventa" +
                    "\n7. Prekid rada");
                var input = Console.ReadLine();
                Console.WriteLine();
                switch (input)
                {
                    case "1":
                        var addEvent = AddEvent(eventList);
                        if (addEvent != null)
                        {
                            if (ContinueConfirm())
                            {
                                eventList.Add(addEvent, new List<Person>() { });
                            }
                        }
                        Console.Clear();
                        break;
                    case "2":
                        DeleteEvent(eventList);
                        break;
                    case "3":
                        EditEvent(eventList);
                        break;
                    case "4":
                        AddPerson(eventList, peopleList);
                        break;
                    case "5":
                        RemovePerson(eventList, peopleList);
                        break;
                    case "6":
                        EventDetailsMain(eventList);
                        break;
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        InputError();
                        break;
                }
            }
        }
        static Event AddEvent(Dictionary<Event, List<Person>> eventList)
        {
            while (true)
            {
                int resultType = 0;
                var startTime = "";
                var endTime = "";
                DateTime parsedStartTime, parsedEndTime;
                Console.WriteLine("Ime eventa? (0 za povratak)");
                var eventName = Console.ReadLine();
                if (eventName == "0")
                {
                    Console.Clear();
                    return null;
                }
                var nameAvailable = true;
                foreach (var nameCheck in eventList)
                {
                    if (nameCheck.Key.EventName == eventName)
                    {
                        nameAvailable = false;
                    }
                }
                if (nameAvailable == false)
                {
                    Console.WriteLine("To ime je vec iskoristeno.");
                    continue;
                }
                Console.WriteLine("Tip eventa? (1-" + Enum.GetNames(typeof(Event.EventTypeList)).Length + ", 0 za povratak)");
                var i = 0;
                while (i < Enum.GetNames(typeof(Event.EventTypeList)).Length)
                {
                    Console.WriteLine(i + 1 + " - " + (Event.EventTypeList)i);
                    i++;
                }
                var eventType = Console.ReadLine();
                if (eventType == "0")
                {
                    Console.Clear();
                    return null;
                }
                else if (int.TryParse(eventType, out resultType) && resultType > 0 && resultType <= Enum.GetNames(typeof(Event.EventTypeList)).Length + 1)
                {
                    do
                    {
                        Console.WriteLine("Pocetno vrijeme? (format DD/MM/YYYY HH:MM, 0 za povratak)");
                        startTime = Console.ReadLine();
                        if (startTime == "0")
                        {
                            Console.Clear();
                            return null;
                        }
                        else if (!DateTime.TryParse(startTime, out parsedStartTime))
                        {
                            Console.Clear();
                            InputError();
                        }
                    } while (!DateTime.TryParse(startTime, out parsedStartTime));
                    do
                    {
                        Console.WriteLine("Zavrsno vrijeme? (format DD/MM/YYYY HH:MM, 0 za povratak)");
                        endTime = Console.ReadLine();
                        if (endTime == "0")
                        {
                            Console.Clear();
                            return null;
                        }
                        else if (!DateTime.TryParse(endTime, out parsedEndTime))
                        {
                            Console.Clear();
                            InputError();
                        }
                    } while (!DateTime.TryParse(endTime, out parsedEndTime));
                    var alreadyBusy = false;
                    foreach (var checkBusy in eventList.Keys)
                    {
                        if (parsedStartTime < checkBusy.EndTime && checkBusy.StartTime < parsedEndTime)
                        {
                            alreadyBusy = true;
                        }
                    }
                    if (parsedStartTime > parsedEndTime)
                    {
                        Console.Clear();
                        Console.WriteLine("Zavrsno vrijeme mora biti poslije pocetnog vremena. Pokusajte ponovno\n");
                    }
                    else if (alreadyBusy)
                    {
                        Console.Clear();
                        Console.WriteLine("U tom vremenu vec postoji event. Pokusajte ponovno\n");
                    }
                    else
                    {
                        Console.Clear();
                        var returnValue = new Event(eventName, parsedStartTime, parsedEndTime, resultType - 1);

                        return returnValue;
                    }
                }
                else
                {
                    InputError();
                    continue;
                }
            }
            return null;
        }
        static void DeleteEvent(Dictionary<Event, List<Person>> eventList)
        {
            while (true)
            {
                Console.WriteLine("Koji event zelite obsisati? (0 za povratak)");
                foreach (var eventListToDelete in eventList)
                {
                    Console.WriteLine(eventListToDelete.Key.EventName);
                }
                var eventToDelete = Console.ReadLine();
                if (eventToDelete == "0")
                {
                    Console.Clear();
                    return;
                }
                var exist = false;
                foreach (var eventListToDelete in eventList)
                {
                    if (eventToDelete == eventListToDelete.Key.EventName)
                    {
                        exist = true;
                        if (ContinueConfirm())
                        {
                            eventList.Remove(eventListToDelete.Key);
                            Console.Clear();
                            return;
                        }
                        else
                        {
                            Console.Clear();
                            continue;
                        }
                    }
                }
                if (exist == false)
                {
                    Console.Clear();
                    InputError();
                    continue;
                }
            }
        }
        static void EditEvent(Dictionary<Event, List<Person>> eventList)
        {
            while (true)
            {
                Console.WriteLine("Koji event zelite urediti? (0 za povratak)");
                foreach (var eventListToEdit in eventList)
                {
                    Console.WriteLine(eventListToEdit.Key.EventName);
                }
                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    Console.Clear();
                    return;
                }
                var exist = false;
                foreach(var eventListToEdit in eventList)
                {
                    if (userInput == eventListToEdit.Key.EventName)
                    {
                        exist = true;
                        Console.Clear();
                        var tempValue = eventListToEdit.Value;
                        eventList.Remove(eventListToEdit.Key);
                        EventDetails(new Tuple<Event, List<Person>>(eventListToEdit.Key, eventListToEdit.Value), userInput);
                        Console.WriteLine("\nUnesite nove vrijednosti:");
                        var tempEvent = (AddEvent(eventList));
                        if (tempEvent == null)
                        {
                            Console.Clear();
                            return;
                        }
                        if (ContinueConfirm())
                        {
                            eventList.Add(tempEvent, tempValue);
                            return;
                        }
                        else
                        {
                            eventList.Add(eventListToEdit.Key, eventListToEdit.Value);
                            return;
                        }
                    }
                }
                if (exist == false)
                {
                    InputError();
                    continue;
                }
            }
        }
        static void AddPerson(Dictionary<Event, List<Person>> eventList, List<Person> peopleList)
        {
            long oib;
            while (true)
            {
                Console.WriteLine("Upisite OIB osobe: (0 za povratak)");
                foreach (var person in peopleList)
                {
                    Console.WriteLine(person.FirstName + " " + person.LastName + " " + person.OIB);
                }
                var oibUntested = Console.ReadLine();
                if (oibUntested == "0")
                {
                    Console.Clear();
                    return;
                }
                if (long.TryParse(oibUntested, out oib))
                {
                }
                else
                {
                    InputError();
                    continue;
                }
                var personExist = false;
                foreach (var person in peopleList)
                {
                    if (oib == person.OIB)
                    {
                        personExist = true;

                        break;
                    }
                }
                if (personExist)
                {
                    Console.WriteLine("Na koji event zelite dodati osobu? (0 za povratak)");
                    foreach (var eventListToEdit in eventList)
                    {
                        Console.WriteLine(eventListToEdit.Key.EventName);
                    }
                    var eventInput = Console.ReadLine();
                    if (eventInput == "0")
                    {
                        Console.Clear();
                        return;
                    }
                    var eventExist = false;
                    foreach (var eventListToAdd in eventList)
                    {
                        if (eventInput == eventListToAdd.Key.EventName)
                        {
                            eventExist = true;
                            if (ContinueConfirm())
                            {
                                var alreadyOnList = false;
                                foreach(var people in eventListToAdd.Value)
                                {
                                    if (people.OIB == oib)
                                    {
                                        alreadyOnList = true;
                                        Console.Clear();
                                        Console.WriteLine("Osoba je vec an tom eventu.");
                                        return;
                                    }
                                }
                                if (!alreadyOnList)
                                {
                                    foreach (var people in peopleList)
                                    {
                                        if (people.OIB == oib)
                                        {
                                            eventListToAdd.Value.Add(people);
                                            Console.Clear();
                                            return;
                                        }
                                    }                                    
                                }
                            }
                            else
                            {
                                Console.Clear();
                                return;
                            }
                        }
                    }
                    if (eventExist == false)
                    {
                        Console.Clear();
                        InputError();
                        continue;
                    }
                }
                else
                {
                    InputError();
                    continue;
                }
            }
        }
        static void RemovePerson(Dictionary<Event, List<Person>> eventList, List<Person> peopleList)
        {
            while (true)
            {
                Console.WriteLine("Koji event zelite urediti? (0 za povratak)");
                foreach (var eventListToEdit in eventList)
                {
                    Console.WriteLine(eventListToEdit.Key.EventName);
                }
                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    Console.Clear();
                    return;
                }
                var exist = false;
                foreach (var eventListToEdit in eventList)
                {
                    if (userInput == eventListToEdit.Key.EventName)
                    {
                        exist = true;
                        Console.Clear();
                        Console.WriteLine("Na tom eventu sudjeluju:\n");
                        foreach (var person in eventListToEdit.Value)
                        {
                            Console.WriteLine(person.FirstName + " " + person.LastName + " " + person.OIB);
                        }
                        Console.WriteLine("\nUpisite OIB osobe koju zelite obrisati: (0 za povratak)");
                        var personNum = Console.ReadLine();
                        if (personNum == "0")
                        {
                            Console.Clear();
                            return;
                        }
                        else if (long.TryParse(personNum, out long personOib))
                        {
                            foreach(var person in eventListToEdit.Value)
                            {
                                if (person.OIB == personOib)
                                {
                                    if (ContinueConfirm())
                                    {
                                        eventListToEdit.Value.Remove(person);
                                        Console.Clear();
                                        return;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            InputError();
                            continue;
                        }

                    }
                }
                if (!exist)
                {
                    InputError();
                    continue;
                }
            }
        }
        static void EventDetailsMain(Dictionary<Event, List<Person>> eventList)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Koji event zelite pregledati? (0 za povratak)\n");
                foreach (var eventDetails in eventList.Keys)
                {
                    Console.WriteLine(eventDetails.EventName);
                }
                Console.WriteLine();
                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    Console.Clear();
                    return;
                }
                var exist = false;
                foreach (var eventDetails in eventList)
                {
                    if (userInput == eventDetails.Key.EventName)
                    {
                        exist = true;
                        switch (EventDetailsSubMenu())
                        {
                            case 0:
                                return;
                            case 1:
                                EventDetails(new Tuple<Event, List<Person>>(eventDetails.Key, eventDetails.Value), userInput);
                                EnterToReturn();
                                return;
                            case 2:
                                EventDetailsPeople(new Tuple<Event, List<Person>>(eventDetails.Key, eventDetails.Value));
                                EnterToReturn();
                                return;
                            case 3:
                                EventDetails(new Tuple<Event, List<Person>>(eventDetails.Key, eventDetails.Value), userInput);
                                Console.WriteLine("\nOsobe na eventu:");
                                EventDetailsPeople(new Tuple<Event, List<Person>>(eventDetails.Key, eventDetails.Value));
                                EnterToReturn();
                                return;
                        }

                    }
                }
                if (exist == false)
                {
                    InputError();
                }
            }
        }
        static int EventDetailsSubMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Upisite redni broj operacije koju zelite izvrsiti:" +
                    "\n1 - Ispis detalja eventa(ime eventa, tip eventa, pocetno vrijeme, zavrsno vrijeme, trajanje, broj ljudi na eventu)" +
                    "\n2 - Ispis osoba na eventu" +
                    "\n3 - Ispis svih detalja eventa" +
                    "\n0 - povratak");
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int returnValue))
                {
                    if (returnValue >= 0 && returnValue <= 3)
                    {
                        return returnValue;
                    }
                    else
                    {
                        InputError();
                    }
                }
                else
                {
                    InputError();
                }
            }
            return 0;
        }
        static void EventDetails(Tuple<Event, List<Person>> eventDetails, string userInput)
        {
            var numCounted = 0;
            if (eventDetails.Item1.EventName == userInput)
            {
                numCounted = eventDetails.Item2.Count;
            }
            Console.WriteLine("Ime eventa: " + eventDetails.Item1.EventName +
                "\nTip eventa: " + eventDetails.Item1.EventType +
                "\nPocetno vrijeme: " + eventDetails.Item1.StartTime +
                "\nZavrsno vrijeme: " + eventDetails.Item1.EndTime +
                "\nTrajanje: " + (eventDetails.Item1.EndTime - eventDetails.Item1.StartTime) +
                "\nBroj ljudi na eventu: " + numCounted);
        }
        static void EventDetailsPeople(Tuple<Event, List<Person>> eventDetails)
        {
            var i = 1;
            foreach (var person in eventDetails.Item2)
            {
                Console.WriteLine(i + " - " + person.FirstName + " " + person.LastName);
                i++;
            }
        }
        static void InputError()
        {
            Console.Clear();
            Console.WriteLine("Krivi unos. Pokusajte ponovno:\n");
        }
        static bool ContinueConfirm()
        {
            Console.WriteLine("Zelite li nastaviti? d/da/y/yes ako zelite nastaviti:");
            var confirm = Console.ReadLine();
            if (confirm == "d" || confirm=="da" || confirm == "y" || confirm == "yes")
                return true;
            else
                return false;
        }
        static void EnterToReturn()
        {
            Console.WriteLine("\nPritisnite enter za povratak");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
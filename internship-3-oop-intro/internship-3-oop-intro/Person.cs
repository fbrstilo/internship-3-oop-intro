using System;
using System.Collections.Generic;
using System.Text;

namespace internship_3_oop_intro
{
    public class Person
    {
        public Person(string firstName, string lastName, long oib, long cellNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            OIB = oib;
            CellNumber = cellNumber;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long OIB { get; set; }
        public long CellNumber { get; set; }

    }
}

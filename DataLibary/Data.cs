using System;
using System.Collections.Generic;

namespace DataLibary
{
    [Serializable]
    public class Data
    {
        private List<Person> persons = new List<Person>();
        private List<string> broadcastMsg = new List<string>();
        private List<string> bikeIDs = new List<string>();

        public List<string> BikeIDs
        {
            get { return bikeIDs; }
            set { bikeIDs = value; }
        }

        public List<string> BroadcastMsg
        {
            get { return broadcastMsg; }
            set { broadcastMsg = value; }
        }

        public List<Person> Persons
        {
            get { return persons; }
            set { persons = value; }
        }
    }
}
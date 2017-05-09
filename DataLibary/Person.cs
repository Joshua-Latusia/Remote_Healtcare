using System;
using System.Collections.Generic;

namespace DataLibary
{
    [Serializable]
    public class Person
    {
        private string bikeID;
        private string firstname;
        private string lastName;
        private string pictureUrl;
        private List<Session> sessions;
        private List<string> messages;
       

        public Person(string pictureUrl, string firstname, string lastName, string bikeID)
        {
            this.pictureUrl = pictureUrl;
            this.firstname = firstname;
            this.lastName = lastName;
            BikeId = bikeID;
            sessions = new List<Session>();
            messages = new List<string>();
        }

        public List<string> Messages
        {
            get { return messages; }
            set { messages = value; }
        }


        public string BikeId
        {
            get { return bikeID; }
            private set { bikeID = value; }
        }

        public string PictureUrl
        {
            get { return pictureUrl; }
            set { pictureUrl = value; }
        }

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public List<Session> Sessions
        {
            get { return sessions; }
            set { sessions = value; }
        }
    }
}
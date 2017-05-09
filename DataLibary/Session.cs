using System;
using System.Collections.Generic;

namespace DataLibary
{
    [Serializable]
    public class Session
    {
        private List<string[]> list = new List<string[]>();
        // 1. {pulse in HZ}
        // 2. {rpm}
        // 3. {speed in 0.1km/h}
        // 4. {distance in 0.1 km}
        // 5. {requested power}
        // 6. {energy in kJ}
        // 7. {time in minutes:seconds}
        // 8. {actual power}
        // 9. {currenttime}
        // 10. {ergometerID}


        private int sessionID;

        public int SessionId
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        public List<string[]> List
        {
            get { return list; }
            set { list = value; }
        }


        public void addToList(string[] data)
        {
            list.Add(data);
        }
    }
}
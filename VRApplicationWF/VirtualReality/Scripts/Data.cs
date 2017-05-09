using System.Collections.Generic;

namespace VRApplicationWF.VirtualReality.Scripts
{
    public class Clientinfo
    {
        public string host { get; set; }
    }


    public class Data
    {
        public Clientinfo clientinfo { get; set; }
        public string id { get; set; }
    }

    public class SessionList
    {
        public List<Data> data { get; set; }
    }

    //getmethod from scene

    public class Component
    {
        public List<float> position { get; set; }
        public List<float> rotation { get; set; }
        public List<double> scale { get; set; }
        public string type { get; set; }
        public string light { get; set; }
        public int? timeOfDay { get; set; }
    }

    public class Datum
    {
        public List<Component> components { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
    }

    public class Data2
    {
        public List<Datum> data { get; set; }
        public string id { get; set; }
    }

    public class DataRoot
    {
        public string id { get; set; }
        public Data2 data { get; set; }
    }

    public class RootObject
    {
        public string id { get; set; }
        public DataRoot data { get; set; }
    }
}
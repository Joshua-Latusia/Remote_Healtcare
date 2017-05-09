using System;
using System.Collections.Generic;

namespace clientserver
{
    [Serializable]
    internal class Users
    {
        private List<User> users = new List<User>();

        public List<User> Users1
        {
            get { return users; }
            set { users = value; }
        }
    }
}
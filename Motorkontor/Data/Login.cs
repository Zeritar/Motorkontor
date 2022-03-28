using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class Login
    {
        public int id { get; private set; }
        public string username { get; set; }
        public string password { get; set; }

        public bool hasChanged { get; set; } = false;

        public bool deleted { get; set; } = false;

        public Login() : this(0)
        {

        }

        public Login(int _id)
        {
            id = _id;
        }
    }
}

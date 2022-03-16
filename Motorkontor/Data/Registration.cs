using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class Registration
    {
        public int registrationId { get; private set; }
        public DateTime firstRegistrationDate { get; set; }

        public virtual Customer customer { get; set; }
        public virtual Vehicle vehicle { get; set; }

        public Registration() : this(0)
        {

        }
        
        public Registration(int _id)
        {
            registrationId = _id;
        }
    }
}

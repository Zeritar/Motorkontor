using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class Customer
    {
        public int customerID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public DateTime createDate { get; set; }

        public virtual Address address { get; set; }

        public Customer() : this(0)
        {

        }

        public Customer(int _id)
        {
            customerID = _id;
        }
    }
}

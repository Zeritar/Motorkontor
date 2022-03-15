using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class Address
    {
        public int addressId { get; private set; }
        public string streetAndNo { get; set; }
        public DateTime createDate { get; set; }
        public virtual ZipCode zipCode { get; set; }

        public Address() : this(0)
        {

        }

        public Address(int _id)
        {
            addressId = _id;
        }
    }
}

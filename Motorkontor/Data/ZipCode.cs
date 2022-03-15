using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class ZipCode
    {
        public int zipCodeId { get; private set; }
        public string zipCodeName { get; set; }
        public string cityName { get; set; }

        public ZipCode() : this(0)
        {

        }

        public ZipCode(int _id)
        {
            zipCodeId = _id;
        }
    }
}

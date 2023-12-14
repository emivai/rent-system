using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentSystem.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string entity, int id)
            : base(String.Format($"{entity} with id: {id} was not found!"))
        {

        }
        public NotFoundException(string text)
           : base(String.Format(text))
        {

        }
    }
}

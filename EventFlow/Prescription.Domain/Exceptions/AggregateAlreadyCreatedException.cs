using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain.Exceptions
{
    public class AggregateAlreadyCreatedException : InvalidOperationException
    {
        public AggregateAlreadyCreatedException(string aggregateName)
            : base($"Cannot call Create on an already existing {aggregateName}")
        {

        }
    }
}

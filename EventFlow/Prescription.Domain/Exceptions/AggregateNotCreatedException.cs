using System;

namespace Prescription.Domain.Exceptions
{
    public class AggregateNotCreatedException : InvalidOperationException
    {
        public AggregateNotCreatedException(string aggreateName, string methodName)
            : base($"Cannot call {methodName} on a newly instantiated {aggreateName}")
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace Prescription.Domain.Exceptions
{
    public class PrescriptionAlreadyReleasedException : Exception
    {
        public PrescriptionAlreadyReleasedException()
            : base("Cannot release an already released prescription")
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain.Exceptions
{
    public class EmptyPrescriptionException : InvalidOperationException
    {
        public EmptyPrescriptionException()
            : base("Cannot release an empty Prescription")
        {
        }
    }
}

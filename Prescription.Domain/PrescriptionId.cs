using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain
{
    public class PrescriptionId : Identity<PrescriptionId>
    {
        protected PrescriptionId(string value) : base(value)
        {
        }
    }
}

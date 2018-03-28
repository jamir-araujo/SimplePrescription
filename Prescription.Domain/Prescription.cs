using EventFlow.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain
{
    public class Prescription : AggregateRoot<Prescription, PrescriptionId>
    {
        protected Prescription(PrescriptionId id) : base(id)
        {
        }
    }
}

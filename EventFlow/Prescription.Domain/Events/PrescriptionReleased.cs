using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Aggregates;

namespace Prescription.Domain.Events
{
    public class PrescriptionReleased : AggregateEvent<Prescription, PrescriptionId>
    {
        public PrescriptionId PrescriptionId { get; }
        public DateTime EndDate { get; }

        public PrescriptionReleased(PrescriptionId prescriptionId, DateTime endDate)
        {
            PrescriptionId = prescriptionId;
            EndDate = endDate;
        }
    }
}

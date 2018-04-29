using System;
using EventFlow.Commands;

namespace Prescription.Domain.Commands
{
    public class ReleasePrescription : Command<Prescription, PrescriptionId>
    {
        public DateTime EndDate { get; set; }

        protected ReleasePrescription(PrescriptionId aggregateId, DateTime endDate)
            : base(aggregateId)
        {
            EndDate = endDate;
        }
    }
}

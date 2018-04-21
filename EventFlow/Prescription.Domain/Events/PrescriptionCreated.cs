using EventFlow.Aggregates;
using System;

namespace Prescription.Domain.Events
{
    public class PrescriptionCreated : AggregateEvent<Prescription, PrescriptionId>
    {
        public PrescriptionId PrescriptionId { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime CreateDate { get; set; }

        public PrescriptionCreated(
            PrescriptionId prescriptionId,
            Guid patientId,
            string patientName,
            DateTime createDate)
        {
            PrescriptionId = prescriptionId;
            PatientId = patientId;
            PatientName = patientName;
            CreateDate = createDate;
        }
    }
}

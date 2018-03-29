using EventFlow.Aggregates;
using Prescription.Domain;
using System;

namespace Prescription.Events
{
    public class PrescriptionCreated : AggregateEvent<Domain.Prescription, PrescriptionId>
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

using System;
using EventFlow.Commands;

namespace Prescription.Domain.Commands
{
    public class CreateNewPrescription : Command<Prescription, PrescriptionId>
    {
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }

        public CreateNewPrescription(PrescriptionId agreggateId, Guid patientId, string patientName)
            : base(agreggateId)
        {
            PatientId = patientId;
            PatientName = patientName;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EventFlow.Snapshots;

namespace Prescription.Domain
{
    public class PrescriptionSnapshot : ISnapshot
    {
        public Guid PrescriptionId { get; }
        public Guid PatientId { get; }
        public string PatientName { get; }
        public DateTimeOffset CreatedDate { get; }
        public List<MedicationPrescriptionSnapshot> Medications { get; }

        public PrescriptionSnapshot(
            Guid prescriptionId,
            Guid patientId, 
            string patientName,
            DateTimeOffset createdDate,
            IEnumerable<MedicationPrescription> medications)
        {
            PrescriptionId = prescriptionId;
            PatientId = patientId;
            PatientName = patientName;
            CreatedDate = createdDate;

            Medications = medications?
                .Select(m => m.GetSnapshot())
                .ToList();
        }
    }

    public class MedicationPrescriptionSnapshot
    {
        public Guid MedicationPrescriptionId { get; set; }
        public Guid PrescriptionId { get; }
        public string MedicationName { get; }
        public decimal Quantity { get; }
        public int Frequency { get; set; }
        public string AdministrationRoute { get; }
        public DateTimeOffset CreatedDate { get; }

        public MedicationPrescriptionSnapshot(
            Guid id,
            Guid prescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string administrationRoute,
            DateTimeOffset createdDate)
        {
            MedicationPrescriptionId = id;
            PrescriptionId = prescriptionId;
            MedicationName = medicationName;
            Quantity = quantity;
            Frequency = frequency;
            AdministrationRoute = administrationRoute;
            CreatedDate = createdDate;
        }
    }
}

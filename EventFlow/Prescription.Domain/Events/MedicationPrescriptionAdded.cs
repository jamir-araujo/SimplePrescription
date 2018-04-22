using System;
using EventFlow.Aggregates;

namespace Prescription.Domain.Events
{
    public class MedicationPrescriptionAdded : AggregateEvent<Prescription, PrescriptionId>
    {
        public PrescriptionId PrescriptionId { get; set; }
        public MedicationPrescriptionId MedicationPrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public decimal Quantity { get; set; }
        public int Frequency { get; set; }
        public string AdminitrationRoute { get; set; }
        public DateTime CreatedDate { get; set; }

        public MedicationPrescriptionAdded(
            MedicationPrescriptionId medicationPrescriptionId,
            PrescriptionId prescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string adminitrationRoute,
            DateTime createDate)
        {
            PrescriptionId = prescriptionId;
            MedicationPrescriptionId = medicationPrescriptionId;
            MedicationName = medicationName;
            Quantity = quantity;
            Frequency = frequency;
            AdminitrationRoute = adminitrationRoute;
            CreatedDate = createDate;
        }
    }
}

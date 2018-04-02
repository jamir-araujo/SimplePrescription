using EventFlow.Aggregates;

namespace Prescription.Domain.Events
{
    public class MedicationPrescriptionAdded : AggregateEvent<Prescription, PrescriptionId>
    {
        public PrescriptionId PrescriptionId { get; set; }
        public MedicationPrescriptionId MedicationPrescriptionId { get; set; }

        public MedicationPrescriptionAdded(PrescriptionId prescriptionId, MedicationPrescriptionId medicationPrescriptionId)
        {
            PrescriptionId = prescriptionId;
            MedicationPrescriptionId = medicationPrescriptionId;
        }
    }
}

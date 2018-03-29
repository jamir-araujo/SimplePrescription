using EventFlow.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain.Events
{
    public class MedicationPrescriptionCreated : AggregateEvent<MedicationPrescription, MedicationPrescriptionId>
    {
        public PrescriptionId PrescriptionId { get; set; }
        public MedicationPrescriptionId MedicationPrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public decimal Quantity { get; set; }
        public int Frequency { get; set; }
        public string AdminitrationRoute { get; set; }
        public DateTime CreateDate { get; set; }

        public MedicationPrescriptionCreated(
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
            CreateDate = createDate;
        }
    }
}

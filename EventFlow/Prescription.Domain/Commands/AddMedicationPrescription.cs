using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Commands;

namespace Prescription.Domain.Commands
{
    public class AddMedicationPrescription : Command<Prescription, PrescriptionId>
    {
        public MedicationPrescriptionId MedicationPrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public decimal Quantity { get; set; }
        public int Frequency { get; set; }
        public string AdminitrationRoute { get; set; }

        public AddMedicationPrescription(
            PrescriptionId agreggateId,
            MedicationPrescriptionId medicationPrescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string adminitrationRoute)
            : base(agreggateId)
        {
            MedicationPrescriptionId = medicationPrescriptionId;
            MedicationName = medicationName;
            Quantity = quantity;
            Frequency = frequency;
            AdminitrationRoute = adminitrationRoute;
        }
    }
}

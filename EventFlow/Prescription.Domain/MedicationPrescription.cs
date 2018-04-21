using EventFlow.Core;
using EventFlow.Entities;
using Prescription.Domain.Events;
using System;

namespace Prescription.Domain
{
    public class MedicationPrescription : Entity<MedicationPrescriptionId>
    {
        private PrescriptionId _prescriptionId;
        private string _medication;
        private decimal _quantity;
        private int _frequency;
        private string _administrationRoute;

        internal MedicationPrescription(
            MedicationPrescriptionId id,
            PrescriptionId prescriptionId,
            string medicationName,
            int quantity,
            int frequency,
            string adminitrationRoute)
            : base(id)
        {
            _prescriptionId = prescriptionId;
            _medication = medicationName;
            _quantity = quantity;
            _frequency = frequency;
            _administrationRoute = adminitrationRoute;
        }
    }

    public class MedicationPrescriptionId : Identity<MedicationPrescriptionId>
    {
        public MedicationPrescriptionId(string value) : base(value)
        {
        }
    }
}

using EventFlow.Core;
using EventFlow.Entities;
using Prescription.Domain.Events;
using System;

namespace Prescription.Domain
{
    public class MedicationPrescription : Entity<MedicationPrescriptionId>
    {
        private readonly DateTimeOffset _createdDate;

        private PrescriptionId _prescriptionId;
        private string _medication;
        private decimal _quantity;
        private int _frequency;
        private string _administrationRoute;

        internal MedicationPrescription(
            MedicationPrescriptionId id,
            PrescriptionId prescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string adminitrationRoute,
            DateTimeOffset createdDate)
            : base(id)
        {
            _prescriptionId = prescriptionId;
            _medication = medicationName;
            _quantity = quantity;
            _frequency = frequency;
            _administrationRoute = adminitrationRoute;

            _createdDate = createdDate;
        }

        internal MedicationPrescriptionSnapshot GetSnapshot()
        {
            return new MedicationPrescriptionSnapshot(
                Id.GetGuid(),
                _prescriptionId.GetGuid(),
                _medication,
                _quantity,
                _frequency,
                _administrationRoute,
                _createdDate);
        }
    }

    public class MedicationPrescriptionId : Identity<MedicationPrescriptionId>
    {
        public MedicationPrescriptionId(string value) : base(value)
        {
        }
    }
}

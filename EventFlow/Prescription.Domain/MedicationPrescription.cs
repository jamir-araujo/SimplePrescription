using EventFlow.Core;
using EventFlow.Entities;
using Prescription.Domain.Events;
using Prescription.Domain.Snapshot;
using System;

namespace Prescription.Domain
{
    public class MedicationPrescription : Entity<MedicationPrescriptionId>, ISnapshotProvider<MedicationPrescriptionSnapshot>
    {
        private readonly DateTime _createdDate;

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
            DateTime createdDate)
            : base(id)
        {
            _prescriptionId = prescriptionId;
            _medication = medicationName;
            _quantity = quantity;
            _frequency = frequency;
            _administrationRoute = adminitrationRoute;

            _createdDate = createdDate;
        }

        public MedicationPrescriptionSnapshot GetSnapshot()
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

        internal static MedicationPrescription New(MedicationPrescriptionSnapshot snapshot)
        {
            return new MedicationPrescription(
                MedicationPrescriptionId.With(snapshot.MedicationPrescriptionId),
                PrescriptionId.With(snapshot.PrescriptionId),
                snapshot.MedicationName,
                snapshot.Quantity,
                snapshot.Frequency,
                snapshot.AdministrationRoute,
                snapshot.CreatedDate);
        }
    }

    public class MedicationPrescriptionId : Identity<MedicationPrescriptionId>
    {
        public MedicationPrescriptionId(string value) : base(value)
        {
        }
    }
}

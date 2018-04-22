using System;
using System.Linq;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;
using Prescription.Domain.Events;
using Prescription.Domain.Snapshot;

namespace Prescription.Domain
{
    public class Prescription : AggregateRoot<Prescription, PrescriptionId>, ISnapshotableEntity<PrescriptionSnapshot>
    {
        private Guid _patientId;
        private string _patientName;
        private DateTime _createdDate;
        private List<MedicationPrescription> _medications;

        internal Prescription(PrescriptionId id)
            : base(id)
        {
            Register<PrescriptionCreated>(OnPrescriptionCreated);
            Register<MedicationPrescriptionAdded>(OnMedicationPrescriptionAdded);
        }

        public void Create(Guid patientId, string patientName)
        {
            if (!IsNew)
            {
                throw new InvalidOperationException("Cannot call Create on an already existing Prescription");
            }

            Emit(new PrescriptionCreated(Id, patientId, patientName, DateTime.UtcNow));
        }

        public void AddMedicationPrescription(
            MedicationPrescriptionId medicationPrescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string administrationRoute)
        {
            Checks.NotNull(medicationPrescriptionId, nameof(medicationPrescriptionId));
            Checks.NotNullEmptyOrWhiteSpaces(medicationName, nameof(medicationName));
            Checks.NotZeroOrLess(quantity, nameof(quantity));
            Checks.NotZeroOrLess(frequency, nameof(frequency));
            Checks.NotNullEmptyOrWhiteSpaces(administrationRoute, nameof(administrationRoute));

            if (IsNew)
            {
                throw new InvalidOperationException("Cannot call AddMedicationPrecription on a newly created Prescription");
            }

            Emit(new MedicationPrescriptionAdded(medicationPrescriptionId, Id, medicationName, quantity, frequency, administrationRoute, DateTime.UtcNow));
        }

        public void Relase(DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private void OnPrescriptionCreated(PrescriptionCreated @event)
        {
            _patientId = @event.PatientId;
            _patientName = @event.PatientName;
            _createdDate = @event.CreateDate;
        }

        private void OnMedicationPrescriptionAdded(MedicationPrescriptionAdded @event)
        {
            _medications = _medications ?? new List<MedicationPrescription>();
            _medications.Add(new MedicationPrescription(
                @event.MedicationPrescriptionId,
                @event.PrescriptionId,
                @event.MedicationName,
                @event.Quantity,
                @event.Frequency,
                @event.AdminitrationRoute,
                @event.CreatedDate));
        }

        public PrescriptionSnapshot GetSnapshot()
        {
            return new PrescriptionSnapshot(
                Id.GetGuid(),
                _patientId,
                _patientName,
                _createdDate,
                _medications);
        }

        public void LoadFromSnapshot(PrescriptionSnapshot snapshot)
        {
            _patientId = snapshot.PatientId;
            _patientName = snapshot.PatientName;
            _createdDate = snapshot.CreatedDate;

            _medications = snapshot.Medications?
                .Select(MedicationPrescription.New)
                .ToList();

            Version++;
        }
    }

    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class PrescriptionId : Identity<PrescriptionId>
    {
        public PrescriptionId(string value) : base(value)
        {
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;
using Prescription.Domain.Events;
using Prescription.Domain.Snapshot;
using Prescription.Domain.Exceptions;

namespace Prescription.Domain
{
    public class Prescription : AggregateRoot<Prescription, PrescriptionId>, ISnapshotableEntity<PrescriptionSnapshot>
    {
        private Guid _patientId;
        private string _patientName;
        private DateTime _createdDate;
        private List<MedicationPrescription> _medications;
        private DateTime? _endDate;

        internal Prescription(PrescriptionId id)
            : base(id)
        {
            Register<PrescriptionCreated>(OnPrescriptionCreated);
            Register<MedicationPrescriptionAdded>(OnMedicationPrescriptionAdded);
            Register<PrescriptionReleased>(OnPrescriptionReleased);
        }

        public void Create(Guid patientId, string patientName)
        {
            if (!IsNew)
            {
                throw new AggregateAlreadyCreatedException(nameof(Prescription));
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
                throw new AggregateNotCreatedException(nameof(Prescription), nameof(AddMedicationPrescription));
            }

            Emit(new MedicationPrescriptionAdded(medicationPrescriptionId, Id, medicationName, quantity, frequency, administrationRoute, DateTime.UtcNow));
        }

        public void Relase(DateTime endDate)
        {
            Checks.NotInThePast(endDate, DateTime.UtcNow, nameof(endDate));

            if (IsNew)
            {
                throw new AggregateNotCreatedException(nameof(Prescription), nameof(Relase));
            }

            if (_medications == null || _medications.Count == 0)
            {
                throw new EmptyPrescriptionException();
            }

            if (_endDate.HasValue)
            {
                throw new PrescriptionAlreadyReleasedException();
            }

            Emit(new PrescriptionReleased(Id, endDate));
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

        private void OnPrescriptionReleased(PrescriptionReleased @event)
        {
            _endDate = @event.EndDate;
        }

        public PrescriptionSnapshot GetSnapshot()
        {
            return new PrescriptionSnapshot(
                Version,
                Id.GetGuid(),
                _patientId,
                _patientName,
                _createdDate,
                _medications,
                _endDate);
        }

        public void LoadFromSnapshot(PrescriptionSnapshot snapshot)
        {
            Version = snapshot.Version;
            _patientId = snapshot.PatientId;
            _patientName = snapshot.PatientName;
            _createdDate = snapshot.CreatedDate;
            _endDate = snapshot.EndDate;

            _medications = snapshot.Medications?
                .Select(MedicationPrescription.New)
                .ToList();
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

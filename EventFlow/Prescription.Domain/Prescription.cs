using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;
using Prescription.Domain.Events;

namespace Prescription.Domain
{
    public class Prescription : AggregateRoot<Prescription, PrescriptionId>
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

            Emit(new MedicationPrescriptionAdded(medicationPrescriptionId, Id, medicationName, quantity, frequency, administrationRoute, DateTimeOffset.Now));
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
            //_medications.Add(@event.MedicationPrescriptionId);
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

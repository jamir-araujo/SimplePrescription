using EventFlow.Aggregates;
using EventFlow.Core;
using Prescription.Domain.Events;
using System;

namespace Prescription.Domain
{
    public class MedicationPrescription : AggregateRoot<MedicationPrescription, MedicationPrescriptionId>
    {
        private PrescriptionId _prescriptionId;
        private string _medication;
        private decimal _quantity;
        private int _frequency;
        private string _administrationRoute;

        protected MedicationPrescription(MedicationPrescriptionId id)
            : base(id)
        {
            Register<MedicationPrescriptionCreated>(OnMedicationPrescriptionCreated);
        }

        public MedicationPrescription(
            MedicationPrescriptionId id,
            PrescriptionId prescriptionId,
            string medicationName,
            int quantity,
            int frequency,
            string adminitrationRoute)
            : this(id)
        {
            Emit(new MedicationPrescriptionCreated(id, prescriptionId, medicationName, quantity, frequency, adminitrationRoute, DateTime.UtcNow));
        }

        private void OnMedicationPrescriptionCreated(MedicationPrescriptionCreated @event)
        {
            _prescriptionId = @event.PrescriptionId;
            _medication = @event.MedicationName;
            _quantity = @event.Quantity;
            _frequency = @event.Frequency;
            _administrationRoute = @event.AdminitrationRoute;
        }
    }

    public class MedicationPrescriptionId : Identity<MedicationPrescriptionId>
    {
        public MedicationPrescriptionId(string value) : base(value)
        {
        }
    }
}

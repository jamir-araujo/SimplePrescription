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

        internal MedicationPrescription(MedicationPrescriptionId id)
            : base(id)
        {
            Register<MedicationPrescriptionCreated>(OnMedicationPrescriptionCreated);
        }

        public void Create(PrescriptionId prescriptionId, string medicationName, int quantity, int frequency, string adminitrationRoute)
        {
            if (!IsNew)
            {
                throw new InvalidOperationException("Cannot call Create on an already existing MedicationPrescription");
            }

            Emit(new MedicationPrescriptionCreated(Id, prescriptionId, medicationName, quantity, frequency, adminitrationRoute, DateTime.UtcNow));
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

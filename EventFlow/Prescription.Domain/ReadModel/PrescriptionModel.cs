using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Prescription.Domain.Events;

namespace Prescription.Domain.ReadModel
{
    public class PrescriptionModelIReadModel :
        IReadModel,
        IAmReadModelFor<Prescription, PrescriptionId, PrescriptionCreated>,
        IAmReadModelFor<Prescription, PrescriptionId, MedicationPrescriptionAdded>,
        IAmReadModelFor<Prescription, PrescriptionId, PrescriptionReleased>
    {
        private List<MedicationPrescriptionModel> _medications;

        public Guid Id { get; private set; }
        public Guid PatientId { get; private set; }
        public string PatientName { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public IReadOnlyCollection<MedicationPrescriptionModel> Medications { get => _medications; }

        void IAmReadModelFor<Prescription, PrescriptionId, PrescriptionCreated>.Apply(IReadModelContext context, IDomainEvent<Prescription, PrescriptionId, PrescriptionCreated> domainEvent)
        {
            var @event = domainEvent.AggregateEvent;

            Id = @event.PrescriptionId.GetGuid();
            PatientId = @event.PatientId;
            PatientName = @event.PatientName;
            CreateDate = @event.CreateDate;
        }

        void IAmReadModelFor<Prescription, PrescriptionId, MedicationPrescriptionAdded>.Apply(IReadModelContext context, IDomainEvent<Prescription, PrescriptionId, MedicationPrescriptionAdded> domainEvent)
        {
            var @event = domainEvent.AggregateEvent;

            _medications = _medications ?? new List<MedicationPrescriptionModel>();

            _medications.Add(new MedicationPrescriptionModel(
                @event.MedicationPrescriptionId.GetGuid(),
                @event.PrescriptionId.GetGuid(),
                @event.MedicationName,
                @event.Quantity,
                @event.Frequency,
                @event.AdminitrationRoute,
                @event.CreatedDate));
        }

        void IAmReadModelFor<Prescription, PrescriptionId, PrescriptionReleased>.Apply(IReadModelContext context, IDomainEvent<Prescription, PrescriptionId, PrescriptionReleased> domainEvent)
        {
            EndDate = domainEvent.AggregateEvent.EndDate;
        }
    }

    public class MedicationPrescriptionModel
    {
        public Guid Id { get; }
        private Guid PrescriptionId { get; }
        private string Medication { get; }
        private decimal Quantity { get; }
        private int Frequency { get; }
        private string AdministrationRoute { get; }
        public DateTime CreatedDate { get; }

        internal MedicationPrescriptionModel(
            Guid id,
            Guid prescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string adminitrationRoute,
            DateTime createdDate)
        {
            Id = id;
            PrescriptionId = prescriptionId;
            Medication = medicationName;
            Quantity = quantity;
            Frequency = frequency;
            AdministrationRoute = adminitrationRoute;
            CreatedDate = createdDate;
        }
    }
}

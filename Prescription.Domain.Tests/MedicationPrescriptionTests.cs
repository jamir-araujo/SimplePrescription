using Prescription.Domain.Events;
using System;
using System.Linq;
using Xunit;
using Prescription.Domain;

namespace Prescription.Tests
{
    public class MedicationPrescriptionTests
    {
        private readonly MedicationPrescriptionId _sutId;
        private readonly MedicationPrescription _sut;
        private readonly PrescriptionId _prescriptionId;
        private readonly string _medicationName;
        private readonly int _quantity;
        private readonly int _frequency;
        private readonly string _adminitrationRoute;

        public MedicationPrescriptionTests()
        {
            _sutId = MedicationPrescriptionId.New;
            _sut = new MedicationPrescription(_sutId);

            _prescriptionId = PrescriptionId.New;
            _medicationName = "aas";
            _quantity = 1;
            _frequency = 2;
            _adminitrationRoute = "oral";
        }

        [Fact]
        public void Create_Should_EmitMedicationPrescriptionCreatedEvent()
        {
            _sut.Create(_prescriptionId, _medicationName, _quantity, _frequency, _adminitrationRoute);

            var uncommitedEvents = _sut.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<MedicationPrescriptionCreated>(uncommitedEvent.AggregateEvent);
            Assert.Equal(_sutId, @event.MedicationPrescriptionId);
            Assert.Equal(_prescriptionId, @event.PrescriptionId);
            Assert.Equal(_medicationName, @event.MedicationName);
            Assert.Equal(_frequency, @event.Frequency);
            Assert.Equal(_adminitrationRoute, @event.AdminitrationRoute);
            Assert.InRange(@event.CreateDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        }

        [Fact]
        public void Create_Should_Throw_When_AggregateIsNotNew()
        {
            _sut.Create(_prescriptionId, _medicationName, _quantity, _frequency, _adminitrationRoute);
            Assert.Throws<InvalidOperationException>(() => _sut.Create(_prescriptionId, _medicationName, _quantity, _frequency, _adminitrationRoute));
        }
    }
}

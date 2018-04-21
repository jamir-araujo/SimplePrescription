using Prescription.Domain;
using Prescription.Domain.Events;
using System;
using System.Linq;
using Xunit;

namespace Prescription.Tests
{
    public class PrescriptionTests
    {
        private readonly PrescriptionId _sutId;
        private readonly Domain.Prescription _sut;
        private readonly Guid _patientId;
        private readonly string _patientName;

        public PrescriptionTests()
        {
            _sutId = PrescriptionId.New;
            _sut = new Domain.Prescription(_sutId);

            _patientId = Guid.NewGuid();
            _patientName = "John Doe";

            _sut.ApplyEvents(new[] { new PrescriptionCreated(_sutId, _patientId, _patientName, DateTime.UtcNow) });
        }

        [Fact]
        public void Create_Should_EmitPrescriptionCreatedEventAndSetTheStateCorrectly()
        {
            var newSut = new Domain.Prescription(_sutId);
            newSut.Create(_patientId, _patientName);

            var uncommitedEvents = newSut.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<PrescriptionCreated>(uncommitedEvent.AggregateEvent);
            Assert.Equal(_sutId, @event.PrescriptionId);
            Assert.Equal(_patientId, @event.PatientId);
            Assert.Equal(_patientName, @event.PatientName);
            Assert.InRange(@event.CreateDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);

            var sutSnapshot = newSut.GetSnapshot();

            Assert.Equal(_sutId.GetGuid(), sutSnapshot.PrescriptionId);
            Assert.Equal(_patientId, sutSnapshot.PatientId);
            Assert.Equal(_patientName, sutSnapshot.PatientName);
            Assert.InRange(sutSnapshot.CreatedDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        }

        [Fact]
        public void Create_Should_Throw_When_AggregateIsNotNew()
        {
            Assert.Throws<InvalidOperationException>(() => _sut.Create(_patientId, _patientName));
        }

        [Fact]
        public void AddMedicationPrescription_Should_Throw_When_MedicationPrescriptionIdIsNull()
        {
            Assert.Throws<ArgumentNullException>("medicationPrescriptionId", () => _sut.AddMedicationPrescription(null, "medicatioName", 1, 1, "administrationRoute"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddMedicationPrescription_Should_Throw_When_MedicationNameIdIsNullEmptyOrWhiteSpaces(string medicatioName)
        {
            Assert.Throws<ArgumentNullException>("medicationName", () => _sut.AddMedicationPrescription(MedicationPrescriptionId.New, medicatioName, 1, 1, "administrationRoute"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddMedicationPrescription_Should_Throw_When_QuantityIsZeroOrLess(decimal quantity)
        {
            Assert.Throws<InvalidOperationException>(() => _sut.AddMedicationPrescription(MedicationPrescriptionId.New, "medicationName", quantity, 1, "administrationRoute"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddMedicationPrescription_Should_Throw_When_FrequencyIsZeroOrLess(int frequency)
        {
            Assert.Throws<InvalidOperationException>(() => _sut.AddMedicationPrescription(MedicationPrescriptionId.New, "medicationName", 1, frequency, "administrationRoute"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AddMedicationPrescription_Should_Throw_When_AdministrationRouteIsNullEmptyOrWhiteSpaces(string administrationRoute)
        {
            Assert.Throws<ArgumentNullException>(() => _sut.AddMedicationPrescription(MedicationPrescriptionId.New, "medicationName", 1, 1, administrationRoute));
        }

        [Fact]
        public void AddMedicationPrescription_Should_EmitMedicationAddedEventAndAddTheMedicationToTheInternalList()
        {
            var medicationPrescriptionId = MedicationPrescriptionId.New;
            var medicationName = "medicationName";
            var quantity = 1M;
            var frequency = 1;
            var administrationRoute = "administrationRoute";

            _sut.AddMedicationPrescription(
                medicationPrescriptionId, 
                medicationName,
                quantity,
                frequency,
                administrationRoute);

            var uncommitedEvents = _sut.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<MedicationPrescriptionAdded>(uncommitedEvent.AggregateEvent);
            Assert.Equal(medicationPrescriptionId, @event.MedicationPrescriptionId);
            Assert.Equal(medicationName, @event.MedicationName);
            Assert.Equal(quantity, @event.Quantity);
            Assert.Equal(frequency, @event.Frequency);
            Assert.Equal(administrationRoute, @event.AdminitrationRoute);
            Assert.InRange(@event.CreatedDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);

            var sutSnapshot = _sut.GetSnapshot();

            var medicationPrescription = Assert.Single(sutSnapshot.Medications);
            Assert.Equal(_sutId.GetGuid(), medicationPrescription.PrescriptionId);
            Assert.Equal(medicationPrescriptionId.GetGuid(), medicationPrescription.MedicationPrescriptionId);
            Assert.Equal(medicationName, medicationPrescription.MedicationName);
            Assert.Equal(quantity, medicationPrescription.Quantity);
            Assert.Equal(frequency, medicationPrescription.Frequency);
            Assert.Equal(administrationRoute, medicationPrescription.AdministrationRoute);
            Assert.InRange(medicationPrescription.CreatedDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        }

        [Fact]
        public void AddMedicationPrescription_Should_Throw_When_AggregateIsNew()
        {
            var newSut = new Domain.Prescription(PrescriptionId.New);

            var medicationPrescriptionId = MedicationPrescriptionId.New;
            var medicationName = "medicationName";
            var quantity = 1M;
            var frequency = 1;
            var administrationRoute = "administrationRoute";

            Assert.Throws<InvalidOperationException>(() => newSut.AddMedicationPrescription(medicationPrescriptionId, medicationName, quantity, frequency, administrationRoute));
        }
    }
}

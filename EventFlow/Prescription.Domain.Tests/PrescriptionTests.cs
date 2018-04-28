using Prescription.Domain;
using Prescription.Domain.Events;
using Prescription.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Prescription.Tests
{
    public class PrescriptionTests
    {
        private readonly int _sutInitialVersion;
        private readonly PrescriptionId _sutId;
        private readonly Domain.Prescription _sut;
        private readonly Guid _patientId;
        private readonly string _patientName;
        private readonly PrescriptionSnapshot _initialState;

        public PrescriptionTests()
        {
            _sutInitialVersion = 1;
            _sutId = PrescriptionId.New;
            _sut = new Domain.Prescription(_sutId);
            _patientId = Guid.NewGuid();
            _patientName = "John Doe";

            _initialState = new PrescriptionSnapshot(
                _sutInitialVersion,
                _sutId.GetGuid(),
                _patientId,
                _patientName,
                DateTime.UtcNow,
                null,
                null);

            _sut.LoadFromSnapshot(_initialState);
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

            var endState = newSut.GetSnapshot();

            Assert.Equal(newSut.Version, endState.Version);
            Assert.Equal(_sutId.GetGuid(), endState.PrescriptionId);
            Assert.Equal(_patientId, endState.PatientId);
            Assert.Equal(_patientName, endState.PatientName);
            Assert.InRange(endState.CreatedDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
            Assert.Null(endState.EndDate);
        }

        [Fact]
        public void Create_Should_Throw_When_AggregateIsNotNew()
        {
            Assert.Throws<AggregateAlreadyCreatedException>(() => _sut.Create(_patientId, _patientName));
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

            var endState = _sut.GetSnapshot();

            Assert.Equal(_sut.Version, endState.Version);
            var medicationPrescription = Assert.Single(endState.Medications);
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

            Assert.Throws<AggregateNotCreatedException>(() => newSut.AddMedicationPrescription(medicationPrescriptionId, medicationName, quantity, frequency, administrationRoute));
        }

        [Fact]
        public void Release_Should_EmitAPrescriptionReleasedEventAndSetTheEndDate()
        {
            var initialState = new PrescriptionSnapshot(
                _initialState.Version + 1,
                _initialState.PrescriptionId,
                _initialState.PatientId,
                _initialState.PatientName,
                _initialState.CreatedDate,
                new List<MedicationPrescription>(),
                _initialState.EndDate);

            initialState.Medications.Add(new MedicationPrescriptionSnapshot(
                MedicationPrescriptionId.New.GetGuid(),
                initialState.PrescriptionId,
                "medicationName",
                1,
                1,
                "administrationRoute",
                DateTime.UtcNow));

            _sut.LoadFromSnapshot(initialState);

            var prescriptionEndDate = DateTime.UtcNow.AddDays(1);

            _sut.Relase(prescriptionEndDate);

            var uncommitedEvents = _sut.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<PrescriptionReleased>(uncommitedEvent.AggregateEvent);
            Assert.Equal(_sut.Id, @event.PrescriptionId);
            Assert.Equal(prescriptionEndDate, @event.EndDate);

            var endState = _sut.GetSnapshot();

            Assert.Equal(_sut.Version, endState.Version);
            Assert.Equal(prescriptionEndDate, endState.EndDate);
        }

        [Fact]
        public void Release_Should_Throw_When_PrescriptionIsNew()
        {
            var newSut = new Domain.Prescription(PrescriptionId.New);

            Assert.Throws<AggregateNotCreatedException>(() => newSut.Relase(DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void Release_Should_Throw_When_EndDateIsInThePass()
        {
            Assert.Throws<InvalidOperationException>(() => _sut.Relase(DateTime.UtcNow.AddDays(-1)));
        }

        [Fact]
        public void Release_Should_Throw_When_PrescriptionHasNotMedications()
        {
            Assert.Throws<EmptyPrescriptionException>(() => _sut.Relase(DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void Release_Should_Throw_When_PrescriptionHasAlreadyBeenReleased()
        {
            var state = new PrescriptionSnapshot(
                3,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "PatientName",
                DateTime.Now,
                new List<MedicationPrescription>(),
                DateTime.Now);

            state.Medications.Add(new MedicationPrescriptionSnapshot(
                MedicationPrescriptionId.New.GetGuid(),
                state.PrescriptionId,
                "medicationName",
                1,
                1,
                "administrationRoute",
                DateTime.UtcNow));

            _sut.LoadFromSnapshot(state);

            Assert.Throws<PrescriptionAlreadyReleasedException>(() => _sut.Relase(DateTime.UtcNow.AddDays(1)));
        }
    }
}

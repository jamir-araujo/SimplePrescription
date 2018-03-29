using Prescription.Events;
using System;
using System.Linq;
using Xunit;

namespace Prescription.Domain.Tests
{
    public class PrescriptionTests
    {
        [Fact]
        public void Constructor_Should_EmitPrescriptionCreatedEvent()
        {
            var prescriptionId = PrescriptionId.New;
            var patientId = Guid.NewGuid();
            var patientName = "John Doe";

            var prescription = new Prescription(prescriptionId, patientId, patientName);

            var uncommitedEvents = prescription.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<PrescriptionCreated>(uncommitedEvent.AggregateEvent);
            Assert.Equal(prescriptionId, @event.PrescriptionId);
            Assert.Equal(patientId, @event.PatientId);
            Assert.Equal(patientName, @event.PatientName);
            Assert.InRange(@event.CreateDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        }
    }
}

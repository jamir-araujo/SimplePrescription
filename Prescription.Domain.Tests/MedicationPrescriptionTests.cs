using Prescription.Domain.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Prescription.Domain.Tests
{
    public class MedicationPrescriptionTests
    {
        [Fact]
        public void Constructor_Should_EmitMedicationPrescriptionCreatedEvent()
        {
            var prescriptionId = PrescriptionId.New;
            var medicationPrescriptionId = MedicationPrescriptionId.New;
            var medicationName = "aas";
            var quantity = 1;
            var frequency = 2;
            var adminitrationRoute = "oral";

            var medicationPrescription = new MedicationPrescription(
                medicationPrescriptionId,
                prescriptionId,
                medicationName,
                quantity,
                frequency,
                adminitrationRoute);

            var uncommitedEvents = medicationPrescription.UncommittedEvents.ToList();

            var uncommitedEvent = Assert.Single(uncommitedEvents);
            var @event = Assert.IsType<MedicationPrescriptionCreated>(uncommitedEvent.AggregateEvent);
            Assert.Equal(medicationPrescriptionId, @event.MedicationPrescriptionId);
            Assert.Equal(prescriptionId, @event.PrescriptionId);
            Assert.Equal(medicationName, @event.MedicationName);
            Assert.Equal(frequency, @event.Frequency);
            Assert.Equal(adminitrationRoute, @event.AdminitrationRoute);
            Assert.InRange(@event.CreateDate, DateTime.UtcNow.AddSeconds(-1), DateTime.UtcNow);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Prescription.Domain;
using Prescription.Domain.Commands;

namespace Prescription.Application.CommandHandlers
{
    public class AddMedicationPrescriptionHandler : CommandHandler<Domain.Prescription, PrescriptionId, AddMedicationPrescription>
    {
        public override Task ExecuteAsync(Domain.Prescription aggregate, AddMedicationPrescription command, CancellationToken cancellationToken)
        {
            aggregate.AddMedicationPrescription(
                command.MedicationPrescriptionId,
                command.MedicationName,
                command.Quantity,
                command.Frequency,
                command.AdminitrationRoute);

            return Task.CompletedTask;
        }
    }
}

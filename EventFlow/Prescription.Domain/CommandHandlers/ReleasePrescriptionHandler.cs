using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Prescription.Domain;
using Prescription.Domain.Commands;

namespace Prescription.Application.CommandHandlers
{
    public class ReleasePrescriptionHandler : CommandHandler<Domain.Prescription, PrescriptionId, ReleasePrescription>
    {
        public override Task ExecuteAsync(Domain.Prescription aggregate, ReleasePrescription command, CancellationToken cancellationToken)
        {
            aggregate.Relase(command.EndDate);

            return Task.CompletedTask;
        }
    }
}

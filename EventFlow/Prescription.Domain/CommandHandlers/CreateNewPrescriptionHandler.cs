﻿using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Prescription.Domain;
using Prescription.Domain.Commands;

namespace Prescription.Application.CommandHandlers
{
    public class CreateNewPrescriptionHandler : CommandHandler<Domain.Prescription, PrescriptionId, CreateNewPrescription>
    {
        public override Task ExecuteAsync(Domain.Prescription aggregate, CreateNewPrescription command, CancellationToken cancellationToken)
        {
            aggregate.Create(command.PatientId, command.PatientName);

            return Task.CompletedTask;
        }
    }
}

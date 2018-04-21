using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Commands;

namespace Prescription.Domain.Commands
{
    public class AddMedicationPrescription : Command<Prescription, PrescriptionId>
    {
        public AddMedicationPrescription(PrescriptionId id)
            : base(id)
        {

        }
    }
}

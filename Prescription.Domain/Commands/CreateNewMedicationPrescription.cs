using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Commands;

namespace Prescription.Domain.Commands
{
    public class CreateNewMedicationPrescription : Command<MedicationPrescription, MedicationPrescriptionId>
    {
        public CreateNewMedicationPrescription(MedicationPrescriptionId id)
            : base(id)
        {

        }
    }
}

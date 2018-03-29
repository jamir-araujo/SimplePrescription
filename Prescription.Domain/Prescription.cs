using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;
using Prescription.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prescription.Domain
{
    public class Prescription : AggregateRoot<Prescription, PrescriptionId>
    {
        private Guid _patientId;
        private string _pattineName;
        private DateTime _createdDate;

        protected Prescription(PrescriptionId id) 
            : base(id)
        {
            Register<PrescriptionCreated>(OnPrescriptionCreated);
        }

        public Prescription(PrescriptionId id, Guid patientId, string patientName)
            : this(id)
        {
            Emit(new PrescriptionCreated(Id, patientId, patientName, DateTime.UtcNow));
        }

        private void OnPrescriptionCreated(PrescriptionCreated prescriptionCreated)
        {
            _patientId = prescriptionCreated.PatientId;
            _pattineName = prescriptionCreated.PatientName;
            _createdDate = prescriptionCreated.CreateDate;
        }
    }

    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class PrescriptionId : Identity<PrescriptionId>
    {
        public PrescriptionId(string value) : base(value)
        {
        }
    }
}

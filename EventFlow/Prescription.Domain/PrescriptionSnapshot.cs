﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EventFlow.Snapshots;
using Prescription.Domain.Snapshot;

namespace Prescription.Domain
{
    public class PrescriptionSnapshot : VersionedSnapshot
    {
        public Guid PrescriptionId { get; }
        public Guid PatientId { get; }
        public string PatientName { get; }
        public DateTime CreatedDate { get; }
        public List<MedicationPrescriptionSnapshot> Medications { get; }
        public DateTime? EndDate { get; }

        public PrescriptionSnapshot(
            int version,
            Guid prescriptionId,
            Guid patientId,
            string patientName,
            DateTime createdDate,
            IEnumerable<MedicationPrescription> medications,
            DateTime? endDate)
            : base(version)
        {
            PrescriptionId = prescriptionId;
            PatientId = patientId;
            PatientName = patientName;
            CreatedDate = createdDate;
            EndDate = endDate;

            Medications = medications?
                .Select(m => m.GetSnapshot())
                .ToList();
        }
    }

    public class MedicationPrescriptionSnapshot
    {
        public Guid MedicationPrescriptionId { get; set; }
        public Guid PrescriptionId { get; }
        public string MedicationName { get; }
        public decimal Quantity { get; }
        public int Frequency { get; set; }
        public string AdministrationRoute { get; }
        public DateTime CreatedDate { get; }

        public MedicationPrescriptionSnapshot(
            Guid id,
            Guid prescriptionId,
            string medicationName,
            decimal quantity,
            int frequency,
            string administrationRoute,
            DateTime createdDate)
        {
            MedicationPrescriptionId = id;
            PrescriptionId = prescriptionId;
            MedicationName = medicationName;
            Quantity = quantity;
            Frequency = frequency;
            AdministrationRoute = administrationRoute;
            CreatedDate = createdDate;
        }
    }
}

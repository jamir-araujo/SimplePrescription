using Prescription.Domain.Events;
using System;
using System.Linq;
using Xunit;
using Prescription.Domain;

namespace Prescription.Tests
{
    public class MedicationPrescriptionTests
    {
        private readonly MedicationPrescriptionId _sutId;
        private readonly MedicationPrescription _sut;
        private readonly PrescriptionId _prescriptionId;
        private readonly string _medicationName;
        private readonly int _quantity;
        private readonly int _frequency;
        private readonly string _adminitrationRoute;

        //public MedicationPrescriptionTests()
        //{
        //    _sutId = MedicationPrescriptionId.New;
        //    _sut = new MedicationPrescription(_sutId);

        //    _prescriptionId = PrescriptionId.New;
        //    _medicationName = "aas";
        //    _quantity = 1;
        //    _frequency = 2;
        //    _adminitrationRoute = "oral";
        //}
    }
}

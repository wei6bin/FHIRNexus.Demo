// -------------------------------------------------------------------------------------------------
// Copyright (c) Integrated Health Information Systems Pte Ltd. All rights reserved.
// -------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Hl7.Fhir.Model;
using Ihis.FhirEngine.Data.Models;

namespace Fhir.Relational.Demo2.Data;

[GeneratedFhirPoco<Appointment>("Conformance/Appointment-custom.StructureDefinition.json")]
public partial class AppointmentEntity
{
    [FhirExtensionMapping("https://fhir.synapxe.sg/StructureDefinition/ext_supportingdoc_version")]
    public string BusinessVersion { get; set; }
    public CustomIdentifierEntity Identifier { get; set; }
}
[Hl7.Fhir.Introspection.FhirType("Identifier", "http://hl7.org/fhir/StructureDefinition/Identifier", IsResource = false)]
public class CustomIdentifierEntity : BaseOwnedEntity
{
    [StringLength(255)]
    public string Value { get; set; }

    public PeriodEntity? Period { get; set; }
}

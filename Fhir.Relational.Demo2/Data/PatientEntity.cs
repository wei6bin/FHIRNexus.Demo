using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;
using Ihis.FhirEngine.Data.Models;

namespace Fhir.Relational.Demo2.Data;

[GeneratedFhirPoco<Patient>("Conformance/MyPatient.StructureDefinition.json")]
public partial class PatientEntity
{
    [FhirExtensionMapping("http://hl7.org/fhir/StructureDefinition/patient-animal")]
    public PatientAnimal? Animal { get; set; }

    [FhirType("Extension", IsResource = false)]
    public class PatientAnimal : BaseOwnedEntity
    {
        [FhirExtensionMapping("species")]
        public CodeableConceptSlimEntity Species { get; set; }

        [FhirExtensionMapping("breed")]
        public CodeableConceptSlimEntity Breed { get; set; }

        [FhirExtensionMapping("genderStatus")]
        public CodeableConceptSlimEntity GenderStatus { get; set; }

        [FhirType("CodeableConcept", "http://hl7.org/fhir/StructureDefinition/CodeableConcept", IsResource = false)]
        public class CodeableConceptSlimEntity : BaseOwnedEntity
        {
            public string? Text { get; set; }

            public CodingEntity? Coding { get; set; }
        }
    }
}

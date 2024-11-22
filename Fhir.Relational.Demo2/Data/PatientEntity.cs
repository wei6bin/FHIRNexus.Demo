using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;
using Ihis.FhirEngine.Data.Models;

namespace Fhir.Relational.Demo2.Data;

[GeneratedFhirPoco<Patient>("Conformance/MyPatient.StructureDefinition.json")]
public partial class PatientEntity
{
    [FhirExtensionMapping("http://hl7.org/fhir/StructureDefinition/patient-animal")]
    public PatientAnimal? Animal { get; set; }

    [FhirExtensionMapping("http://hl7.org/fhir/StructureDefinition/patient-birthTime")]
    public DateTimeEntity? BirthTime { get; set; }

    public List<CommunicationComponent> Communication { get; set; } = [];

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

    [FhirType("Patient#Communication", IsNestedType = true)]
    public class CommunicationComponent : BackboneEntity
    {
        public CodeableConceptSlimEntity? Language { get; set; }

        [FhirExtensionMapping("http://hl7.org/fhir/StructureDefinition/patient-proficiency")]
        public Proficiency? Proficiency { get; set; }

        public bool? Preferred { get; set; }
    }

    [FhirType("Extension", IsResource = false)]
    public class Proficiency : BaseOwnedEntity
    {
        [FhirExtensionMapping("level")]
        public CodeableConceptSlimEntity Level { get; set; }

        [FhirExtensionMapping("type")]
        public CodeableConceptSlimEntity Type { get; set; }

        [FhirType("CodeableConcept", "http://hl7.org/fhir/StructureDefinition/CodeableConcept", IsResource = false)]
        public class CodeableConceptSlimEntity : BaseOwnedEntity
        {
            public string? Text { get; set; }

            public CodingEntity? Coding { get; set; }
        }
    }
}

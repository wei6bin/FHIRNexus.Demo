using System.Net;
using Hl7.Fhir.Model;
using Ihis.FhirEngine.Core;
using Task = System.Threading.Tasks.Task;

namespace Synapxe.FhirWebApi.NoSQL.Handlers;

[FhirHandlerClass(AcceptedType = nameof(Patient))]
public class PatientHandler
{
    [FhirHandler("PostInteraction", HandlerCategory.PostInteraction, FhirInteractionType.SearchType)]
    public Task ValidateNoAppointmentConflictAsync(IFhirContext context, CancellationToken cancellationToken)
    {
        var searchResult = context.Response.TakeAllResources().FirstOrDefault();
        foreach (var resource in (searchResult?.Resource as Bundle)?.Entry)
        {
            // Assuming resource is of type ResourceProxy and has a Resource property
            if (resource.Resource is not Patient patient || patient.Name == null) continue;

            var hasJack = patient.Name.Any(n =>
                n.Text != null && n.Text.Contains("Jack", StringComparison.OrdinalIgnoreCase));
            if (!hasJack)
            {
                continue;
            }

            context.Response.AddResource(patient);
        }

        context.Response.StatusCode = HttpStatusCode.OK;

        return Task.CompletedTask;
    }
}

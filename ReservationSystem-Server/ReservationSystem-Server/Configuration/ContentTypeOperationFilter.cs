using JetBrains.Annotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReservationSystem_Server.Configuration;

/// <summary>
/// A swagger operation filter that removes all response types other than application/json
/// </summary>
[UsedImplicitly]
public class ContentTypeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody != null)
        {
            operation.RequestBody.Content = operation.RequestBody.Content
                .Where(x => x.Key == "application/json")
                .ToDictionary(x => x.Key, x => x.Value);
        }

        foreach (KeyValuePair<string, OpenApiResponse> response in operation.Responses)
        {
            response.Value.Content = response.Value.Content
                .Where(x => x.Key == "application/json")
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
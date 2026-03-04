using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RepLoopBackend.SharedKernel.Swagger;

public class BearerSecurityDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference("Bearer", document), [] }
        });
    }
}

﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carting.WebApi.Helpers;
public class SwaggerParameterFilters : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.Single(p => p.Name == "version");

        if (versionParameter != null)
        {
            operation.Parameters.Remove(versionParameter);
        }
    }
}
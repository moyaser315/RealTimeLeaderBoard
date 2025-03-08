using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Dtos;
using FluentValidation;

namespace App.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validtor = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validtor is not null)
            {
                var entity = context.Arguments
                .OfType<T>()
                .FirstOrDefault(a => a?.GetType() == typeof(T));


                if (entity is null)
                {
                    return await next(context);
                }
                var validation = await validtor.ValidateAsync(entity);
                if (!validation.IsValid)
                {
                    return Results.ValidationProblem(validation.ToDictionary());
                }

            }
            else
            {
                return Results.Problem("Couldn't find type to validate");
            }
            return await next(context);
        }
    }
}
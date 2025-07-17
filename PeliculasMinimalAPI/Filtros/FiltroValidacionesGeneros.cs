
using FluentValidation;
using PeliculasMinimalAPI.DTOs;
using System.Net.WebSockets;

namespace PeliculasMinimalAPI.Filtros
{
    public class FiltroValidacionesGeneros : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<CrearGeneroDTOs>>();


            if (validador is null)
            {
                return await next(context);
            }

            var insumoValidar = context.Arguments.OfType<CrearGeneroDTOs>().FirstOrDefault();

            if (insumoValidar is null)
            {
                return TypedResults.Problem("No pudo ser encontrada la entidad a validar");
            }

            var resultadoValidacion = await validador.ValidateAsync(insumoValidar);
            
            if (!resultadoValidacion.IsValid)
            {
                return TypedResults.ValidationProblem(resultadoValidacion.ToDictionary());
            }

            return await next(context);
        }
    }
}

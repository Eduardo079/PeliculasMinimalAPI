
using FluentValidation;
using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Filtros
{
    public class FiltroValidaciones<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<T>>();


            if (validador is null)
            {
                return await next(context);
            }

            var insumoValidar = context.Arguments.OfType<T>().FirstOrDefault();

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

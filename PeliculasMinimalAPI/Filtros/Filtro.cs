
using AutoMapper;
using PeliculasMinimalAPI.Repositorio;

namespace PeliculasMinimalAPI.Filtros
{
    public class Filtro : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var paramRepositorioGenero = context.Arguments.OfType<IRepositorioGeneros>().FirstOrDefault();
            var paramEntero = context.Arguments.OfType<int>().FirstOrDefault();
            var paramMapper = context.Arguments.OfType<IMapper>().FirstOrDefault();
            //Este codigo se ejecuta antes del endpoint
            var resultado = await next(context);

            //Este codigo se ejecuta despues del emdpoint
            return resultado;
        }
    }
}

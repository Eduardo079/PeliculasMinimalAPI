using Microsoft.EntityFrameworkCore;

namespace PeliculasMinimalAPI.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsetarParametrosPaginacionEncabecera<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double cantidad = await  queryable.CountAsync();
            httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}

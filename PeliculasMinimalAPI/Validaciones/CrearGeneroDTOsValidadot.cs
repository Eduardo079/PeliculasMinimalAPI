using FluentValidation;
using PeliculasMinimalAPI.DTOs;
using PeliculasMinimalAPI.Repositorio;

namespace PeliculasMinimalAPI.Validaciones
{
    public class CrearGeneroDTOsValidadot : AbstractValidator<CrearGeneroDTOs>
    {
        public CrearGeneroDTOsValidadot(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            var valorDerutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDerutaId is string valorstring)
            {
                int.TryParse(valorstring, out id);
            }

            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoMensajeRequerido)
                .MaximumLength(50).WithMessage(Utilidades.MaximumLengthMensaje)
                .Must(Utilidades.PrimeraLetraEnMayuscula).WithMessage(Utilidades.PrimeraLetraMayusculaMensaje)
                .MustAsync(async (nombre, _) =>
                {
                    var existe = await repositorioGeneros.Existe(id, nombre);
                    return !existe;
                }).WithMessage(g => $"Ya existe un Genero con ese Nombre {g.Nombre}");
        }

       
    }
}

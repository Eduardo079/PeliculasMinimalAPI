using FluentValidation;
using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Validaciones
{
    public class CrearActoDTOsValidador: AbstractValidator<CrearActorDTOs>
    {
        public CrearActoDTOsValidador()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.CampoMensajeRequerido)
                .MaximumLength(150).WithMessage(Utilidades.MaximumLengthMensaje);

            var fechaMinima = new DateTime(1900, 1, 1);

            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima)
                .WithMessage(Utilidades.GreaterThanOrEqualToMensaje(fechaMinima));
        }
    }
}

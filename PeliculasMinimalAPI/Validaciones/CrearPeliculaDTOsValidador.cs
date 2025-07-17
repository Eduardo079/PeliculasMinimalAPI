using FluentValidation;
using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Validaciones
{
    public class CrearPeliculaDTOsValidador: AbstractValidator<CrearPeliculasDTOs>
    {
        public CrearPeliculaDTOsValidador()
        {
            RuleFor(p => p.Titulo).NotEmpty().WithMessage(Utilidades.CampoMensajeRequerido)
                .MaximumLength(150).WithMessage(Utilidades.MaximumLengthMensaje);
        }
    }
}

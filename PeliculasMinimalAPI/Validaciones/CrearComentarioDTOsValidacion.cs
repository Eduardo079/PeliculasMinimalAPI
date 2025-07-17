using FluentValidation;
using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Validaciones
{
    public class CrearComentarioDTOsValidacion: AbstractValidator<CrearComentariosDTOs>
    {
        public CrearComentarioDTOsValidacion()
        {
            RuleFor(c => c.Cuerpo).NotEmpty().WithMessage(Utilidades.CampoMensajeRequerido);
        }
    }
}

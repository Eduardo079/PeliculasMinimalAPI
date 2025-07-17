using FluentValidation;
using PeliculasMinimalAPI.DTOs;

namespace PeliculasMinimalAPI.Validaciones
{
    public class CrearGeneroDTOsValidadot: AbstractValidator<CrearGeneroDTOs>
    {
        public CrearGeneroDTOsValidadot()
        {
            RuleFor(x => x.Nombre).NotEmpty();
        }
    }
}

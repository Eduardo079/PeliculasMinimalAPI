namespace PeliculasMinimalAPI.Validaciones
{
    public static class Utilidades
    {
        public static string CampoMensajeRequerido = "El campo {PropertyName} es requerido";
        public static string MaximumLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength) caracteres";
        public static string PrimeraLetraMayusculaMensaje = "El campo {PropertyName} debe comenzar con mayuscula";

        public  static string GreaterThanOrEqualToMensaje(DateTime fechaMinima)
        {
            return "El campo {PropertyName} debe ser posterior a " + fechaMinima.ToString("yyyy-MM-dd");
        }

        public static bool PrimeraLetraEnMayuscula(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }

            var primeroLetra = valor[0].ToString();
            return primeroLetra == primeroLetra.ToUpper();
        }
    }
}

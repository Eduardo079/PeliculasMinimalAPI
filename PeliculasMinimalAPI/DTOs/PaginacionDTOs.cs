namespace PeliculasMinimalAPI.DTOs
{
    public class PaginacionDTOs
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecorsPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecorsPagina) ? cantidadMaximaRecorsPagina : value;
            }
        }
    }
}

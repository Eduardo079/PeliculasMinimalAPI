
namespace PeliculasMinimalAPI.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor ihttpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor ihttpContextAccessor)
        {
            this.env = env;
            this.ihttpContextAccessor = ihttpContextAccessor;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            using (var sr = new MemoryStream())
            {
                await archivo.CopyToAsync(sr);
                var contenido = sr.ToArray();
                await File.AppendAllBytesAsync(ruta, contenido);
            }
            var url = $"{ihttpContextAccessor.HttpContext!.Request.Scheme}://{ihttpContextAccessor.HttpContext.Request.Host}";
            var urlArchivo = Path.Combine(url, contenedor, nombreArchivo).Replace("\\", "/") ;
            return urlArchivo;
        }

        public Task Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return Task.CompletedTask;
            }
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath , contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo)) 
            { 
                File.Delete(directorioArchivo);
            }
            return Task.CompletedTask;
        }
    }
}

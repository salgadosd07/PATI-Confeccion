using Microsoft.AspNetCore.Mvc;

namespace PATI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArchivosController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ArchivosController> _logger;

    public ArchivosController(IWebHostEnvironment environment, ILogger<ArchivosController> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadFile([FromForm] IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se recibió ningún archivo");

            // Validar tamaño (máx 5MB)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("El archivo excede el tamaño máximo permitido (5MB)");

            // Validar extensión
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".xlsx", ".xls" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!extensionesPermitidas.Contains(extension))
                return BadRequest("Tipo de archivo no permitido");

            // Crear carpeta de uploads si no existe
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Generar nombre único
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);

            // Guardar archivo
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retornar URL relativa
            var url = $"/uploads/{nombreArchivo}";
            return Ok(new { url, nombreOriginal = file.FileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir archivo");
            return StatusCode(500, "Error interno al subir el archivo");
        }
    }

    [HttpGet("download/{nombreArchivo}")]
    public IActionResult DownloadFile(string nombreArchivo)
    {
        try
        {
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);

            if (!System.IO.File.Exists(rutaCompleta))
                return NotFound("Archivo no encontrado");

            var fileBytes = System.IO.File.ReadAllBytes(rutaCompleta);
            var extension = Path.GetExtension(nombreArchivo).ToLowerInvariant();

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".xls" => "application/vnd.ms-excel",
                _ => "application/octet-stream"
            };

            return File(fileBytes, contentType, nombreArchivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al descargar archivo");
            return StatusCode(500, "Error interno al descargar el archivo");
        }
    }

    [HttpDelete("delete/{nombreArchivo}")]
    public IActionResult DeleteFile(string nombreArchivo)
    {
        try
        {
            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
            var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);

            if (!System.IO.File.Exists(rutaCompleta))
                return NotFound("Archivo no encontrado");

            System.IO.File.Delete(rutaCompleta);
            return Ok(new { message = "Archivo eliminado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar archivo");
            return StatusCode(500, "Error interno al eliminar el archivo");
        }
    }
}

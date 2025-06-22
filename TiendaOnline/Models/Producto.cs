using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Marca { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Categoria { get; set; } = string.Empty;
        [Precision(16, 2)]
        public decimal Precio { get; set; }

        public string Descripcion { get; set; } = string.Empty;
        [MaxLength(100)]
        public string NombreArchivoImagen { get; set; } = string.Empty;

        public DateTime CreadoEn { get; set; }
    }
}
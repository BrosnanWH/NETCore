using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class ProductoDto
    {
        //DTO = Data Transfer Object
        //Este modelo nos va a permitir crear producto nuevo y editar producto
        //Vamos a utilizar algunas de las propiedades ol modelo Producto.cs
        [Required, MaxLength(100)]
        
        public string Nombre { get; set; } = string.Empty;
        [Required, MaxLength(100)]
               
        public string Marca { get; set; } = string.Empty;
        [Required, MaxLength(100)]
               
        public string Categoria { get; set; } = string.Empty;
        [Required]
              
        public decimal Precio { get; set; }
        [Required]
              
        public string Descripcion { get; set; } = string.Empty;
        //En este caso, se requiere el archivo en si, no solo el nombre.
        //Cuando se vaya a crear un producto nuevo, el archivo es requerido
        //Cuando se vaya a editar un producto, el archivo es opcional, por eso lleva el ?
        public IFormFile? ArchivoImagen { get; set; }
    }
}

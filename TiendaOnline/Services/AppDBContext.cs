using Microsoft.EntityFrameworkCore;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
        //Vamos a agregar la propiedad que nos permitirá crear una tabla llamada Producte en la BD
        //Es una propiedad llamada Productos (este sera el nombre de la tabla en la BD) de tipo DbSet de Producto
        public DbSet<Producto> Productos { get; set; }

    }
}

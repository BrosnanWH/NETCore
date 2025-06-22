using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TiendaOnline.Models;
using TiendaOnline.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TiendaOnline.Controllers
{
    public class ControladorProductos : Controller
    {
        private readonly AppDBContext context;
        private readonly IWebHostEnvironment environment;
        //En la acción Index() tenemos que leer la lista de productos de la base de datos
        //y tenemos que pasar la lista a la vista. Para leer los datos de la base de datos
        //necesitamos de AppDBContext que ya agregamos al contenedor de servicios.
        //Para solicitarlo del contenedor de servicios necesitamos crear un constructor.
    public ControladorProductos(AppDBContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()

    {    //Una vez creado el constructor ControladorProductos y el campo context
         //podemos usar context para leer los productos de la base de datos
            var productos = context.Productos.OrderByDescending(p => p.Id).ToList();
            //Ahora pasamos la lista de productos a la vista
            return View(productos);

    }
        //Vamos a agregar la acción para Crear producto nuevo
    public IActionResult Create()
        {
            return View();
        }
        //Vamos a agregar la acciom para Crear producto cuando se presiona el boton Enviar
        //Vamos a inctuir el parámetro del modelo ProductoDto
        //Para eso se debe indicar que se requiere el método HttpPost
        [HttpPost]
    
    public IActionResult Create(ProductoDto productoDto)
        {
            //Debemos validar el campo del archivo de imagen, ya que es opcional
            if (productoDto.ArchivoImagen == null)
            {
                ModelState.AddModelError("ArchivoImagen", "El archivo de imagen es requerido, ");

            }
            //Ahora veamos si tenemos errores de validación en ProductoOto
            //Si el estado del modelo no es valido, retornamos la vista Create con los datos del objeto productoDto enviado
            if (!ModelState.IsValid)
            {
                return View(productoDto);
            }
            //Para guardar el archivo de la imagen del producto, vamos a nombrarlo con la fecha de creacion
            string nuevoNombreArchivo = DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            //Vamos a agregar la extension del archivo al nombre del archivo
            nuevoNombreArchivo += Path.GetExtension(productoDto.ArchivoImagen!.FileName);
            //Vamos a obtener la la ruta (wwwroot/productos/) completa donde se guardara la imagen nueva
            string rutaCompletaImagen = environment.WebRootPath + "/productos/" + nuevoNombreArchivo;
            using (var flujoBytes = System.IO.File.Create(rutaCompletaImagen))
            {
                productoDto.ArchivoImagen.CopyTo(flujoBytes);
            }
            //Guardar el producto nuevo en la base de datos
            Producto producto = new Producto()
            {
                Nombre = productoDto.Nombre,
                Marca = productoDto.Marca,
                Categoria = productoDto.Categoria,
                Precio = productoDto.Precio,
                Descripcion = productoDto.Descripcion,
                NombreArchivoImagen = nuevoNombreArchivo,
                CreadoEn = DateTime.Now,
            };
            context.Productos.Add(producto); //Guardar producto en la tabla Productos
            context.SaveChanges(); //Hacer efectivo el cambio en la base de datos

            //Si todo está bien, redireccionamos al usuario a la lista de productos
            return RedirectToAction("Index", "ControladorProductos");
        }
        //Vamos a agregar la accion para Editar producto cuando se presiona el boton Editar
        //Se va a requerir el Id del producto, el Id se agregara a la UR

        [HttpPost]

        public IActionResult Edit(int id)
        {
            var producto = context.Productos.Find(id);
            if (producto == null)
                return RedirectToAction("Index", "ControladorProductos");

            var productoDto = new ProductoDto
            {
                Nombre = producto.Nombre,
                Marca = producto.Marca,
                Categoria = producto.Categoria,
                Precio = producto.Precio,
                Descripcion = producto.Descripcion
                // ArchivoImagen no se asigna en GET
            };

            ViewData["Id"] = producto.Id;
            ViewData["NombreArchivoImagen"] = producto.NombreArchivoImagen;
            ViewData["CreadoEn"] = producto.CreadoEn.ToString("dd/MM/yyyy");

            return View(productoDto);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditPost(int id, ProductoDto productoDto)

        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                return View(productoDto);
            }

            var producto = context.Productos.Find(id);
            if (producto == null)
                return RedirectToAction("Index", "ControladorProductos");

            producto.Nombre = productoDto.Nombre;
            producto.Marca = productoDto.Marca;
            producto.Categoria = productoDto.Categoria;
            producto.Precio = productoDto.Precio;
            producto.Descripcion = productoDto.Descripcion;

            if (productoDto.ArchivoImagen != null)
            {
                string nuevoNombreArchivo = DateTime.Now.ToString("ddMMyyyyHHmmssfff") +
                                            Path.GetExtension(productoDto.ArchivoImagen.FileName);
                string rutaCompletaImagen = Path.Combine(environment.WebRootPath, "productos", nuevoNombreArchivo);
                using (var flujoBytes = System.IO.File.Create(rutaCompletaImagen))
                {
                    productoDto.ArchivoImagen.CopyTo(flujoBytes);
                }
                producto.NombreArchivoImagen = nuevoNombreArchivo;
            }

            context.SaveChanges();
            return RedirectToAction("Index", "ControladorProductos");
        }
    }
}

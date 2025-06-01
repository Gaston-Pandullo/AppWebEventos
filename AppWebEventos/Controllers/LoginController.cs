using AccesoDatos.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace AppWebEventos.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Username == username && u.Password == password);
            
            // Validacion de usuario y contraseña
            if(usuario != null)
            {
                // Si el usuario y contraseña son correctos, redirige a Home.
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Mensaje = "Usuario o contraseña incorrectos, por favor intente nuevamente.";

            return View();
        }
    }
}

using AccesoDatos.DBContext;
using AppWebEventos.ViewModels;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

namespace AppWebEventos.Controllers
{
    public class InvitadoController : Controller
    {
        private readonly AppDbContext _context;
        public InvitadoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int eventoId)
        {
            var evento = await _context.Eventos.FindAsync(eventoId);
            if (evento == null)
                return NotFound();

            var invitados = await _context.Invitados
                .Where(i => i.EventoId == eventoId)
                .ToListAsync();

            ViewBag.Evento = evento;
            return View(invitados);
        }
        // GET: /Invitado/Crear/5
        public IActionResult Crear(int eventoId)
        {
            //var nuevoInvitado = new Invitado
            //{
            //    EventoId = eventoId
            //};
            //return View(nuevoInvitado);

            /*Version con ViewModel*/
            var vm = new InvitadoFormViewModel
            {
                EventoId = eventoId
            };
            return View(vm);
        }

        // POST: /Invitado/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(InvitadoFormViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Invitados.Add(invitado);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index", new { eventoId = invitado.EventoId });
            //}

            //ViewBag.EventoId = invitado.EventoId;

            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //{
            //    Console.WriteLine(error.ErrorMessage); // O usá el logger si preferís
            //}


            //return View(invitado);

            /*Version con ViewModel*/
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var nuevoInvitado = new Invitado
            {
                NombreCompleto = model.NombreCompleto,
                MayorDeEdad = model.MayorDeEdad,
                ConfirmoAsistencia = model.ConfirmoAsistencia,
                EventoId = model.EventoId
            };

            _context.Invitados.Add(nuevoInvitado);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { eventoId = model.EventoId });
        }

        // GET: /Invitado/CargaMasiva
        [HttpGet]
        public IActionResult CargaMasiva(int eventoId)
        {
            ViewBag.EventoId = eventoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargaMasiva(int eventoId, IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                ModelState.AddModelError("", "Por favor subí un archivo válido.");
                ViewBag.EventoId = eventoId;
                return View();
            }

            var listaInvitados = new List<Invitado>();

            using (var stream = archivoExcel.OpenReadStream())
            {
                var workbook = new XSSFWorkbook(stream); // Usa HSSFWorkbook si es .xls
                var sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++) // Empieza en 1 para saltear encabezado
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var invitado = new Invitado
                    {
                        EventoId = eventoId,
                        NombreCompleto = row.GetCell(0)?.ToString() ?? string.Empty,
                        MayorDeEdad = row.GetCell(1)?.ToString().ToLower() == "sí" || row.GetCell(1)?.ToString().ToLower() == "si",
                        ConfirmoAsistencia = row.GetCell(2)?.ToString().ToLower() == "sí" || row.GetCell(2)?.ToString().ToLower() == "si"
                    };

                    if (!string.IsNullOrWhiteSpace(invitado.NombreCompleto))
                    {
                        listaInvitados.Add(invitado);
                    }
                }
            }

            _context.Invitados.AddRange(listaInvitados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { eventoId });
        }

    }
}


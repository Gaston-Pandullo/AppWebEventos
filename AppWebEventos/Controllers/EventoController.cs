using AccesoDatos.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace AppWebEventos.Controllers
{
    public class EventoController : Controller
    {
        private readonly AppDbContext _context;
        public EventoController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string filtro = "")
        {
            var eventosNoRealizados = _context.Eventos.Where(x => x.Activo && !x.Realizado);
            var eventosRealizados = _context.Eventos.Where(x => x.Activo && x.Realizado);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();

                eventosNoRealizados = eventosNoRealizados.Where(x =>
                x.Titulo.ToLower().Contains(filtro) ||
                x.Descripcion.ToLower().Contains(filtro) ||
                x.Direccion.ToLower().Contains(filtro));

                eventosRealizados = eventosRealizados.Where(x =>
                x.Titulo.ToLower().Contains(filtro) ||
                x.Descripcion.ToLower().Contains(filtro) ||
                x.Direccion.ToLower().Contains(filtro));
            }

            var vistaModelo = new
            {
                Activos = await eventosNoRealizados.OrderBy(x => x.FechaEvento).ToListAsync(),
                Realizados = await eventosRealizados.OrderByDescending(x => x.FechaEvento).ToListAsync(),
                Filtro = filtro
            };

            return View(vistaModelo);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.FechaCreacion = DateTime.Now;
                evento.Activo = true;
                evento.Realizado = false;

                if (string.IsNullOrWhiteSpace(evento.Direccion))
                {
                    evento.Direccion = " - ";
                }

                if (string.IsNullOrWhiteSpace(evento.UbicacionGoogleMaps))
                {
                    evento.UbicacionGoogleMaps = " - ";
                }

                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }


        [HttpGet]
        public IActionResult Editar(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Evento evento)
        {
            if (ModelState.IsValid)
            {
                var eventoDb = await _context.Eventos.FindAsync(evento.Id);
                if (eventoDb == null)
                {
                    return NotFound();
                }

                eventoDb.Titulo = evento.Titulo;
                eventoDb.Descripcion = evento.Descripcion;
                eventoDb.FechaEvento = evento.FechaEvento;
                eventoDb.UbicacionGoogleMaps = evento.UbicacionGoogleMaps;

                eventoDb.Direccion = evento.Direccion;

                eventoDb.Realizado = evento.Realizado;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(evento);
        }


        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var evento = _context.Eventos.Find(id); // Find busca por la PK.
            if (evento == null) { return NotFound(); }

            evento.Activo = false;

            _context.Eventos.Update(evento);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EventosRealizados(string filtro = "")
        {
            //Eventos realizados y activos.
            var eventosRealizados = _context.Eventos.Where(x => x.Realizado && x.Activo);

            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                var eventosFiltrados = await eventosRealizados.Where(x =>
                    x.Titulo.ToLower().Contains(filtro) ||
                    x.Descripcion.ToLower().Contains(filtro) ||
                    x.Direccion.ToLower().Contains(filtro))
                    .OrderByDescending(x => x.FechaEvento)
                    .ToListAsync();

                // Lista filtrada.
                return View(eventosFiltrados);
            }
            else
            {
                // Lista sin filtrar.
                return View(eventosRealizados);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarComoRealizado(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null || !evento.Activo)
            {
                return NotFound();
            }

            evento.Realizado = true;
            _context.Update(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}

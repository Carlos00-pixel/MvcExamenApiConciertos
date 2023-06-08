using Microsoft.AspNetCore.Mvc;
using MvcExamenApiConciertos.Models;
using MvcExamenApiConciertos.Services;

namespace MvcExamenApiConciertos.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceApiConciertos service;
        private ServiceStorageS3 serviceS3;
        private string BucketUrl;

        public ConciertosController(ServiceApiConciertos service,
            IConfiguration configuration, ServiceStorageS3 serviceS3)
        {
            this.service = service;
            this.BucketUrl = configuration.GetValue<string>("AWS:BucketUrl");
            this.serviceS3 = serviceS3;
        }

        public async Task<IActionResult> Index()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            ViewData["BUCKETURL"] = this.BucketUrl;
            return View(eventos);
        }

        public async Task<IActionResult> Categorias()
        {
            List<Categoria> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }

        public async Task<IActionResult> BuscarPorCategoria()
        {
            List<Evento> eventos = await this.service.GetEventosAsync();
            ViewData["BUCKETURL"] = this.BucketUrl;
            return View(eventos);
        }

        [HttpPost]
        public async Task<IActionResult> BuscarPorCategoria(int id)
        {
            Evento evento = await this.service.FindEventoPorCategoriaAsync(id);
            return View(evento);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceS3.UploadFileAsync(file.FileName, stream);
            }
            await this.service.CreateEventoAsync(evento.Nombre, evento.Artista, evento.IdCategoria, file.FileName);
            return RedirectToAction("Index");
        }
    }
}

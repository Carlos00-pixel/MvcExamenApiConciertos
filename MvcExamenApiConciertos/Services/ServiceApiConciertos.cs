using MvcExamenApiConciertos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcExamenApiConciertos.Services
{
    public class ServiceApiConciertos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiConciertos(IConfiguration configuration, KeysModel model)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = model.ApiPersonajes;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task CreateEventoAsync
            (string nombre, string artista, int idcategoria, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Conciertos";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Evento evento = new Evento
                {
                    Nombre = nombre,
                    Artista = artista,
                    IdCategoria = idcategoria,
                    Imagen = imagen
                };
                string json = JsonConvert.SerializeObject(evento);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "api/Conciertos/GetEventoList";
            List<Evento> eventos = await this.CallApiAsync<List<Evento>>(request);
            return eventos;
        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "api/Conciertos/GetCategoriaList";
            List<Categoria> categorias = await this.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }

        public async Task<Evento> FindEventoPorCategoriaAsync(int id)
        {
            string request = "api/Conciertos/FindEventoPorCategoria/" + id;
            Evento evento = await this.CallApiAsync<Evento>(request);
            return evento;
        }
    }
}

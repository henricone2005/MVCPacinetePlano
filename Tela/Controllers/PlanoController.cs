using Microsoft.AspNetCore.Mvc;
using Tela.Models;
using System.Net.Http.Headers;

namespace Tela.Controllers
{
    public class PlanoController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PlanoController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Exibir todos os planos
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync("api/planos");

            if (response.IsSuccessStatusCode)
            {
                var planos = await response.Content.ReadFromJsonAsync<List<Plano>>();
                return View(planos);
            }

            return View(new List<Plano>());
        }

        // Detalhes de um plano específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var plano = await response.Content.ReadFromJsonAsync<Plano>();
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        // Formulário para criar um novo plano
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Plano plano)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = JsonContent.Create(plano);

                var response = await client.PostAsync("api/planos", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(plano);
        }

        // Formulário para editar um plano existente
        public async Task<IActionResult> Editar(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var plano = await response.Content.ReadFromJsonAsync<Plano>();
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Plano plano)
        {
            if (id != plano.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = JsonContent.Create(plano);

                var response = await client.PutAsync($"api/planos/{id}", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(plano);
        }

        // Exibir confirmação para exclusão de um plano
        public async Task<IActionResult> Excluir(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var plano = await response.Content.ReadFromJsonAsync<Plano>();
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.DeleteAsync($"api/planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}

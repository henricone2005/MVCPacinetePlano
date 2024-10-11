using Microsoft.AspNetCore.Mvc;
using Tela.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Tela.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PacienteController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Método para exibir todos os pacientes
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync("api/pacientes");

            if (response.IsSuccessStatusCode)
            {
                var pacientes = await response.Content.ReadFromJsonAsync<List<Paciente>>();
                return View(pacientes);
            }

            return View(new List<Paciente>());
        }

        // Método para criar um novo paciente
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = JsonContent.Create(paciente);

                var response = await client.PostAsync("api/pacientes", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(paciente);
        }

        // Outros métodos do controlador...
    }
}

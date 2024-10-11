using Microsoft.AspNetCore.Mvc;
using Tela.Models;
using System.Net.Http.Headers;

namespace Tela.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PacienteController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Exibir todos os pacientes
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

        // Detalhes de um paciente específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var paciente = await response.Content.ReadFromJsonAsync<Paciente>();
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        // Formulário para criar um novo paciente
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

        // Formulário para editar um paciente existente
        public async Task<IActionResult> Editar(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var paciente = await response.Content.ReadFromJsonAsync<Paciente>();
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = JsonContent.Create(paciente);

                var response = await client.PutAsync($"api/pacientes/{id}", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(paciente);
        }

        // Exibir confirmação para exclusão de um paciente
        public async Task<IActionResult> Excluir(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"api/pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var paciente = await response.Content.ReadFromJsonAsync<Paciente>();
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.DeleteAsync($"api/pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}

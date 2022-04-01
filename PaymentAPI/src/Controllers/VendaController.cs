using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Models;
using PaymentAPI.Views;
using PaymentAPI.Repositories;

namespace PaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly VendaRepository _repository;

        public VendaController(VendaRepository repository)
        {
            _repository = repository;
        }

        private static string textoVendaNaoEncontrada(Guid idVenda)
        {
            return $"Nenhuma venda foi localizada com o id: {idVenda}";
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> CriarVenda([FromBody] Venda venda)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.CriarVenda(venda);
                return CreatedAtAction("CriarVenda", result);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{idVenda}")]
        public async Task<IActionResult> ObterVendaPeloId(Guid idVenda)
        {
            var vendaLocalizada = await _repository.ObterVenda(idVenda);

            if (vendaLocalizada == null) return NotFound(textoVendaNaoEncontrada(idVenda));

            VendaView venda = new VendaView(
                vendaLocalizada.Id,
                vendaLocalizada.Data,
                vendaLocalizada.Status,
                vendaLocalizada.Vendedor,
                vendaLocalizada.Itens);

            return Ok(venda);
        }

        [HttpPut("{idVenda}")]
        public async Task<IActionResult> AtualizarStatusDaVenda(Guid idVenda, [FromBody] StatusVenda novoStatus)
        {
            Venda vendaLocalizada = await _repository.ObterVenda(idVenda);

            if (vendaLocalizada == null) return NotFound(textoVendaNaoEncontrada(idVenda));

            var erroAtualizacao = vendaLocalizada.AtualizarStatus(novoStatus.Status);

            if (erroAtualizacao == "")
            {
                _repository.AlterarStatusVenda(vendaLocalizada);
                return NoContent();
            }

            return BadRequest(erroAtualizacao);
        }
    }
}
using System;
using Xunit;
using PaymentAPI.Models;
using PaymentAPI.Controllers;
using PaymentAPI.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PaymentAPI.Tests
{
    public class PaymentAPITest
    {
        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
        private Venda _vendaComDoisItens()
        {
            Vendedor vendedor = new Vendedor(
                "12345678911",
                "Wallace Ferreira",
                "wallace@mail.com",
                2122224444);

            Produto produto1 = new Produto("Xiaomi mi 9T", 3200);
            Produto produto2 = new Produto("Xiaomi 11T PRO", 5200);

            VendaItem vendaItem1 = new VendaItem(produto1, 2);
            VendaItem vendaItem2 = new VendaItem(produto2, 3);

            List<VendaItem> itens = new List<VendaItem>();
            itens.Add(vendaItem1);
            itens.Add(vendaItem2);

            return new Venda(vendedor, itens.AsEnumerable());
        }

        [Fact]
        public async void CriarVenda_DeveRetornarUmaVendaCriada()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var result = await vendaController.CriarVenda(_vendaComDoisItens());

            Assert.IsType<ActionResult<Venda>>(result);
        }

        [Fact]
        public async void ObterVendaPeloId_DeveRetornarUmaVendaPeloIdentificador()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);
            var vendaLocalizada = await vendaController.ObterVendaPeloId(resultObject.Id);

            Assert.IsType<OkObjectResult>(vendaLocalizada);
        }

        [Fact]
        public async void ObterVendaPeloId_DeveFalharAoRetornarUmaVendaPeloIdentificador()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var newId = Guid.NewGuid();
            var vendaLocalizada = await vendaController.ObterVendaPeloId(newId);

            Assert.IsType<NotFoundObjectResult>(vendaLocalizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveAtualizarStatusDeAguardandoPagamentoParaPagamentoAprovado()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);
            Assert.IsType<NoContentResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveAtualizarStatusDeAguardandoPagamentoParaCancelado()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.CANCELADA);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);
            Assert.IsType<NoContentResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveAtualizarStatusDePagamentoAprovadoParaEnviadoTransportadora()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);
            Assert.IsType<NoContentResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveAtualizarStatusDePagamentoAprovadoParaCancelado()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.CANCELADA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);
            Assert.IsType<NoContentResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveAtualizarStatusDeEnviadoTransportadoraParaEntregue()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENTREGUE);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<NoContentResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDeAguardandoPagamentoParaEnviadoTransportadora()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDeAguardandoPagamentoParaEntregue()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.ENTREGUE);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDePagamentoAprovadoParaEntregue()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.ENTREGUE);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharPorNaoLocalizarAVenda()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(Guid.NewGuid(), novoStatus);

            Assert.IsType<NotFoundObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDeEnviadoTransportadoraParaAguardandoPagamento()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.AGUARDANDO_PAGAMENTO);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDeEnviadoTransportadoraParaPagamentoAprovado()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }

        [Fact]
        public async void AtualizarStatusDaVenda_DeveFalharAoAlterarStatusDeEnviadoTransportadoraParaCancelado()
        {
            var mockRepository = new Mock<VendaRepository>();
            VendaController vendaController = new VendaController(mockRepository.Object);

            var actionResult = await vendaController.CriarVenda(_vendaComDoisItens());

            var resultObject = GetObjectResultContent<Venda>(actionResult);

            var novoStatus = new StatusVenda(StatusVenda.PAGAMENTO_APROVADO);
            var vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.ENVIADO_PARA_TRANSPORTADORA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            novoStatus = new StatusVenda(StatusVenda.CANCELADA);
            vendaAtualizada = await vendaController.AtualizarStatusDaVenda(resultObject.Id, novoStatus);

            Assert.IsType<BadRequestObjectResult>(vendaAtualizada);
        }
    }
}

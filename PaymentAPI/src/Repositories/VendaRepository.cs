using System;
using System.Threading.Tasks;
using PaymentAPI.Models;
using System.Collections.Generic;
using PaymentAPI.Views;

namespace PaymentAPI.Repositories
{
    public class VendaRepository
    {
        //Dados em memória - Mock
        private List<Venda> Vendas = new List<Venda>();

        public Task<Venda> CriarVenda(Venda venda)
        {
            Venda novaVenda = new Venda(venda.Vendedor, venda.Itens);
            this.Vendas.Add(novaVenda);
            return Task.FromResult(novaVenda);
        }

        public Task<Venda> ObterVenda(Guid idVenda)
        {
            Venda vendaLocalizada = this.Vendas.Find(v => v.Id == idVenda);
            return Task.FromResult(vendaLocalizada);
        }

        public void AlterarStatusVenda(Venda venda)
        {
            Venda vendaLocalizada = this.Vendas.Find(v => v.Id == venda.Id);
            vendaLocalizada = venda;
        }
    }
}
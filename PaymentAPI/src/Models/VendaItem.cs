using System;

namespace PaymentAPI.Models
{
    public class VendaItem : CadastroBase
    {
        public Guid IdVenda { get; set; }
        public Produto Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ObterValorItem () => Math.Round(this.Quantidade * this.Produto.Valor, 2);


        public VendaItem():base() {}
        public VendaItem(Produto produto, decimal quantidade):base()
        {
            this.Produto = produto;
            this.Quantidade = quantidade;
        }

    }
}
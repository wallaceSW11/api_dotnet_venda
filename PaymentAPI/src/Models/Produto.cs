using System;

namespace PaymentAPI.Models
{
    public class Produto : CadastroBase
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }

        public Produto():base() {}
        public Produto(string descricao, decimal valor):base() {
            Descricao = descricao;
            Valor = valor;
        }
    }
}
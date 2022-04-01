using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentAPI.Models
{
    public class Venda : CadastroBase
    {
        public DateTime Data { get; private set; }
        public Vendedor Vendedor { get; set; }
        public IEnumerable<VendaItem> Itens { get; set; }


        private string _status = "";
        public string Status
        {
            get => _status;
            set => this.AtualizarStatus(value);
        }
        public decimal ValorTotal => Math.Round(this.Itens?.Sum(itens => itens.ObterValorItem()) ?? 0, 2);

        public Venda():base() {}
        public Venda(Vendedor vendedor, IEnumerable<VendaItem> itens) : base()
        {
            Data = DateTime.Now;
            Vendedor = vendedor;
            Itens = itens;
            Status = StatusVenda.AGUARDANDO_PAGAMENTO;

            foreach(var item in itens)
            {
                item.IdVenda = this.Id;
            }
        }

        private string _textoStatusNaoPoderaSerAlterado(string status)
        {
            return $"O Status '{status}' não poderá ser alterado.";
        }
        private string _validarAlteracao(string novoStatusVenda, string[] statusValidos)
        {
            if (statusValidos.Contains(novoStatusVenda))
            {
                this._status = novoStatusVenda;
                return "";
            }
            else
                return $"O status '{this._status}' só poderá ser atualizado para '{string.Join("' ou '", statusValidos)}'";

        }
        public string AtualizarStatus(string statusVenda)
        {
            switch (this._status)
            {
                case "":
                  this._status = statusVenda;
                  return "";

                case StatusVenda.AGUARDANDO_PAGAMENTO:
                    return _validarAlteracao(statusVenda, new string[] {
                        StatusVenda.PAGAMENTO_APROVADO, StatusVenda.CANCELADA });

                case StatusVenda.PAGAMENTO_APROVADO:
                    return _validarAlteracao(statusVenda, new string[] {
                         StatusVenda.ENVIADO_PARA_TRANSPORTADORA, StatusVenda.CANCELADA });

                case StatusVenda.ENVIADO_PARA_TRANSPORTADORA:
                    return _validarAlteracao(statusVenda, new string[] { StatusVenda.ENTREGUE });

                case StatusVenda.CANCELADA:
                case StatusVenda.ENTREGUE:
                    return _textoStatusNaoPoderaSerAlterado(this._status);

                default:
                    return "";
            }

        }
    }
}
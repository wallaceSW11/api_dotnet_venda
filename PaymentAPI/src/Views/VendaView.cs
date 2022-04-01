using System;
using System.Collections.Generic;
using PaymentAPI.Models;

namespace PaymentAPI.Views
{
    public class VendaView
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public Vendedor Vendedor { get; set; }
        public IEnumerable<VendaItem> Itens { get; set; }
        public string Status { get; set; }

        public VendaView() {}
        public VendaView(Guid id, DateTime data, string status, Vendedor vendedor, IEnumerable<VendaItem> itens)
        {
            Id = id;
            Data = data;
            Vendedor = vendedor;
            Itens = itens;
            Status = StatusVenda.AGUARDANDO_PAGAMENTO;
        }

    }

}
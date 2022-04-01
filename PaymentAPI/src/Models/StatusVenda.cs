using System;

namespace PaymentAPI.Models
{

    public class StatusVenda
    {
        public const string AGUARDANDO_PAGAMENTO = "Aguardando pagamento";
        public const string PAGAMENTO_APROVADO = "Pagamento aprovado";
        public const string ENVIADO_PARA_TRANSPORTADORA = "Enviado para transportadora";
        public const string ENTREGUE = "Entregue";
        public const string CANCELADA = "Cancelada";
        public string Status { get; set; }

        public StatusVenda() {}
        public StatusVenda(string status)
        {
            Status = status;
        }

    }

}
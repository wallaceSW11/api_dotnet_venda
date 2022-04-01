using System;

namespace PaymentAPI.Models
{
    public abstract class CadastroBase
    {
        public Guid Id { get; protected set; }
        public CadastroBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
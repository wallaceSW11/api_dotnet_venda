using System;

namespace PaymentAPI.Models
{
    public class Vendedor : CadastroBase
    {
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }

        public Vendedor():base() {}
        public Vendedor(string cpf, string nome, string email, int telefone):base()
        {
            this.CPF = cpf;
            this.Nome = nome;
            this.Email = email;
            this.Telefone = telefone;
        }

    }
}
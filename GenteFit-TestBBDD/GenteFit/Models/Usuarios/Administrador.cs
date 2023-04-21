using System.Transactions;

namespace GenteFit.Models.Usuarios
{
    public class Administrador : User
    {
        public bool IsAdmin { get; set; } 

        public Administrador() : base()
        {
            IsAdmin = true;
        }

        public Administrador(String email, String pass) : base(email, pass)
        {
            IsAdmin = true;
        }
    }
}

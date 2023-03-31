using System.Transactions;

namespace GenteFit.Models.Usuarios
{
    public class Administrador : User
    {
        public bool isAdmin { get; set; }

        public Administrador() : base()
        {
            isAdmin = true;
        }

        public Administrador(String email, String pass) : base(email, pass)
        {
            isAdmin = true;
        }
    }
}

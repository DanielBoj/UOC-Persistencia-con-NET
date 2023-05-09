namespace GenteFit.Models.Usuarios
{
    public class Empleado : User
    {

        public Empleado() : base()
        {
        }

        public Empleado(string email, string pass) : base(email, pass)
        {
        }
    }
}

namespace GenteFit.Models.Prototypes
{
    public class Direccion
    {
        public string Domicilio { get; set; }
        public string Poblacion { get; set; }
        public int Cp { get; set; }
        public string Pais { get; set; }

        public Direccion() { }

        public Direccion(string dir, string poblacion, int cp, string pais)
        {
            Domicilio = dir;
            Poblacion = poblacion;
            Cp = cp;
            Pais = pais;
        }
    }
}

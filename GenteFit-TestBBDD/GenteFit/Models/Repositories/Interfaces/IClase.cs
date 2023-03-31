namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IClase
    {
        bool InsertClase(Clase clase);
        bool UpdateClase(Clase clase);
        bool DeleteClase(string id);
        List<Clase> GetAllClases();
        Clase GetClaseById(string id);
    }
}

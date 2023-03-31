namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IHorario
    {
        bool InsertHorario(Horario horario);
        bool UpdateHorario(Horario horario);
        bool DeleteHorario(string id);
        List<Horario> GetAllHorarios();
        Horario GetHorarioById(string id);
    }
}

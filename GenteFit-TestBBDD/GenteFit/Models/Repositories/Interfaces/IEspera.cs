namespace GenteFit.Models.Repositories.Interfaces
{
    public interface IEspera
    {
        bool InsertEspera(Espera espera);
        bool UpdateEspera(Espera espera);
        bool DeleteEspera(string id);
        List<Espera> GetAllEsperas();
        Espera GetEsperaById(string id);
    }
}

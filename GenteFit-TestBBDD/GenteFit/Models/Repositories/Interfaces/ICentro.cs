using GenteFit.Models;
using GenteFit.Models.Collections;

namespace GenteFit.Models.Repositories.Interfaces
{
    public interface ICentro
    {
        bool InsertCentro(Centro centro);
        bool UpdateCentro(Centro centro);
        bool DeleteCentro(string id);
        List<Centro> GetAllCentros();
        Centro GetCentroById(string id);
    }
}

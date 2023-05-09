using GenteFit.Models;

// Las interfaces definen las firmas de los métodos que implementarán los objetos DAO.
namespace GenteFit.Models.Repositories.Interfaces
{
	public interface IClasePartial
	{
		Task<List<Clase>> GetAllClases();
		Task<Clase> GetClaseById(string id);
	}
}

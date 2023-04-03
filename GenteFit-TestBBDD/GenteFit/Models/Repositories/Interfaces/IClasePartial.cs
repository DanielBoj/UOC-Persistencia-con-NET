namespace GenteFit.Models.Repositories.Interfaces
{
	public interface IClasePartial
	{
		List<Clase> GetAllClases();
		Clase GetClaseById(string id);
	}
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;

// Esta clse se encarga de instanciar una conexión al servidor de la base de datos.
namespace GenteFit.Models.Repositories
{
    public class DataConnection
    {
        // Cliente para la conexión al servidor MongoDB
        public MongoClient client;

        // Interfaz de referencia a la BBDD
        public IMongoDatabase db;

        // Creamos el constructor
        public DataConnection(IOptions<DataConnectionSettings> connSettings)
        {
            // Inicializamos el cliente y conectamos con el servidor Atlas -> Obtenemos el String de conexión como environment.
            client = new MongoClient(connSettings.Value.ConnectionString);

            // Si Mongo no encuentra la BBDD en el servidor, la creará
            db = client.GetDatabase(connSettings.Value.ConnectionString);
        }

        public DataConnection()
        {
            // Inicializamos el cliente y conectamos con el servidor Atlas -> Obtenemos el String de conexión como environment
            client = new MongoClient(env.Development.Mongo_db.mongo_db_url);

            // Si Mongo no encuentra la BBDD en el servidor, la creará
            db = client.GetDatabase(env.Development.Mongo_db.mongo_db_name);
        }

    }
}

using GenteFit.Models.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GenteFit.Models.Repositories.Collections
{
    public class EsperaCollection : IEspera
    {
        // Referencia al repositorio de datos de MongoDB e inicializamos la instancia, así nuestra clase irá a buscar la BBDD para conectarse con ella.
        internal DataConnection _repository = new();

        // Importamos el Driver y nuestro modelo de referencia.
        private IMongoCollection<Espera> Collection;

        // Si la colecciónb no existe, Mongo la creará automáticamente.
        public EsperaCollection()
        {
            // Enlazamos nuestro modelo con la colección en MongoDB indicando nuestro modelo de referencia y teniendo en cuenta que el nombre que le asignemos, será el de la colección en MongoDB.
            Collection = _repository.db.GetCollection<Espera>("Espera");
        }

        public async Task<List<Espera>> GetAllEsperas()
        {
            try
            {
                // Obtenemos los datos desde la BBDD mediante una peteción, query, a la colección.
                // Importamos los datos como un Documento Bson y los convertimos en una lista. Hemos de usar un método asíncrono para esperar a la respuesta del servidor.
                return await Collection
                    .FindAsync(new BsonDocument()).Result.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            // Devolvemos una lista vacía
            return new List<Espera>();
        }

        public async Task<Espera> GetEsperaById(string id)
        {
            if (id == null) return new Espera();

            try
            {
                return await Collection.FindAsync(
                    // Obtenemos los datos desde la BBDD mediante una peteción, query, a la colección y buscando por el ID -> _id en Mongo.
                    // Realizamos un destructuring y asignamos el documento de Mongo al resultado de la query.
                    // Buscamos un documento en Mongo en el que su ID sea igual al ID que pasamos por parámetro y convertimos al tipo de dato ObjectId de Mongo.
                    // Si no realizamos la conversión, Mongo no puede hacer el matching.
                    new BsonDocument { { "_id", new ObjectId(id) } })
                        .Result.FirstAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return new Espera();
            }
        }

        public async Task<bool> InsertEspera(Espera espera)
        {
            //if (centro == null) throw new ArgumentNullException(nameof(centro));
            if (espera == null) return false;

            try
            {
                await Collection.InsertOneAsync(espera);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }
        }

        public async Task<bool> UpdateEspera(Espera espera)
        {
            if (espera == null) return false;

            try
            {
                // Tenemos que crear un filtro y construir una query, usamos el Pipe para encadenar funciones
                // El primer paso es construir el filtro mediante el obj Builders.

                var filter = Builders<Espera>
                    // A continuación hacemos la llamada a la función para definir qué filtro aplicaremos
                    .Filter
                    // Por último generamos la condición de filtrado mediante una función lambda que le dice al filtro que deben coincidir los Id de los objetos.
                    // Src es el documento de la BBDD.
                    .Eq(src => src.Id, espera.Id);

                // Ahora ya podemos llamar a la acción de Mongo aplicando el filtro que pasamos como parámetro para que Mongo realice la búsqueda
                await Collection.ReplaceOneAsync(filter, espera);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }
        }

        public async Task<bool> DeleteEspera(string id)
        {
            if (id == null) return false;

            try
            {
                // Igual que en el caso de la modificación de un documento, debemos comenzar creando un filtro para poder buscar el documento en la colección de MongoDB.
                var filter = Builders<Espera>
                    .Filter
                    .Eq(src => src.Id, new (id));

                // Una vez creado el método de filtrado, podemos llamar a la acción de MongoDB y pasarle el filtro.
                await Collection.DeleteOneAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }
        }
    }
}

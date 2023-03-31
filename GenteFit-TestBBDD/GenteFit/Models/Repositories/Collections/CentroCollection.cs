using MongoDB.Driver;
using GenteFit.Models.Repositories.Interfaces;
using MongoDB.Bson;

namespace GenteFit.Models.Repositories.Collections
{
    public class CentroCollection : ICentro
    {
        // Referencia al repositorio de datos de MongoDB e inicializamos la instancia, así nuestra clase irá a buscar la BBDD para conectarse con ella.
        internal DataConnection _repository = new();

        // Importamos el Driver y nuestro modelo de referencia.
        private IMongoCollection<Centro> Collection;

        // Si la colecciónb no existe, Mongo la creará automáticamente.
        public CentroCollection()
        {
            // Enlazamos nuestro modelo con la colección en MongoDB indicando nuestro modelo de referencia y teniendo en cuenta que el nombre que le asignemos, será el de la colección en MongoDB.
            Collection = _repository.db.GetCollection<Centro>("Centro");
        }

        public List<Centro> GetAllCentros()
        {
            try
            {
                // Obtenemos los datos desde la BBDD mediante una peteción, query, a la colección.
                // Importamos los datos como un Documento Bson y los convertimos en una lista. Hemos de usar un método asíncrono para esperar a la respuesta del servidor.
                var query = Collection
                    .Find(new BsonDocument()).ToListAsync();

                // Devolvemos el res de la query
                return query.Result;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
            // Devolvemos una lista vacía
            return new List<Centro>();
        }

        public Centro GetCentroById(string id)
        {
            if (id == null) return new Centro();

            try
            {
                var centro = Collection.Find(
                    // Obtenemos los datos desde la BBDD mediante una peteción, query, a la colección y buscando por el ID -> _id en Mongo.
                    // Realizamos un destructuring y asignamos el documento de Mongo al resultado de la query.
                    // Buscamos un documento en Mongo en el que su ID sea igual al ID que pasamos por parámetro y convertimos al tipo de dato ObjectId de Mongo.
                    // Si no realizamos la conversión, Mongo no puede hacer el matching.
                    new BsonDocument { { "_id", new ObjectId(id) } })
                        .FirstAsync().Result;

                return centro;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return new Centro();
            }
        }

        public bool InsertCentro(Centro centro)
        {
            //if (centro == null) throw new ArgumentNullException(nameof(centro));
            if (centro == null) return false;

            try
            {
                Collection.InsertOneAsync(centro);

                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }
        }

        public bool UpdateCentro(Centro centro)
        {
            if (centro == null) return false;

            try
            {
                // Tenemos que crear un filtro y construir una query, usamos el Pipe para encadenar funciones
                // El primer paso es construir el filtro mediante el obj Builders.
                
                var filter = Builders<Centro>
                    // A continuación hacemos la llamada a la función para definir qué filtro aplicaremos
                    .Filter
                    // Por último generamos la condición de filtrado mediante una función lambda que le dice al filtro que deben coincidir los Id de los objetos.
                    // Src es el documento de la BBDD.
                    .Eq(src => src.Id, centro.Id);

                // Ahora ya podemos llamar a la acción de Mongo aplicando el filtro que pasamos como parámetro para que Mongo realice la búsqueda
                Collection.ReplaceOneAsync(filter, centro);

                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }
        }

        public bool DeleteCentro(string id)
        {
            if (id == null) return false;

            try
            {
                // Igual que en el caso de la modificación de un documento, debemos comenzar creando un filtro para poder buscar el documento en la colección de MongoDB.
                var filter = Builders<Centro>
                    .Filter
                    .Eq(src => src.Id, new ObjectId(id));

                // Una vez creado el método de filtrado, podemos llamar a la acción de MongoDB y pasarle el filtro.
                Collection.DeleteOneAsync(filter);

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

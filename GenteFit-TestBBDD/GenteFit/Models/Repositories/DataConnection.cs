﻿using MongoDB.Driver;

namespace GenteFit.Models.Repositories
{
    public class DataConnection
    {
        // Cliente para la conexión al servidor MongoDB
        public MongoClient client;

        // Interfaz de referencia a la BBDD
        public IMongoDatabase db;

        // Creamos el constructor
        public DataConnection()
        {
            // Inicializamos el cliente y conectamos con el servidor Atlas -> Obtenemos el String de conexión como environment
            client = new MongoClient(env.Development.mongo_db.mongo_db_url);

            // Si Mongo no encuentra la BBDD en el servidor, la creará
            db = client.GetDatabase(env.Development.mongo_db.mongo_db_name);
        }
    }
}
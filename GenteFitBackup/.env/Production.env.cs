namespace GenteFit.env
{
    public class Production
    {
        public string? mongo_db_user;
        public string? mongo_db_pass;
        public string? mongo_db_name;
        public string? mongo_db_url;
        public string? python_url;

        private static readonly Production mongo_db = new()
        {
            mongo_db_user = "dboj",
            mongo_db_pass = "WlNUIgyIrJUCccUo",
            mongo_db_name = "gentefit-dboj",
            mongo_db_url = "mongodb+srv://dboj:WlNUIgyIrJUCccUo@persistencia-gentefit.ehftwrr.mongodb.net/admin",
            python_url = "http://localhost:5005"
        };

        public static Production Mongo_db => mongo_db;
    }
}

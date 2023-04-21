namespace GenteFit.env
{
    public class Development
    {
        public string? mongo_db_user;
        public string? mongo_db_pass;
        public string? mongo_db_name;
        public string? mongo_db_url;

        private static readonly Development mongo_db = new()
        {
            mongo_db_user = "dboj",
            mongo_db_pass = "WlNUIgyIrJUCccUo",
            mongo_db_name = "gentefit-dboj",
            mongo_db_url = "mongodb+srv://dboj:WlNUIgyIrJUCccUo@persistencia-gentefit.ehftwrr.mongodb.net/admin",
        };

        public static Development Mongo_db => mongo_db;
    }
}

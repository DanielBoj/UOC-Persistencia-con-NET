namespace GenteFit.env
{
    public class Production
    {
        //public static readonly string mongodb = "mongodb+srv://dboj:WlNUIgyIrJUCccUo@persistencia-gentefit.ehftwrr.mongodb.net/?retryWrites=true&w=majority";

        public string? mongo_db_user;
        public string? mongo_db_pass;
        public string? mongo_db_name;
        public string? mongo_db_url;

        public static readonly Production mongo_db = new()
        {
            mongo_db_user = "dboj",
            mongo_db_pass = "WlNUIgyIrJUCccUo",
            mongo_db_name = "gentefit-dboj",
            mongo_db_url = "mongodb+srv://dboj:WlNUIgyIrJUCccUo@persistencia-gentefit.ehftwrr.mongodb.net/admin",
        };
    }
}

namespace Catalog.Configs
{
    public class MongoDbConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {
                //return $"mongodb://{Host}:{Port}"; //without authentication
                return $"mongodb://{User}:{Password}@{Host}:{Port}"; //with authentication
            }
        }
    }
}
namespace MessengerAPI.Repositories;

public class CassandraOptions
{
    public string[] ContactPoints { get; init; }
    public int Port { get; set; } = 9042;
    public string Keyspace { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}

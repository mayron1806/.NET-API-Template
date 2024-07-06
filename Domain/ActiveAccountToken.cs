namespace Domain;

public class ActiveAccountToken
{
    // Se necessário, você pode adicionar um construtor adicional para inicialização conveniente
    public ActiveAccountToken(string content, int userId, DateTime expiresAt)
    {
        Content = content;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; }
    public string Content { get; }
    public User? User { get; }
    public int UserId { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }
}

namespace Domain;

public class ResetPasswordToken
{
    // Se necessário, você pode adicionar um construtor adicional para inicialização conveniente
    public ResetPasswordToken(string content, int userId, DateTime expiresAt)
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

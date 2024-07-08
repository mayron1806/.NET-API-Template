namespace Domain;

public class Organization
{
    public Organization(string plan, bool planActive)
    {
        Plan = plan;
        PlanActive = planActive;
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; }
    public string? Plan { get; }
    public bool PlanActive { get; }
    public IEnumerable<Member>? Members { get; }
    public DateTime CreatedAt { get; }
    public IEnumerable<Transfer>? Transfers { get; }
}

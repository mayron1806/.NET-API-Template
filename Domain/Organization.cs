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
    public string? Plan { get; private set; }
    public bool PlanActive { get; private set; }
    public IEnumerable<Member>? Members { get; private set; }
    public DateTime CreatedAt { get; }
    public IEnumerable<Transfer>? Transfers { get; private set; }

    public void AddMember(User user, bool owner = false)
    {
        var members = Members?.ToList() ?? [];
        members.Add(new Member(user.Id, Id, owner));
        Members = members;
    }
}

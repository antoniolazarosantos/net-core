namespace IWantApp.Domain;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime EditedBy { get; set; }
    public string CreatedOn { get; set; }
    public DateTime EditedOn { get; set; }
}

namespace BaseProject.Domain.Models;

public class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public string CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public bool? IsActive { get; set; }
    public void Created(string userid)
    {
        IsDeleted = false;
        CreatedOn = DateTimeOffset.UtcNow;
        CreatedBy = userid;
        Id = Guid.Empty;
    }

    public void Modified(string? userid = null)
    {
        ModifiedOn = DateTimeOffset.UtcNow;
        ModifiedBy = userid;
    }

    public void Deleted(string? userid = null)
    {
        IsDeleted = true;
        ModifiedOn = DateTimeOffset.UtcNow;
        ModifiedBy = userid;
    }
}


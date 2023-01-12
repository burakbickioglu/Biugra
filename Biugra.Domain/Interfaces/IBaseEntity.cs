namespace Biugra.Domain.Interfaces;

public interface IBaseEntity
{
    Guid Id { get; }
    DateTimeOffset CreatedOn { get; }
    DateTimeOffset? ModifiedOn { get; }
    string CreatedBy { get; }
    string? ModifiedBy { get; }
    bool IsDeleted { get; }

    void Created(string userid);
    void Modified(string? userid);
    void Deleted(string? userid);
}



using Biugra.Domain.Enums;

namespace Biugra.Domain.Models;

public class GeneralContent : BaseEntity
{
    public ContentType Type { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }

}

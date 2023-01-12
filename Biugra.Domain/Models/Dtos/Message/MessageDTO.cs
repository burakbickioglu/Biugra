using Biugra.Domain.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biugra.Domain.Models.Dtos.Message;

public class MessageDTO
{
    public Guid? Id { get; set; }
    public string? AppUserId { get; set; }
    public UserDTO? AppUser { get; set; }
    public string Title { get; set; }
    public string Question { get; set; }
    public string? Answer { get; set; }
}

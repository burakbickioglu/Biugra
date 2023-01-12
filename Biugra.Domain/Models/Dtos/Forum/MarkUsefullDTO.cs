using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biugra.Domain.Models.Dtos.Forum;
public class MarkUsefullDTO
{
    public Guid UserId { get; set; }
    public Guid ForumId { get; set; }
}

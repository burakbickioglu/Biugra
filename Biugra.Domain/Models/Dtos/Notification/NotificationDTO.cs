using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biugra.Domain.Models.Dtos.Notification;

public class NotificationDTO
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; }
    public string Message { get; set; }
}

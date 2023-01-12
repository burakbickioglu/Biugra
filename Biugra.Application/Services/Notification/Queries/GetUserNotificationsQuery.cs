using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.Notification;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Notification.Queries;
public class GetUserNotificationsQuery : CommandBase<CommandResult<List<NotificationDTO>>>
{
    public Guid Id { get; set; }

    public GetUserNotificationsQuery(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetUserNotificationsQuery, CommandResult<List<NotificationDTO>>>
    {
        private readonly IGenericRepository<Domain.Models.Notification> _notificationRepository;
        private readonly IMapper _mapper;
        public Handler(IGenericRepository<Domain.Models.Notification> notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult<List<NotificationDTO>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetAllFiltered(p => p.AppUserId == request.Id.ToString()).ToListAsync();
            return notifications != null
                ? CommandResult<List<NotificationDTO>>.GetSucceed(_mapper.Map<List<NotificationDTO>>(notifications))
                : CommandResult<List<NotificationDTO>>.GetSucceed(new List<NotificationDTO>());

        }
    }
}

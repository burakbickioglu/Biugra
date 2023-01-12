using Biugra.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Infrastructure.Interfaces.Repositories;

namespace Biugra.Service.Services.Forum.Commands;
public class MarkUsefullCommand : CommandBase<CommandResult>
{
    public MarkUsefullDTO Model { get; set; }

    public MarkUsefullCommand(MarkUsefullDTO model)
    {
        Model = model;
    }


    public class Handler : IRequestHandler<MarkUsefullCommand, CommandResult>
    {
        private readonly IGenericRepository<Domain.Models.Forum> _forumRepository;
        private readonly IGenericRepository<Domain.Models.Notification> _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<Domain.Models.Vallet> _valletRepository;
        public Handler(IGenericRepository<Domain.Models.Forum> forumRepository, IGenericRepository<Domain.Models.Notification> notificationRepository, ICurrentUserService currentUserService, IGenericRepository<Vallet> valletRepository)
        {
            _forumRepository = forumRepository;
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
            _valletRepository = valletRepository;
        }

        public async Task<CommandResult> Handle(MarkUsefullCommand request, CancellationToken cancellationToken)
        {
            var forum = await _forumRepository.GetFirst(p => p.Id == request.Model.ForumId);
            if (forum == null)
                return CommandResult.NotFound();
            forum.IsHelpfull = true;
            await _forumRepository.Update(forum);
            await _notificationRepository.Add(new Domain.Models.Notification
            {
                AppUserId = request.Model.UserId.ToString(),
                Message = "Forumun beğenildi ! Profilini kontrol et"
            });

            var userWallet = await _valletRepository.GetFirst(p => p.AppUserId == request.Model.UserId.ToString());
            userWallet.Balance += 10;
            await _valletRepository.Update(userWallet);
            return CommandResult.GetSucceed();            
        }
    }
}

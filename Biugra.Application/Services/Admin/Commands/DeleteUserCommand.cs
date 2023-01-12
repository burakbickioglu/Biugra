using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Services.Admin.Commands;

public class DeleteUserCommand : CommandBase<CommandResult>
{
    public Guid Id { get; set; }

    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteUserCommand, CommandResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return CommandResult.NotFound();

            var deleteResult = await _userManager.DeleteAsync(user);
            return deleteResult.Succeeded
                 ? CommandResult.GetSucceed()
                 : CommandResult.GetFailed(ErrorMessageConstants.USER_CANT_DELETE);
        }
    }
}

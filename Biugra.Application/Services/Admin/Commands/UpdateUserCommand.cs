
using BaseProject.Domain.Models;
using Biugra.Domain.Models.Dtos;
using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Service.Services.Admin.Commands;

public class UpdateUserCommand : CommandBase<CommandResult<UserDTO>>
{
    public UserDTO Model { get; set; }

    public UpdateUserCommand(UserDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateUserCommand, CommandResult<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CommandResult<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Model.Id);
            if (user == null)
                return CommandResult<UserDTO>.GetFailed(ErrorMessageConstants.USER_NOT_FOUND);

            user.FirstName = request.Model.FirstName;
            user.LastName = request.Model.LastName;
            user.Age = request.Model.Age != null ? (int)request.Model.Age : user.Age;
            user.Address = request.Model.Address != null ? request.Model.Address : user.Address;
            user.HighSchool = request.Model.HighSchool != null ? request.Model.HighSchool : user.HighSchool;

            //user.Country = request.Model.Country;
            //user.Title = request.Model.Title;
            //user.Email = request.Model.Email; //  değiştirilemeyecek
            // user titles ve country gelecek
            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded
                ? CommandResult<UserDTO>.GetSucceed(request.Model)
                : CommandResult<UserDTO>.GetFailed(ErrorMessageConstants.USER_CANT_UPDATE);
        }
    }
}

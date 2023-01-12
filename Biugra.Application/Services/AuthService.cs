
//namespace Biugra.Application.Services;

//public class AuthService : IAuthService
//{
//    private readonly UserManager<AppUser> _userManager;
//    private readonly SignInManager<AppUser> _signInManager;
//    private readonly ITokenHandler _tokenHandler;
//    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
//    {
//        _userManager = userManager;
//        _signInManager = signInManager;
//        _tokenHandler = tokenHandler;
//    }

//    public async Task<CommandResult<RegisterResponseDTO>> Register(RegisterRequestDTO model)
//    {
//        IdentityResult result = await _userManager.CreateAsync(new AppUser
//        {
//            Id = Guid.NewGuid().ToString(),
//            UserName = model.Email,

//            Email = model.Email,
//            //Name = model.Name,
//            //Surname = model.Surname,
//        }, model.Password);


//        if (result.Succeeded)
//        {
//            return CommandResult<RegisterResponseDTO>.GetSucceed(
//         new RegisterResponseDTO { Success = result.Succeeded, Message = "Kullanıcı başarıyla oluşturulmuştur" });
//        }

//        else
//        {
//            RegisterResponseDTO response = new() { Success = result.Succeeded };
//            foreach (var error in result.Errors)
//            {
//                response.Message += $"{error.Code} - {error.Description}";
//            }
//            return CommandResult<RegisterResponseDTO>.GetFailed("",response);
//        }
//    }

//    public async Task<CommandResult<LoginResponseDTO>> Login(LoginRequestDTO model)
//    {
//        AppUser user = await _userManager.FindByEmailAsync(model.Email);

//        if (user == null)
//            return CommandResult<LoginResponseDTO>.GetFailed("",  new LoginResponseDTO { Message = "E-mail veya şifre hatalı.." } );

//        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

//        if (result.Succeeded)
//        {
//            // yetkiler belirlenmeli
//            Token token = _tokenHandler.CreateAccessToken(1);
//            return CommandResult<LoginResponseDTO>.GetSucceed(new LoginResponseDTO { Success = true, Message = "Giriş başarılı" , Token=token.AccessToken, Expiration=token.Expiration, Userid= user.Id, Name=user.Name});
//        }
//        return CommandResult<LoginResponseDTO>.GetFailed("", new LoginResponseDTO { Message = "Giriş başarısız", Success = false });
//    }
//}

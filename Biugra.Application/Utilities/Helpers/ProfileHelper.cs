
using Biugra.Service.Mapper;

namespace Biugra.Service.Utilities.Helpers;

public static class ProfileHelper
{
    public static List<Profile> GetProfiles()
    {
        return new List<Profile>
        {
            new BaseEntityProfile(),
            new ForumProfile(),
            new UserProfile(),
            new CommentProfile(),
            new CategoryProfile(),
            new TeacherProfile(),
            new ProvidedServiceProfile()
        };
    }
}

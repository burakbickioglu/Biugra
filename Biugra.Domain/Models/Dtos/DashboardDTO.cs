using Biugra.Domain.Models.Dtos.User;

namespace Biugra.Domain.Models.Dtos;
public class DashboardDTO
{
    public int TodayCommentCount { get; set; }
    public int ThisMonthCommentCount { get; set; }
    public int TotalCommentCount { get; set; }
    public int TodayForumCount { get; set; }
    public int ThisMonthForumCount { get; set; }
    public int TotalForumCount { get; set; }
    public int TodayUserCount { get; set; }
    public int ThisMonthUserCount { get; set; }
    public int TotalUserCount { get; set; }
    public List<UserDTO>? LastFiveUser { get; set; }
}

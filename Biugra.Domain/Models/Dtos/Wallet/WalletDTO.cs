
namespace Biugra.Domain.Models.Dtos.Wallet;
public class WalletDTO
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; }
    public decimal Balance { get; set; }
}

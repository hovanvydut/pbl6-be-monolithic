namespace Monolithic.Models.DTO;

public class UserLoginResponseDTO
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string DisplayName { get; set; }

    public string AccessToken { get; set; }
}
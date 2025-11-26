namespace TDM.Api.Contracts.Users;

public record LoginUserRequest(
    string Login,
    string Password);

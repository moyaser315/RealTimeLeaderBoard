namespace App.Dtos;

public record class UserDto(
    int Id,
    string Name,
    string Email,
    string Password,
    string UserName
);
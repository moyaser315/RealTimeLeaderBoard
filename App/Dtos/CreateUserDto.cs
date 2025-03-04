namespace App.Dtos;

public record class CreateUserDto(
    string Name,
    string Email,
    string Password,
    string UserName
);
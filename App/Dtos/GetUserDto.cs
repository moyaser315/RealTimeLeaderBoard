namespace App.Dtos;

public record class GetUserDto(
    string Name,
    string Email,
    string UserName
);
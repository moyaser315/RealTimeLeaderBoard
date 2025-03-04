namespace App.Dtos;

public record class GameScoreDto(
    string UserName,
    int Score,
    DateTime TimeStamp
);
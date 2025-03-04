namespace App.Dtos;

public record class GameScoreDto(
    string UserName,
    string GameName,
    int Score,
    DateTime TimeStamp
);
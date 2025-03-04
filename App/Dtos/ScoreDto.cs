namespace App.Dtos;

public record class ScoreDto(
        string GameName,
        int Score,
        DateTime TimeStamp
);
namespace App.Dtos;

public record class ScoreDto(
        int Id,
        string GameName,
        int Score,
        DateTime TimeStamp
);
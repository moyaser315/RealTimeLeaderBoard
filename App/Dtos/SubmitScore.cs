namespace App.Dtos;

public record class SubmitScoreDto(
        int UserId,
        int GameID,
        int Score,
        DateTime TimeStamp
);
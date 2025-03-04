using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Dtos;
using App.Models;

namespace App.Mapping
{
    public static class ScoreMapping
    {
        public static GameScoreDto ToGameScoreDto(this ScoreModel score)
        {
            return new GameScoreDto(
                UserName: score.User!.UserName,
                Score: score.Score,
                TimeStamp: score.TimeStamp
            );

        }

        public static ScoreDto ToScoreDto(this ScoreModel score)
        {
            return new ScoreDto(
                Id: score.Id,
                GameName: score.Game!.Name,
                Score: score.Score,
                TimeStamp: score.TimeStamp
            );

        }

        public static ScoreModel ToScoreModel(this SubmitScoreDto score, int id)
        {
            return new ScoreModel
            {
                UserId = score.UserId,
                GameID = id,
                Score = score.Score,
                TimeStamp = score.TimeStamp

            };
        }
    }
}
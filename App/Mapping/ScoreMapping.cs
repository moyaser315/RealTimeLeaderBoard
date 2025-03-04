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
        public static GameScoreDto ToScoreListDto(this ScoreModel score){
            return new GameScoreDto(
                UserName:score.User!.UserName,
                Score: score.Score,
                TimeStamp: score.TimeStamp
            );
            
        }
    }
}
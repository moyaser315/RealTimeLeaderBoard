using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Dtos;
using App.Models;
using Npgsql.Replication;

namespace App.Mapping
{
    public static class GameMapping
    {
        public static GamesDto ToDto(this GameModel game)
        {
            return new GamesDto(GameName: game.Name);
        }
        

    }
}
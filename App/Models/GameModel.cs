using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<ScoreModel>? Scores { get; set; }
    }
}
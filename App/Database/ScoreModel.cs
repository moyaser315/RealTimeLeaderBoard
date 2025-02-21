using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Database
{
    public class ScoreModel
    {
        public int Id { get; set; }
        public string GameName{ get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

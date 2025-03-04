using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace App.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set;}
        public ICollection<ScoreModel> Scores { get; set; } = new List<ScoreModel>();

    }
}
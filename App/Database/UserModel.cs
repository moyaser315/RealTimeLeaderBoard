using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Database
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        private string _email;
        [EmailAddress]
        [Required]
        public string Email { get => _email;
        set => _email = value.ToLower(); }
        public ICollection<ScoreModel> Scores{ get; set; } = new List<ScoreModel>();

    }
}
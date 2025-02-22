using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using App.Database;

namespace App.Models
{
    public class UniqueUserAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string Name = value?.ToString();
            ApplicationDbContext context = new ApplicationDbContext();
            UserModel user = context.Users.FirstOrDefault(x => x.UserName == Name);
            if (user == null){
                return ValidationResult.Success;
            }
            return new ValidationResult("This User Name is already taken.");
        }
    }
}
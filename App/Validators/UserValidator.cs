using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using App.Database;
using App.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Validators
{

    public class UserValidator : AbstractValidator<UserModel>
    {
        private readonly ApplicationDbContext _context;
        public UserValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(u => u.Name).NotEmpty();
            RuleFor(u => u.Email)
            .EmailAddress()
            .NotEmpty()
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
            }).WithMessage("Email already exists.");

            RuleFor(u => u.UserName)
            .MinimumLength(4)
            .NotEmpty()
            .MustAsync(async (username, cancellationToken) =>
            {
                return await _context.Users.AnyAsync(u => u.UserName == username, cancellationToken);
            });
        }

    }
}
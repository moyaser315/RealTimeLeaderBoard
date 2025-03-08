using App.Database;
using App.Dtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Validators
{

    public class UserValidator : AbstractValidator<CreateUserDto>
    {
        private readonly ApplicationDbContext _context;
        public UserValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(u => u.Name).NotEmpty().WithMessage("can't be empty.");

            RuleFor(u => u.Email)
            .NotEmpty().WithMessage("can't be empty.")
            .EmailAddress().WithMessage("Must be a valid email address")
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
            }).WithMessage("Email already exists.");

            RuleFor(u => u.UserName)
            .MinimumLength(4).WithMessage("Must be at least 4 charachters")
            .NotEmpty().WithMessage("this field can't be empty")
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await _context.Users.AnyAsync(u => u.UserName == username, cancellationToken);
            }).WithMessage("UserName Already Exists");

            RuleFor(u => u.Password)
            .NotEmpty().WithMessage("this field can't be empty")
            .MinimumLength(6).WithMessage("Must be at least 4 charachters")
            .Matches(@"[A-Z]+").WithMessage("Must Contain at least 1 uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Must Contain at least 1 lowercase letter")
            .Matches(@"[0-9]+").WithMessage("Must Contain at least 1 number");
        }

    }
}
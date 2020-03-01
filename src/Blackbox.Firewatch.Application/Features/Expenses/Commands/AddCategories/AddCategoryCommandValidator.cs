using Blackbox.Firewatch.Application.Common.Interfaces;
using Blackbox.Firewatch.Application.Infrastructure;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox.Firewatch.Application.Features.Expenses.Commands.AddCategories
{
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoriesCommand>
    {
        public AddCategoryCommandValidator(IUserRepository users)
        {
            RuleFor(c => c.OwnerId)
                .NotEmpty()
                .NotNull()
                .WithMessage("A user id must be provided.");

            RuleFor(c => c.OwnerId)
                .Must(id => users.People.Any(p => p.Id == id))
                .WithMessage(c => ErrorMessages.PERSON_NOT_EXISTS(c.OwnerId));
        }
    }
}

﻿using FluentValidation;
using MyMusic.API.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Validation
{
    public class SaveUserResourceValidation : AbstractValidator<UserResource>
    {
        public SaveUserResourceValidation()
        {
            RuleFor(m => m.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.FirstName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.LastName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(m => m.Password)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}

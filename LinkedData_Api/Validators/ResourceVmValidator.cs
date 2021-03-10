using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;
using LinkedData_Api.Model.ViewModels;

namespace LinkedData_Api.Validators
{
    public class ResourceVmValidator : AbstractValidator<ResourceVm>
    {
        public ResourceVmValidator()
        {
            RuleFor(x => x.Predicates)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .ChildRules(x => x.RuleForEach(y => y.Keys))
                .SetValidator(new PredicatesDictionaryValidator());
        }

        public class NamedResourceValidator : AbstractValidator<NamedResourceVm>
        {
            public NamedResourceValidator()
            {
                RuleFor(x => x.ResourceCurie)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("{PropertyName} must not be null.")
                    .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                    .Must(x => Regex.IsMatch(x, @"^[^:]+\:[^:]+$"))
                    .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
                RuleFor(x => x.Predicates)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("{PropertyName} must not be null.")
                    .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                    .ChildRules(x => x.RuleForEach(y => y.Keys))
                    .SetValidator(new PredicatesDictionaryValidator());
            }
        }

        public class PredicatesDictionaryValidator : AbstractValidator<Dictionary<string, PredicateContent>>
        {
            public PredicatesDictionaryValidator()
            {
                RuleFor(x => x.Keys)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("{PropertyName} must not be null.")
                    .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                    .Must(x => x.All(IsCurie))
                    .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
                RuleForEach(x => x.Values).SetValidator(new PredicateContentValidator());
            }
        }

        public class PredicateContentValidator : AbstractValidator<PredicateContent>
        {
            public PredicateContentValidator()
            {
                RuleFor(x => x.Curies)
                    .Cascade(CascadeMode.Stop)
                    .Must(x => x == null || x.All(IsCurie))
                    .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
                RuleFor(x => x.Literals)
                    .Cascade(CascadeMode.Stop)
                    .ChildRules(x => x.RuleForEach(y => y).SetValidator(new LiteralValidator()));
            }

            public class LiteralValidator : AbstractValidator<Literal>
            {
                public LiteralValidator()
                {
                    RuleFor(x => x.Value)
                        .Cascade(CascadeMode.Stop)
                        .NotNull().WithMessage("{PropertyName} must not be null.")
                        .NotEmpty().WithMessage("{PropertyName} must not be empty.");
                }
            }
        }

        private static bool IsCurie(string curie)
        {
            return Regex.IsMatch(curie, @"^[^:]+\:[^:]+$");
        }
    }
}
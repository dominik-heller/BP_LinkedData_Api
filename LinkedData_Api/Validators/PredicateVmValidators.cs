using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using LinkedData_Api.Model.ViewModels;

namespace LinkedData_Api.Validators
{
    public class PredicateVmValidators : AbstractValidator<PredicateVm>
    {
        public PredicateVmValidators()
        {
            RuleFor(x => x.Curies)
                .Cascade(CascadeMode.Stop)
                .Must(x => x == null || x.All(IsCurie))
                .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
            RuleFor(x => x.Literals)
                .Cascade(CascadeMode.Stop)
                .ChildRules(x =>
                    x.RuleForEach(y => y)
                        .SetValidator(new ResourceVmValidator.PredicateContentValidator.LiteralValidator()));
        }

        public class NamedPredicateVmValidators : AbstractValidator<NamedPredicateVm>
        {
            public NamedPredicateVmValidators()
            {
                RuleFor(x => x.PredicateCurie)
                    .Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("{PropertyName} must not be null.")
                    .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                    .Must(x => Regex.IsMatch(x, @"^[^:]+\:[^:]+$"))
                    .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
                RuleFor(x => x.Curies)
                    .Cascade(CascadeMode.Stop)
                    .Must(x => x == null || x.All(IsCurie))
                    .WithMessage("{PropertyName} must be valid curie, i.e. qname ('prefix:value').");
                RuleFor(x => x.Literals)
                    .Cascade(CascadeMode.Stop)
                    .ChildRules(x =>
                        x.RuleForEach(y => y)
                            .SetValidator(new ResourceVmValidator.PredicateContentValidator.LiteralValidator()));
            }
        }

        private static bool IsCurie(string curie)
        {
            return Regex.IsMatch(curie, @"^[^:]+\:[^:]+$");
        }
    }
}
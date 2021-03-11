using System;
using System.Text.RegularExpressions;
using FluentValidation;
using LinkedData_Api.Model.ViewModels;

namespace LinkedData_Api.Validators
{
    public class EndpointVmValidator : AbstractValidator<EndpointVm>
    {
        public EndpointVmValidator()
        {
            RuleFor(x => x.EndpointName).NotEmpty().Length(3, 15).Matches(@"\A\S+\z");
            RuleFor(x => x.DefaultGraph).Must(x => x == null || ValidatorCustomMethods.CheckIfIsValidUri(x))
                .WithMessage("{PropertyName} must be valid uri (i.e. absolute uri or urn).");
            RuleFor(x => x.EndpointUrl).Must(ValidatorCustomMethods.CheckIfIsValidAbsoluteUrl)
                .WithMessage("{PropertyName} must be valid absolute uri.");
            RuleFor(x => x.SupportedMethods).Cascade(CascadeMode.Stop).Must(x =>
                x == null || (x.Sparql10 != null && x.Sparql11 != null) &&
                (x.Sparql10.Equals("yes") && x.Sparql11.Equals("yes") ||
                 x.Sparql10.Equals("yes") && x.Sparql11.Equals("no") ||
                 x.Sparql10.Equals("no") && x.Sparql11.Equals("yes"))).WithMessage(
                "{PropertyName} must be represented with values 'yes' or 'no' and at least one has to be 'yes'.");
            RuleForEach(x => x.Namespaces).SetValidator(new NamespaceValidator());
            RuleForEach(x => x.NamedGraphs).SetValidator(new NamedGraphValidator());
            RuleForEach(x => x.EntryClass).SetValidator(new EntryClassValidator());
            RuleForEach(x => x.EntryResource).SetValidator(new EntryResourceValidator());
        }


        private class NamespaceValidator : AbstractValidator<Namespace>
        {
            public NamespaceValidator()
            {
                RuleFor(x => x.Prefix).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Uri).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Uri).Must(ValidatorCustomMethods.CheckIfIsValidAbsoluteUrl)
                    .WithMessage("{PropertyName} must be valid uri (i.e absolute url).");
            }
        }

        private class NamedGraphValidator : AbstractValidator<NamedGraph>
        {
            public NamedGraphValidator()
            {
                RuleFor(x => x.GraphName).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Uri).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Uri).Must(ValidatorCustomMethods.CheckIfIsValidUri)
                    .WithMessage("{PropertyName} must be valid uri (i.e absolute url or urn).");
            }
        }

        private class EntryClassValidator : AbstractValidator<EntryClass>
        {
            public EntryClassValidator()
            {
                RuleFor(x => x.GraphName).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Command).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Command)
                    .Must(x => x != null && x.ToLower().Contains("select") && x.ToLower().Contains("where") &&
                               x.Contains("{") && x.Contains("}"))
                    .WithMessage("{PropertyName} is not valid select sparql command.");
            }
        }

        private class EntryResourceValidator : AbstractValidator<EntryResource>
        {
            public EntryResourceValidator()
            {
                RuleFor(x => x.GraphName).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Command).Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("{PropertyName} must not be empty.");
                RuleFor(x => x.Command)
                    .Must(x => x != null && x.ToLower().Contains("select") && x.ToLower().Contains("where") &&
                               x.Contains("{") && x.Contains("}"))
                    .WithMessage("{PropertyName} is not valid select sparql command.");
            }
        }
    }
}
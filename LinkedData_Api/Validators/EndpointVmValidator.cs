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
            RuleFor(x => x.DefaultGraph).Must(x=>x==null || CheckIfIsValidUri(x)).WithMessage($"'default_graph' must be valid uri (i.e. absolute uri or urn).");
            RuleFor(x => x.EndpointUrl).Must(CheckIfIsValidAbsoluteUrl)
                .WithMessage($"'EndpointUrl' must be valid absolute uri.");
            RuleFor(x => x.SupportedMethods).Must(x =>
                    x == null || (x.Sparql10.Equals("yes") && x.Sparql11.Equals("yes")) ||
                    (x.Sparql10.Equals("yes") && x.Sparql11.Equals("no")) ||
                    (x.Sparql10.Equals("no") && x.Sparql11.Equals("yes")))
                .WithMessage(
                    $"'supported_methods': 'sparql10' and 'sparql11' must be represented with values 'yes' or 'no' and at least one has to be 'yes'.");
            RuleForEach(x => x.Namespaces).SetValidator(new NamespaceValidator());
            RuleForEach(x => x.NamedGraphs).SetValidator(new NamedGraphValidator());
            RuleForEach(x => x.EntryClass).SetValidator(new EntryClassValidator());
            RuleForEach(x => x.EntryResource).SetValidator(new EntryResourceValidator());
        }


        private class NamespaceValidator : AbstractValidator<Namespace>
        {
            public NamespaceValidator()
            {
                RuleFor(x => x.Prefix).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'prefix must not be empty.'");
                RuleFor(x => x.Uri).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'uri' must not be empty.");
                RuleFor(x => x.Uri).Must(CheckIfIsValidUri).WithMessage("'uri' must be valid uri (i.e absolute url or urn).");
            }

        }
        
        private class NamedGraphValidator : AbstractValidator<NamedGraph>
        {
            public NamedGraphValidator()
            {
                RuleFor(x => x.GraphName).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'graph_name' must not be empty.'");
                RuleFor(x => x.Uri).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'uri' must not be empty.");
                RuleFor(x => x.Uri).Must(CheckIfIsValidUri).WithMessage("'uri' must be valid uri (i.e absolute url or urn).");
            }

        }
        
        private class EntryClassValidator : AbstractValidator<EntryClass>
        {
            public EntryClassValidator()
            {
                RuleFor(x => x.GraphName).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'graph_name' must not be empty.'");
                RuleFor(x => x.Command).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'command' must not be empty.");
                RuleFor(x => x.Command.ToLower()).Must(x=>x.Contains("select") && x.Contains("where") && x.Contains("{") && x.Contains("}")).WithMessage("'command' is not valid select sparql command.'");
            }

        }
        
        private class EntryResourceValidator : AbstractValidator<EntryResource>
        {
            public EntryResourceValidator()
            {
                RuleFor(x => x.GraphName).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'graph_name' must not be empty.'");
                RuleFor(x => x.Command).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("'command' must not be empty.");
                RuleFor(x => x.Command.ToLower()).Must(x=>x.Contains("select") && x.Contains("where") && x.Contains("{") && x.Contains("}")).WithMessage("'command' is not valid select sparql command.'");
            }

        }
        
        
        //only https or http absolute uri

        private static bool CheckIfIsValidAbsoluteUrl(string url)
        {
            if (url == null) return false;
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        //only absolute uri or urn
        private static bool CheckIfIsValidUri(string uri)
        {
            if (uri == null) return false;
            return CheckIfIsValidAbsoluteUrl(uri) || Regex.IsMatch(uri,@"^urn:[a-z0-9][a-z0-9-]{0,31}:[a-z0-9()+,\-.:=@;$_!*'%/?#]+$");
        }


    }
}
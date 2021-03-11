using System;
using System.Text.RegularExpressions;

namespace LinkedData_Api.Validators
{
    public class ValidatorCustomMethods
    {
        //only https or http absolute uri
        public static bool CheckIfIsValidAbsoluteUrl(string url)
        {
            if (url == null) return false;
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        //only absolute uri or urn
        public static bool CheckIfIsValidUri(string uri)
        {
            if (uri == null) return false;
            return CheckIfIsValidAbsoluteUrl(uri) ||
                   Regex.IsMatch(uri, @"^urn:[a-z0-9][a-z0-9-]{0,31}:[a-z0-9()+,\-.:=@;$_!*'%/?#]+$");
        }
        
        public static bool IsCurie(string curie)
        {
            return Regex.IsMatch(curie, @"^[^:]+\:[^:]+$");
        }
    }
}
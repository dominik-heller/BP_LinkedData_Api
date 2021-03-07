namespace LinkedData_Api.Helpers
{
    public static class HelperClass
    {
        public static string GetEndpointUrl(string url)
        {
            var x = url.Split("api/");
            var y = x[1].Split("/");
            return x[0] + "api/" + y[0];
        }
    }
}
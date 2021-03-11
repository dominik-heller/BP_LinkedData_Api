using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{
    public class ValidationErrorVm
    {
        [JsonProperty("validationErrors")]
        public List<ErrorModel> ValidationErrors { get; set; } = new List<ErrorModel>();

        [JsonProperty("customError", NullValueHandling = NullValueHandling.Ignore)]
        public CustomErrorVm CustomError { get; set; }
    }

    public class ErrorModel
    {
        [JsonProperty("fieldName")] public string FieldName { get; set; }
        [JsonProperty("message")] public string ErrorMessage { get; set; }
    }
}
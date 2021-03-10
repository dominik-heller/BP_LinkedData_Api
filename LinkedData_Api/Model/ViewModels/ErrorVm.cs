using System.Collections.Generic;

namespace LinkedData_Api.Model.ViewModels
{
    public class ErrorVm
    {
        public List<ErrorModel> ValidationErrors { get; set; } = new List<ErrorModel>();
        public string CustomErrorMessage { get; set; }
    }

    public class ErrorModel
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
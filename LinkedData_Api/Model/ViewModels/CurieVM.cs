using System.Collections.Generic;

namespace LinkedData_Api.Model.ViewModels
{
    public class CurieVm
    {
        public List<string> Curies { get; set; }

        public CurieVm()
        {
            Curies = new List<string>();
        }
    }
}
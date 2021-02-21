using System.Collections.Generic;

namespace LinkedData_Api.Model.Contracts.ResponsesVM
{
    public class IdsVm
    {
        public List<string> Ids { get; set; }

        public IdsVm()
        {
            Ids = new List<string>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;

namespace LinkedData_Api.Model.ViewModels
{
    public class LiteralVm
    {
        public List<string> Literals { get; set; }

        public LiteralVm()
        {
            Literals = new List<string>();
        }
    }
}
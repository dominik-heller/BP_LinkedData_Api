using System.ComponentModel.DataAnnotations;

namespace LinkedData_Api.Model.ViewModels
{
    public class NamedResourceVm : ResourceVm
    {
        public string ResourceCurie { get; set; }
    }
}
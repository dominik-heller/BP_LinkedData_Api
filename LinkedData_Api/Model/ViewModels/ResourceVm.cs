using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinkedData_Api.Model.ViewModels
{


    public class PropertyContent
    {
        public List<string> Curies;
        public List<Literal> Literals;

        public PropertyContent()
        {
            Curies = new List<string>();
            Literals = new List<Literal>();
        }
    }

    public class ResourceVm
    {
        public Dictionary<string, PropertyContent> Properties { get; set; }

        public ResourceVm()
        {
            Properties = new Dictionary<string, PropertyContent>();
         //   Properties = new Dictionary<string, PropertyContent>();
         //   Properties.Add("das",new PropertyContent());
         //   Properties["das"].Curies.Add("dasda");
        }
    }
}
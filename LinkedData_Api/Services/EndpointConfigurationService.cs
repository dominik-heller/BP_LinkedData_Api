#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinkedData_Api.DataModel.EndpointConfigurationDto;
using Newtonsoft.Json;

namespace LinkedData_Api.Services
{
    public class EndpointConfigurationService : IEndpointConfigurationService
    {
        private List<EndpointDto> _endpointDtos;

        public EndpointConfigurationService()
        {
            _endpointDtos = new List<EndpointDto>();
            ProcessConfigurationFiles();
        }

        public void ProcessConfigurationFiles()
        {
            string[] fileEntries = Directory.GetFiles("JsonFiles/EndpointConfiguration");
            foreach (string fileName in fileEntries)
            {
                EndpointDto endpoint = JsonConvert.DeserializeObject<EndpointDto>(File.ReadAllText(fileName));
                //TODO: Check for same endpoint_names (bud vytvorit provizorni name (jako u namespacu) a nebo vynechat a vyhodit vyjimku) 
                _endpointDtos.Add(endpoint);
            }

            Console.WriteLine();
        }

        public string? GetEndpointUrl(string endpointName)
        {
            return _endpointDtos.FirstOrDefault(x => x.EndpointName.Equals(endpointName))?.EndpointUrl;
        }
    }
}
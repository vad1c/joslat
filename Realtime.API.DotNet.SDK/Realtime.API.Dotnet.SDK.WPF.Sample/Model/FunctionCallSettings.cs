using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.WPF.Sample.Model
{
    public class FunctionCallSettings
    {
        public string Type { get; set; }  // Function Type
        public string Name { get; set; }  // Function Name
        public string Description { get; set; }  // Function Description
        public FunctionParameters Parameters { get; set; }  // Function Parameters

    }

    public class FunctionParameters
    {
        public string Type { get; set; }  // Parameter Type (e.g., "object")
        public Dictionary<string, FunctionProperty> Properties { get; set; }  // Properties of the parameters
        public List<string> Required { get; set; }  // Required parameters
    }

    public class FunctionProperty
    {
        public string Type { get; set; }  // Type of the property (e.g., "string")
        public string Description { get; set; }  // Description of the property
    }
}

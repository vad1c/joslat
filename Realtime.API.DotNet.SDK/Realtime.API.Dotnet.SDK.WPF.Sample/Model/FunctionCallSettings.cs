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
        public string type { get; set; }  // Function Type
        public string name { get; set; }  // Function Name
        public string description { get; set; }  // Function Description
        public FunctionParameters parameters { get; set; }  // Function Parameters

    }

    public class FunctionParameters
    {
        public string type { get; set; }  // Parameter Type (e.g., "object")
        public Dictionary<string, FunctionProperty> properties { get; set; }  // Properties of the parameters
        public List<string> required { get; set; }  // Required parameters
    }

    public class FunctionProperty
    {
        public string type { get; set; }  // Type of the property (e.g., "string")
        public string description { get; set; }  // Description of the property
    }
}

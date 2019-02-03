using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Models
{
    public class WatchDogErrorModel
    {
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }

        public WatchDogErrorModel()
        {

        }

        public WatchDogErrorModel(string methodName, string errorMessage, string parameter1 = "", string parameter2 = "", string parameter3 = "", string parameter4 = "", string parameter5 = "")
        {
            MethodName = methodName;
            ErrorMessage = errorMessage;
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
            Parameter4 = parameter4;
            Parameter5 = parameter5;
        }

    }
}

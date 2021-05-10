using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INE.K8s.Models
{
    public class Info
    {
        public SortedDictionary<string, string> envVariables {get;set; }
        public Dictionary<string, string> headers {get;set; }
        public Dictionary<string, string> routeValues {get;set; }

        public Info(){
            envVariables = new SortedDictionary<string, string>();
            headers = new Dictionary<string, string>();
            routeValues = new Dictionary<string, string>();
        }
    }
}
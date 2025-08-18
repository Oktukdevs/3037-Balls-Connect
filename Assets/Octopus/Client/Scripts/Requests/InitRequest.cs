using System.Collections.Generic;
using UnityEngine;

namespace Octopus.Client
{
    [System.Serializable]
    public class InitRequest : Request
    {
        public override void GenerateBody()
        {
            base.GenerateBody();
            
            body += ",\"" + Constants.ReqType + "\":\"" + ReqType.create.ToString() + "\"";
        }

        public override void GenerateURL()
        {
            url = Settings.GetDomain();
        }
    }
}

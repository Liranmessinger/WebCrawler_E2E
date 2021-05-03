using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace master.BLL
{
    public class CommonHelper
    {
        public void CheckUrlArrayIsValid(ref List<string> urlArr)
        {
            Predicate<string> uriValidator=delegate (string uri)
            {
                Uri urilocal=null;
                if(!Uri.TryCreate(uri,UriKind.Absolute,out urilocal))
                    return true;
                else
                    return false;
            };

            urlArr.RemoveAll(a => uriValidator(a));
        }

        public string GetDomainFromUri(Uri uri)
        {
            if (uri != null)
                return uri.Host;
            else
                return null;
        }
    }
}

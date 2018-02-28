using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Auth
{
    public interface IAuthInfo
    {
        string GetAppId();
        string GetAppSecret();
        Uri GetRedirectUri();
    }
}

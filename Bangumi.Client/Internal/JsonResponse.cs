using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Internal
{
#pragma warning disable IDE1006 // 命名样式
#pragma warning disable CS0649 
    internal class JsonResponse
    {
        public string request;
        public int code;
        public string error;

        public virtual void Check()
        {
            if (!string.IsNullOrWhiteSpace(this.error))
                throw new InvalidOperationException(this.error);
        }
    }
#pragma warning restore CS0649 
#pragma warning restore IDE1006 // 命名样式
}

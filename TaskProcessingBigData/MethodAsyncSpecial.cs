using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingBigData
{
    public class MethodAsyncSpecial
    {
        delegate string delegateSayInfo(string info);

        public string SayHello()
        {
            delegateSayInfo delegatesayinfo = new delegateSayInfo(SayInfo);

            IAsyncResult delegateresult = delegatesayinfo.BeginInvoke("", null, null); //会在新线程中执行
            string resultstr = delegatesayinfo.EndInvoke(delegateresult);
            return "";
        }

        public string SayInfo(string info)
        {
            return info;
        }
    }
}

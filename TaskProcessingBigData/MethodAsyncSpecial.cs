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
        public string SaySayInfo()
        {
            delegateSayInfo delegatesayinfo = new delegateSayInfo(SaySayHello);

            IAsyncResult delegateresult = delegatesayinfo.BeginInvoke("laomaotao", null, null); //会在新线程中执行
            string resultstr = delegatesayinfo.EndInvoke(delegateresult);
            return "";
        }

        public string SaySayHello(string info)
        {
            return $"hello,{info}";
        }

    }
}

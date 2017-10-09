using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public interface IJob
    {
        bool ShouldRunNow(DateTime now);
        void Run();
    }
}

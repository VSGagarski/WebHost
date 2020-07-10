using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public interface IServiceHost
    {
        Task RunAsync();
    }
}

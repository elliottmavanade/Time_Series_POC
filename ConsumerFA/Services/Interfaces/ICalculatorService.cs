using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerFA.Services.Interfaces
{
    public interface ICalculatorService
    {
        int Calculate(List<int> data, string aggregation);
    }
}

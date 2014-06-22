using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallPlan
{
    public class CallPlan
    {
        private readonly IList<Func<dynamic, Task<dynamic>>> _steps = new List<Func<dynamic, Task<dynamic>>>();

        public CallPlan(IList<Func<dynamic, Task<dynamic>>> steps)
        {
            _steps = steps;
        }

        public void Clear()
        {
            _steps.Clear();
        }

        public void AddStep(Func<dynamic, Task<dynamic>> step)
        {
            _steps.Add(step);
        }

        public async Task<dynamic> Execute(dynamic inputData)
        {
            foreach (var step in _steps)
            {
                inputData = await step(inputData);
            }
            return inputData;
        }
    }
}

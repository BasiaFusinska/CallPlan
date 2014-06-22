using System;
using System.Collections.Generic;

namespace CallPlan
{
    public class CallPlan
    {
        private readonly IList<Func<dynamic, dynamic>> _steps = new List<Func<dynamic, dynamic>>();

        public CallPlan(IList<Func<dynamic, dynamic>> steps)
        {
            _steps = steps;
        }

        public void Clear()
        {
            _steps.Clear();
        }

        public void AddStep(Func<dynamic, dynamic> step)
        {
            _steps.Add(step);
        }

        public dynamic Execute(dynamic inputData)
        {
            foreach (var step in _steps)
            {
                inputData = step(inputData);
            }
            return inputData;
        }
    }
}

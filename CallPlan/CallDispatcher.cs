using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CallPlan
{
    public class CallDispatcher
    {
        private readonly IServiceHandler _serviceHandler;
        private readonly IGroupAssigner _groupAssigner;
        private readonly IEnumerable<AgentsGroup> _groups;

        public CallDispatcher(IServiceHandler serviceHandler,
                              IGroupAssigner groupAssigner,
                              IEnumerable<AgentsGroup> groups)
        {
            _serviceHandler = serviceHandler;
            _groupAssigner = groupAssigner;

            _groups = groups;
        }

        public CallPlan CreateCallPlan(dynamic inputData)
        {
            return new CallPlan(
                new Func<dynamic, Task<dynamic>>[]
                {
                    async interaction => await _serviceHandler.HandleService(((IInteraction) interaction).Originator),
                    response => Task.Run(() => _groupAssigner.AssignGroup((ServiceResponse)response, _groups) as dynamic),
                    agentsGroup => Task.Run(() => ((AgentsGroup) agentsGroup).Assign((IInteraction) inputData) as dynamic)
                });
        }
    }
}

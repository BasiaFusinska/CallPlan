using System;
using System.Collections.Generic;

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
                new Func<dynamic, dynamic>[]
                {
                    interaction => _serviceHandler.HandleService(((IInteraction) interaction).Originator).Result,
                    response => _groupAssigner.AssignGroup((ServiceResponse)response, _groups),
                    agentsGroup => ((AgentsGroup) agentsGroup).Assign((IInteraction) inputData)
                });
        }
    }
}

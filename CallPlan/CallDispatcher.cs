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

        //public async void Dispatch(IInteraction interaction)
        //{
        //    var response = await _serviceHandler.HandleService(interaction.Originator);
        //    var matchingGroup = _groupAssigner.AssignGroup(response, _groups);
        //    matchingGroup.Assign(interaction);
        //}

        public CallPlan CreateCallPlan(dynamic inputData)
        {
            return new CallPlan(
                new Func<dynamic, dynamic>[]
                {
                    interaction => _serviceHandler.HandleService(((IInteraction) interaction).Originator).Result,
                    response => _groupAssigner.AssignGroup((ServiceResponse)response, _groups),
                    group =>
                        {
                            ((AgentsGroup) group).Assign((IInteraction) inputData);
                            return null;
                        }
                }
                );
        }
    }
}

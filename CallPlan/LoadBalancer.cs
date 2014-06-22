using System;
using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class LoadBalancer : ILoadBalancer
    {
        public void AssignInteraction(IInteraction interaction, Queue<Agent> agents)
        {
            var email = interaction as EmailInteraction;
            if (email != null)
            {
                AssignEmail(email, agents);
                return;
            }

            var call = interaction as CallInteraction;
            if (call != null)
            {
                AssignCall(call, agents);
            }
        }

        private void AssignCall(CallInteraction call, IEnumerable<Agent> agents)
        {
            var agent = agents.First();
            if (agent.Calls.Any())
                throw new Exception();

            agent.Calls.Add(call);
            //move to end
        }

        private void AssignEmail(EmailInteraction email, IEnumerable<Agent> agents)
        {
        }
    }
}

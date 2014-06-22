using System;
using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class LoadBalancer : ILoadBalancer
    {
        public Agent AssignInteraction(IInteraction interaction, Queue<Agent> agents)
        {
            var email = interaction as EmailInteraction;
            if (email != null)
            {
                return AssignEmail(email, agents);
            }

            var call = interaction as CallInteraction;
            if (call != null)
            {
                return AssignCall(call, agents);
            }
            return null;
        }

        private static Agent AssignCall(CallInteraction call, Queue<Agent> agents)
        {
            var agent = agents.Peek();
            if (agent.Calls.Any())
                throw new InteractionsOverflowException();

            agent.Calls.Add(call);

            agent = agents.Dequeue();
            agents.Enqueue(agent);

            return agent;
        }

        private static Agent AssignEmail(EmailInteraction email, Queue<Agent> agents)
        {
            var agent = agents.Peek();
            if (agent.Emails.Count >= 5)
                throw new InteractionsOverflowException();

            agent.Emails.Add(email);

            agent = agents.Dequeue();
            agents.Enqueue(agent);

            return agent;
        }
    }
}

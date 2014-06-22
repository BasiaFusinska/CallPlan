using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class LoadBalancer : ILoadBalancer
    {
        public Agent AssignInteraction(IInteraction interaction, ConcurrentQueue<Agent> agents)
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

        private static Agent AssignCall(CallInteraction call, ConcurrentQueue<Agent> agents)
        {
            Agent agent;
            while(!agents.TryPeek(out agent)) {}

            if (agent.Calls.Any())
                throw new InteractionsOverflowException();

            agent.Calls.Add(call);

            while(!agents.TryDequeue(out agent)) {}
            agents.Enqueue(agent);

            return agent;
        }

        private static Agent AssignEmail(EmailInteraction email, ConcurrentQueue<Agent> agents)
        {
            Agent agent;
            while (!agents.TryPeek(out agent)) { }

            if (agent.Emails.Count >= 5)
                throw new InteractionsOverflowException();

            agent.Emails.Add(email);

            while (!agents.TryDequeue(out agent)) { }
            agents.Enqueue(agent);

            return agent;
        }
    }
}

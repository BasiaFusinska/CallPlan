using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CallPlan
{
    public class LoadBalancer : ILoadBalancer
    {
        public Agent AssignInteraction(IInteraction interaction, ConcurrentQueue<Agent> agents)
        {
            Agent agent;
            while (!agents.TryPeek(out agent)) { }

            var email = interaction as EmailInteraction;
            if (email != null)
            {
                return AssignInteraction(email, agents, agent.Emails, bag => bag.Count >= 5);
            }

            var call = interaction as CallInteraction;
            if (call != null)
            {
                return AssignInteraction(call, agents, agent.Calls, bag => bag.Any());
            }
            return null;
        }

        private static Agent AssignInteraction<T>(T interaction, 
                                                  ConcurrentQueue<Agent> agents, 
                                                  ConcurrentBag<T> bag,
                                                  Func<ConcurrentBag<T>, bool> predicate)
            where T: IInteraction
        {
            if (predicate(bag))
                throw new InteractionsOverflowException();

            bag.Add(interaction);

            Agent agent;
            while (!agents.TryDequeue(out agent)) { }
            agents.Enqueue(agent);

            return agent;
        }
    }
}

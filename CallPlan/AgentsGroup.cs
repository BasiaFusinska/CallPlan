﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CallPlan
{
    public class AgentsGroup
    {
        private readonly ILoadBalancer _loadBalancer;
        private readonly IDictionary<Type, ConcurrentQueue<Agent>> _queues;

        private readonly IList<Agent> _agents; 
        public AgentsGroup(ILoadBalancer loadBalancer, string name, IList<Agent> agents)
        {
            _loadBalancer = loadBalancer;

            Name = name;
            _agents = agents;

            _queues = new Dictionary<Type, ConcurrentQueue<Agent>>
            {
                {typeof(CallInteraction), new ConcurrentQueue<Agent>(_agents)},
                {typeof(EmailInteraction), new ConcurrentQueue<Agent>(_agents)},
            };
        }
        public string Name { get; private set; }
        public IEnumerable<Agent> Agents { get { return _agents; } }

        public Agent Assign(IInteraction interaction)
        {
            return _loadBalancer.AssignInteraction(interaction, _queues[interaction.GetType()]);
        }
    }
}

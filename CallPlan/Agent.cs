using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class Agent
    {
        private readonly ConcurrentBag<CallInteraction> _calls = new ConcurrentBag<CallInteraction>();
        private readonly ConcurrentBag<EmailInteraction> _emails = new ConcurrentBag<EmailInteraction>();

        public Agent(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public ConcurrentBag<CallInteraction> Calls
        {
            get { return _calls; }
        }

        public ConcurrentBag<EmailInteraction> Emails
        {
            get { return _emails; }
        }

    }
}

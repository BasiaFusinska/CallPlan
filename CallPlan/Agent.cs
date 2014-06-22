using System;
using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class Agent
    {
        private readonly IList<CallInteraction> _calls = new List<CallInteraction>();
        private readonly IList<EmailInteraction> _emails = new List<EmailInteraction>();

        private readonly IList<IInteraction> _interactions = new List<IInteraction>(); 
        public void Assign(IInteraction interaction)
        {
            _interactions.Add(interaction);
        }

        public Agent(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public IList<IInteraction> Interactions
        {
            get { return _interactions; }
        }

        public IList<CallInteraction> Calls
        {
            get { return _calls; }
        }

        public IList<EmailInteraction> Emails
        {
            get { return _emails; }
        }

    }
}

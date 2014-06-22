using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallPlan
{
    public interface ILoadBalancer
    {
        void AssignInteraction(IInteraction interaction, Queue<Agent> agents);
    }

}

using System.Collections.Generic;
using System.Linq;

namespace CallPlan
{
    public class GroupAssigner : IGroupAssigner
    {
        public AgentsGroup AssignGroup(ServiceResponse response, IEnumerable<AgentsGroup> groups)
        {
            if (response == ServiceResponse.Exception || response == ServiceResponse.Timeout)
                return groups.FirstOrDefault(g => g.Name == "C");

            return (int) response %2 == 0 ? groups.FirstOrDefault(g => g.Name == "A") : groups.FirstOrDefault(g => g.Name == "B");
        }
    }
}

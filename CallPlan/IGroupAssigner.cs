using System.Collections.Generic;

namespace CallPlan
{
    public interface IGroupAssigner
    {
        AgentsGroup AssignGroup(ServiceResponse response, IEnumerable<AgentsGroup> groups);
    }
}

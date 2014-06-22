using System.Collections.Generic;
using CallPlan;
using FluentAssertions;
using NUnit.Framework;

namespace CallPlanTests
{
    [TestFixture]
    public class GroupAssignerTests
    {
        [Test]
        public void when_group_exists_group_assigner_shoud_assign_response_to_proper_group()
        {
            var groups = new List<AgentsGroup>
            {
                new AgentsGroup(null, "A", null),
                new AgentsGroup(null, "B", null),
                new AgentsGroup(null, "C", null),
            };

            var groupAssigner = new GroupAssigner();
            var group1 = groupAssigner.AssignGroup(ServiceResponse.Response1, groups);
            var group2 = groupAssigner.AssignGroup(ServiceResponse.Response2, groups);
            var group3 = groupAssigner.AssignGroup(ServiceResponse.Response3, groups);
            var group4 = groupAssigner.AssignGroup(ServiceResponse.Response4, groups);
            var group5 = groupAssigner.AssignGroup(ServiceResponse.Timeout, groups);
            var group6 = groupAssigner.AssignGroup(ServiceResponse.Exception, groups);

            group1.Name.Should().Be("B");
            group2.Name.Should().Be("A");
            group3.Name.Should().Be("B");
            group4.Name.Should().Be("A");
            group5.Name.Should().Be("C");
            group6.Name.Should().Be("C");
        }

        [Test]
        public void when_group_dos_not_exists_group_assigner_shoud_return_null_group()
        {
            var groups = new List<AgentsGroup>
            {
                new AgentsGroup(null, "A", null),
                new AgentsGroup(null, "C", null),
                new AgentsGroup(null, "D", null),
            };

            var groupAssigner = new GroupAssigner();
            var group = groupAssigner.AssignGroup(ServiceResponse.Response1, groups);

            group.Should().BeNull();
        }
    }
}

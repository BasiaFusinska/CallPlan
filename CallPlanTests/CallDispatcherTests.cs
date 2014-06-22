using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallPlan;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CallPlanTests
{
    [TestFixture]
    public class CallDispatcherTests
    {
        [Test]
        public void BurnDownTest()
        {
            var loadBalancerMock = new Mock<ILoadBalancer>();

            var groups = new List<AgentsGroup>
            {
                new AgentsGroup(loadBalancerMock.Object, "A", new[] {new Agent("1A"), new Agent("2A"), new Agent("3A")}),
                new AgentsGroup(loadBalancerMock.Object, "B", new[] {new Agent("1B"), new Agent("2B"), new Agent("3B")}),
                new AgentsGroup(loadBalancerMock.Object, "C", new[] {new Agent("1C"), new Agent("2C"), new Agent("3C")}),
            };

            var serviceMock = new Mock<IServiceHandler>();
            serviceMock.Setup(x => x.HandleService(It.IsAny<string>())).Returns(Task.FromResult(ServiceResponse.Response1));

            var assignerMock = new Mock<IGroupAssigner>();
            assignerMock.Setup(x => x.AssignGroup(It.IsAny<ServiceResponse>(), It.IsAny<IEnumerable<AgentsGroup>>()))
                .Returns(groups[0]);

            var dispatcher = new CallDispatcher(serviceMock.Object, assignerMock.Object, groups);

            //dispatcher.Dispatch(new EmailInteraction("someemail@aaa.com"));
            //dispatcher.Dispatch(new CallInteraction("2572461841"));

            //foreach (var group in groups)
            //{
            //    foreach (var agent in group.Agents)
            //    {
            //        //(agent[typeof (MailInteraction)] <= 5).Should().BeTrue();
            //        //(agent[typeof(CallInteraction)] <= 1).Should().BeTrue();
            //    }
            //}

            var callPlan = dispatcher.CreateCallPlan(new EmailInteraction("someemail@aaa.com"));

            callPlan.Execute(new EmailInteraction("someemail@aaa.com"));
        }
    }
}

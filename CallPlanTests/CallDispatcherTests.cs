using System;
using System.Collections.Generic;
using System.Linq;
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
        public void IntegrationTest()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(s => s.GetResponse("12345")).Returns(Task.FromResult("1"));
            categoryServiceMock.Setup(s => s.GetResponse("23456")).Returns(Task.FromResult("2"));
            categoryServiceMock.Setup(s => s.GetResponse("45678")).Callback<string>(_ => { throw new TimeoutException();});
            categoryServiceMock.Setup(s => s.GetResponse("email1@email.com")).Returns(Task.FromResult("3"));
            categoryServiceMock.Setup(s => s.GetResponse("email2@email.com")).Returns(Task.FromResult("4"));
            categoryServiceMock.Setup(s => s.GetResponse("email3@email.com")).Callback<string>(_ => { throw new Exception(); });

            var loadBalancer = new LoadBalancer();

            var groups = new List<AgentsGroup>
            {
                new AgentsGroup(loadBalancer, "A", new[] {new Agent("1A"), new Agent("2A"), new Agent("3A")}),
                new AgentsGroup(loadBalancer, "B", new[] {new Agent("1B"), new Agent("2B"), new Agent("3B")}),
                new AgentsGroup(loadBalancer, "C", new[] {new Agent("1C"), new Agent("2C"), new Agent("3C")}),
            };

            var dispatcher = new CallDispatcher(new ServiceHandler(categoryServiceMock.Object), new GroupAssigner(), groups);

            var callInteraction1 = new CallInteraction("12345");
            var callInteraction2 = new CallInteraction("23456");
            var callInteraction3 = new CallInteraction("34567");

            var emailInteraction1 = new EmailInteraction("email1@email.com");
            var emailInteraction2 = new EmailInteraction("email2@email.com");
            var emailInteraction3 = new EmailInteraction("email3@email.com");


            var callPlan = dispatcher.CreateCallPlan(callInteraction1);
            var agent1 = callPlan.Execute(callInteraction1) as Agent;

            callPlan = dispatcher.CreateCallPlan(callInteraction2);
            var agent2 = callPlan.Execute(callInteraction2) as Agent;

            callPlan = dispatcher.CreateCallPlan(callInteraction3);
            var agent3 = callPlan.Execute(callInteraction3) as Agent;

            callPlan = dispatcher.CreateCallPlan(emailInteraction1);
            var agent4 = callPlan.Execute(emailInteraction1) as Agent;

            callPlan = dispatcher.CreateCallPlan(emailInteraction2);
            var agent5 = callPlan.Execute(emailInteraction2) as Agent;

            callPlan = dispatcher.CreateCallPlan(emailInteraction3);
            var agent6 = callPlan.Execute(emailInteraction3) as Agent;

            agent1.Name.Should().Be("1B");
            agent1.Calls.First().Should().Be(callInteraction1);
            agent2.Name.Should().Be("1A");
            agent2.Calls.First().Should().Be(callInteraction2);
            agent3.Name.Should().Be("1C");
            agent3.Calls.First().Should().Be(callInteraction3);
            agent4.Name.Should().Be("1B");
            agent4.Emails.First().Should().Be(emailInteraction1);
            agent5.Name.Should().Be("1A");
            agent5.Emails.First().Should().Be(emailInteraction2);
            agent6.Name.Should().Be("1C");
            agent6.Emails.First().Should().Be(emailInteraction3);
        }
    }
}

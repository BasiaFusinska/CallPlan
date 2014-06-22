using System.Collections.Generic;
using System.Linq;
using CallPlan;
using FluentAssertions;
using NUnit.Framework;

namespace CallPlanTests
{
    [TestFixture]
    public class LoadBalancerTests
    {
        [Test]
        public void interactions_should_be_added_to_agents()
        {
            var agent1 = new Agent("A1");
            var agent2 = new Agent("A2");

            var call = new CallInteraction("12345");
            var email = new EmailInteraction("email@email.com");

            var agents = new Queue<Agent>(new [] {agent1, agent2});
            var loadBalancer = new LoadBalancer();

            var callAgent = loadBalancer.AssignInteraction(call, agents);
            var emailAgent = loadBalancer.AssignInteraction(email, agents);

            callAgent.Should().Be(agent1);
            callAgent.Calls.Count.Should().Be(1);
            callAgent.Calls.First().Should().Be(call);

            emailAgent.Should().Be(agent2);
            emailAgent.Emails.Count.Should().Be(1);
            emailAgent.Emails.First().Should().Be(email);
        }

        [Test]
        public void adding_interactions_should_be_balanced_between_agents()
        {
            var agent1 = new Agent("A1");
            var agent2 = new Agent("A2");

            var agents = new Queue<Agent>(new[] { agent1, agent2 });
            var loadBalancer = new LoadBalancer();

            loadBalancer.AssignInteraction(new EmailInteraction("email1@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email2@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email3@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email4@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email5@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email6@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email7@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email8@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email9@email.com"), agents);

            agent1.Emails.Count.Should().Be(5);
            agent2.Emails.Count.Should().Be(4);
        }

        [Test]
        [ExpectedException(typeof(InteractionsOverflowException))]
        public void overloaded_calls_should_throw_exception()
        {
            var agent1 = new Agent("A1");
            var agent2 = new Agent("A2");

            var agents = new Queue<Agent>(new[] { agent1, agent2 });
            var loadBalancer = new LoadBalancer();

            loadBalancer.AssignInteraction(new CallInteraction("12345"), agents);
            loadBalancer.AssignInteraction(new CallInteraction("23456"), agents);
            loadBalancer.AssignInteraction(new CallInteraction("34567"), agents);
        }

        [Test]
        [ExpectedException(typeof(InteractionsOverflowException))]
        public void overloaded_emails_should_throw_exception()
        {
            var agent1 = new Agent("A1");
            var agent2 = new Agent("A2");

            var agents = new Queue<Agent>(new[] { agent1, agent2 });
            var loadBalancer = new LoadBalancer();

            loadBalancer.AssignInteraction(new EmailInteraction("email1@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email2@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email3@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email4@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email5@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email6@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email7@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email8@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email9@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email10@email.com"), agents);
            loadBalancer.AssignInteraction(new EmailInteraction("email11@email.com"), agents);
        }


    }
}

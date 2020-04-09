using Automatonymous;
using Xunit;

namespace LinFx.Test.MassTransit.Automatonymous
{
    public class RelationshipStateMachineTests
    {
        private readonly RelationshipStateMachine machine = new RelationshipStateMachine();

        [Fact]
        public void Should_Be_Friend()
        {
            var relationship = new Relationship();
            machine.RaiseEvent(relationship, machine.Hello);

            Assert.Equal(machine.Friend, relationship.CurrentState);
        }

        [Fact]
        public void Should_Be_Friend_2()
        {
            var relationship = new Relationship();
            machine.RaiseEvent(relationship, machine.PissOff);
            machine.RaiseEvent(relationship, machine.Hello);

            Assert.Equal(machine.Friend, relationship.CurrentState);
        }

        [Fact]
        public void Should_Be_Friend_3()
        {
            var relationship = new Relationship();
            machine.RaiseEvent(relationship, machine.PissOff);
            machine.RaiseEvent(relationship, machine.Introduce);

            Assert.Equal(machine.Friend, relationship.CurrentState);
        }
    }

    /// <summary>
    /// 状态机
    /// </summary>
    class RelationshipStateMachine : AutomatonymousStateMachine<Relationship>
    {
        public RelationshipStateMachine()
        {
            Event(() => Hello);
            Event(() => PissOff);
            Event(() => Introduce);

            State(() => Friend);
            State(() => Enemy);

            InstanceState(x => x.CurrentState);

            Initially(
                When(Hello)
                    .TransitionTo(Friend),

                When(PissOff)
                    .TransitionTo(Enemy),

                When(Introduce)
                    .Then(ctx => ctx.Instance.Name = ctx.Data.Name)
                    .TransitionTo(Friend)
            );

            During(Friend,
                When(PissOff)
                .TransitionTo(Friend)
            );

            During(Enemy,
                When(Hello)
                    .TransitionTo(Friend),
                When(Introduce)
                    .TransitionTo(Friend)
            );
        }

        public State Friend { get; private set; }
        public State Enemy { get; private set; }

        public Event Hello { get; private set; }
        public Event PissOff { get; private set; }
        public Event<Person> Introduce { get; private set; }
    }

    /// <summary>
    /// 状态
    /// </summary>
    class Relationship
    {
        public State CurrentState { get; set; }

        public string Name { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
    }


}

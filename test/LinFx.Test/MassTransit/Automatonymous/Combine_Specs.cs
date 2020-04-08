using Automatonymous;
using System.Threading.Tasks;
using Xunit;

namespace LinFx.Test.MassTransit.Automatonymous
{
    public class When_combining_events_into_a_single_event
    {
        [Fact]
        public async Task Should_have_called_combined_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);
            await _machine.RaiseEvent(_instance, _machine.First);
            await _machine.RaiseEvent(_instance, _machine.Second);

            Assert.True(_instance.Called);
        }

        [Fact]
        public async Task Should_not_call_for_one_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);
            await _machine.RaiseEvent(_instance, _machine.First);

            Assert.False(_instance.Called);
        }

        [Fact]
        public async Task Should_not_call_for_one_other_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);
            await _machine.RaiseEvent(_instance, _machine.Second);

            Assert.False(_instance.Called);
        }

        TestStateMachine _machine;
        Instance _instance;


        class Instance
        {
            public CompositeEventStatus CompositeStatus { get; set; }
            public bool Called { get; set; }
            public State CurrentState { get; set; }
        }


        sealed class TestStateMachine : AutomatonymousStateMachine<Instance>
        {
            public TestStateMachine()
            {
                CompositeEvent(() => Third, x => x.CompositeStatus, First, Second);

                Initially(
                    When(Start)
                        .TransitionTo(Waiting));

                During(Waiting,
                    When(Third)
                        .Then(context => context.Instance.Called = true)
                        .Finalize());
            }

            public State Waiting { get; private set; }

            public Event Start { get; private set; }

            public Event First { get; private set; }
            public Event Second { get; private set; }
            public Event Third { get; private set; }
        }
    }

    public class When_combining_events_with_an_int_for_state
    {
        [Fact]
        public async Task Should_have_called_combined_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);

            Assert.False(_instance.Called);

            await _machine.RaiseEvent(_instance, _machine.First);
            await _machine.RaiseEvent(_instance, _machine.Second);

            Assert.True(_instance.Called);

            Assert.Equal(2, _instance.CurrentState);
        }

        [Fact]
        public async Task Should_not_call_for_one_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);
            await _machine.RaiseEvent(_instance, _machine.First);

            Assert.False(_instance.Called);
        }

        [Fact]
        public async Task Should_have_initial_state_with_zero()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);

            Assert.Equal(3, _instance.CurrentState);
        }

        [Fact]
        public async Task Should_not_call_for_one_other_event()
        {
            _machine = new TestStateMachine();
            _instance = new Instance();

            await _machine.RaiseEvent(_instance, _machine.Start);
            await _machine.RaiseEvent(_instance, _machine.Second);

            Assert.False(_instance.Called);
        }

        TestStateMachine _machine;
        Instance _instance;

        class Instance
        {
            public int CompositeStatus { get; set; }
            public bool Called { get; set; }
            public int CurrentState { get; set; }
        }


        sealed class TestStateMachine : AutomatonymousStateMachine<Instance>
        {
            public TestStateMachine()
            {
                InstanceState(x => x.CurrentState);

                CompositeEvent(() => Third, x => x.CompositeStatus, First, Second);

                Initially(
                    When(Start)
                        .TransitionTo(Waiting));

                During(Waiting,
                    When(Third)
                        .Then(context => context.Instance.Called = true)
                        .Finalize());
            }

            public State Waiting { get; private set; }

            public Event Start { get; private set; }

            public Event First { get; private set; }
            public Event Second { get; private set; }
            public Event Third { get; private set; }
        }
    }
}

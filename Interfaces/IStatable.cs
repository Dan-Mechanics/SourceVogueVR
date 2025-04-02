namespace VogueVR.FSM
{
    public interface IStatable
    {
        void EnterState(FiniteStateMachine machine);
        void DoTick();
        void DoFixedTick();
        void ExitState();
    }
}
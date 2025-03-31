namespace CodeScripts.Abstraction
{
    public abstract class IState
    {
        private int _priority;

        public IState(int priority = 0) => _priority = priority;

        public abstract void Call();
        public abstract void Overview(IData data);

        public virtual void Abort() { }

        public static bool operator >=(IState a, IState b) => a._priority >= b._priority;
        public static bool operator <=(IState a, IState b) => a._priority <= b._priority;
    }
}
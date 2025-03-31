using CodeScripts.ImprovedDelegate.Abstraction;
using System;

namespace CodeScripts.ImprovedDelegate.Event
{
    public class ImprovedEvent<T> where T : Delegate
    {
        private IImprovedDelegate<T> _improvedAction;

        public ImprovedEvent(IImprovedDelegate<T> improvedAction) 
            => _improvedAction = improvedAction;

        public void AddListener(T action, int priority = 0) 
            => _improvedAction.AddListener(action, priority);

        public void RemoveListener(T action, int priority) 
            => _improvedAction.RemoveListener(action, priority);

        public void RemoveListeners(T action)
            => _improvedAction.RemoveListeners(action);
    }
}
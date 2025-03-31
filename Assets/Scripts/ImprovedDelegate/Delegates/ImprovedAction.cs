using CodeScripts.ImprovedDelegate.Abstraction;
using System;

namespace CodeScripts.ImprovedDelegate
{
    public class ImprovedAction : IImprovedDelegate<Action>
    {
        public void SaveInvoke()
        {
            foreach (int key in _sortedKeys)
                foreach (Action d in _subscribers[key])
                    d?.Invoke();
        }        
    }
}
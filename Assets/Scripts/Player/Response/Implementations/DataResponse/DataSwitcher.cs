using System.Collections.Generic;
using CodeScripts.Abstraction;

namespace CodeScripts.PlayerResponse
{
   
    public sealed class DataSwitcher
    {
        public readonly Dictionary<object, IData> Container = new();
    }

}
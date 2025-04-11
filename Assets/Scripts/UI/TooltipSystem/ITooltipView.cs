using CodeScripts.Abstraction;

namespace CodeScripts.UI.TooltipSystem
{
    
    public interface ITooltipView
    {
        public int TimeToShowMS { get; }
        public void SetData(IData data);
        public void Show();
        public void Hide();
        public void UpdatePosition();
    }

}
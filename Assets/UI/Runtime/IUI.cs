namespace UnityEngine
{
    public interface IUI
    {
        string Name { get; set; }
        GameObject GameObject { get; set; }
    }

    public interface IUIWidget : IUI
    {
        UIWidgetOptions Options { get; set; }
    }

    public interface IUIWindow : IUI
    {
        UIWindowOptions Options { get; set; }
    }
}
namespace UnityEngine
{
    public interface IUI
    {
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
namespace SourceSharp.SP.Public.Game.Client;

public interface IViewPortPanel
{
    public string GetName();
    public void SetData(KeyValues[] data);
    public void Reset();
    public void Update();
    public bool NeedsUpdate();
    public bool HasInputElements();

    public void ShowPanel(bool state);

    public VGui.VPANEL GetVPanel();
    public bool IsVisible();
    public void SetParent(VGui.PANEL parent);
}

public interface IViewPort
{
    public void UpdateAllPanels();
    public void ShowPanel(string name, bool state);
    public void ShowPanel(IViewPortPanel panel, bool state);
    public void ShowBackground(bool show);
    public IViewPortPanel FindPanelByName(string panelName);
    public IViewPortPanel GetActivePanel();
    public void PostMessageToPanel(string name, KeyValues[] keyValues);
}
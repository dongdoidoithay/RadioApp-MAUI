using CommunityToolkit.Maui.Views;
using RadioApp.HandCursorControls;

namespace RadioApp.Controls;

public partial class MessageDialog : Popup
{
    public MessageDialog(string message)
    {
        InitializeComponent();
        LblMessage.Text = message;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        HandCursor.Binding();
    }
    private void BtnClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
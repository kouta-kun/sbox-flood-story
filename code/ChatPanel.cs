using Sandbox.UI;

namespace Sandbox;

public class ChatPanel : PanelComponent
{
	private List<string> _messages = new ();
	
	protected override void OnStart()
	{
		base.OnStart();
		Panel.StyleSheet.Load("Chat.scss");
		SetClass( "panel", true );
	}

	public void ShowMessage( string message )
	{
		_messages.Add( message );
		_messages = _messages.TakeLast( 5 ).ToList();

		Panel.DeleteChildren( true );
		foreach (var msg in _messages)
		{
			var label = Panel.AddChild<Label>();
			label.Text = msg;
		}
	}
}

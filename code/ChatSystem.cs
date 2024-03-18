namespace Sandbox;

public class ChatSystem : Component
{
	public void Say( GameObject from, string message )
	{
		GameObject.Components.Get<ChatPanel>().ShowMessage(from.Name + ": " + message);
	}

	public void RawSay( string rawMessage )
	{
		GameObject.Components.Get<ChatPanel>().ShowMessage( rawMessage );
	}
}

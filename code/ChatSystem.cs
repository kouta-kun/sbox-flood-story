namespace Sandbox;

public class ChatSystem : Component
{
	public void Say( GameObject from, string message )
	{
		GameObject.Components.Get<ChatPanel>().ShowMessage(from.Name + ": " + message);
	}
}

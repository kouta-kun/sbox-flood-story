namespace Sandbox;

public abstract class Interactable : Component
{
	public const string Tag = "interactable";

	protected override void OnEnabled()
	{
		base.OnEnabled();
		if ( !GameObject.Tags.Has( Tag ) )
		{
			GameObject.Tags.Add( Tag );
		}
	}

	public abstract void Approach( bool enable );

	public abstract IEnumerable<ButtonInformation> Use();

	protected T GetPlayerComponent<T>() where T : Component
	{
		return Scene.Children.Find( go => go.Name == "Player" ).Components.Get<T>();
	}

	protected T GetScreenPanel<T>() where T : PanelComponent
	{
		return Scene.Children.Find( go => go.Name == "Screen" ).Components.Get<T>();
	}

	protected T GetScreenComponent<T>() where T : Component
	{
		return Scene.Children.Find( go => go.Name == "Screen" ).Components.Get<T>() ?? Scene.Children.Find(go => go.Name == "ChatScreen").Components.Get<T>();
	}
}

using Sandbox.UI;

namespace Sandbox;

public class NamePanel : PanelComponent
{
	private GameObject Parent => GameObject.Parent;

	protected override void OnStart()
	{
		base.OnStart();
		var label = Panel.AddChild<Label>();
		label.Text = Parent.Name;
	}
}

using System;
using Sandbox.UI;

namespace Sandbox;

public struct ButtonInformation
{
	public string Label;

	public Action Action;
}

public class InteractionPanel : PanelComponent
{
	private List<Button> buttons = new();

	public InteractionPanel()
	{
	}

	protected override void OnEnabled()
	{
		base.OnEnabled();
		Panel.StyleSheet.Load( "Classes.scss" );
	}

	public IEnumerable<ButtonInformation> Buttons
	{
		set
		{
			ClearButtons();

			if ( value is not null && value.Any() )
			{
				foreach ( var buttonInformation in value )
				{
					var btn = Panel.AddChild<Button>();
					btn.Text = buttonInformation.Label;
					btn.AddEventListener( "onclick", () =>
					{
						buttonInformation.Action();
						ClearButtons();
						Hide();
					} );
					buttons.Add( btn );
				}

				Show();
			}
			else
			{
				Hide();
			}
		}
	}

	public void Hide()
	{
		SetClass("panel-enabled", false);
	}

	public void Show()
	{
		SetClass("panel-enabled", true);
	}

	private void ClearButtons()
	{
		foreach (var button in buttons)
		{
			button.Delete();
		}

		buttons.Clear();
	}

	public bool Visible => buttons.Count > 0;
}

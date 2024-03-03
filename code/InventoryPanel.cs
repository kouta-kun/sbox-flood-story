using System;
using Sandbox.UI;

namespace Sandbox;

public class InventoryPanel : PanelComponent
{
	[Property] public InventoryComponent inventory;

	protected override void OnEnabled()
	{
		base.OnEnabled();
		Panel.StyleSheet.Load( "Classes.scss" );
	}


	public void Hide()
	{
		SetClass( "panel-enabled", false );
		Panel.DeleteChildren();
	}

	public void Show( Action<IEnumerable<InventoryItem>> interact = null )
	{
		SetClass( "panel-enabled", true );
		Panel.DeleteChildren();
		foreach ( var item in inventory.Items.GroupBy( k => k.Name ) )
		{
			var btn = Panel.AddChild<Button>();
			btn.Text = item.Count() + "x " + item.Key;
			if ( interact != null )
			{
				btn.AddEventListener( "onclick", () =>
					{
						interact( item );
						Hide();
					}
				);
			}
			else
			{
				btn.Disabled = true;
				btn.SetClass( "disabled", true );
			}
		}

		if ( interact != null )
		{
			var btn = Panel.AddChild<Button>();
			btn.Text = "Cancel";
			btn.AddEventListener( "onclick", () =>
			{
				interact( Enumerable.Empty<InventoryItem>() );
				Hide();
			} );
		}
	}

	public void Toggle()
	{
		if ( Visible ) Hide();
		else Show();
	}

	public bool Visible => HasClass( "panel-enabled" );
}

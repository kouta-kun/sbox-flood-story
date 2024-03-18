namespace Sandbox;

public class YoungKidNPCComponent : Interactable
{
	private bool WasPacified;

	protected override void OnStart()
	{
		base.OnStart();
		WasPacified = false;
		Components.Get<SkinnedModelRenderer>().Set( "scale_height", 0.85f );
	}

	public override void Approach( bool enable )
	{
		if ( enable )
		{
			if ( !WasPacified ) GetScreenComponent<ChatSystem>().RawSay( "The kid is crying." );
			else GetScreenComponent<ChatSystem>().RawSay( "The kid is focused on the candy." );
		}
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		var talkTo = new[]
		{
			new ButtonInformation
			{
				Label = "Talk",
				Action = () =>
				{
					if ( !WasPacified )
						GetScreenComponent<ChatSystem>().Say( GameObject,
							"I'm scared of the storm, I wanna see my mommy!" );
					else
						GetScreenComponent<ChatSystem>().RawSay( "The kid is too focused on the candy." );
				}
			}
		};
		var giveItem = new[]
		{
			new ButtonInformation
			{
				Label = "Give Item",
				Action = () =>
				{
					GetScreenPanel<InventoryPanel>().Show( ( items ) =>
					{
						items = items.ToList();
						if ( !items.Any() ) return;
						var inventoryItem = items.First();
						if ( inventoryItem.Name != "Candy" ) return;
						GetPlayerComponent<InventoryComponent>().Items.Remove( inventoryItem );
						GetScreenComponent<ChatSystem>().Say( GameObject, "Wow, thanks a lot mister!" );
						WasPacified = this.AccessGameManager().KidPacified = true;
					} );
				}
			}
		};
		return WasPacified
			? talkTo
			: talkTo.Concat( giveItem );
	}
}

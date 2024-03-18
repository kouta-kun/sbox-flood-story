namespace Sandbox;

public class VendingMachineComponent : Interactable
{
	public override void Approach( bool enable )
	{
		
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Buy Candy (20 credits)",
				Action = () =>
				{
					var inventoryComponent = GetPlayerComponent<InventoryComponent>();
					var coins = inventoryComponent.Items.Where(p => p.Name == "Coin").ToList();
					if ( coins.Count >= 20 )
					{
						var inventoryItem = new InventoryItem( "Coin" );
						for ( var i = 0; i < 20; i++ )
						{
							inventoryComponent.Items.Remove( inventoryItem );
						}

						inventoryComponent.Items.Add( new InventoryItem( "Candy" ) );

					}
					else
					{
						GetScreenComponent<ChatSystem>().RawSay("You don't have enough coins.");
					}
				}
			}
		};
	}
}

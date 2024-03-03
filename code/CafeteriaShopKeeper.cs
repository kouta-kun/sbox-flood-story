namespace Sandbox;

public class CafeteriaShopKeeper : Interactable
{
	public override void Approach( bool enable )
	{
		if ( enable )
		{
			GetScreenComponent<ChatSystem>().Say(GameObject, "Welcome!");
		}
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Buy Coffee",
				Action = () =>
				{
					var inventoryItems = GetPlayerComponent<InventoryComponent>().Items;
					if ( inventoryItems.Count( k => k.Name == "Coin" ) >= 20 )
					{
						var coinItem = new InventoryItem("Coin");
						for(var i = 0; i < 20; i++)
						{
							inventoryItems.Remove( coinItem );
						}

						inventoryItems.Add(new InventoryItem("Coffee"));
						GetScreenComponent<ChatSystem>().Say(GameObject, "Thank you, come again!");
					}
				}
			}
		};
	}
}

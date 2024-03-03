using System;

namespace Sandbox;

public class TestInteractable : Interactable
{
	public override void Approach( bool enable )
	{
		return;
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = GameObject.Name + " - Change color",
				Action = () =>
				{
					GameObject.Components.Get<ModelRenderer>().Tint = Random.Shared.Color();
				}
			},
			new ButtonInformation
			{
				Label = GameObject.Name + " - Give Test Item",
				Action = () =>
				{
					GetPlayerComponent<InventoryComponent>().Items
						.Add( new InventoryItem( GameObject.Name + " fruit" ) );
				}
			},
			new ButtonInformation
			{
				Label = GameObject.Name + " - Consume item",
				Action = () =>
				{
					GetScreenPanel<InventoryPanel>().Show( items =>
						{
							items = items.ToList();
							if ( !items.Any() )
							{
								Log.Info( "Canceled" );
							}
							else
							{
								var inventoryItem = items.First();
								Log.Info( "Consuming " + inventoryItem.Name );
								GetPlayerComponent<InventoryComponent>().Items.Remove( inventoryItem );
							}
						}
					);
				}
			}
		};
	}
}

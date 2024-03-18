using System.ComponentModel;
using System.Threading.Tasks;

namespace Sandbox;

public class HomelessMoises : Interactable
{

	public override void Approach( bool enable )
	{
		if ( enable )
		{
			GetScreenComponent<ChatSystem>().Say( GameObject, "Hey there." );
		}
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Talk",
				Action = () =>
				{
					GetScreenComponent<ChatSystem>().Say( GameObject,
						"This rain is making me really cold... I could use a hot coffee." );
				}
			},
			new ButtonInformation
			{
				Label = "Give Item",
				Action = () =>
				{
					GetScreenComponent<InventoryPanel>().Show( ( itemSet ) =>
					{
						itemSet = itemSet.ToList();
						if ( !itemSet.Any() ) return;
						var selectedItem = itemSet.First();
						if ( selectedItem.Name == "Coffee" )
						{
							GetPlayerComponent<InventoryComponent>().Items.Remove( selectedItem );
							GetScreenComponent<ChatSystem>().Say( GameObject,
								"Thank you so much! I was about to freeze to death..." );
							Task.DelayRealtimeSeconds( 4 ).ContinueWith( ( t ) =>
							{
								this.AccessGameManager().GiveCoffee();
								GetScreenComponent<ChatSystem>().Say( GameObject,
									"Hey, I think the tide is lowering!" );
							} );
						}
					} );
				}
			}
		};
	}
}

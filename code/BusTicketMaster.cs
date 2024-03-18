namespace Sandbox;

public class BusTicketMaster : Interactable
{
	public override void Approach( bool enable )
	{
		if ( enable )
		{
			GetScreenComponent<ChatSystem>().Say( GameObject, "Welcome!" );
		}
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Buy Bus Ticket",
				Action = () =>
				{
					var gameManagerComponent = this.AccessGameManager();
					if ( gameManagerComponent
					    .GaveOutCoffee )
					{
						GetScreenComponent<ChatSystem>().Say( GameObject,
							"The flood stopped! The bus is leaving in a couple of minutes." );
						Task.DelayRealtimeSeconds( 5 ).ContinueWith( ( _ ) =>
							gameManagerComponent.Finish() );
					}
					else
					{
						GetScreenComponent<ChatSystem>().Say( GameObject,
							"Sorry man, buses aren't going out due to the floods." );
					}
				}
			}
		};
	}
}

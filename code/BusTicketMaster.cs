namespace Sandbox;

public class BusTicketMaster : Interactable
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
				Label = "Buy Bus Ticket",
				Action = () =>
				{
					GetScreenComponent<ChatSystem>().Say(GameObject, "Sorry man, buses aren't going out due to the floods.");
				}
			}
		};
	}
}

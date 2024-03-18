namespace Sandbox;

public class PizzaComponent : Interactable
{
	public override void Approach( bool enable )
	{
		if ( enable )
			GetScreenComponent<ChatSystem>().RawSay( "There's an old pizza on the bin." );
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Eat",
				Action = () =>
				{
					GetScreenComponent<ChatSystem>().RawSay( "This pizza looks way too old to eat. You're now dead." );
					this.AccessGameManager().AteBadPizza = true;
					GetPlayerComponent<SkinnedModelRenderer>().Set("sit", 2);
					GetPlayerComponent<SkinnedModelRenderer>().Set("sit_pose", 2);
					Task.DelayRealtimeSeconds( 3 ).ContinueWith( ( t ) =>
					{
						this.AccessGameManager().Finish();
					} );
				}
			}
		};
	}
}

namespace Sandbox;

public class BikeComponent : Interactable
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
				Label = "Ride Bike",
				Action = () =>
				{
					GetScreenComponent<ChatSystem>().RawSay("Not only is this not your bike, but you can't ride a bike across a flood!");
					this.AccessGameManager().RodeBike = true;
				}
			}
		};
	}
}

namespace Sandbox;

public class PhoneComponent : Interactable
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
				Label = "Call",
				Action = () =>
				{
					GetScreenComponent<ChatSystem>().RawSay(
						"You don't remember any phone numbers, " +
						"and anyone you could call is probably in a similar situation."
					);
					this.AccessGameManager().TriedToCall = true;
				}
			}
		};
	}
}

namespace Sandbox;

public static class ExtendRandom
{
	public static T RandomChoice<T>( this IEnumerable<T> self )
	{
		return self.Skip( Game.Random.Next( self.Count() ) ).First();
	}
}

public class GenericNPCComponent : Interactable
{
	private string[] approachTexts = { "Hey...", "Hey.", "Hey!" };

	private string[] smalltalkTexts =
	{
		"Ugh, I'm running late for work.", "I feel so bad for the homeless guy around the corner...",
		"I love rain!", "I'm supposed to be at the airport right now!"
	};

	private string[] smalltalkNoFloodTexts =
	{
		"The rain has finally stopped!", "I can finally get home.", "Aww, I wish it was still raining."
	};

	public override void Approach( bool enable )
	{
		if ( enable )
			GetScreenComponent<ChatSystem>().Say( GameObject, approachTexts.RandomChoice() );
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Small talk",
				Action = () =>
				{
					this.AccessGameManager().PeopleSpokenTo.Add( GameObject.Name );
					GetScreenComponent<ChatSystem>().Say( GameObject,
						(this.AccessGameManager().GaveOutCoffee ? smalltalkNoFloodTexts : smalltalkTexts)
						.RandomChoice() );
				}
			}
		};
	}
}

namespace Sandbox;

public class NoticeBoardComponent : Interactable
{
	private string[] BillboardTexts = {
		"Public transport strike next friday from 08:00 to 14:00",
		"Like how we drive? Tell us at 9784-2564-11!",
		"Bus movies showing at 21:00 every day next week at Nazionale Bus Compagnia Theater. Free entry with a recent bus ticket."
	};

	private HashSet<string> ReadTexts = new HashSet<string>();

	protected override void OnStart()
	{
		base.OnStart();
		ReadTexts.Clear();
	}

	public override void Approach( bool enable )
	{
		
	}

	public override IEnumerable<ButtonInformation> Use()
	{
		return new[]
		{
			new ButtonInformation
			{
				Label = "Read Notice board",
				Action = () =>
				{
					var inventoryComponent = GetPlayerComponent<InventoryComponent>();
					if ( !inventoryComponent.Items.Contains( new InventoryItem( "Debit Card" ) ) )
					{
						GetScreenComponent<ChatSystem>().RawSay( "\"Lost Debit Card card found near ATM? We found it.\"" );
						GetScreenComponent<ChatSystem>().RawSay( "Your debit card was stapled under the note." );
						inventoryComponent.Items.Add(new InventoryItem("Debit Card"));
						return;
					}
					var billboardText = BillboardTexts.RandomChoice();
					ReadTexts.Add( billboardText );
					GetScreenComponent<ChatSystem>().RawSay('"' + billboardText + '"');
					if ( Game.Random.NextDouble() < 0.2 )
					{
						GetScreenComponent<ChatSystem>().RawSay("You find a 20 credit bill stuck under the note.");
						inventoryComponent.Items.AddRange(Enumerable.Repeat(new InventoryItem("Coin"), 20));
					}

					if ( ReadTexts.Count == BillboardTexts.Length )
					{
						this.AccessGameManager().ReadEntireBoard = true;
					}
				}
			}
		};
	}
}

namespace Sandbox;

public class ATMMachineComponent : Interactable
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
				Label = "Withdraw credits",
				Action = () =>
				{
					GetScreenPanel<InventoryPanel>().Show( (items) =>
					{
						items = items.ToList();
						if ( !items.Any() ) return;
						var item = items.First();
						var chatSystem = GetScreenComponent<ChatSystem>();
						switch (item.Name)
						{
							case "Debit Card":
								{
									var chance = Game.Random.NextDouble();
									var inventory = GetPlayerComponent<InventoryComponent>();
									switch ( chance )
									{
										case < 0.2:
											chatSystem.RawSay( "The ATM Machine ate your card!" );
											inventory.Items.Remove( item );
											break;
										case < 0.5:
											chatSystem.RawSay( "The bank system timed out. Try again." );
											break;
										default:
											{
												var addedCoins = Game.Random.Next( 50, 150 );
												inventory.Items.AddRange( Enumerable.Repeat( new InventoryItem( "Coin" ),
													addedCoins ) );
												chatSystem.RawSay( "You've withdrawn " + addedCoins +
												                   " coins! Thanks for being a Bank of gm_bigcity client!" );
												break;
											}
									}

									break;
								}
							default:
								chatSystem.RawSay( "That's not a debit card!" );
								break;
						}
					});
				}
			}
		};
	}
}

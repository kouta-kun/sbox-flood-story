using Sandbox.UI;

namespace Sandbox;

public struct InventoryItem
{
	public readonly string Name;

	public InventoryItem( string name )
	{
		Name = name;
	}
}

public class InventoryComponent : Component
{
	[Property] public readonly List<InventoryItem> Items = new();
}

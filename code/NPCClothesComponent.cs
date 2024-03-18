using System;

namespace Sandbox;

public class NpcClothesComponent : Component
{
	[Property]
	public string ClothesJSON = "";

	protected override void OnStart()
	{
		base.OnStart();
		ReloadClothes();
	}
	
	protected override void OnUpdate()
	{
		base.OnUpdate();
		var screenPos = Scene.Camera.PointToScreenPixels( this.Transform.Position + Vector3.Up * 96);
		if(screenPos.x > 0 && screenPos.x < Screen.Width && screenPos.y > 0 && screenPos.y < Screen.Height)
			Gizmo.Draw.ScreenText(GameObject.Name, screenPos, flags:TextFlag.Center);
	}

	public void ReloadClothes()
	{
		foreach ( var gameObject in GameObject.Children.Where( p => p.Tags.Has( "clothing" ) ) )
		{
			gameObject.Destroy();
		}

		var containers = ClothingContainer.CreateFromJson( ClothesJSON );

		if ( GameManager.ActiveScene != GameObject.Scene )
		{
			Log.Error("You are not in the active scene. Reload, restart S&Box, or do something but I cannot proceed safely.");
			return;
		}

		containers.Apply( GameObject.Components.Get<SkinnedModelRenderer>() );

	}
}

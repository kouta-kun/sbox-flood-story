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

		ExtensionClass.Apply( containers, GameObject.Components.Get<SkinnedModelRenderer>() );
		
	}
}

public static class ExtensionClass
{
	public static void Apply( ClothingContainer self, SkinnedModelRenderer body )
	{
		self.Reset( body );
		var clothing1 = self.Clothing;
		var material1 = clothing1?.Select( (Func<Clothing, string>)(x => x?.SkinMaterial) )
			.Where( (Func<string, bool>)(x => !string.IsNullOrWhiteSpace( x )) )
			.Select( (Func<string, Material>)(Material.Load) )
			.FirstOrDefault();
		var clothing2 = self.Clothing;
		var material2 = clothing2?.Select( (Func<Clothing, string>)(x => x?.EyesMaterial) )
			.Where( (Func<string, bool>)(x => !string.IsNullOrWhiteSpace( x )) )
			.Select( (Func<string, Material>)(Material.Load) )
			.FirstOrDefault();
		if ( material1 != null )
			body.SetMaterialOverride( material1, "skin" );
		if ( material2 != null )
			body.SetMaterialOverride( material2, "eyes" );
		foreach ( var clothing3 in self.Clothing )
		{
			var model1 =
				clothing3.GetModel(
					self.Clothing.Except( new[] { clothing3 } ) );
			if ( !string.IsNullOrEmpty( model1 ) && string.IsNullOrEmpty( clothing3.SkinMaterial ) )
			{
				var model2 = Model.Load( model1 );
				if ( model2 is { IsError: false } )
				{
					var gameObject = new GameObject( false, "Clothing - " + clothing3.ResourceName ) { Parent = body.GameObject };
					gameObject.Tags.Add( "clothing" );
					var skinnedModelRenderer2 = gameObject.Components.Create<SkinnedModelRenderer>();
					skinnedModelRenderer2.Model = Model.Load( clothing3.Model );
					skinnedModelRenderer2.BoneMergeTarget = body;
					if ( material1 != null )
						skinnedModelRenderer2.SetMaterialOverride( material1, "skin" );
					if ( material2 != null )
						skinnedModelRenderer2.SetMaterialOverride( material2, "eyes" );
					if ( !string.IsNullOrEmpty( clothing3.MaterialGroup ) )
						skinnedModelRenderer2.MaterialGroup = clothing3.MaterialGroup;
					gameObject.Enabled = true;
				}
			}
		}

		foreach ( var (name, value) in self.GetBodyGroups() )
		{
			body.SetBodyGroup( name, value );
		}
	}
}

using System;

namespace Sandbox;

public class PointerComponent : Component
{
	[Property] public float DegreeRotation = 45;
	private PlayerComponent _player;
	private Vector3 _playerDistanceVector;
	private CameraMovementComponent _cameraMovementComponent;

	protected override void OnStart()
	{
		_player = Scene.Children.Find(go => go.Name == "Player").Components.Get<PlayerComponent>();
		_playerDistanceVector = (Transform.Position - _player.Transform.Position).WithZ(0)/*.ClampLength( 250 )*/;
		_cameraMovementComponent =
			Scene.Children.Find( go => go.Name == "Camera" ).Components.Get<CameraMovementComponent>();
	}

	protected override void OnFixedUpdate()
	{
		Vector2 rotatedDelta = 0;
		if ( !Input.Down( "attack2" ) )
		{
			var mouseDelta = Input.MouseDelta;

			var degreeRotation = DegreeRotation - _cameraMovementComponent.AngleRotate;
			var cosine = (float)Math.Cos( degreeRotation.DegreeToRadian() );
			var sine = (float)Math.Sin( degreeRotation.DegreeToRadian() );

			rotatedDelta = new Vector2(
				mouseDelta.x * cosine - mouseDelta.y * sine,
				mouseDelta.x * sine + mouseDelta.y * cosine
			);
		}

		if(rotatedDelta.Length > 0)
		{

			Transform.Position += new Vector3(
				rotatedDelta.x, -rotatedDelta.y, 0f
			);
			_playerDistanceVector = (Transform.Position - _player.Transform.Position).ClampLength( 750 );


			Transform.Position = _player.Transform.Position + _playerDistanceVector;
		}
		else
		{
			Transform.Position = _player.Transform.Position + _playerDistanceVector;
		}

		if ( Input.Pressed( "attack1" ) )
		{
			_player.Target = Transform.Position;
		}
	}
}

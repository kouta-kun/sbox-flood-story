using System;

namespace Sandbox;

public class CameraMovementComponent : Component
{
	[Property] public GameObject Follow;

	private Vector3 _positionDistance;
	[Property] public float AngleRotate = 0;
	private float _camRotate = 0;

	private float _camDistMult = 1;

	protected override void OnAwake()
	{
		base.OnAwake();
		_positionDistance = GameObject.Transform.Position - Follow.Transform.Position;
		_camRotate = GameObject.Transform.Rotation.Angles().yaw;
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		GameObject.Transform.Position = Follow.Transform.Position +
		                                (_positionDistance.RotateAround( Vector3.Zero,
			                                Rotation.FromAxis( Vector3.Up, AngleRotate ) ) * _camDistMult);
		GameObject.Transform.Rotation =
			GameObject.Transform.Rotation.Angles().WithYaw( _camRotate + AngleRotate ).ToRotation();
	}

	protected override void OnUpdate()
	{
		if ( Input.Down( "attack2" ) )
		{
			AngleRotate += Input.MouseDelta.x * Time.Delta * 4;
		}
		
		// Log.Info(Input.MouseWheel.ToString());

		if ( Math.Abs(Input.MouseWheel.y) > 0 )
		{
			_camDistMult = Math.Clamp( _camDistMult - Input.MouseWheel.y * Time.Delta, 0.1f, 2.5f );
		}
	}
}

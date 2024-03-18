using System;

public sealed class PlayerComponent : Component
{
	private const int InteractionRadius = 100;
	private CharacterController _characterController;
	private Vector3 _target;
	private SkinnedModelRenderer _skinnedModelRenderer;
	private InteractionPanel _interactionPanel;
	private InventoryPanel _inventoryPanel;
	private InventoryComponent _inventory;

	[Property] public Vector3 Velocity => _characterController?.Velocity ?? Vector3.Zero;
	[Property] public Vector3 Direction => (Target - Transform.Position).Normal;
	[Property] public float Distance => Target.Distance( Transform.Position );
	[Property] public float AngleDelta => Vector3.GetAngle( Direction, Transform.Rotation.Forward );

	protected override void OnStart()
	{
		FindSceneObjects();

		ApplyClothes();

		TranslateToMapStartup();

		_inventory.Items.Clear();
		_inventory.Items.AddRange( Enumerable.Repeat( new InventoryItem( "Coin" ), 50 ) );
		_inventory.Items.Add(new InventoryItem("Debit Card"));

		Target = Transform.Position;
	}

	private void FindSceneObjects()
	{
		_characterController = GameObject.Components.Get<CharacterController>();
		_skinnedModelRenderer = GameObject.Components.Get<SkinnedModelRenderer>();
		_screenObject = Scene.Children.Find( go => go.Name == "Screen" );
		_interactionPanel = _screenObject.Components.Get<InteractionPanel>();
		_inventoryPanel = _screenObject.Components.Get<InventoryPanel>();
		_inventory = GameObject.Components.Get<InventoryComponent>();
	}

	private void ApplyClothes()
	{
		var clothings = ClothingContainer.CreateFromLocalUser();
		clothings.Apply( _skinnedModelRenderer );
	}

	private void TranslateToMapStartup()
	{
		var infoPlayerStartObj = Scene.Children.Find( go => go.Name == "Map" ).Children
			.Find( go => go.Name == "info_player_start" );

		Transform.Position = infoPlayerStartObj.Transform
			.Position;
		Transform.Rotation = infoPlayerStartObj.Transform.Rotation;
	}

	private List<GameObject> _lastFrameInteractables = new(10);
	private List<GameObject> _thisFrameInteractables = new(10);
	private int _lastFrameStringHash = 0;
	private GameObject _screenObject;

	protected override void OnUpdate()
	{
		// Gizmo.Draw.LineSphere( Transform.Position, InteractionRadius );

		HandleInteraction();
	}

	private void HandleInteraction()
	{
		var interactablesHash = FindInteractables();
		if ( interactablesHash != _lastFrameStringHash )
		{
			foreach ( var gameObject in _lastFrameInteractables.Where( i => !_thisFrameInteractables.Contains( i ) ) )
			{
				gameObject.Components.Get<HighlightOutline>( true ).Enabled = false;
				gameObject.Components.Get<Interactable>().Approach( false );
			}

			foreach ( var gameObject in _thisFrameInteractables.Where(i => !_lastFrameInteractables.Contains(i)) )
			{
				gameObject.Components.Get<Interactable>().Approach( true );
			}

			foreach ( var gameObject in _thisFrameInteractables )
			{
				var highlightOutline = gameObject.Components.Get<HighlightOutline>( true );
				if ( highlightOutline is null )
				{
					highlightOutline = gameObject.Components.Create<HighlightOutline>();
					highlightOutline.Color = Color.Cyan;
				}

				highlightOutline.Enabled = true;
			}

			_lastFrameInteractables.Clear();
			_lastFrameInteractables.AddRange( _thisFrameInteractables );
			_lastFrameStringHash = interactablesHash;
		}

		if ( Input.Pressed( "use" ) && !_inventoryPanel.Visible )
		{
			if ( _thisFrameInteractables.Count == 0 || _interactionPanel.Visible )
			{
				_interactionPanel.Buttons = null;
			}
			else
			{
				_interactionPanel.Buttons = Enumerable
					.Repeat( _thisFrameInteractables.SelectMany( a => a.Components.Get<Interactable>().Use() ), 1 )
					.SelectMany( a => a );
			}
		}

		if ( Input.Pressed( "inventory" ) && !_interactionPanel.Visible )
		{
			_inventoryPanel.Toggle();
		}

		// if ( Input.Pressed( "reload" ) )
		// {
		// 	Scene.Children.Find( go => go.Name == "GM" ).Components.Get<GameManagerComponent>().Finish();
		// }
	}

	private int FindInteractables()
	{
		_thisFrameInteractables.Clear();
		_thisFrameInteractables.AddRange(
			Scene.FindInPhysics( new Sphere( Transform.Position, InteractionRadius ) )
				.Where( p => p.Tags.Has( Interactable.Tag ) )
		);

		var interactablesHash =
			_thisFrameInteractables.Aggregate( 0, ( a, b ) => string.GetHashCode( a + "," + b.Name ) );
		return interactablesHash;
	}

	protected override void OnFixedUpdate()
	{
		if ( !_characterController.IsOnGround && _characterController.Velocity.z >= -15f )
		{
			_characterController.Accelerate( Vector3.Down * 15 );
		}

		if ( Input.Pressed( "duck" ) )
		{
			_characterController.Height = 32;
			_skinnedModelRenderer.Set( "duck", 1.0f );
		}
		else if ( Input.Released( "duck" ) )
		{
			_characterController.Height = 64;
			_skinnedModelRenderer.Set( "duck", 0.0f );
		}

		HandleMovement();

		_characterController.Move();
	}

	private void HandleMovement()
	{
		if ( Target.Distance( Transform.Position ) > 2f )
		{
			var speed = 80;

			if ( Input.Down( "run" ) )
				speed = 200;
			
			var direction = (Target - Transform.Position).Normal;

			var targetPosition = Transform.Position + direction * speed * Time.Delta;

			var initialPosition = Transform.Position;
			_characterController.MoveTo( targetPosition, true );
			var finalPosition = Transform.Position;

			var moveLength = (finalPosition - initialPosition).Length;
			var transformTranslationLength = (targetPosition - initialPosition).Length;
			if ( Math.Abs( moveLength - transformTranslationLength ) > 0.5 &&
			     Math.Abs( finalPosition.z - initialPosition.z ) < 0.5 )
			{
				// Log.Info( initialPosition + " -> " + finalPosition + " = " + moveLength + " - " +
				//           transformTranslationLength + " = " + Math.Abs(moveLength - transformTranslationLength) );
				// Log.Info("Z diff = " + Math.Abs(finalPosition.z - initialPosition.z));
				Target = Transform.Position;
			}

			var transformRotation = Rotation.LookAt( direction, Vector3.Up ).Angles();
			transformRotation.pitch = 0;
			transformRotation.roll = 0;
			Transform.Rotation = Transform.Rotation.Angles().LerpTo( transformRotation, 0.9995f * Time.Delta * 2.5f );

			direction.x = -direction.x;

			direction = direction.RotateAround( Vector3.Zero,
				Rotation.FromAxis( Vector3.Up, 180 + Transform.Rotation.Angles().yaw ) );

			_skinnedModelRenderer.Set( "move_x", direction.x * speed );
			_skinnedModelRenderer.Set( "move_y", direction.y * speed );
			_skinnedModelRenderer.Set( "move_z", 0 );
		}
		else
		{
			_skinnedModelRenderer.Set( "move_x", 0 );
			_skinnedModelRenderer.Set( "move_y", 0 );
			_skinnedModelRenderer.Set( "move_z", 0 );
		}
	}

	public Vector3 Target
	{
		get => _target;
		set => _target = value.WithZ( Transform.Position.z );
	}
}

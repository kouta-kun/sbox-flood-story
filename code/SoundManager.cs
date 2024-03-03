namespace Sandbox;

public class SoundManager : Component
{
	[Property] public SoundEvent RainLoopSound;
	private SoundHandle _rainHandle;
	private TimeSince _lastThunder = 0;

	protected override void OnStart()
	{
		base.OnStart();
		_rainHandle = Sound.Play( RainLoopSound );
	}
}

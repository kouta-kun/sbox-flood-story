using System.Threading.Tasks;
using Sandbox;

public static class GameManagerAccess
{
	public static GameManagerComponent AccessGameManager<T>( this T component ) where T : Component
	{
		return component.Scene.Children.Find( go => go.Name == "GM" ).Components
			.Get<GameManagerComponent>();
	}
}

public sealed class GameManagerComponent : Component
{
	private GameObject _waterObject;
	public bool GaveOutCoffee { get; private set; }
	public bool KidPacified;
	public HashSet<string> PeopleSpokenTo { get; init; } = new();
	public bool AteBadPizza { get; set; }
	public bool TriedToCall { get; set; }
	public bool ReadEntireBoard { get; set; }
	public bool RodeBike { get; set; }

	protected override void OnStart()
	{
		base.OnStart();
		_waterObject = Scene.Children.Find( go => go.Name == "Map" ).Children.Find( go => go.Name == "func_brush" );
		GaveOutCoffee = false;
		KidPacified = false;
		AteBadPizza = false;
		TriedToCall = false;
		ReadEntireBoard = false;
		RodeBike = false;
		PeopleSpokenTo.Clear();
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( GaveOutCoffee && _waterObject.Transform.LocalPosition.z > -80 )
		{
			Log.Info(_waterObject.Transform.LocalPosition.z);
			_waterObject.Transform.LocalPosition += Vector3.Down * Time.Delta * 2.5f;
		}
	}

	public void GiveCoffee()
	{
		GaveOutCoffee = true;
	}

	public void Finish()
	{
		var screenObj = Scene.Children.Find( go => go.Name == "GameOverScreen" );
		Log.Info(screenObj);
		var gos = screenObj.Components.Get<GameOverScreen>( true );
		Log.Info(gos);
		(gos.Score, gos.Motives) = this.GetGameScore(); 
		gos.Enabled = true;
	}

	public (int score, List<string> motives) GetGameScore()
	{
		int score = 0;
		List<string> motives = new();
		if ( AteBadPizza )
		{
			motives.Add("Ate bad Pizza - -1000");
			score -= 1000;
		}
		motives.Add(GaveOutCoffee ? "Helped Homeless Moises - 100" : "Didn't help Homeless Moises - 0");
		score += GaveOutCoffee ? 100 : 0;
		motives.Add(KidPacified ? "Helped Scared Kid - 75" : "Didn't help Scared Kid - 0");
		score += KidPacified ? 75 : 0;
		motives.Add("Talked to " + PeopleSpokenTo.Count + " out of " + Scene.Children.Count(a => a.Components.Get<GenericNPCComponent>() != null) + " people - " + 50 * PeopleSpokenTo.Count);
		score += PeopleSpokenTo.Count * 50;
		motives.Add(TriedToCall ? "Tried to call on the phone - 25" : "Didn't try to call on the phone - 0");
		score += TriedToCall ? 25 : 0;
		motives.Add(ReadEntireBoard ? "Read the entire notice board - 65" : "Didn't read the entire notice board - 0");
		score += ReadEntireBoard ? 65 : 0;
		motives.Add(RodeBike ? "Tried to ride the bike - 20" : "Didn't try to ride the bike - 0");
		score += RodeBike ? 20 : 0;
		return (score, motives);
	}
}

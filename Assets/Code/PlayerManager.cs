using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerManager : MonoBehaviour
{
		string redPlayerName = "CPU";
		string bluePlayerName = "CPU";
		string greenPlayerName = "CPU";
		string yellowPlayerName = "CPU";
		int height = 2 * Screen.height / 3 + 45;
		int widthRed = Screen.width / 2 + 270;
		int distance = 220;
		public GUISkin skin;
		private List<Player> players = new List<Player> ();
		private List<int> list = new List<int> ();
		public static Player currentPlayer;
		public int currentPlayerIdx = 0;
		public GameController controller;
		public bool showCharSelect = true;
		public int redWins = 0;
		public int blueWins = 0;
		public int greenWins = 0;
		public int yellowWins = 0;
		private float nameBoxWidth = 120.0f;
		private float nameBoxHeight = 20.0f;
		private int blue = -1;
		private int red = -1;
		private int yellow = -1;
		private int green = -1;
		public bool isGameOver = false;
		private string winner = "";

		public static PlayerManager playerManager;
	
		void Awake ()
		{
				if (playerManager == null) {
						DontDestroyOnLoad (playerManager);
						playerManager = this;
				} else if (playerManager != this) {
						Destroy (gameObject);
				}
		}
	
		void Start ()
		{
				Screen.orientation = ScreenOrientation.Landscape;
				UnityEngine.Random.seed = Mathf.FloorToInt (Time.time);
				DontDestroyOnLoad (gameObject);
				for (int i=0; i<4; i++) {
						list.Add (i);
				}
				SetUpTurnOrder ();
				AddAllPlayersAsInactive ();
		}

		public void Reset ()
		{
				redPlayerName = "CPU";
				bluePlayerName = "CPU";
				greenPlayerName = "CPU";
				yellowPlayerName = "CPU";
				players = new List<Player> ();
				for (int i=0; i<4; i++) {
						list.Add (i);
				}
				SetUpTurnOrder ();
				AddAllPlayersAsInactive ();
		
		}
	
		public int GetWins (string colour)
		{

				if (string.Equals (colour, "red", StringComparison.OrdinalIgnoreCase))
						return redWins;
				if (string.Equals (colour, "blue", StringComparison.OrdinalIgnoreCase))
						return blueWins;
				if (string.Equals (colour, "green", StringComparison.OrdinalIgnoreCase))
						return greenWins;
				if (string.Equals (colour, "yellow", StringComparison.OrdinalIgnoreCase))
						return yellowWins;
				return 0;
		}

		public void IncrementWins (string colour)
		{
				if (string.Equals (colour, "red", StringComparison.OrdinalIgnoreCase)) {
						redWins++;
						if (redWins > 10) {
								redWins = 0;
						}
				}

				if (string.Equals (colour, "blue", StringComparison.OrdinalIgnoreCase)) {
						blueWins++;
						if (blueWins > 10) {
								blueWins = 0;
						}
				}
				if (string.Equals (colour, "green", StringComparison.OrdinalIgnoreCase)) {
						greenWins++;
						if (greenWins > 10) {
								greenWins = 0;
						}
				}
				if (string.Equals (colour, "yellow", StringComparison.OrdinalIgnoreCase)) {
						yellowWins++;
						if (yellowWins > 10) {
								yellowWins = 0;
						}
				}
		}

		/*private void CreatePlayerDetails ()
		{

				players.Add (new Player (bluePlayerName, Player.Colour.BLUE, blue));	
				players.Add (new Player (greenPlayerName, Player.Colour.GREEN, green));
				players.Add (new Player (yellowPlayerName, Player.Colour.YELLOW, yellow));	
				players.Add (new Player (redPlayerName, Player.Colour.RED, red));

		}*/

		public void AddActivePlayer (Player.Colour colour)
		{
				players.Add (new Player (colour, GetTurn (colour), false));

		}

		public void RemoveActivePlayer (Player.Colour colour)
		{
				players.Add (new Player (colour, GetTurn (colour), true));
		}

		int GetNextTurn (int prev)
		{
				int nextTurn = prev + 1;
				if (nextTurn == 4) {
						nextTurn = 0;
				}
				return nextTurn;
		}

		public void StartGame ()
		{
				showCharSelect = false;
				if (players.ToArray ().Length == 0) {
						AddAllPlayersAsInactive ();
						currentPlayerIdx = UnityEngine.Random.Range (0, 4);
				}
				players.Sort ();
				currentPlayer = players [currentPlayerIdx];
				SetUpPlayerPrefs ();
				Application.LoadLevel (currentPlayer.getPlayerColour ().ToString ());
				//UpdatePortraits ();
		}

		public void LoadNextLevel ()
		{
				SaveLevel ();
				currentPlayerIdx++;
				if (currentPlayerIdx == players.Count) {
						currentPlayerIdx = 0;
				}
				currentPlayer = players [currentPlayerIdx];	
//				Inventory inventory = GameObject.FindObjectOfType<Inventory> ();
//				if (inventory) {
//						inventory.Save ();
//				}
				Application.LoadLevel (currentPlayer.getPlayerColour ().ToString ());
				//UpdatePortraits ();
				print (currentPlayer.getPlayerColour ().ToString ());
		}

		/*	public Player GetPlayerByName (string name)
		{
				foreach (Player player in players) {
						bool isEqual = string.Equals (name, player.getPlayerName (), StringComparison.OrdinalIgnoreCase);
						if (isEqual) {
								return player;
						}
				}
				return null;
		} */

		public Player GetPlayerByColour (string colour)
		{
				foreach (Player player in players) {
						string playerColour = player.getPlayerColour ().ToString ();
						if (string.Equals (playerColour, colour, StringComparison.OrdinalIgnoreCase)) {
								return player;
						}
								
				}
				return null;
		}

		public void OnLevelWasLoaded (int i)
		{
				print (Application.loadedLevelName);

				if (Application.loadedLevelName != "Character Select" && !isGameOver) {
						LoadLevel ();
						//GameController.isCPU = PlayerManager.currentPlayer.isCPU;
						foreach (Tile tile in GameObject.FindObjectsOfType<Tile>()) {
								tile.flag = false;
						}
				}
				
		}
	
		public void WinScreen (string winner)
		{
//				foreach (PlayerController player in GameObject.FindObjectsOfType<PlayerController> ()) {
//						Destroy (player);
//				}
//
//				Destroy (GameObject.FindObjectOfType<GameController> ());
//				Destroy (GameObject.FindObjectOfType<Inventory> ());
				this.winner = winner;
				isGameOver = true;
				Application.LoadLevel (winner + "_win_screen");
		}

		public string GetCurrentLevelName ()
		{
				return Application.loadedLevelName;
		}

		void OnApplicationQuit ()
		{
				// Make sure prefs are saved before quitting.
				PlayerPrefs.Save ();
		}

		void SetUpPlayerPrefs ()
		{
				PlayerPrefs.SetString ("RED", GenerateRandomTiles ());
				PlayerPrefs.SetString ("YELLOW", GenerateRandomTiles ());
				PlayerPrefs.SetString ("BLUE", GenerateRandomTiles ());
				PlayerPrefs.SetString ("GREEN", GenerateRandomTiles ());
		}

		string GenerateRandomTiles ()
		{
				string randomTiles = "";
				for (int i=0; i<3; i++) {
						randomTiles += Tile.getRandomTypeString () + ":" + Tile.GetRandomRotation () + ":" + i.ToString ();
						randomTiles += "/";
				}
				return randomTiles;
		}

	
		void SaveLevel ()
		{
				string playerColour = currentPlayer.getPlayerColour ().ToString ();
				string playerHandString = "";
				foreach (DeckTile tile in GameObject.FindObjectsOfType<DeckTile>()) {
						//Tile tile = deckTile.GetComponent<Tile> ();
						if (tile != null) {
								playerHandString += tile.Serialize ();
								playerHandString += "/";
								Destroy (tile.gameObject);
						}
				}
				PlayerPrefs.SetString (playerColour, playerHandString);

		} 
	
		public void LoadLevel ()
		{
				GameController c = GameObject.FindObjectOfType<GameController> ();
				{
						if (c == null && !isGameOver) {
								c = Instantiate (controller) as GameController;
						}
				}
				string playerColor = currentPlayer.getPlayerColour ().ToString ();
				string[] tileStrings = PlayerPrefs.GetString (playerColor).Split ('/');
				if (tileStrings == null || tileStrings.Length < PlayerDeck.numberOfSlots) {
						tileStrings = new string[PlayerDeck.numberOfSlots];
				}
				GameObject deckObject = GameObject.FindGameObjectWithTag (playerColor + "_deck");
				PlayerDeck deck = null;
				if (deckObject != null) {
						deck = deckObject.GetComponent<PlayerDeck> ();
				}
				if (deck != null) {
						GameObject[] currentPlayerDeck = new GameObject[PlayerDeck.numberOfSlots];
						if (tileStrings != null) {
								for (int i=0; i<PlayerDeck.numberOfSlots; i++) {
										DeckTile dt = DeckTile.Deserialize (tileStrings [i]);
										currentPlayerDeck [dt.GetSlotIndex ()] = dt.gameObject;
								}
								deck.SetUpSpecificSlots (currentPlayerDeck);
				
						} else {
								deck.SetUpRandomSlots ();
						}
				}

		
		} 

		void UpdatePortraits ()
		{
				string playerColor = currentPlayer.getPlayerColour ().ToString ();
				GameObject[] portraitsOn = GameObject.FindGameObjectsWithTag ("Portrait on");
				GameObject[] portraitsOff = GameObject.FindGameObjectsWithTag ("Portrait off");
				foreach (GameObject portraitOn in portraitsOn) {
						portraitOn.GetComponent<SpriteRenderer> ().sortingLayerName = "off";
						if (GetFormattedName (portraitOn).Equals ("portrait_" + playerColor + "_on")) {
								portraitOn.GetComponent<SpriteRenderer> ().sortingLayerName = "on";
						}
				}
				foreach (GameObject portraitOff in portraitsOff) {
						portraitOff.GetComponent<SpriteRenderer> ().sortingLayerName = "on";
						if (GetFormattedName (portraitOff).Equals ("portrait_" + playerColor + "_off")) {
								portraitOff.GetComponent<SpriteRenderer> ().sortingLayerName = "off";
						}
				}

		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}

		public static string GetPlayerString ()
		{
				return currentPlayer.getPlayerColour ().ToString ();
		}

		private void SetUpTurnOrder ()
		{
				blue = UnityEngine.Random.Range (0, 4);
				red = GetNextTurn (blue);
				yellow = GetNextTurn (red);
				green = GetNextTurn (green);
		}

		private int GetTurn (Player.Colour colour)
		{
				switch (colour) {
				case Player.Colour.BLUE:
						return blue;
				case Player.Colour.RED:
						return red;
				case Player.Colour.YELLOW:
						return yellow;
				case Player.Colour.GREEN:
						return green;
				}
				return -1;
		}

		void AddAllPlayersAsInactive ()
		{
				foreach (Player.Colour colour in Player.GetColours()) {
						RemoveActivePlayer (colour);
				}
		}
}

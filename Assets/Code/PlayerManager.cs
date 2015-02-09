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
				UnityEngine.Random.seed = Mathf.FloorToInt (Time.time);
				DontDestroyOnLoad (gameObject);
				for (int i=0; i<4; i++) {
						list.Add (i);
				}
		}

		void OnGUI ()
		{				
				if (showCharSelect) {
						GUI.skin = skin;
						redPlayerName = GUI.TextField (new Rect (widthRed, height, nameBoxWidth, nameBoxHeight), redPlayerName, 10);
						bluePlayerName = GUI.TextField (new Rect (widthRed - 3 * distance + 40, height, nameBoxWidth, nameBoxHeight), bluePlayerName, 10);
						greenPlayerName = GUI.TextField (new Rect (widthRed - 2 * distance + 15, height, nameBoxWidth, nameBoxHeight), greenPlayerName, 10);
						yellowPlayerName = GUI.TextField (new Rect (widthRed - distance, height, nameBoxWidth, nameBoxHeight), yellowPlayerName, 10);
						if (GUI.Button (new Rect (Screen.width / 2 - 50, height + 80, 100, 40), "PLAY")) {
								CreatePlayerDetails ();
								StartGame ();
						}
			
				}
		
		}

		public void Reset ()
		{
				redPlayerName = "CPU";
				bluePlayerName = "CPU";
				greenPlayerName = "CPU";
				yellowPlayerName = "CPU";
				players = new List<Player> ();

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

		private void CreatePlayerDetails ()
		{
				int blue = UnityEngine.Random.Range (0, 4);
				int green = GetNextTurn (blue);
				int yellow = GetNextTurn (green);
				int red = GetNextTurn (yellow);
				players.Add (new Player (bluePlayerName, Player.Colour.BLUE, blue));	
				players.Add (new Player (greenPlayerName, Player.Colour.GREEN, green));
				players.Add (new Player (yellowPlayerName, Player.Colour.YELLOW, yellow));	
				players.Add (new Player (redPlayerName, Player.Colour.RED, red));

		}

		int GetNextTurn (int prev)
		{
				int nextTurn = prev + 1;
				if (nextTurn == 4) {
						nextTurn = 0;
				}
				return nextTurn;
		}
		private void StartGame ()
		{
				showCharSelect = false;
				players.Sort ();
				currentPlayer = players [currentPlayerIdx];
				SetUpPlayerPrefs ();
				Application.LoadLevel (currentPlayer.getPlayerColour ().ToString ());
				GameController c = Instantiate (controller) as GameController;
				UpdatePortraits ();
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
				UpdatePortraits ();
				print (currentPlayer.getPlayerColour ().ToString ());
		}

		public Player GetPlayerByName (string name)
		{
				foreach (Player player in players) {
						bool isEqual = string.Equals (name, player.getPlayerName (), StringComparison.OrdinalIgnoreCase);
						if (isEqual) {
								return player;
						}
				}
				return null;
		}

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
				if (Application.loadedLevelName != "Character Select") {
						LoadLevel ();
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
				string playerColor = currentPlayer.getPlayerColour ().ToString ();
				string[] tileStrings = PlayerPrefs.GetString (playerColor).Split ('/');
				if (tileStrings == null || tileStrings.Length < PlayerDeck.numberOfSlots) {
						tileStrings = new string[PlayerDeck.numberOfSlots];
				}
				PlayerDeck deck = GameObject.FindGameObjectWithTag (playerColor + "_deck").GetComponent<PlayerDeck> ();
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
}

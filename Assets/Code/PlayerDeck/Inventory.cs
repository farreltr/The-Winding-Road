using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

public class Inventory : MonoBehaviour
{
		public Tile[] inventory;
		public Tile[] slots;
		//private TileDB database;
		public Tile[] database;
		public int slotsX, slotsY;
		private bool showInventory;
		public GUISkin skin;	
		private static string EMPTY_STRING = "";	
		public bool showToolTip;
		public string tooltip;
		public bool draggingTile;
		public Tile draggedTile;
		public int prevIdx;
		public Event e;
		private float tileWidth = 40.0f;
		private float tileHeight = 40.0f;
		private bool disabled = false;
		private bool displayLabel = false;
		public bool isRestart;
		private bool hasBeenLoaded = false;
		public bool isCPU;
		public bool isCPUStarted;

		// Use this for initialization
		void Start ()
		{
				slots = new Tile[slotsX * slotsY];
				inventory = new Tile[slotsX * slotsY];
				database = Resources.LoadAll<Tile> ("Tiles/Prefabs/");
				for (int i=0; i<inventory.Length; i++) {
						Tile saveTile = CheckPlayerPrefs (i);
						if (saveTile == null) {
								int tileId = UnityEngine.Random.Range (0, database.Length - 1);
								Tile DBTile = database [tileId];
								inventory [i] = CreateTile (DBTile.name);			
						} else {
								inventory [i] = saveTile;
						}
				}
				PlayerManager pm = GameObject.FindObjectOfType<PlayerManager> ();
				
				if (pm != null && pm.currentPlayer.isCPU) {
						this.gameObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
						isCPU = pm.currentPlayer.isCPU;
						int index = UnityEngine.Random.Range (0, 3);
						draggingTile = true;
						draggedTile = inventory [index];
						inventory [index] = null;
						prevIdx = index;
						GameController gc = GameObject.FindObjectOfType<GameController> ();
						Wait ();
						gc.processCPUPlayer ();
				}
		}

		IEnumerator Wait ()
		{
				yield return new WaitForSeconds (10);	
		}

		bool IsEmpty ()
		{
				foreach (Tile tile in inventory) {
						if (tile != null && !tile.isEmpty ()) {
								return false;
						}
				}	
				return true;
		}

		void OnGUI ()
		{

				GUI.skin = skin;

				if (!disabled) {
						DrawInventory ();
				} else {	
						isRestart = GUI.Button (new Rect (Screen.width / 2, Screen.height / 2, 128, 128), EMPTY_STRING);
				}
		}

		public void DrawInventory ()
		{
				tooltip = EMPTY_STRING;
				GUI.skin = skin;
				//Rect boxRect = new Rect ((4 * Screen.width / 5) - 10, (1 * Screen.height / 6) - 10, tileWidth * 2f, tileHeight * 4.5f);
				//GUI.Box (boxRect, EMPTY_STRING);
		
				e = Event.current;
				int i = 0;
				for (int y=0; y<slotsY; y++) {
						for (int x=0; x<slotsX; x++) {
								Rect slotRect = new Rect (x * tileWidth * 1.1f + (4 * Screen.width / 5), y * (tileHeight + 10) * 1.1f + (1 * Screen.height / 6), tileWidth * 1.1f, tileHeight * 1.1f);
								Rect tileRect = new Rect (slotRect.x + (tileWidth * 0.05f), slotRect.y + (tileHeight * 0.05f), tileWidth, tileHeight);
								Rect rotRightRect = new Rect (slotRect.x - (0.4f * tileWidth), slotRect.y + (0.6f * tileHeight), slotRect.width * 0.4f, slotRect.height * 0.4f);
								Rect rotLeftRect = new Rect (slotRect.x + (1.1f * tileWidth), slotRect.y + (0.6f * tileHeight), slotRect.width * 0.4f, slotRect.height * 0.4f);
								GUI.Box (slotRect, EMPTY_STRING, skin.GetStyle ("Slot"));	
								Tile tile = slots [i];
								tile = inventory [i];

								if (tile != null && !tile.isEmpty ()) {
										if (GUI.Button (rotRightRect, EMPTY_STRING, skin.GetStyle ("Rotate Right"))) {
												tile.RotateRight ();
										}
										if (GUI.Button (rotLeftRect, EMPTY_STRING, skin.GetStyle ("Rotate Left"))) {
												tile.RotateLeft ();
										}
										GUI.DrawTexture (tileRect, tile.GetIcon ());
										if (slotRect.Contains (e.mousePosition)) {
												if (!isCPU && e.button == 0 && e.type == EventType.mouseDrag && !draggingTile) {
														draggingTile = true;
														draggedTile = tile;
														inventory [i] = null;
														prevIdx = i;
												}
												if (!isCPU && e.type == EventType.mouseUp && draggingTile) {
														inventory [prevIdx] = inventory [i];
														inventory [i] = draggedTile;
														draggingTile = true;
												}
						
												if (!draggingTile) {
														CreateToolTip (tile);
														showToolTip = true;
												}
						
												if (!isCPU && Input.GetMouseButtonDown (0)) {
														//highlight box
												}
										}
								}
				
								if (slotRect.Contains (e.mousePosition)) {
										if (!isCPU && e.type == EventType.mouseUp && draggingTile) {
												inventory [i] = draggedTile;
												draggingTile = false;
												draggedTile = null;
						
										}
					
								}
				
				
								if (tooltip == EMPTY_STRING) {
										showToolTip = false;
								}
								i++;
						}
			
				}
				if (draggingTile && !isCPU) {
						GUI.DrawTexture (new Rect (Event.current.mousePosition.x - tileWidth / 2, Event.current.mousePosition.y - tileHeight / 2, tileWidth, tileHeight), draggedTile.GetIcon ());
				}
		}
	
		void CreateToolTip (Tile tile)
		{
				tooltip = "<color=#4DA4BF>" + tile.name + "</color>\n\n";
		}
	
		public static Tile CreateTile (string name)
		{
				GameObject newTile = new GameObject ();
				Tile tile = newTile.AddComponent<Tile> ();
				tile.SetUpTile (Tile.getTileType (name));
				return tile;
		}

		public void SetDisabled ()
		{
				disabled = true;

		}

		public void Save ()
		{		
				int i = 0;
				string colour = GetColour ();
				foreach (Tile tile in inventory) {
						if (tile != null) {
								PlayerPrefs.SetString (colour + "_type_" + i, GetFormattedName (tile.gameObject));
								PlayerPrefs.SetInt (colour + "_index_" + i, i);
								PlayerPrefs.SetFloat (colour + "_rotation_" + i, tile.gameObject.transform.eulerAngles.z);
						}
						i++;

				}
		}

		public Tile CheckPlayerPrefs (int i)
		{
				string colour = GetColour ();
				Tile tile = null;
				string type = PlayerPrefs.GetString (colour + "_type_" + i);
				int index = PlayerPrefs.GetInt (colour + "_index_" + i);
				float z = PlayerPrefs.GetFloat (colour + "_rotation_" + i);
				if (type != "") {
						tile = CreateTile (type);
						Vector3 rotation = new Vector3 (0.0f, 0.0f, z);
						tile.gameObject.transform.rotation = Quaternion.Euler (rotation);
				}
				return tile;
						
		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}

		private string GetColour ()
		{
				string sceneName = Application.loadedLevelName;
				return sceneName.Replace ("_win_screen", "");
		
		}
	
}
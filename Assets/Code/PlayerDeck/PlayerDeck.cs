using UnityEngine;
using System.Collections;


public class PlayerDeck : MonoBehaviour
{
		public static DeckTile selectedTile;
		private DeckTile[] slots = new DeckTile[3];
		public static int numberOfSlots = 3;
		public bool disabled = false;
		public static Vector2[] slotPosition = {
				new Vector2 (6.85f, 4.05f),
				new Vector2 (6.85f, 1.8f),
				new Vector2 (6.85f, -0.45f)
		};
		public Transform rotateButton;
		public Transform slot;

		void Start ()
		{
				System.DateTime now = System.DateTime.Now;
				Random.seed = Mathf.FloorToInt (now.Millisecond * now.Minute * now.Day);
				selectedTile = null;
				slots = new DeckTile[numberOfSlots];
				disabled = false;
		}

		public void SetSelected (DeckTile select)
		{
				selectedTile = select;
		}

		public DeckTile getSelected ()
		{
				return selectedTile;
		}

		public bool isSelected ()
		{
				return selectedTile != null;
		}

		public void SetUpSpecificSlots (GameObject[] tiles)
		{
				for (int i = 0; i < numberOfSlots; i++) {
						if (tiles [i] != null) {
								PutTileInSlot (tiles [i], i);
						}
							
				}
				AddSlots ();
				AddRotateButtons ();
		}


		public void SetUpRandomSlots ()
		{
				GameObject[] tiles = Resources.LoadAll<GameObject> ("Tiles/");
				slots = new DeckTile[numberOfSlots];
				for (int i = 0; i < numberOfSlots; i++) {
						GameObject randomTile = tiles [Random.Range (0, tiles.Length)];
						randomTile.transform.rotation = GetUpRandomRotation ();
						GameObject slot = Instantiate (randomTile) as GameObject;
						PutTileInSlot (slot, i);	
				}
				AddSlots ();
				AddRotateButtons ();

		}

		public static Quaternion GetUpRandomRotation ()
		{
				Vector3 randomRotation = Vector3.zero;
				float randomZRotation = 90f * Random.Range (0, 4);
				randomRotation.z = randomZRotation;
				return Quaternion.Euler (randomRotation);
		}

		public void PutTileInSlot (GameObject slot, int i)
		{
				slot.transform.position = slotPosition [i];
				slot.transform.GetComponent<SpriteRenderer> ().sortingLayerName = "Deck Tile";
				slot.layer = 11;
				DeckTile deckTile = slot.GetComponent<DeckTile> ();
				if (deckTile == null) {
						deckTile = slot.AddComponent<DeckTile> ();
				}
				deckTile.SetSlotPosition (slotPosition [i]);
				deckTile.SetSlotIndex (i);
				deckTile.GetComponent<Tile> ().slotIndex = i;
				slots [i] = deckTile;
				slots [i].transform.parent = this.gameObject.transform;
		}

		void AddSlots ()
		{
				for (int i=0; i< numberOfSlots; i++) {
						//Vector3 slotPosition = new Vector3 (slotPosition [i].x, slotPosition [i].y, 0.0f);
						Transform s = Instantiate (slot, slotPosition [i], slot.rotation) as Transform;
						s.transform.parent = this.transform;
				}
		
		}

		void AddRotateButtons ()
		{
				for (int i=0; i< numberOfSlots; i++) {
						Vector3 rotatePosition = new Vector3 (slotPosition [i].x + TileMap.tileSize / 2, slotPosition [i].y - TileMap.tileSize / 2, -1f);
						Transform rotate = Instantiate (rotateButton, rotatePosition, rotateButton.rotation) as Transform;
						rotate.GetComponent<Rotate> ().index = i;
						rotate.transform.parent = this.transform;
				}

		}

		public Tile GetTileFromIndex (int index)
		{
				foreach (DeckTile dt in GameObject.FindObjectsOfType<DeckTile>()) {
						if (dt != null && dt.GetSlotIndex () == index) {
								return dt.GetComponent<Tile> ();
						}
				}
				return null;
		}

		public Tile[] GetTiles ()
		{
				Tile[] tiles = new Tile[numberOfSlots];
				int i = 0;
				foreach (DeckTile deckTile in slots) {
						if (deckTile != null) {
								tiles [i] = deckTile.GetComponent<Tile> ();
						}
						
						i++;
				}
				return tiles;
		}

		void Update ()
		{
				foreach (DeckTile dt in slots) {
						if (dt != null) {
								dt.disabled = disabled;
						}
				}
		}

		public DeckTile GetRandomDeckTile ()
		{
				return slots [Random.Range (0, 3)];
		}
}

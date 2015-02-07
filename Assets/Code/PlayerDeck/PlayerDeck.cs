using UnityEngine;
using System.Collections;


public class PlayerDeck : MonoBehaviour
{
		public static DeckTile selectedTile;
		private DeckTile[] slots;
		private int numberOfSlots = 3;
		private Vector2[] slotPosition = {new Vector2 (6.85f, 4.05f), new Vector2 (6.85f, 1.8f), new Vector2 (6.85f, -0.45f)};
		public Transform rotateButton;

		void Start ()
		{
				System.DateTime now = System.DateTime.Now;
				Random.seed = Mathf.FloorToInt (now.Millisecond * now.Minute * now.Day);
				selectedTile = null;
				SetUpSlots ();
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

		private void SetUpSlots ()
		{
				GameObject[] tiles = Resources.LoadAll<GameObject> ("Tiles/");
				slots = new DeckTile[numberOfSlots];
				for (int i = 0; i < numberOfSlots; i++) {
						GameObject randomTile = tiles [Random.Range (0, tiles.Length)];
						randomTile.transform.rotation = SetUpRandomRotation ();
						GameObject slot = Instantiate (randomTile) as GameObject;
						PutTileInSlot (slot, i);	
				}
				AddRotateButtons ();

		}

		private Quaternion SetUpRandomRotation ()
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
				slots [i] = deckTile;
				deckTile.SetSlotPosition (slotPosition [i]);
				deckTile.SetSlotIndex (i);
				deckTile.GetComponent<Tile> ().slotIndex = i;
				slot.transform.parent = this.transform;
		}

		void AddRotateButtons ()
		{
				for (int i=0; i< numberOfSlots; i++) {
						Vector3 rotatePosition = new Vector2 (slotPosition [i].x + TileMap.tileSize / 2, slotPosition [i].y - TileMap.tileSize / 2);
						Transform rotate = Instantiate (rotateButton, rotatePosition, rotateButton.rotation) as Transform;
						rotate.GetComponent<Rotate> ().index = i;
						rotate.transform.parent = this.transform;
				}

		}

		public Tile GetTileFromIndex (int index)
		{
				return slots [index].GetComponent<Tile> ();
		}
}

using UnityEngine;
using System.Collections;


public class PlayerDeck : MonoBehaviour
{

		public static DeckTile selectedTile;
		private DeckTile[] slots;
		private int numberOfSlots = 3;
		private Vector2[] position = {new Vector2 (6.85f, 4.05f), new Vector2 (6.85f, 1.8f), new Vector2 (6.85f, -0.45f)};

		void Start ()
		{
				System.DateTime now = System.DateTime.Now;
				Random.seed = Mathf.FloorToInt (now.Millisecond * now.Minute * now.Day);
				selectedTile = null;
				SetUpSlots ();
		}

		public void SetSelected (DeckTile select)
		{
				{
						if (selectedTile != null) {
								selectedTile.SetNotSelectedColour ();	
								selectedTile.SetSelectedColour ();
						}
						selectedTile = select;

				}
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
						//slot.transform.localScale = new Vector3 (1f, 1f, 1f);
						slot.transform.position = position [i];
						slot.transform.GetComponent<SpriteRenderer> ().sortingLayerName = "Deck Tile";
						slot.layer = 11;
						DeckTile deckTile = slot.GetComponent<DeckTile> ();
						if (deckTile == null) {
								deckTile = slot.AddComponent<DeckTile> ();
						}
						slots [i] = deckTile;
						slot.transform.parent = this.transform;
				}

		}

		private Quaternion SetUpRandomRotation ()
		{
				Vector3 randomRotation = Vector3.zero;
				float randomZRotation = 90f * Random.Range (0, 4);
				randomRotation.z = randomZRotation;
				return Quaternion.Euler (randomRotation);

		}
}

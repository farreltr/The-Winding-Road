using UnityEngine;
using System.Collections;

public class Arrows : MonoBehaviour
{
		private GameObject sword;
		public static Arrows arrows;
		private float tileSize = 1f;
	
		void Awake ()
		{
				if (arrows == null) {
						DontDestroyOnLoad (arrows);
						arrows = this;
				} else if (arrows != this) {
						Destroy (gameObject);
				}
		}

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				this.tileSize = TileMap.tileSize;
				sword = Resources.Load<GameObject> ("Arrows/arrow");
				SpriteRenderer renderer = sword.GetComponent<SpriteRenderer> ();
				renderer.sortingLayerName = "Arrow";
				renderer.sortingOrder = 5;

				RightArrows ();
				LeftArrows ();
				UpArrows ();
				DownArrows ();

		}
	
		void RightArrows ()
		{
				for (int y = 1; y < TileMap.size_y - 1; y++) {
						SetUpArrow (0, y, Direction.RIGHT, Quaternion.Euler (0, 0, 90));
				}
		}

		void LeftArrows ()
		{
				for (int y = 1; y < TileMap.size_y - 1; y++) {
						SetUpArrow (TileMap.size_x - 1, y, Direction.LEFT, Quaternion.Euler (0, 0, 270));
				}
		}

		void UpArrows ()
		{
				for (int x = 1; x <  TileMap.size_x - 1; x++) {

						SetUpArrow (x, 0, Direction.UP, Quaternion.Euler (0, 0, 180));
				}
		}

		void DownArrows ()
		{
				for (int x = 1; x <  TileMap.size_x - 1; x++) {
						SetUpArrow (x, TileMap.size_y - 1, Direction.DOWN, Quaternion.Euler (0, 0, 0));
				}
		}

		void SetUpArrow (int xCoord, int yCoord, Direction direction, Quaternion rotation)
		{
				Vector3 position = new Vector3 ((tileSize * xCoord) - 4.4f, (tileSize * yCoord) - 4.4f, 0);
				GameObject go = (GameObject)Instantiate (sword, position, rotation);
				BoxCollider2D collider = go.AddComponent<BoxCollider2D> ();
				collider.size = new Vector2 (1f, 1.25f);
				collider.center = new Vector2 (0f, 0.15f);
				collider.isTrigger = true;
				Arrow arrow = go.GetComponent<Arrow> ();
				arrow.transform.tag = "Arrow";
				arrow.gameObject.layer = this.gameObject.layer;
				arrow.name = arrow.transform.tag;
				arrow.transform.parent = this.gameObject.transform;
				arrow.SetDirection (direction);
				arrow.SetXcoord (xCoord);
				arrow.SetYcoord (yCoord);

		}

}
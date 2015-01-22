using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
		public static int boardSizeX = 7;
		public static int boardSizeY = 7;
		private GameObject[] tilePrefabs;
		private static string BOARD_TILE = "Board";


		void Start ()
		{
				tilePrefabs = Resources.LoadAll<GameObject> ("Tiles/");
				DontDestroyOnLoad (gameObject);
				System.DateTime now = System.DateTime.Now;
				Random.seed = Mathf.FloorToInt (now.Millisecond * now.Minute * now.Day);
				BuildBoard ();
				//transform.localScale = Vector3.one * 1.07f;
		}

		void BuildBoard ()
		{
				int i = 0;
				for (int x=0; x < boardSizeX; x++) {
						for (int y=0; y < boardSizeY; y++) {
								GameObject tilePrefab = tilePrefabs [Random.Range (0, tilePrefabs.Length)];	
								Vector3 position = new Vector3 ((TileMap.tileSize * x) - 3.3f, (TileMap.tileSize * y) - 3.3f, 0.0f);
								Quaternion rotation = Quaternion.Euler (0, 0, 90 * (Random.Range (0, 4)));
								GameObject go = Instantiate (tilePrefab, position, rotation) as GameObject;					
								go.transform.parent = this.transform;
								go.transform.GetComponent<SpriteRenderer> ().sortingLayerName = BOARD_TILE;
								Tile tile = retrieveTile (go);
								tile.coordinate = new Vector2 (x, y);
								tile.tag = BOARD_TILE;
								i++;
						}
				}  


		}  
		private Tile retrieveTile (GameObject go)
		{
				Tile tile;
				if (go.GetComponent<Tile> () == null) {
						tile = go.AddComponent<Tile> ();
			
				} else {
						tile = go.GetComponent<Tile> ();
				}		
				return tile;
		}

}

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
		private PlayerDeck deck;
		private bool restart;
		private bool gameOver;
		public int numberOfPLayers = 1;
		public int currentCoordinateX;
		public int currentCoordinateY;
		public static int boardSizeX = TileMap.size_x - 2;
		public static int boardSizeY = TileMap.size_y - 2;
		public GameObject[] tilePrefabs;
		private Vector3 mousePosition = Vector3.zero;
		public bool isTileMoving = false;
		public bool isCPU = false;
		public string gameOverText = "";
		private DeckTile selectedTile;
		private Arrow endPosition;
		private Direction direction = Direction.NONE;
		private Tile[] currentPlayerDeck = new Tile[3];
		private bool isDragging = false;
		private DeckTile draggingTile;
		private Tile endTile;
		private int draggingTileIndex;

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				gameOver = false;
				restart = false;
				deck = GameObject.FindObjectOfType<PlayerDeck> ();

		}
	
		void Update ()
		{

				if (isDragging && draggingTile.IsHit ()) {
						draggingTile.transform.parent = GameObject.FindObjectOfType<Board> ().transform;
						draggingTile.gameObject.GetComponent<Tile> ().coordinate = new Vector2 (currentCoordinateX - 1, currentCoordinateY - 1);
						bool done = HandleTile ();	
						if (done) {
								isDragging = false;
								DeckTile draggable = draggingTile.GetComponent<DeckTile> ();
								draggingTileIndex = draggable.GetSlotIndex ();
								if (draggable != null) {
										draggable.enabled = false;
										Tile tile = draggable.gameObject.GetComponent<Tile> ();
										if (tile != null) {
												tile.UpdateTileRefs ();
										}

										Destroy (draggable);
								}
								print ("done");
								StartCoroutine ("Wait", 3);
						}

				} 

//				if (isDragging && endPosition != null) {
//						PlayerDeck.selectedTile.transform.position = endPosition.transform.position;
//						direction = endPosition.GetDirection ();
//						currentCoordinateX = endPosition.GetXcoord ();
//						currentCoordinateY = endPosition.GetYcoord ();
//						selectedTile = draggingTile;
//						selectedTile.GetComponent<DeckTile> ().enabled = false;
//						selectedTile.transform.parent = GameObject.FindObjectOfType<Board> ().transform;
//						selectedTile.SetNotSelectedColour ();
//						bool done = HandleTile ();	
//						selectedTile = null;
//						endPosition = null;
//						if (done) {
//								print ("done");
//								//SaveLevel ();
//								//PlayerManager.playerManager.LoadNextLevel ();	
//								//LoadLevel ();
//						}
//
//				}
//				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//				RaycastHit hitInfo;
//				playerDeck = GameObject.FindObjectOfType<PlayerDeck> ();
//				bool isHit = TileMap.tileMap.collider.Raycast (ray, out hitInfo, Mathf.Infinity);
//
//				if (draggingTile.IsReleased ()) {
//						mousePosition = hitInfo.point;
//						currentCoordinateX = Mathf.FloorToInt (hitInfo.point.x / TileMap.tileSize);
//						currentCoordinateY = Mathf.FloorToInt (hitInfo.point.y / TileMap.tileSize);
//
//				}
//		
//				if (TileMap.tileMap != null && isHit && isDragging) {
//						mousePosition = hitInfo.point;
//						currentCoordinateX = Mathf.FloorToInt (hitInfo.point.x / TileMap.tileSize);
//						currentCoordinateY = Mathf.FloorToInt (hitInfo.point.y / TileMap.tileSize);
//
//						if (draggingTile.IsReleased ()) {
//								GameObject o = draggingTile.gameObject;
//								o.transform.parent = GameObject.FindObjectOfType<Board> ().transform;
//								o.transform.position = new Vector3 (currentCoordinateX, currentCoordinateY, 0.0f);
//								o.GetComponent<DeckTile> ().enabled = false;
//								bool done = HandleTile ();		
//								if (done) {
//										print ("done");
//										//	PlayerManager.playerManager.LoadNextLevel ();	
//								}
//																			
//										
//						}
//				}
		}

		public void SetDestination (Arrow endPosition)
		{
				this.endPosition = endPosition;
		}

		private bool HandleTile ()
		{
				BlockUserInput ();
				ShiftBoardTiles ();
				ShiftPlayers ();
				EndTileFall ();
				TriggerMoveEvent ();
				return true;
		}

		void BlockUserInput ()
		{
				PlayerDeck pd = GameObject.FindObjectOfType<PlayerDeck> ();
				pd.disabled = true;

		}

		void TriggerMoveEvent ()
		{
				PlayerController[] knights = GameObject.FindObjectsOfType<PlayerController> ();
				foreach (PlayerController knight in knights) {
						//knight.isTriggerAction = true;
				}
		}

		private void EndTileFall ()
		{
				endTile.slotIndex = draggingTile.GetSlotIndex ();
				endTile.slotPosition = PlayerDeck.slotPosition [draggingTile.GetSlotIndex ()];
				endTile.setToDestroy = true;
				endTile = null;
		}


		public void processCPUPlayer ()
		{
				switch (Random.Range (0, 4)) {
				case 0:
						{
								currentCoordinateX = 0;
								currentCoordinateY = Random.Range (0, 9);
								break;
						}

				case 1:
						{
								currentCoordinateX = Random.Range (0, 9);
								currentCoordinateY = 0;
								break;
						}
				case 2:
						{
								currentCoordinateX = TileMap.size_x - 1;
								currentCoordinateY = Random.Range (0, 9);
								break;
						}
				case 3:
						{
								currentCoordinateX = Random.Range (0, 9);
								currentCoordinateY = TileMap.size_y - 1;
								break;
						}
				
				}
				StartCoroutine ("Wait", 3);


		}
	
		IEnumerator Wait (int secs)
		{
				yield return new WaitForSeconds (secs);
				//HandleTile ();	
				//isCPU = false;
				//inventory.isCPU = false;
				//inventory.isCPUStarted = false;
				//PlayerManager.playerManager.LoadNextLevel ();	
				PlayerManager.playerManager.LoadNextLevel ();	
		}

		void ShiftPlayers ()
		{
				PlayerController[] players = GameObject.FindObjectsOfType<PlayerController> ();
				foreach (PlayerController player in players) {
						switch (direction) {
						case Direction.RIGHT:
								player.ShiftRight (currentCoordinateX, currentCoordinateY);
								break;
						case Direction.LEFT:
								player.ShiftLeft (currentCoordinateX, currentCoordinateY);
								break;
						case Direction.DOWN:
								player.ShiftDown (currentCoordinateX, currentCoordinateY);
								break;
						case Direction.UP:
								player.ShiftUp (currentCoordinateX, currentCoordinateY);
								break;
						}

				}
		}

		private Vector2 GetNewCoordinate ()
		{
				switch (direction) {
				case Direction.RIGHT:
						return new Vector2 (currentCoordinateX, currentCoordinateY - 1);
				case Direction.LEFT:
						return new Vector2 (currentCoordinateX - 2, currentCoordinateY - 1);
				case Direction.DOWN:
						return new Vector2 (currentCoordinateX - 1, currentCoordinateY - 2);
				case Direction.UP:
						return new Vector2 (currentCoordinateX - 1, currentCoordinateY);
				default:
						return Vector2.zero;
				}


		}

		private Vector3 GetPositionForNewTile ()
		{
				Vector3 position = Vector3.zero;
				switch (direction) {
				case Direction.RIGHT:
						position.x = currentCoordinateX + 1.5f;
						position.y = currentCoordinateY + 0.5f;
						break;
				case Direction.LEFT: 
						position.x = currentCoordinateX - 0.5f;
						position.y = currentCoordinateY + 0.5f;
						break;
				case Direction.DOWN:
						position.x = currentCoordinateX + 0.5f;
						position.y = currentCoordinateY - 0.5f;
						break;
				case Direction.UP:
						position.x = currentCoordinateX + 0.5f;
						position.y = currentCoordinateY + 1.5f;
						break;
				default:
						break;
				}
				position *= TileMap.tileSize;
				position.z = 0.5f;
				return position;

		}

		private Tile GetTileFromPosition ()
		{
				switch (direction) {
				case Direction.RIGHT:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX + boardSizeX - 1, currentCoordinateY - 1));		
				case Direction.LEFT: 
						return GameObject.FindGameObjectWithTag (string.Concat (currentCoordinateX - boardSizeX - 1, currentCoordinateY - 1)).GetComponent<Tile> ();
				case Direction.DOWN:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, currentCoordinateY + boardSizeY - 1));	
				case Direction.UP:
						return GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, currentCoordinateY - boardSizeY - 1));		
				default:
						return new Tile ();
				}
		}
	
		private void ShiftBoardTiles ()
		{
				switch (direction) {
				case Direction.RIGHT:
						bool isFirstRight = true;
						for (int x=boardSizeX - 1; x>-1; x--) {
								Tile tile = GetTileAtCoordinate (new Vector2 (x, currentCoordinateY - 1));
								if (tile != null) {
										tile.shiftRight ();
										if (isFirstRight) {
												endTile = tile;
												isFirstRight = false;
										}
								}
								
						}
						draggingTile.GetComponent<Tile> ().shiftRight ();
						break;
				case Direction.LEFT: 
						bool isFirstLeft = true;
						for (int x=0; x<boardSizeX; x++) {
								Tile tile = GetTileAtCoordinate (new Vector2 (x, currentCoordinateY - 1));
								if (tile != null) {
										tile.shiftLeft ();
										if (isFirstLeft) {
												endTile = tile;
												isFirstLeft = false;
										}
								}
						}
						draggingTile.GetComponent<Tile> ().shiftLeft ();
						break;
				case Direction.DOWN:
						bool isFirstDown = true;
						for (int y=0; y<boardSizeY; y++) {
								Tile tile = GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, y));
								if (tile != null) {
										tile.shiftDown ();
										if (isFirstDown) {
												endTile = tile;
												isFirstDown = false;
										}
								}			
						}
						draggingTile.GetComponent<Tile> ().shiftDown ();
						break;								
				case Direction.UP:
						bool isFirstUp = true;
						for (int y=boardSizeY - 1; y>-1; y--) {	
								Tile tile = GetTileAtCoordinate (new Vector2 (currentCoordinateX - 1, y));
								if (tile != null) {
										tile.shiftUp ();	
										if (isFirstUp) {
												endTile = tile;
												isFirstUp = false;
										}
								}							
						}
						draggingTile.GetComponent<Tile> ().shiftUp ();
						break;
				}
		}

		public static Tile GetTileAtCoordinate (Vector2 coordinate)
		{
				GameObject[] boardTiles = GameObject.FindGameObjectsWithTag ("Board");
				foreach (GameObject boardTile in boardTiles) {
						Tile tile = boardTile.GetComponent<Tile> ();
						if (tile != null && tile.isAt (coordinate)) {
								return tile;
						}
				}
				return null;

		}
	


		public bool IsTileMoving ()
		{
				foreach (Tile tile in GameObject.FindObjectsOfType<Tile>()) {
						if (tile.flag) {
								return true;
						}				
				}
				return false;
		}
	
		string GetMoveDirection (TilePosition tilePosition)
		{
				switch (tilePosition) {
				case TilePosition.LEFT:
						return "right";		
				case TilePosition.RIGHT: 
						return "left";
				case TilePosition.TOP:
						return "down";	
				case TilePosition.BOTTOM:
						return "up";
				default:
						throw new System.NotImplementedException ();
				}
				
		}

		private static string GetFormattedName (GameObject o)
		{
				return o.name.Replace ("(Clone)", "");
		}


		public void GameOver ()
		{
				gameOver = true;
				foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
						PlayerController controller = player.GetComponent<PlayerController> ();
//						Animator animator = player.GetComponent<Animator> ();	
						if (controller != null) {
//								controller.Stop ();
								if (controller.isWinner) {
										PlayerManager.playerManager.WinScreen (controller.GetName ());
//										Instantiate (Resources.Load<GUITexture> ("End Screens/" + controller.GetName ()));
										//GameObject.FindObjectOfType<Inventory> ().SetDisabled ();	
										PlayerManager pm = GameController.FindObjectOfType<PlayerManager> ();
										Player p = pm.GetPlayerByColour (controller.name);
										//gameOverText = p.getPlayerName () + " Wins!";										
								}
						}

//						if (animator != null) {
//								animator.enabled = false;
//						}
						restart = true;
				}

		}  

		public void OnLevelWasLoaded (int i)
		{
				isCPU = PlayerManager.currentPlayer.isCPU;
		}


		public enum TilePosition
		{
				TOP,
				BOTTOM,
				LEFT,
				RIGHT,
				NONE
		}

		public void SetDragging (bool isDragging)
		{
				this.isDragging = isDragging;
		}

		public bool IsDragging ()
		{
				return isDragging;
		}

		public void SetDraggingTile (DeckTile draggingTile)
		{
				this.draggingTile = draggingTile;
		}
	
		public DeckTile GetDraggingTile ()
		{
				return draggingTile;
		}

		public void SetCurrentX (int x)
		{
				currentCoordinateX = x;
		}

		public void SetCurrentY (int y)
		{
				currentCoordinateY = y;
		}

		public void SetDirection (Direction direction)
		{
				this.direction = direction;
		}
	
		public Direction GetDirection ()
		{
				return direction;
		}
	
}

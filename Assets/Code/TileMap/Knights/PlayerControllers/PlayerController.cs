using UnityEngine;
using System.Collections;

public abstract class PlayerController : MonoBehaviour
{
		public float speed;
		private static float SPEED = 0.5f;
		public static float STOPPED = 0.0f;
		public Vector2 direction;	
		private Animator animator;
		private TileMap tileMap;	
		private float minX = -3.6f;
		private float maxX = 3.7f;
		private float minY = -3.6f;
		private float maxY = 3.7f;
		private float boundary = 0.1f;
		public static Vector2 RIGHT = new Vector2 (1.0f, 0.0f);
		public static Vector2 LEFT = new Vector2 (-1.0f, 0.0f);
		public static Vector2 UP = new Vector2 (0.0f, 1.0f);
		public static Vector2 DOWN = new Vector2 (0.0f, -1.0f);
		public static Vector2 STOP = new Vector2 (0.0f, 0.0f);
		public bool isWinner = false;
		public bool isRespawn = true;
		public Vector2 respawnPosition;
		public Vector3 startDirection;
		new string name;
		public Vector3 startPosition;
		public Vector3 endPosition;
		public bool flag = false;
		private float duration = 8f;
		public const float ZERO = 0.0f;
		public const float NINETY = 90.0f;
		public const float ONE_EIGHTY = 180.0f;
		public const float TWO_SEVENTY = 270.0f;
		private bool change = true;
		public Transform defaultParent;
		private int count = 0;

		/*
	 * In "respawn mode" disable all colliders and set up initial "walk to gate" waypoints
	 * In Update() - Auto adjust to centre of tile based on direction X
	 * Check when out of bounds and adjust 1/2 X
	 * Turn on corners X
	 * Turn random on T junctions
	 * Shift appropriately on tile insert
	 * collide with castle to win
	 * pop if off board 
	 * don't collide with deck tiles X
	 * */
		

		public void Start ()
		{
				animator = this.GetComponent<Animator> ();
				direction = startDirection;
				//this.transform.localPosition = respawnPosition;
				this.speed = STOPPED;
				isRespawn = true;
				//this.speed = SPEED;
				defaultParent = this.transform.parent;
		}
 
		void Update ()
		{

				bool isOutOfBounds = IsOutOfbounds ();
				if (isRespawn) {
						animator.enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().sprite = GetRespawnSprite ();
				} else {
						animator.enabled = true;
				}
				if (!isRespawn && isOutOfBounds) {
						ChangeDirection ();
						//MoveIntoBounds ();
				}

				MaybeTurn ();
				TotallyOutOfBounds ();
				if (!isOutOfBounds && !isTileMoving () && isRespawn) {
						isRespawn = false;
				}

				if (isRespawn && !flag) {
						if (canMove () && !isTileMoving ()) {
								gameObject.GetComponent<BoxCollider2D> ().enabled = true;
								speed = SPEED;
								animator.enabled = true;
						}
				}

				

				if (flag && !Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude) && !isRespawn) {
						//move the gameobject to the desired position
						gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, endPosition, 1 / (duration * (Vector3.Distance (gameObject.transform.position, endPosition))));
				} else {
						Vector3 movement = direction * speed;
						movement *= Time.deltaTime;
						transform.Translate (movement);
						SetDirection ();
						CentreCharacter ();
				}
				if (flag && Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						flag = false;
				}
		
		}

		bool isTileMoving ()
		{
				GameController gameController = GameObject.FindObjectOfType<GameController> ();
				return gameController.IsTileMoving ();
		}

		void CentreCharacter ()
		{
				if (!IsOutOfbounds ()) {
						bool centerHoriz = Mathf.Approximately (direction.y, 0f);
						bool centerVert = Mathf.Approximately (direction.x, 0f);
						Vector2 coordinate = GetCurrentCoordinate ();
						Vector3 newPosition = transform.localPosition;
						if (centerHoriz) {					
								newPosition.y = 1.9f + (coordinate.y * 1.1f);
				
						} else if (centerVert) {
								newPosition.x = 1.7f + (coordinate.x * 1.1f);
						}
						transform.localPosition = newPosition;

				}

		}


		public abstract bool canMove ();

		Vector2 GetCurrentCoordinate ()
		{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.1f);
				Vector2 coordinate = new Vector2 (Mathf.RoundToInt (transform.position.x + 3f) / TileMap.tileSize, Mathf.RoundToInt (transform.position.y + 3f) / TileMap.tileSize);
				foreach (Collider2D collider2D in colliders) {
						Tile tile = collider2D.gameObject.GetComponent<Tile> ();
						DeckTile deckTile = collider2D.gameObject.GetComponent<DeckTile> ();
						if (tile != null && deckTile == null) {
								coordinate = tile.coordinate;
						}
				}
				return coordinate;
		}

		public Tile GetCurrentTile ()
		{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.1f);
				foreach (Collider2D collider2D in colliders) {
						Tile tile = collider2D.gameObject.GetComponent<Tile> ();
						return tile;
				}
				return null;
		}


		void SetDirection ()
		{
				if (direction.y > 0) {
						animator.SetInteger ("direction", 2);
				} else if (direction.y < 0) {
						animator.SetInteger ("direction", 0);
				} else if (direction.x > 0) {
						animator.SetInteger ("direction", 3);
				} else if (direction.x < 0) {
						animator.SetInteger ("direction", 1);
				}
		}

		void OnTriggerEnter2D (Collider2D collide)
		{
				if (collide.gameObject.GetComponent<DeckTile> () == null) {
						if (!isRespawn && !IsMe (collide)) {
								if (collide is BoxCollider2D) {
										StairsCollider castle = collide.gameObject.GetComponent<StairsCollider> ();
										if (castle == null) {
												ChangeDirection ();
										}						
										PlayerController collisionController = collide.gameObject.GetComponent<PlayerController> ();
										if (collisionController != null) {
												collisionController.ChangeDirection ();
										}
					
								}
				
						}


				}

		}

		bool IsMe (Collider2D collider)
		{
				return collider == this.collider2D;
		}

		public void ChangeDirection ()
		{
				if (change) {
						direction = -1 * direction;
						StartCoroutine ("WaitHalfSec");
						change = false;
				}
				


		}

		IEnumerator WaitHalfSec ()
		{
				yield return new WaitForSeconds (0.5f);
				change = true;
		}
	
	
		bool IsOutOfbounds ()
		{
				if (transform.position.x < minX || transform.position.x > maxX) {
						return true;
				}
		
				if (transform.position.y < minY || transform.position.y > maxY) {
						return true;
				}
				return false;
		
		}

		void TotallyOutOfBounds ()
		{
				Vector3 myPosition = transform.position;
				if (transform.position.x < (minX - 0.5f)) {
						respawn ();
				}
				if (transform.position.x > (maxX + 0.5f)) {
						respawn ();
				}
				if (transform.position.y < (minY - 0.5f)) {
						respawn ();
				}
				if (transform.position.y > (maxY + 0.5f)) {
						respawn ();
				}
		}

		public abstract string GetName ();
		public abstract Sprite GetRespawnSprite ();

		public void Stop ()
		{
				direction = STOP;
				transform.Translate (direction);
		}

		public void TurnRight ()
		{
				if (direction == RIGHT) {
						direction = DOWN;
				} else if (direction == LEFT) {
						direction = UP;
				} else if (direction == UP) {
						direction = RIGHT;
				} else if (direction == DOWN) {
						direction = LEFT;
				}
		}

		public void TurnLeft ()
		{
				if (direction == RIGHT) {
						direction = UP;
				} else if (direction == LEFT) {
						direction = DOWN;
				} else if (direction == UP) {
						direction = LEFT;
				} else if (direction == DOWN) {
						direction = RIGHT;
				}
		
		}

		public bool checkRespawn (int x, int y)
		{
				int x_idx = GetColumn ();
				int y_idx = GetRow ();
				return x == x_idx && y == y_idx;
		}

		public bool isOnRow (int y)
		{
				return !isRespawn && (y - 1).Equals (GetRow ());
		}

		private int GetRow ()
		{
				if (isRespawn) {
						return -1;
				}
				int row = Mathf.RoundToInt ((transform.localPosition.y - 1.9f) / TileMap.tileSize);
				return row;
		}

		public bool isOnColumn (int x)
		{ 	
				//return x.Equals (GetCoordFromPos (GetPosition ().x));
				return !isRespawn && (x - 1).Equals (GetColumn ());
		}

		private int GetColumn ()
		{
				if (isRespawn) {
						return -1;
				}
				int column = Mathf.RoundToInt ((transform.localPosition.x - 1.7f) / TileMap.tileSize);
				return column;
		}
		
		private int GetCoordFromPos (float pos)
		{
				return (Mathf.RoundToInt (pos / TileMap.tileSize));
		}

		/*public Tile GetTileAtCoordinate (Vector2 coordinate)
		{
				Vector3 position = new Vector3 (TileMap.tileSize * (0.5f + coordinate.x), TileMap.tileSize * (0.5f + coordinate.y), 0.0f);
				GameObject[] boardTiles = GameObject.FindGameObjectsWithTag ("Board Tile");
				foreach (GameObject boardTile in boardTiles) {
						if ((boardTile.transform.position - position).magnitude < 2) {
								return boardTile.GetComponent<Tile> ();
						}
				}
				return null;		
		} */
	
		void respawn ()
		{
				speed = STOPPED;
				animator.SetBool ("pop", true);
				gameObject.audio.Play ();	
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				StartCoroutine ("Wait");
		}

		IEnumerator Wait ()
		{
				yield return new WaitForSeconds (1);
				this.transform.localPosition = respawnPosition;
				animator.SetBool ("pop", false);
				direction = startDirection;
				transform.Translate (direction);
				isRespawn = true;
		}

		public abstract Vector3 GetRespawnPosition ();
		public abstract string GetRespawnCastle ();

		public Vector3 GetPosition ()
		{
				return transform.position;

		}

		public bool isEqual (float x, float y)
		{
				return Mathf.RoundToInt (x) == Mathf.RoundToInt (y);				
		}

		public void ShiftRight (int x, int y)
		{
				if (this.isOnRow (y)) {
						if (checkRespawn (x + Board.boardSizeX - 1, y - 1)) {
								respawn ();
						} else {
								if (!isRespawn) {
										startPosition = gameObject.transform.position;	
										endPosition = new Vector3 (startPosition.x + TileMap.tileSize, startPosition.y, startPosition.z);
										flag = true;
								} else {
										Wait ();
								}
						}
				}
		}

		public void ShiftUp (int x, int y)
		{
				if (isOnColumn (x)) {
						if (checkRespawn (x - 1, y + Board.boardSizeY - 1)) {
								respawn ();
						} else {
								if (!isRespawn) {
										startPosition = gameObject.transform.position;
										endPosition = new Vector3 (startPosition.x, startPosition.y + TileMap.tileSize, startPosition.z);
										flag = true;
								}
						}
				}
		}

		public void ShiftLeft (int x, int y)
		{
				if (isOnRow (y)) {
						if (checkRespawn (x - Board.boardSizeX - 1, y - 1)) {
								respawn ();
						} else {
								if (!isRespawn) {
										startPosition = gameObject.transform.position;
										endPosition = new Vector3 (startPosition.x - TileMap.tileSize, startPosition.y, startPosition.z);
										flag = true;
								}
						}
				}
		}

		public void ShiftDown (int x, int y)
		{
				if (isOnColumn (x)) {
						if (checkRespawn (x - 1, y - Board.boardSizeY - 1)) {
								respawn ();
						} else {
								if (!isRespawn) {
										startPosition = gameObject.transform.position;	
										endPosition = new Vector3 (startPosition.x, startPosition.y - TileMap.tileSize, startPosition.z);
										flag = true;
								}
						}
				}
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
				PlayerController knight = collision.gameObject.GetComponent<PlayerController> ();
				StairsCollider castle = collision.gameObject.GetComponent<StairsCollider> ();
				if (knight != null) {
						ChangeDirection ();
						knight.ChangeDirection ();
				}
				if (castle != null) {
						// do nothing
				}
				
		}

		void MaybeTurn ()
		{
				if (count > 6) {
						Tile t = GetCurrentTile ();
						if (t != null && t.type == Tile.TileType.TJunction) {
								Turn (t);
						}

						if (t != null && t.type == Tile.TileType.Curve) {
								TurnCurve (t);
						}
						count = 0;
				}
				count++;

		}

		void TurnCurve (Tile t)
		{
				int rotation = Mathf.FloorToInt (t.gameObject.transform.rotation.eulerAngles.z);
				if (isCenterHoriz (t) && direction == RIGHT) {
						if (rotation == 180) {
								TurnRight ();
						}
						if (rotation == 90) {
								TurnLeft ();
						}
				
				}
			
				if (isCenterHoriz (t) && direction == LEFT) {
						if (rotation == 0) {
								TurnRight ();
						}
						if (rotation == 270) {
								TurnLeft ();
						}
				}
			
				if (isCenterVert (t) && direction == UP) {
						if (rotation == 270) {
								TurnRight ();
						}
						if (rotation == 180) {
								TurnLeft ();
						}
				}
			
				if (isCenterVert (t) && direction == DOWN) {
						if (rotation == 0) {
								TurnLeft ();
						}
						if (rotation == 90) {
								TurnRight ();
						}
				
				}
		}

		void Turn (Tile t)
		{
				int rotation = Mathf.FloorToInt (t.gameObject.transform.rotation.eulerAngles.z);
				if (isCenterHoriz (t) && direction == RIGHT) {

						if (rotation == 90) {
								LeftOrStraight ();
						}
						if (rotation == 180) {
								RightOrLeft ();
						}
						if (rotation == 270) {
								RightOrStraight ();
						}
				}
		
				if (isCenterHoriz (t) && direction == LEFT) {
						if (rotation == 0) {
								RightOrLeft ();
						}
						if (rotation == 90) {
								RightOrStraight ();
						}
						if (rotation == 270) {
								LeftOrStraight ();
						}
				}
		
				if (isCenterVert (t) && direction == PlayerController.UP) {
						if (rotation == 0) {
								RightOrStraight ();
						}
						if (rotation == 180) {
								LeftOrStraight ();
						}
						if (rotation == 270) {
								RightOrLeft ();
						}
				}
		
				if (isCenterVert (t) && direction == PlayerController.DOWN) {
						if (rotation == 0) {
								LeftOrStraight ();
						}
						if (rotation == 90) {
								RightOrLeft ();
						}
						if (rotation == 180) {
								RightOrStraight ();
						}
			
				}
		}
	
		private void RightOrLeft ()
		{
				if (Random.value > 0.5f) {
						TurnRight ();
				} else {
						TurnLeft ();
				}
				
		}
	
		private void RightOrStraight ()
		{
				if (Random.value > 0.5f) {
						TurnRight ();
				}
		}
	
		private void LeftOrStraight ()
		{
				if (Random.value > 0.5f) {
						TurnLeft ();
				}
		
		}

		private bool isCenterHoriz (Tile t)
		{
				return Mathf.Abs (this.transform.localPosition.x - t.transform.localPosition.x) < 0.03;
		}

		private bool isCenterVert (Tile t)
		{
				return Mathf.Abs (this.transform.localPosition.y - (t.transform.localPosition.y + 0.2f)) < 0.03;
		}

}

using UnityEngine;
using System.Collections;

public abstract class PlayerController : MonoBehaviour
{
		private float speed;
		private static float SPEED = 0.5f;
		public static float STOPPED = 0.0f;
		public Vector2 direction;	
		private Animator animator;
		private TileMap tileMap;	
		private float minX = -3.6f;
		private float maxX = 3.6f;
		private float minY = -3.6f;
		private float maxY = 3.6f;
		private float boundary = 0.1f;
		public static Vector2 RIGHT = new Vector2 (1.0f, 0.0f);
		public static Vector2 LEFT = new Vector2 (-1.0f, 0.0f);
		public static Vector2 UP = new Vector2 (0.0f, 1.0f);
		public static Vector2 DOWN = new Vector2 (0.0f, -1.0f);
		public static Vector2 RIGHT_UP = new Vector2 (1.0f, 1.0f);
		public static Vector2 LEFT_UP = new Vector2 (-1.0f, 1.0f);
		public static Vector2 RIGHT_DOWN = new Vector2 (1.0f, -1.0f);
		public static Vector2 LEFT_DOWN = new Vector2 (-1.0f, -1.0f);
		public static Vector2 STOP = new Vector2 (0.0f, 0.0f);
		public bool isWinner = false;
		public bool isRespawn = true;
		public Vector2 respawnPosition;
		public Vector3 startDirection;
		new string name;
		public Vector3 startPosition;
		public Vector3 endPosition;
		public bool flag = false;
		private float duration = 30f;
		public const float ZERO = 0.0f;
		public const float NINETY = 90.0f;
		public const float ONE_EIGHTY = 180.0f;
		public const float TWO_SEVENTY = 270.0f;
		

		public void Start ()
		{
				animator = this.GetComponent<Animator> ();
				direction = startDirection;
				this.transform.position = respawnPosition;
				//this.speed = STOPPED;
				this.speed = SPEED;
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
						//ChangeDirection ();
						//MoveIntoBounds ();
				}
				if (!isOutOfBounds && !isTileMoving ()) {
						isRespawn = false;
				}
				if (isRespawn && !flag) {
						if (canMove ()) {
								speed = SPEED;
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


		public abstract bool canMove ();


		void SetDirection ()
		{
				if (direction.y > 0) {
						animator.SetInteger ("direction", 2);
				} else
		        if (direction.y < 0) {
						animator.SetInteger ("direction", 0);
				} else
				if (direction.x > 0) {
						animator.SetInteger ("direction", 3);
				} else if (direction.x < 0) {
						animator.SetInteger ("direction", 1);
				}
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
				if (!isRespawn && !IsMe (collision.collider.collider2D)) {
						if (collision.collider.collider2D is BoxCollider2D) {
								ChangeDirection ();
								PlayerController collisionController = collision.gameObject.GetComponent<PlayerController> ();
								if (collisionController != null) {
										collisionController.ChangeDirection ();
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
				/*if (direction == RIGHT) {
						direction = LEFT;
				} else if (direction == LEFT) {
						direction = RIGHT;
				} else if (direction == UP) {
						direction = DOWN;
				} else if (direction == DOWN) {
						direction = UP;
				}	*/
				direction = -1 * direction;
				//transform.Translate (direction);
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

		void MoveIntoBounds ()
		{
				Vector3 myPosition = transform.position;
				if (transform.position.x < minX) {
						myPosition.x += 0.01f;
				}
				if (transform.position.x > maxX) {
						myPosition.x -= 0.01f;
				}
		
				if (transform.position.y < minY) {
						myPosition.y += 0.01f;
				}
				if (transform.position.y > maxY) {
						myPosition.y -= 0.01f;
				}
				transform.Translate (myPosition);
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
				//transform.Translate (direction);
		}

		public void Turn45Right ()
		{
				if (direction == RIGHT) {
						direction = RIGHT_DOWN;
				} else if (direction == LEFT) {
						direction = LEFT_UP;
				} else if (direction == UP) {
						direction = RIGHT_UP;
				} else if (direction == DOWN) {
						direction = LEFT_DOWN;
				} else if (direction == RIGHT_DOWN) {
						direction = DOWN;
				} else if (direction == LEFT_DOWN) {
						direction = LEFT;
				} else if (direction == RIGHT_UP) {
						direction = RIGHT;
				} else if (direction == LEFT_UP) {
						direction = UP;
				}
				//transform.Translate (direction);
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
				//transform.Translate (direction);
		
		}

		public void Turn45Left ()
		{

				if (direction == RIGHT) {
						direction = RIGHT_UP;
				} else if (direction == LEFT) {
						direction = LEFT_DOWN;
				} else if (direction == UP) {
						direction = LEFT_UP;
				} else if (direction == DOWN) {
						direction = RIGHT_DOWN;
				} else if (direction == RIGHT_DOWN) {
						direction = RIGHT;
				} else if (direction == LEFT_DOWN) {
						direction = DOWN;
				} else if (direction == RIGHT_UP) {
						direction = UP;
				} else if (direction == LEFT_UP) {
						direction = LEFT;
				}
				//transform.Translate (direction);
		
		}

		public bool checkRespawn (int x, int y)
		{
				int x_idx = Mathf.FloorToInt (GetPosition ().x / TileMap.tileSize);
				int y_idx = Mathf.FloorToInt (GetPosition ().y / TileMap.tileSize);
				return x == x_idx && y == y_idx;
		}

		public bool isOnRow (int y)
		{
				return y.Equals (Mathf.FloorToInt (transform.localPosition.y / TileMap.tileSize));
		}

		public bool isOnColumn (int x)
		{ 	
				return x.Equals (GetCoordFromPos (GetPosition ().x));
		}
		
		private int GetCoordFromPos (float pos)
		{
				return (Mathf.FloorToInt (pos / TileMap.tileSize));
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
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				animator.SetBool ("pop", true);
				gameObject.audio.Play ();	
				StartCoroutine ("Wait");

		}

		IEnumerator Wait ()
		{
				yield return new WaitForSeconds (2);				
				this.transform.position = respawnPosition;
				animator.SetBool ("pop", false);
				direction = startDirection;
				gameObject.GetComponent<BoxCollider2D> ().enabled = true;
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
				return Mathf.FloorToInt (x) == Mathf.FloorToInt (y);				
		}
	
		public void ShiftRight (int x, int y)
		{
				if (this.isOnRow (y)) {
						if (checkRespawn (x + TileMap.size_x - 2, y)) {
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
						if (checkRespawn (x, y + TileMap.size_y - 2)) {
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
						if (checkRespawn (x - TileMap.size_x - 2, y)) {
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
						if (checkRespawn (x, y - TileMap.size_y - 2)) {
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
}

//		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
//		{
//				// Always send transform (depending on reliability of the network view)
//				if (stream.isWriting) {
//						Vector3 pos = gameObject.transform.localPosition;
//						Quaternion rot = gameObject.transform.localRotation;
//						stream.Serialize (ref pos);
//						stream.Serialize (ref rot);
//				}
//		// When receiving, buffer the information
//		else {
//						// Receive latest state information
//						Vector3 pos = Vector3.zero;
//						Quaternion rot = Quaternion.identity;
//						stream.Serialize (ref pos);
//						stream.Serialize (ref rot);
//						gameObject.transform.localRotation = rot;
//						gameObject.transform.localPosition = pos;//				}
//		}
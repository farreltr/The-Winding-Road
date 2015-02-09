using UnityEngine;
using System.Collections;

public abstract class PlayerControllerNP : MonoBehaviour
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
		private float duration = 50f;
		public const float ZERO = 0.0f;
		public const float NINETY = 90.0f;
		public const float ONE_EIGHTY = 180.0f;
		public const float TWO_SEVENTY = 270.0f;
		private Vector3 nextWayPoint;
		private Vector3 lastWayPoint;
		public bool isTriggerAction = false;
		private bool moving = false;
		

		public void Start ()
		{
				animator = this.GetComponent<Animator> ();
				direction = startDirection;
				this.transform.position = respawnPosition;
				//this.speed = STOPPED;
				this.speed = SPEED;
				nextWayPoint = respawnPosition;
				lastWayPoint = respawnPosition;
				WhereAmI ();

		}
 
		void Update ()
		{

				//bool isOutOfBounds = IsOutOfbounds ();
				bool isOutOfBounds = false;
				if (isRespawn) {
						animator.enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().sprite = GetRespawnSprite ();
				} else {
						animator.enabled = true;
				}
				if (!isRespawn && isOutOfBounds) {
						WhereAmI ();
						//ChangeDirection ();
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
						//Vector3 movement = direction * speed;
						//movement *= Time.deltaTime;
						//transform.Translate (movement);
						if (Mathf.Approximately (gameObject.transform.position.magnitude, nextWayPoint.magnitude) || isTriggerAction) {
								WhereAmI ();
						}
						gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, nextWayPoint, 1 / (duration * (Vector3.Distance (gameObject.transform.position, nextWayPoint))));
						SetDirection ();
				}
				if (flag && Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						flag = false;
				}
		
		}

		void WhereAmI ()
		{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), Tile.RADIUS);
				float shortestDistance = float.PositiveInfinity;
				Vector3 waypoint = transform.position;
				bool isSet = false;
				foreach (Collider2D collider in colliders) {
						if (collider is CircleCollider2D) {
								Vector3 wp = collider.transform.position;
								float distance = Vector3.Distance (transform.position, wp);
								bool isSmallerDistance = distance < shortestDistance;
								bool isCurrent = IsCurrent (wp);
								bool isLast = IsLast (wp);
								bool isDiagonal = IsDiagonal (distance);
								if (isSmallerDistance && !isCurrent && !isLast && !isDiagonal) {
										shortestDistance = distance;
										waypoint = wp;
										isSet = true;
								}
						}

						/*	Tile tile = collider.transform.gameObject.GetComponent<Tile> ();
						if (tile != null) {
								Vector3[] activeWayPoints = tile.GetActiveWayPoints ();
								foreach (Vector3 wp in activeWayPoints) {
										float distance = Vector3.Distance (transform.position, wp);
										bool isSmallerDistance = distance < shortestDistance;
										bool isCurrent = IsCurrent (wp);
										bool isLast = IsLast (wp);
										bool isDiagonal = IsDiagonal (distance);
										if (isSmallerDistance && !isCurrent && !isLast && !isDiagonal) {
												shortestDistance = distance;
												waypoint = wp;
												isSet = true;
										}
								}
						} */

				}	
				if (!isSet) {
						waypoint = lastWayPoint;
				}
				lastWayPoint = nextWayPoint;
				nextWayPoint = waypoint;
				if (!AlmostEqual (lastWayPoint, nextWayPoint, 0.1f)) {
						isTriggerAction = false;
				}
				
		}

		/*	void WhereAmI ()
		{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), 0.1f);
				foreach (Collider2D collider in colliders) {
						Tile tile = collider.transform.gameObject.GetComponent<Tile> ();
						int wpPos = tile.GetPointForPosition (transform.position);
						if (tile != null) {
								switch (direction) {
								case Vector3.right:
										{
												break;
										}
								case Vector3.left:
										{
												break;
										}
								case Vector3.up:
										{
												break;
										}
								case Vector3.down:
										{
												break;
										}

								}
						}
				}
		} */

		bool IsCurrent (Vector3 wp)
		{
				return AlmostEqual (transform.position, wp, 0.1f);
		}
	
		bool IsLast (Vector3 wp)
		{
				if (lastWayPoint == null) {
						return false;
				}

				return AlmostEqual (lastWayPoint, wp, 0.1f);
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
				if (collider is CircleCollider2D) {
						GetNextWayPoint (collider);
				}
				
		}

		void GetNextWayPoint (Collider2D collider2d)
		{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), Tile.RADIUS);
				float smallestDistance = float.PositiveInfinity;
				foreach (Collider2D collider in colliders) {
						if (collider is CircleCollider2D && collider != collider2d) {
								Vector3 wp = collider.transform.position;
								float distance = Vector3.Distance (transform.position, wp);
								if (distance < smallestDistance) {
										smallestDistance = distance;
										nextWayPoint = wp;
								}
						}
				}
				if (smallestDistance == float.PositiveInfinity) {
						nextWayPoint = lastWayPoint;
						ChangeDirection ();
				}
		}

		bool IsDiagonal (float distance)
		{	
				return distance > Tile.RADIUS + 0.1f;
		}

		public static bool AlmostEqual (Vector3 v1, Vector3 v2, float precision)
		{
				bool equal = true;
		
				if (Mathf.Abs (v1.x - v2.x) > precision)
						equal = false;
				if (Mathf.Abs (v1.y - v2.y) > precision)
						equal = false;
				if (Mathf.Abs (v1.z - v2.z) > precision)
						equal = false;
		
				return equal;
		}


		bool IsBackWards (Vector3 wp)
		{
				float x_diff = wp.x - transform.position.x * direction.x;
				float y_diff = wp.y - transform.position.y * direction.y;
				if (x_diff != ZERO && x_diff < transform.position.x) {
						return true;
				}
				if (y_diff != ZERO && y_diff < transform.position.y) {
						return true;
				}
				return false;
		}

		bool isTileMoving ()
		{
				GameController gameController = GameObject.FindObjectOfType<GameController> ();
				if (gameController != null) {
						return gameController.IsTileMoving ();
				}
				return false;	
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
				if (!isRespawn) {
						ChangeDirection ();
						PlayerController collisionController = collision.gameObject.GetComponent<PlayerController> ();
						if (collisionController != null) {
								collisionController.ChangeDirection ();
						}
				}

		}

		public void ChangeDirection ()
		{
				if (direction == RIGHT) {
						direction = LEFT;
				} else if (direction == LEFT) {
						direction = RIGHT;
				} else if (direction == UP) {
						direction = DOWN;
				} else if (direction == DOWN) {
						direction = UP;
				}		
				transform.Translate (direction);
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
				transform.Translate (direction);
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
				transform.Translate (direction);
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
				transform.Translate (direction);
		
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
				transform.Translate (direction);
		
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
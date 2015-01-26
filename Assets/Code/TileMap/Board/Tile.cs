using UnityEngine;
using System.Collections;


[System.Serializable]
public class Tile : MonoBehaviour
{
		public TileType type = TileType.Empty;
		public static Vector3 ZERO_ANGLE = Vector3.zero;
		public static Vector3 NINETY_ANGLE = new Vector3 (0.0f, 0.0f, 90.0f);
		public static Vector3 ONE_EIGHTY_ANGLE = new Vector3 (0.0f, 0.0f, 180.0f);
		public static Vector3 TWO_SEVENTY_ANGLE = new Vector3 (0.0f, 0.0f, 270.0f);
		public Vector2 coordinate = Vector2.zero;
		public Vector3 direction = Vector3.right;
		private Vector3 startPosition;
		public Vector3 endPosition;
		private Vector2 xBounds = Vector2.zero;
		private Vector2 yBounds = Vector2.zero;
		public bool flag = false;
		private float duration = 30f;
		public bool setToDestroy = false;
		public bool destroying = false;
		private Vector3 destroySize = new Vector3 (0.00001f, 0.00001f, 0.00001f);
		private bool isInPlayerHand = false;
		private bool isUpToDate = false;
		public int slotIndex = -1;

		public enum TileType
		{
				Block=0,
				CrossJunction=1,
				TJunction=2,
				Curve=3,
				Straight=4,
				Empty
		}

		public void Start ()
		{
				endPosition = gameObject.transform.position;				
		}

		public static TileType getTileType (string tileString)
		{
				switch (tileString) {
				case "block":
						return TileType.Block;
				case "cross-junction":
						return TileType.CrossJunction;
				case "t-junction":
						return TileType.TJunction;
				case "right-angle-junction":
						return TileType.Curve;
				case "straight-junction":
						return TileType.Straight;
				default :
						return TileType.Empty;
				}
		}

		public static string getTileString (TileType tileType)
		{
				switch (tileType) {
				case TileType.Block:
						return "block";
				case TileType.CrossJunction:
						return "cross-junction";
				case TileType.TJunction:
						return "t-junction";
				case TileType.Curve:
						return "right-angle-junction";
				case  TileType.Straight:
						return "straight-junction";
				default :
						return "";
				}
		}

		public bool isEmpty ()
		{
				return this.type.Equals (TileType.Empty);
		}

		public Texture2D GetIcon ()
		{
				return Resources.Load<Texture2D> ("Tiles/Sprites/" + getRotationString () + "/" + this.name);
		}

		string getRotationString ()
		{
				int rot = Mathf.FloorToInt (gameObject.transform.rotation.eulerAngles.z);
				return string.Concat (rot, "-degree-rotation");
		}

		public void RotateLeft ()
		{
				if (gameObject.transform.rotation.eulerAngles == TWO_SEVENTY_ANGLE) {
						gameObject.transform.rotation = Quaternion.Euler (ZERO_ANGLE);
				} else {
						gameObject.transform.rotation = Quaternion.Euler (new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z + 90));
				}

		}

		public void RotateRight ()
		{
				if (gameObject.transform.eulerAngles == ZERO_ANGLE) {
						gameObject.transform.rotation = Quaternion.Euler (TWO_SEVENTY_ANGLE);
				} else {
						gameObject.transform.rotation = Quaternion.Euler (new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z - 90));
				}

		}

		public void SetUpTile (TileType tileType)
		{
				GameObject tile = this.gameObject;
				AddRigidBody2D (tile);
				AddSpriteRenderer (tile, tileType);
				this.type = tileType;
				
				AddBoxCollider2D (tileType, tile);
		
		}

		static void AddBoxCollider2D (TileType tileType, GameObject tile)
		{
				switch (tileType) {
				case TileType.Block:
//						BoxCollider2D block = tile.AddComponent<BoxCollider2D> ();
						break;
				case TileType.CrossJunction:
						Vector2 sizecj = new Vector2 (0.3f, 0.3f);
						Vector2 ctopleft = new Vector2 (0.35f, 0.35f);
						Vector2 ctopright = new Vector2 (0.35f, -0.35f);
						Vector2 cbottomleft = new Vector2 (-0.35f, 0.35f);
						Vector2 cbottomright = new Vector2 (-0.35f, -0.35f);
						BoxCollider2D topleft = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D topright = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottomleft = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottomright = tile.AddComponent<BoxCollider2D> ();
						topleft.size = sizecj;
						topright.size = sizecj;
						bottomleft.size = sizecj;
						bottomright.size = sizecj;
						topleft.center = ctopleft;
						topright.center = ctopright;
						bottomleft.center = cbottomleft;
						bottomright.center = cbottomright;
						break;
				case TileType.TJunction:
						Vector2 tjsizetop = new Vector2 (1.0f, 0.32f);
						Vector2 tjsizebottom = new Vector2 (0.3f, 0.3f);
						Vector2 tjtop = new Vector2 (0f, 0.35f);
						Vector2 tjbottomleft = new Vector2 (-0.35f, -0.35f);
						Vector2 tjbottomright = new Vector2 (0.35f, -0.35f);
						BoxCollider2D toptj = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottomlefttj = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottomrighttj = tile.AddComponent<BoxCollider2D> ();
						toptj.size = tjsizetop;
						bottomlefttj.size = tjsizebottom;
						bottomrighttj.size = tjsizebottom;
						toptj.center = tjtop;
						bottomlefttj.center = tjbottomleft;
						bottomrighttj.center = tjbottomright;
						break;
				case TileType.Curve:
						BoxCollider2D topRight = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D leftBarrier = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottomBarrier = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D trigger = tile.AddComponent<BoxCollider2D> ();
						topRight.size = new Vector2 (0.3f, 0.3f);
						topRight.center = new Vector2 (0.3f, 0.3f);
						leftBarrier.size = new Vector2 (0.32f, 1.0f);
						leftBarrier.center = new Vector2 (-0.33f, 0.0f);
						bottomBarrier.size = new Vector2 (1.0f, 0.32f);
						bottomBarrier.center = new Vector2 (0.0f, -0.33f);
						trigger.size = new Vector2 (0.8f, 0.08f);
						trigger.center = new Vector2 (-0.13f, -0.13f);
						trigger.isTrigger = true;
						break;
				case TileType.Straight:
						Vector2 sizes = new Vector2 (1.0f, 0.3f);
						Vector2 ctop = new Vector2 (0f, 0.35f);
						Vector2 cbottom = new Vector2 (0f, -0.35f);
						BoxCollider2D top = tile.AddComponent<BoxCollider2D> ();
						BoxCollider2D bottom = tile.AddComponent<BoxCollider2D> ();
						top.size = sizes;
						bottom.size = sizes;
						top.center = ctop;
						bottom.center = cbottom;
						break;
				}
		}

		void AddSpriteRenderer (GameObject tile, TileType tileType)
		{
				SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer> ();		
				tile.name = getTileString (tileType);
				spriteRenderer.sprite = Resources.Load <Sprite> ("Tiles/Sprites/0-degree-rotation/" + tile.name);
		}

		public static void AddRigidBody2D (GameObject gameObject)
		{
				Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D> ();
				rigidbody2D.mass = 10.0f;
				rigidbody2D.drag = 0.0f;
				rigidbody2D.angularDrag = 0.0f;
				rigidbody2D.gravityScale = 1.0f;
				rigidbody2D.fixedAngle = true;
				rigidbody2D.isKinematic = true;
				rigidbody2D.interpolation = RigidbodyInterpolation2D.None;
				rigidbody2D.sleepMode = RigidbodySleepMode2D.StartAsleep;
				rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		}
	
		public void shiftRight ()
		{
				startPosition = gameObject.transform.position;	
				endPosition = new Vector3 (startPosition.x + TileMap.tileSize, startPosition.y, startPosition.z);
				this.coordinate = new Vector2 (coordinate.x + 1, coordinate.y);	
				flag = true;

		}

		public void shiftLeft ()
		{
				startPosition = gameObject.transform.position;
				endPosition = new Vector3 (startPosition.x - TileMap.tileSize, startPosition.y, startPosition.z);
				this.coordinate = new Vector2 (coordinate.x - 1, coordinate.y);	
				flag = true;		
		}
	
		public void shiftDown ()
		{

				startPosition = gameObject.transform.position;
				endPosition = new Vector3 (startPosition.x, startPosition.y - TileMap.tileSize, startPosition.z);
				this.coordinate = new Vector2 (coordinate.x, coordinate.y - 1);	
				flag = true;
		
		}
	
		public void shiftUp ()
		{
				startPosition = gameObject.transform.position;
				endPosition = new Vector3 (startPosition.x, startPosition.y + TileMap.tileSize, startPosition.z);
				this.coordinate = new Vector2 (coordinate.x, coordinate.y + 1);	
				flag = true;		
		}

		public void SetTileType (TileType type)
		{
				this.type = type;	
		
		}

		//private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;
		private Vector3 syncStartPosition = Vector3.zero;
		private Vector3 syncEndPosition = Vector3.zero;

		void Awake ()
		{
				//lastSynchronizationTime = Time.time;
		}
	
		void Update ()
		{

				if (flag && !Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						MoveTile ();
				} else if (flag && Mathf.Approximately (gameObject.transform.position.magnitude, endPosition.magnitude)) {
						flag = false;
						CheckDestroy ();

				}

				if (destroying && !Mathf.Approximately (gameObject.transform.localScale.magnitude, destroySize.magnitude)) {
						ShrinkTile ();
				} else if (destroying && Mathf.Approximately (gameObject.transform.localScale.magnitude, destroySize.magnitude)) {
						PutTileInSlot ();
						
			
				}
		}

		void PutTileInSlot ()
		{
				PlayerDeck deck = GameObject.FindObjectOfType<PlayerDeck> ();				
				deck.PutTileInSlot (this.gameObject, slotIndex);
				this.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
				destroying = false;
				setToDestroy = false;
		}


		public void UpdateTileRefs ()
		{
				gameObject.tag = "Board";
				gameObject.layer = 9;
				gameObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Board";
				gameObject.transform.parent = GameObject.FindObjectOfType<Board> ().transform;
		}

		void CheckDestroy ()
		{
				if (setToDestroy) {
						destroying = true;
				}

		}

		public bool isAt (Vector2 coordinate)
		{
				return this.coordinate == coordinate;
		}

		void ShrinkTile ()
		{
				gameObject.transform.localScale = Vector3.Lerp (gameObject.transform.localScale, destroySize, 1 / (duration * (Vector3.Distance (gameObject.transform.localScale, destroySize))));
		}

	
		void MoveTile ()
		{
				gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, endPosition, 1 / (duration * (Vector3.Distance (gameObject.transform.position, endPosition))));
		}

		
		bool IsStop ()
		{
				if (transform.position.x < xBounds.x || transform.position.x > xBounds.y) {
						return true;
				}
		
				if (transform.position.y < yBounds.x || transform.position.y > yBounds.y) {
						return true;
				}
				return false;
		
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				if (stream.isWriting) {
						Vector3 pos = gameObject.transform.position;
						Quaternion rot = gameObject.transform.rotation;
						stream.Serialize (ref pos);
						stream.Serialize (ref rot);
				} else {
						Vector3 pos = Vector3.zero;
						Quaternion rot = Quaternion.identity;
						stream.Serialize (ref pos);
						stream.Serialize (ref rot);
						gameObject.transform.rotation = rot;
						gameObject.transform.position = pos;
				}
		}

		private void SyncedMovement ()
		{
				syncTime += Time.deltaTime;
		
				rigidbody2D.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
		}

		public bool IsInPlayerHand ()
		{
				return isInPlayerHand;
		}

		public static TileType getRandomType ()
		{
				int index = Random.Range (0, 5);
				TileType[] tileTypes = GetAllTileTypes ();
				return tileTypes [index];
		}

		public static string getRandomTypeString ()
		{
				return getTileString (getRandomType ());
		}

		public static int GetRandomRotation ()
		{
				int index = Random.Range (0, 4);
				int[] rotations = {0,90,180,270};
				return rotations [index];
		}

		public static TileType[] GetAllTileTypes ()
		{
				TileType[] tileArray = {
						TileType.Block,
						TileType.CrossJunction,
						TileType.Curve,
						TileType.Straight,
						TileType.TJunction
				};
				return tileArray;
		}

		private static Quaternion GetQuaternion (int rotation)
		{
				switch (rotation) {
				case 0:
						return Quaternion.Euler (ZERO_ANGLE);
				case 90:
						return Quaternion.Euler (NINETY_ANGLE);
				case 180:
						return Quaternion.Euler (ONE_EIGHTY_ANGLE);
				case 270:
						return Quaternion.Euler (TWO_SEVENTY_ANGLE);
				}
				return Quaternion.identity;
		}

		public void SetInPlayerHand (bool isInPlayerHand)
		{
				this.isInPlayerHand = isInPlayerHand;
		}

		public string Serialize ()
		{
				int rot = Mathf.FloorToInt (gameObject.transform.rotation.eulerAngles.z);
				return getTileString (this.type) + ":" + rot;
		}

		public static Tile Deserialize (string tileString)
		{
				string[] split = tileString.Split (':');
				Tile tile = Resources.Load ("Tiles/Prefabs/" + split [0]) as Tile;
				tile.transform.rotation = GetQuaternion (int.Parse (split [1]));
				return tile;
		}
	
}


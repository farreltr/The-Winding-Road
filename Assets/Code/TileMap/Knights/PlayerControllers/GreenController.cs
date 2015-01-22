using UnityEngine;
using System.Collections;

public class GreenController : PlayerController
{

		Vector2 startTileCoordinate = new Vector2 (1.0f, 1.0f);
		public int wins = 0;
		public Sprite winPip;
		new Sprite respawnSprite;
	
		new void Start ()
		{
				name = "green";
				//respawnPosition = new Vector3 (180.0f, 90.0f, 0); 
				respawnPosition = this.transform.position;
				this.respawnSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
				startDirection = new Vector2 (0.0f, 1.0f);
				base.Start ();

		}

		public override string GetName ()
		{
				return name;
		}
		public override Vector3 GetRespawnPosition ()
		{
				return respawnPosition;
		}

		public override string GetRespawnCastle ()
		{
				return "blue";
		}
	
		public override Sprite GetRespawnSprite ()
		{
				return respawnSprite;
		}

		public override bool canMove ()
		{	
				Tile startBlock = GetTileAtCoordinate (startTileCoordinate);
				if (startBlock == null)
						return false;
				float rotation = startBlock.transform.rotation.eulerAngles.z;
				switch (startBlock.type) {
				case Tile.TileType.CrossJunction:
						return true;
				case Tile.TileType.Curve:
						{
								if (isEqual (rotation, ONE_EIGHTY) || isEqual (rotation, TWO_SEVENTY)) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.TJunction:
						{
								if (!isEqual (rotation, ONE_EIGHTY)) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.Straight:
						{
								if (isEqual (rotation, NINETY) || isEqual (rotation, TWO_SEVENTY)) {
										return true;
								}
								break;
						}
			
				}
				return false;	
		}


}
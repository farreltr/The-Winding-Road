using UnityEngine;
using System.Collections;

public class RedController : PlayerController
{

		Vector2 startTileCoordinate = new Vector2 (TileMap.size_x - 2, 1.0f);
		new Sprite respawnSprite;

		new void Start ()
		{
				name = "red";
				//respawnPosition = new Vector3 (970.0f, 180.0f, 0);
				startDirection = new Vector2 (-1.0f, 0.0f);
				respawnPosition = this.transform.position;
				this.respawnSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
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
				return "yellow";
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
								if ((isEqual (rotation, ZERO) || isEqual (rotation, TWO_SEVENTY))) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.TJunction:
						{
								if (!isEqual (rotation, TWO_SEVENTY)) {
										return true;
								}
								break;
			
						}
				case Tile.TileType.Straight:
						{
								if ((isEqual (rotation, ZERO) || isEqual (rotation, ONE_EIGHTY))) {
										return true;
								}
								break;
			
						}
						
		
				}
				return false;		
		}
}
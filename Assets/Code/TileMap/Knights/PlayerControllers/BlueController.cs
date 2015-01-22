using UnityEngine;
using System.Collections;

public class BlueController : PlayerController
{
		Vector2 startTileCoordinate = new Vector2 (TileMap.size_x - 2, TileMap.size_y - 2);
		new Sprite respawnSprite;
	
		new void Start ()
		{
				name = "blue";
				//respawnPosition = new Vector3 (900.0f, 980.0f, 0);
				respawnPosition = this.transform.position;
				startDirection = new Vector2 (0.0f, -1.0f);
				respawnSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
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
				return "green";
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
								if ((isEqual (rotation, ZERO) || isEqual (rotation, NINETY))) {
										return true;
								}
								break;

						}
				case Tile.TileType.TJunction:
						{
								if ((!isEqual (rotation, ZERO))) {
										return true;
								}
								break;

						}
				case Tile.TileType.Straight:
						{
								if ((isEqual (rotation, NINETY) || isEqual (rotation, TWO_SEVENTY))) {
										return true;
								}
								break;

						}
				}
				return false;

		}
	
}

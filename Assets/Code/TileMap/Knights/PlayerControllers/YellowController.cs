﻿using UnityEngine;
using System.Collections;

public class YellowController : PlayerController
{
		Vector2 startTileCoordinate = new Vector2 (Board.boardSizeX - 1, Board.boardSizeY - 1);
		new Sprite respawnSprite;

		new void Start ()
		{
				name = "yellow";
				startDirection = new Vector2 (-1.0f, 0.0f);
				respawnPosition = new Vector3 (10f, 8.5f, 0);	
				//respawnPosition = this.transform.position;
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
				return "red";
		}

		public override Sprite GetRespawnSprite ()
		{
				return respawnSprite;
		}

		public override bool canMove ()
		{	
				Tile startBlock = GameController.GetTileAtCoordinate (startTileCoordinate);
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
								if ((!isEqual (rotation, ONE_EIGHTY))) {
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

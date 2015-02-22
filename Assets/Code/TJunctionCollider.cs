using UnityEngine;
using System.Collections;

public class TJunctionCollider : MonoBehaviour
{

		private bool turn = false;
		private bool collide = true;

		private enum Direction
		{
				RIGHT,
				LEFT,
				STRAIGHT
		}

		// Use this for initialization
		void Start ()
		{
				Random.seed = Mathf.FloorToInt (Time.time);
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
				PlayerController playerController = collision.collider.collider2D.transform.GetComponent<PlayerController> ();
				if (playerController != null && !playerController.isRespawn && collide) {			
						int rotation = Mathf.RoundToInt (this.transform.eulerAngles.z);
						Direction direction = getTurningDirection (playerController, rotation);
						if (direction.Equals (Direction.RIGHT)) {
								turn = !turn;
								playerController.TurnRight ();
						} else if (direction.Equals (Direction.LEFT)) {
								turn = !turn;
								playerController.TurnLeft ();
						} else {
								collide = false;
						}
				}
		}

		void OnCollisionStay2D (Collision2D collision)
		{
				print ("colliding");
		}

		void OnCollisionExit2D (Collision2D collision)
		{
				collide = true;
		}

		Direction getTurningDirection (PlayerController playerController, int rotation)
		{
				Vector2 direction = playerController.direction;
				if (direction == PlayerController.RIGHT) {
						if (rotation == 90) {
								return LeftOrStraight ();
						}
						if (rotation == 180) {
								return RightOrLeft ();
						}
						if (rotation == 270) {
								return RightOrStraight ();
						}
				}

				if (direction == PlayerController.LEFT) {
						if (rotation == 0) {
								return RightOrLeft ();
						}
						if (rotation == 90) {
								return RightOrStraight ();
						}
						if (rotation == 270) {
								return LeftOrStraight ();
						}
				}

				if (direction == PlayerController.UP) {
						if (rotation == 0) {
								return RightOrStraight ();
						}
						if (rotation == 180) {
								return LeftOrStraight ();
						}
						if (rotation == 270) {
								return RightOrLeft ();
						}
				}

				if (direction == PlayerController.DOWN) {
						if (rotation == 0) {
								return LeftOrStraight ();
						}
						if (rotation == 90) {
								return RightOrLeft ();
						}
						if (rotation == 180) {
								return RightOrStraight ();
						}
			
				}
				return Direction.STRAIGHT;
		}

		private Direction RightOrLeft ()
		{
				if (turn) {
						return Direction.RIGHT;
				}
				return Direction.LEFT;
		}

		private Direction RightOrStraight ()
		{
				if (turn) {
						return Direction.RIGHT;
				}
				return Direction.STRAIGHT;
		}

		private Direction LeftOrStraight ()
		{
				if (turn) {
						return Direction.LEFT;
				}
				return Direction.STRAIGHT;
		
		}
}

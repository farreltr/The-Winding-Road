using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
	 
		//private Color originalColour;
		private GameController gameController;
		private Direction direction = Direction.NONE;
		private int Xcoord = -1;
		private int Ycoord = -1;
		private Sprite defaultSprite;
		private SpriteRenderer spriteRenderer;
		public Sprite mouseOverSprite;
	
		void Start ()
		{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
				defaultSprite = spriteRenderer.sprite;
				gameController = GameObject.FindObjectOfType<GameController> ();

		}

//		void OnMouseUp ()
//		{
//				getGameController ().SetDestination (this);
//		}  

		void OnMouseOver ()
		{
				spriteRenderer.sprite = mouseOverSprite;
		}

		void OnMouseExit ()
		{
				spriteRenderer.sprite = defaultSprite;
		}

		GameController getGameController ()
		{
				if (gameController == null) {
						gameController = GameObject.FindObjectOfType<GameController> ();
				}
				return gameController;
		}

		public void SetDirection (Direction direction)
		{
				this.direction = direction;
		}

		public Direction GetDirection ()
		{
				return this.direction;
		}

		public int GetXcoord ()
		{
				return Xcoord;
		}

		public void SetXcoord (int Xcoord)
		{
				this.Xcoord = Xcoord;
		}

		public int GetYcoord ()
		{
				return Ycoord;
		}

		public void SetYcoord (int Ycoord)
		{
				this.Ycoord = Ycoord;
		}

}
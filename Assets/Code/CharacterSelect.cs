using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour
{
	
		private PlayerManager playerManager;
		private Player.Colour charColour;
		private bool isActive = false;
		public Sprite onSprite;
		public Sprite offSprite;
		private SpriteRenderer spriteRenderer;
		public bool joinGame = false;
		public bool triggered = true;
	
		void Start ()
		{
				playerManager = GameObject.FindObjectOfType<PlayerManager> ();
				charColour = Player.GetPlayerColour (this.name);
				spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		}
	
		public void TurnOffTheLights ()
		{
				if (triggered) {
						spriteRenderer.sprite = offSprite;
						triggered = false;
				}
				
		}
	
		void Select ()
		{
				playerManager.AddActivePlayer (charColour);
				spriteRenderer.sprite = onSprite;
				isActive = true;
		}
	
		void Deselect ()
		{
				playerManager.RemoveActivePlayer (charColour);
				spriteRenderer.sprite = offSprite;
				isActive = false;
		}
	
		void OnMouseDown ()
		{
				if (joinGame) {
						if (isActive) {
								Deselect ();
						} else {
								Select ();
						}

				}

		}
}

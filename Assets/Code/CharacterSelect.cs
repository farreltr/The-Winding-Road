using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour
{

		private PlayerManager playerManager;
		private Player.Colour charColour;
		private Transform parentChar;
		private bool isActive = false;
		public bool joinGame = false;
	
		void Start ()
		{
				playerManager = GameObject.FindObjectOfType<PlayerManager> ();
				charColour = Player.GetPlayerColour (transform.parent.name);
				parentChar = transform.parent.transform;

		}

		void Select ()
		{
				playerManager.AddActivePlayer (charColour);
				GameObject off = parentChar.FindChild (parentChar.name + "_off").gameObject;
				off.GetComponent<SpriteRenderer> ().sortingLayerName = "off";
				GameObject on = parentChar.FindChild (parentChar.name + "_on").gameObject;
				on.GetComponent<SpriteRenderer> ().sortingLayerName = "on";
				isActive = true;
		}

		void Deselect ()
		{
				playerManager.RemoveActivePlayer (charColour);
				GameObject off = parentChar.FindChild (parentChar.name + "_off").gameObject;
				off.GetComponent<SpriteRenderer> ().sortingLayerName = "on";
				GameObject on = parentChar.FindChild (parentChar.name + "_on").gameObject;
				on.GetComponent<SpriteRenderer> ().sortingLayerName = "off";
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

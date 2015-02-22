using UnityEngine;
using System.Collections;

public class AudioOnOff : MonoBehaviour
{

		public Sprite onButton;
		public Sprite offButton;
		private Music audio;
		private SpriteRenderer spriteRenderer;

		void Start ()
		{
				audio = GameObject.FindObjectOfType<Music> ();
				spriteRenderer = this.GetComponent<SpriteRenderer> ();

		}

		void OnMouseDown ()
		{

				if (audio.isMute ()) {
						audio.Unmute ();
						spriteRenderer.sprite = onButton;

				} else {
						audio.Mute ();
						spriteRenderer.sprite = offButton;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}

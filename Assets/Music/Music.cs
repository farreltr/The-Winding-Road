using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{

		public static Music music;
		public AudioSource audioSource;
		public GUISkin skin;
	
		void Awake ()
		{
				if (music == null) {
						DontDestroyOnLoad (music);
						music = this;
				} else if (music != this) {
						Destroy (gameObject);
				}
		}
	
		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				audioSource = gameObject.GetComponent<AudioSource> ();
		}

		public void Mute ()
		{
				audioSource.mute = true;

		}

		public void Unmute ()
		{
				
				audioSource.mute = false;
		
		}

		public bool isMute ()
		{
				return audioSource.mute;
		
		}

		/*void OnGUI ()
		{
				GUI.skin = skin;
				const int buttonWidth = 50;
				const int buttonHeight = 50;
				float buttonX = 7.5f * Screen.width / 10;
				float buttonY = (9 * Screen.height / 10);
				
				bool startButton = false;
				if (isMute ()) {
						startButton = GUI.Button (new Rect (buttonX, buttonY, buttonWidth, buttonHeight), "");
				} else {
						startButton = GUI.Button (new Rect (buttonX, buttonY, buttonWidth, buttonHeight), "", GUI.skin.GetStyle ("Sound On"));
				}
				

				if (startButton) {
						if (isMute ()) {
								Unmute ();
						} else {
								Mute ();
						}

				}
		} */
}

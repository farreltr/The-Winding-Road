using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour
{

		public float positionX = 0.0f;
		public float positionY = 0.0f;
		public string restart = "Restart";
		public const int buttonWidth = 150;
		public const int buttonHeight = 50;	
		public GUISkin skin;
		GameController controller;
		PlayerManager manager;

		void Start ()
		{
				controller = GameObject.FindObjectOfType<GameController> ();
				manager = GameObject.FindObjectOfType<PlayerManager> ();
				if (controller != null) {
						GUIText texture = GameObject.FindObjectOfType<GUIText> ();
						texture.text = controller.gameOverText;
				}
		}

		void OnGUI ()
		{
				GUI.skin = skin;
				positionX = isRight () ? 2.5f * Screen.width / 4 : 1.2f * Screen.width / 4;
				positionY = 3 * Screen.height / 4;
//				positionX = 400.0f;
//				positionY = 40.0f;
				bool restartButton = GUI.Button (new Rect (positionX, positionY, buttonWidth, buttonHeight), restart);			
				if (restartButton) {
						CleanUpObjects ();
						manager.showCharSelect = true;
						manager.Reset ();
						manager.IncrementWins (GetColour ());
						Application.LoadLevel ("Character Select");
				}
	
		}

		void CleanUpObjects ()
		{
				foreach (Transform child in controller.transform) {
						GameObject.Destroy (child.gameObject);
				}
				Destroy (controller.gameObject);
		}
	
		private bool isRight ()
		{
				string sceneName = Application.loadedLevelName;
				return sceneName == "green_win_screen" || sceneName == "red_win_screen";

		}

		private string GetColour ()
		{
				string sceneName = Application.loadedLevelName;
				return sceneName.Replace ("_win_screen", "");
		
		}
}

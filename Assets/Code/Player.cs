using UnityEngine;
using System.Collections;
using System;

public class Player : IComparable
{

		private string playerName;
		private Colour playerColour;
		private int playerTurn;
		public bool isCPU = false;
		
		public Player (string playerName, Colour playerColour, int playerTurn)
		{
				this.playerName = playerName;
				this.playerColour = playerColour;
				this.playerTurn = playerTurn;
				if (playerName.Equals ("CPU")) {
						isCPU = true;
				}
		}

		public string getPlayerName ()
		{
				return playerName;
		}

		public Colour getPlayerColour ()
		{
				return playerColour;
		}

		public int getPlayerTurn ()
		{
				return  playerTurn;
		}

		public void setPlayerName (string playerName)
		{
				this.playerName = playerName;
		}

		public void setPlayerColour (Colour playerColour)
		{
				this.playerColour = playerColour;
		}

		public void setPlayerTurn (int playerTurn)
		{
				this.playerTurn = playerTurn;
		}

		public bool isPLayerCPU ()
		{
				return isCPU;
		}

		public enum Colour
		{
				RED,
				BLUE,
				GREEN,
				YELLOW
		}

		public int CompareTo (object obj)
		{
				Player orderToCompare = obj as Player;
				if (orderToCompare.getPlayerTurn () < playerTurn) {
						return 1;
				}
				if (orderToCompare.getPlayerTurn () > playerTurn) {
						return -1;
				}
				return 0;
		}
}

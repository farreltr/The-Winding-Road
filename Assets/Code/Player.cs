using UnityEngine;
using System.Collections;
using System;

public class Player : IComparable
{
	
		private Colour playerColour;
		private int playerTurn;
		public bool isCPU = false;

		public Player (Colour playerColour, int playerTurn, bool isCPU)
		{
				this.playerColour = playerColour;
				this.playerTurn = playerTurn;
				this.isCPU = isCPU;
		}
	
		public Colour getPlayerColour ()
		{
				return playerColour;
		}

		public int getPlayerTurn ()
		{
				return  playerTurn;
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

		public static Colour[] GetColours ()
		{
				Colour[] colours = {Colour.RED, Colour.BLUE, Colour.YELLOW, Colour.GREEN};
				return colours;
		}

		public static Colour GetPlayerColour (string colour)
		{
				switch (colour) {
				case "red":
						return Colour.RED;
				case "blue":
						return Colour.BLUE;
				case "green":
						return Colour.GREEN;
				case "yellow":
						return Colour.YELLOW;
				}
				return Colour.RED;
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
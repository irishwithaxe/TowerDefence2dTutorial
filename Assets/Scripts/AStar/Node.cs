using UnityEngine;
using System.Collections;
using System;

public class Node {

	public Point GridPosition { get; private set; }

	public TileScript TileRef { get; private set; }

	/// <summary>
	/// Стоимость движения от стартовой позиции (сквозь родителей)
	/// </summary>
	public int G { get; set; }

	/// <summary>
	/// Расстояние до цели
	/// </summary>
	public int H { get; set; }

	/// <summary>
	/// Сумма расстояния до цели и стоимости движения от стартовой позиции
	/// </summary>
	public int F { get; set; }

	public Node Parent { get; private set; }

	public Node(TileScript tileRef) {
		TileRef = tileRef;
		GridPosition = TileRef.GridPosition;
	}

	public void EmptyValues() {
		Parent = null;
		G = 0;
		H = 0;
		F = 0;
	}

	public void CalcValues(Node paren, Node goal, int gCost) {
		Parent = paren;

		G = gCost + Parent.G;
		H = (Math.Abs(GridPosition.X - goal.GridPosition.X) + Math.Abs(GridPosition.Y - goal.GridPosition.Y)) * 10;
		F = G + H;
	}
}

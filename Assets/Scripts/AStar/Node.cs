using UnityEngine;
using System.Collections;

public class Node {

	public Point GridPosition { get; private set; }

	public TileScript TileRef { get; private set; }

	public Node Parent { get; private set; }

	public Node(TileScript tileRef) {
		TileRef = tileRef;
		GridPosition = TileRef.GridPosition;
	}

	public void CalcValues (Node paren, int gScore) {
		Parent = paren;
	}
}

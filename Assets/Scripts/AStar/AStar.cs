using System;
using System.Collections.Generic;
using System.Linq;

public static class AStar {
	private static Node[,] nodes;
	private static int colsCnt;
	private static int rowsCnt;

	private static void CreateNodes() {
		colsCnt = LevelManager.Instance.Tiles.GetLength(0);
		rowsCnt = LevelManager.Instance.Tiles.GetLength(1);

		nodes = new Node[colsCnt, rowsCnt];

		for (int r = 0; r < rowsCnt; r++)
			for (int c = 0; c < colsCnt; c++) {
				nodes[c, r] = new Node(LevelManager.Instance.Tiles[c, r]);
			}
	}

	public static event ChangeLists OnChangeLists = (ol, cl, p) => { };

	public delegate void ChangeLists(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> finalPath);

	public static Stack<Node> GetPath(Point start, Point goal) {
		if (nodes == null)
			CreateNodes();

		var finalPath = new Stack<Node>();
		var openList = new HashSet<Node>();
		var closedList = new HashSet<Node>();

		var startNode = nodes[start.X, start.Y];
		var goalNode = nodes[goal.X, goal.Y];

		var currentNode = startNode;
		currentNode.EmptyValues();
		goalNode.EmptyValues();

		openList.Add(currentNode);

		while (openList.Any()) {
			for (int xdelta = -1; xdelta <= 1; xdelta++)
				for (int ydelta = -1; ydelta <= 1; ydelta++) {
					if (xdelta == 0 && ydelta == 0)
						continue;

					int newx = currentNode.GridPosition.X + xdelta;
					int newy = currentNode.GridPosition.Y + ydelta;

					if (newx < 0 || newy < 0 || newx >= colsCnt || newy >= rowsCnt)
						continue;

					if (!LevelManager.Instance.Tiles[newx, newy].Walkable)
						continue;

					var neighbor = nodes[newx, newy];

					var gCost = 0;
					if (xdelta == 0 || ydelta == 0)
						gCost = 10;
					else if (!IsConnectedDiagonally(neighbor, currentNode))
						continue;
					else
						gCost = 14;

					if (openList.Contains(neighbor)) {
						if (neighbor.Parent.G > currentNode.G)
							neighbor.CalcValues(currentNode, goalNode, gCost);
					}
					else if (!closedList.Contains(neighbor)) {
						openList.Add(neighbor);
						neighbor.CalcValues(currentNode, goalNode, gCost);
					}
				}

			openList.Remove(currentNode);
			closedList.Add(currentNode);

			if (openList.Any())
				currentNode = openList.BestBy((x1, x2) => x1.F < x2.F);

			if (currentNode == goalNode)
				break;
		}

		if (currentNode == goalNode)
			while (currentNode != null && currentNode != startNode) {
				finalPath.Push(currentNode);
				currentNode = currentNode.Parent;
			}

		OnChangeLists(openList, closedList, finalPath);

		return finalPath;
	}

	private static bool IsConnectedDiagonally(Node node1, Node node2) {
		if (!LevelManager.Instance.Tiles[node1.GridPosition.X, node2.GridPosition.Y].Walkable)
			return false;
		if (!LevelManager.Instance.Tiles[node2.GridPosition.X, node1.GridPosition.Y].Walkable)
			return false;

		return true;
	}

	public static T BestBy<T>(this HashSet<T> hashSet, Func<T, T, bool> FirstIsBest) {
		T best = default(T);

		if (hashSet == null || hashSet.Count == 0)
			return best;

		best = hashSet.FirstOrDefault();
		foreach (var elem in hashSet)
			if (FirstIsBest(elem, best))
				best = elem;

		return best;
	}
}
using UnityEngine;
using System.Collections;

public struct Point {
	public int X { get; set; }
	public int Y { get; set; }

	public Point(int x, int y) {
		X = x;
		Y = y;
	}

	public override string ToString() {
		return X + ", " + Y;
	}

	public override bool Equals(object obj) {
		return base.Equals(obj);
	}

	public override int GetHashCode() {
		return (X + Y).GetHashCode();
	}

	public static Point operator -(Point p1, Point p2) {
		var x = p1.X - p2.X;
		var y = p1.Y - p2.Y;
		return new Point(x, y);
	}

	public static bool operator ==(Point p1, Point p2) {
		return p1.X == p2.X && p1.Y == p2.Y;
	}

	public static bool operator !=(Point p1, Point p2) {
		return p1.X != p2.X || p1.Y != p2.Y;
	}
}

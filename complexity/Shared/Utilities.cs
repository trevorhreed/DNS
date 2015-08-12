
namespace ReducingComplexity.Shared
{
	public interface IAI
	{
		Point GetMove();
	}

	public enum Piece
	{
		Player1,
		Player2,
		Cat,
		None
	}

	public static class PieceExtensions
	{
		public static string GetLabel(this Piece me)
		{
			switch (me)
			{
				case Piece.Player1:
					return "X";
				case Piece.Player2:
					return "O";
				case Piece.Cat:
					return "C";
				default:
					return " ";
			}
		}
	}

	public class Point
	{
		public int x;
		public int y;

		public Point() { }
		public Point(int y, int x)
		{
			this.y = y;
			this.x = x;
		}

		public override string ToString()
		{
			return "{ " + x + ", " + y + " }";
		}
	}

	class Constants
	{
		public static Point[][] Lines = new Point[8][] {
			new Point[3] { new Point(0, 0), new Point(0, 1), new Point(0, 2) },
			new Point[3] { new Point(1, 0), new Point(1, 1), new Point(1, 2) },
			new Point[3] { new Point(2, 0), new Point(2, 1), new Point(2, 2) },
			new Point[3] { new Point(0, 0), new Point(1, 0), new Point(2, 0) },
			new Point[3] { new Point(0, 1), new Point(1, 1), new Point(2, 1) },
			new Point[3] { new Point(0, 2), new Point(1, 2), new Point(2, 2) },
			new Point[3] { new Point(0, 0), new Point(1, 1), new Point(2, 2) },
			new Point[3] { new Point(0, 2), new Point(1, 1), new Point(2, 0) }
		};
	}
}

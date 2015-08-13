using ReducingComplexity.Shared;

namespace ReducingComplexity.Simple.Interfaces
{
	public interface IView
	{
		Point Cursor { get; }
		string Message { get; set; }
		void MoveCursor(int y, int x);
	}

	public interface IGame
	{
		bool GameOver { get; }
		bool NoEmptySquares { get; }
		Piece Winner { get; }
		Piece[,] Squares { get; }
		void Reset();
		void TakeTurn(Point point);
	}
}

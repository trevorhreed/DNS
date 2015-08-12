using ReducingComplexity.Shared;

namespace ReducingComplexity.Simple.Interfaces
{
	public interface IView
	{
		Point GetCursor();
		string Message { get; set; }
		void MoveCursor(int y, int x);
	}

	public interface IGame
	{
		bool IsOver { get; }
		bool NoEmptySquares { get; }
		void Reset();
		Piece[,] Squares { get; }
		void TakeTurn(Point point);
		Piece Turn { get; }
		Piece Winner { get; }
		event GameResetEventHandler GameReset;
		event GameTurnTakenEventHandler TurnTaken;
		event GameEndedEventHandler GameEnded;
	}

	public delegate void GameResetEventHandler();
	public delegate void GameTurnTakenEventHandler(Piece nextTurn);
	public delegate void GameEndedEventHandler(Piece winner);
}

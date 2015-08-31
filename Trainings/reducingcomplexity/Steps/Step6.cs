using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Steps
{
	// Miscellaneous

	public class Step6Controller : IController
	{
		private IComputerAI _ai;
		private IStep6View _view;
		private IStep6Game _game;

		public void Run(IComputerAI ai)
		{
			setup(ai);
			ConsoleKey key;
			do
			{
				key = onKeyPress();
			} while (key != ConsoleKey.Escape);
		}

		private void setup(IComputerAI ai)
		{
			_ai = ai;
			_game = new Step6Game();
			_view = new Step6View(_game);
			_game.Reset();
		}
		private ConsoleKey onKeyPress()
		{
			ConsoleKey key = Console.ReadKey(true).Key;
			switch (key)
			{
				case ConsoleKey.UpArrow:
					_view.MoveCursor(0, -1);
					break;
				case ConsoleKey.DownArrow:
					_view.MoveCursor(0, 1);
					break;
				case ConsoleKey.LeftArrow:
					_view.MoveCursor(-1, 0);
					break;
				case ConsoleKey.RightArrow:
					_view.MoveCursor(1, 0);
					break;
				case ConsoleKey.Enter:
					takeTurns();
					break;
				case ConsoleKey.F3:
					_game.Reset();
					break;
				default:
					break;
			}
			return key;
		}
		private void takeTurns()
		{
			try
			{
				_game.TakeTurn(_view.Cursor);
				if (!_game.IsOver)
				{
					_game.TakeTurn(_ai.GetMove(_game.Squares, Piece.Player2));
				}
			}
			catch (Exception e)
			{
				_view.Message = e.Message;
			}
		}
	}

	public class Step6Game : IStep6Game
	{
		public event GameResetEventHandler GameReset;
		public event TurnTakenEventHandler TurnTaken;

		private Piece _turn;
		private Piece[,] _squares;
		public Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }
		public Piece Winner
		{
			get
			{
				foreach (Point[] line in Constants.Lines)
				{
					if (isWinningLineForPlayer(line, Piece.Player1)) return Piece.Player1;
					if (isWinningLineForPlayer(line, Piece.Player2)) return Piece.Player2;
				}
				return NoEmptySquares ? Piece.Cat : Piece.Empty;
			}
		}
		public bool NoEmptySquares
		{
			get
			{
				foreach (Piece square in _squares)
				{
					if (square == Piece.Empty)
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool IsOver { get { return Winner != Piece.Empty || NoEmptySquares; } }

		public void Reset()
		{
			this._turn = Piece.Player1;
			this._squares = new Piece[3, 3] {
				{ Piece.Empty, Piece.Empty, Piece.Empty },
				{ Piece.Empty, Piece.Empty, Piece.Empty },
				{ Piece.Empty, Piece.Empty, Piece.Empty }
			};

			if (GameReset != null) GameReset();
		}
		public void TakeTurn(Point point)
		{
			validateMove(point);

			this._squares[point.y, point.x] = _turn;
			switchTurns();

			if (TurnTaken != null) TurnTaken();
		}
		private void switchTurns()
		{
			_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
		}
		private void validateMove(Point point)
		{
			if (IsOver) throw new Exception("The game is over! Press F3 to reset the game.");
			if (_squares[point.y, point.x] != Piece.Empty) throw new Exception("That space is already taken. Try an empty space.");
			if (!isPointOnBoard(point)) throw new Exception("Something went horribly wrong.");
		}
		private bool isWinningLineForPlayer(Point[] line, Piece player)
		{
			return
				_squares[line[0].y, line[0].x] == player &&
				_squares[line[1].y, line[1].x] == player &&
				_squares[line[2].y, line[2].x] == player;
		}
		private bool isPointOnBoard(Point point)
		{
			return point.y >= 0 && point.y <= 2 && point.x >= 0 && point.x <= 2;
		}
	}

	public class Step6View : IStep6View
	{
		private static int BOARD_OFFSET_X = 6;
		private static int BOARD_OFFSET_Y = 3;
		private static int PIECE_OFFSET_X = 11;
		private static int PIECE_OFFSET_Y = 5;
		private static int PIECE_SPACING_X = 10;
		private static int PIECE_SPACING_Y = 4;
		private static int MESSAGE_OFFSET_X = BOARD_OFFSET_X + 1;
		private static int MESSAGE_OFFSET_Y = BOARD_OFFSET_Y + 14;

		private IStep6Game _game;
		private Point _cursor = new Point(1, 1);
		public Point Cursor { get { return _cursor; } }
		public String Message
		{
			set
			{
				// We set the MAX_MESSAGE_LENGTH here instead of as a static
				// variable in case the user resizes the window mid-game
				int MAX_MESSAGE_LENGTH = Console.WindowWidth - MESSAGE_OFFSET_X;
				string message = value.Length > MAX_MESSAGE_LENGTH
					? value.Substring(0, MAX_MESSAGE_LENGTH)
					: value.PadRight(MAX_MESSAGE_LENGTH, ' ');

				Console.SetCursorPosition(MESSAGE_OFFSET_X, MESSAGE_OFFSET_Y);
				Console.Write(message);
			}
		}
		#region BOARD SETUP
		private string _gameScreen =
			new string('\n', 3) +
			new string(' ', 6) +
			string.Join(
				"\n" + new string(' ', 6),
				new string[] {
							"┌─────────┬─────────┬─────────┐ ",
							"│         │         │         │ Complex Tic Tac Toe Code",
							"│    X    │    X    │    X    │ ",
							"│         │         │         │ Instructions",
							"├─────────┼─────────┼─────────┤ -----------------------------",
							"│         │         │         │ Escape to exit",
							"│    X    │    X    │    X    │ F3 to reset the game",
							"│         │         │         │ Arrow keys to move the curosr",
							"├─────────┼─────────┼─────────┤ Enter to place your piece",
							"│         │         │         │ ",
							"│    X    │    X    │    X    │ ",
							"│         │         │         │ ",
							"└─────────┴─────────┴─────────┘ "
						}
			);
		#endregion

		public Step6View(IStep6Game game)
		{
			this._game = game;
			_game.GameReset += Reset;
			_game.TurnTaken += UpdateBoard;
			Console.CursorVisible = false;
		}

		public void Reset()
		{
			Console.Clear();
			printBoard();
			printPieces();
			printCursor();
			Message = "Ready.";
		}

		public void MoveCursor(int x, int y)
		{
			printCursor(false);
			_cursor.y += y;
			_cursor.x += x;
			if (this._cursor.y < 0) this._cursor.y = 2;
			if (this._cursor.y > 2) this._cursor.y = 0;
			if (this._cursor.x < 0) this._cursor.x = 2;
			if (this._cursor.x > 2) this._cursor.x = 0;
			printCursor();
		}

		public void UpdateBoard()
		{
			printPieces();
			if (_game.IsOver)
			{
				Message = _game.Winner == Piece.Cat
					? "Cat's game!"
					: _game.Winner.GetLabel() + " is the winner!";
			}
		}

		private void printPieces()
		{
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					Point boardPoint = getBoardPointFromGamePoint(x, y);
					Console.SetCursorPosition(boardPoint.x, boardPoint.y);
					Console.Write(_game.Squares[y, x].GetLabel());
				}
			}
		}

		private Point getBoardPointFromGamePoint(int x, int y)
		{
			return new Point(
				(y * PIECE_SPACING_Y) + PIECE_OFFSET_Y,
				(x * PIECE_SPACING_X) + PIECE_OFFSET_X
			);
		}

		private void printCursor(bool show = true)
		{
			char opening = show ? '(' : ' ';
			char closing = show ? ')' : ' ';
			Point boardPoint = getBoardPointFromGamePoint(_cursor.x, _cursor.y);
			Console.SetCursorPosition(boardPoint.x - 2, boardPoint.y);
			Console.Write(opening);
			Console.SetCursorPosition(boardPoint.x + 2, boardPoint.y);
			Console.Write(closing);
		}

		private void printBoard()
		{
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(this._gameScreen);
		}
	}

	public delegate void GameResetEventHandler();
	public delegate void TurnTakenEventHandler();

	public interface IStep6Game
	{
		bool NoEmptySquares { get; }
		bool IsOver { get; }
		void Reset();
		Piece[,] Squares { get; }
		void TakeTurn(Point point);
		Piece Winner { get; }

		event GameResetEventHandler GameReset;
		event TurnTakenEventHandler TurnTaken;
	}

	public interface IStep6View
	{
		Point Cursor { get; }
		string Message { set; }
		void MoveCursor(int x, int y);
		void Reset();
		void UpdateBoard();
	}
}

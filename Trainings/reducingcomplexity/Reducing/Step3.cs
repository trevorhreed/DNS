using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Reducing
{
	// Factor out discrete domain logic and lower levels of abstraction into separate methods/modules

	public class Step3Controller : IController
	{
		private IComputerAI _ai;
		private IStep3View _view;
		private IStep3Game _game;
		
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
			_game = new Step3Game();
			_view = new Step3View(_game);
			_game.Reset();
			_view.Reset();
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
					_view.Reset();
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
				_view.UpdateBoard();
				if (!(_game.Winner != Piece.None || _game.NoEmptySquares)) // #001, #003:																																																																																																															_gameOver (bool property)
				{
					_game.TakeTurn(_ai.GetMove(_game.Squares, Piece.Player2));
					_view.UpdateBoard();
				}
			}
			catch (Exception e)
			{
				_view.Message = e.Message;
			}
		}
	}

	public class Step3Game : IStep3Game
	{
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
				return NoEmptySquares ? Piece.Cat : Piece.None;
			}
		}
		public bool NoEmptySquares
		{
			get
			{
				bool foundEmptySquare = false;
				foreach (Piece square in _squares)
				{
					if (square == Piece.None)
					{
						foundEmptySquare = true;
					}
				}
				return !foundEmptySquare;
			}
		}

		public void Reset()
		{
			this._turn = Piece.Player1;
			this._squares = new Piece[3, 3] {
				{ Piece.None, Piece.None, Piece.None },
				{ Piece.None, Piece.None, Piece.None },
				{ Piece.None, Piece.None, Piece.None }
			};
		}
		public void TakeTurn(Point point)
		{
			if (!(Winner != Piece.None || NoEmptySquares)) // #001, #003, #004, #005, #013:																																																																																																																	_gameOver (bool property), _validate()
			{
				if (isPointOnBoard(point))
				{
					if (_squares[point.y, point.x] == Piece.None)
					{
						this._squares[point.y, point.x] = _turn;
						_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
					}
					else
					{
						throw new Exception("That space is already taken. Try an empty space.");
					}
				}
				else
				{
					throw new Exception("Something went horribly wrong.");
				}
			}
			else
			{
				throw new Exception("The game is over! Press F3 to reset the game.");
			}
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

	public class Step3View : IStep3View
	{
		private IStep3Game _game;
		private Point _cursor = new Point(1, 1);
		public Point Cursor { get { return _cursor; } }
		public String Message
		{
			set
			{
				string message = value;
				if (message.Length > Console.WindowWidth - 7) message = message.Substring(0, Console.WindowWidth - 7);
				else if (message.Length < Console.WindowWidth - 7) message = message + new string(' ', (Console.WindowWidth - 7) - message.Length);
				Console.SetCursorPosition(7, 17);
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

		public Step3View(IStep3Game game)
		{
			this._game = game;
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
			if (_game.Winner != Piece.None || _game.NoEmptySquares)
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
					int y2 = (y * 4) + 5;
					int x2 = (x * 10) + 11;
					Console.SetCursorPosition(x2, y2);
					Console.Write(_game.Squares[y, x].GetLabel());
				}
			}
		}

		private void printCursor(bool show = true)
		{
			char opening = show ? '(' : ' ';
			char closing = show ? ')' : ' ';
			Console.SetCursorPosition((_cursor.x * 10) + 9, (_cursor.y * 4) + 5);
			Console.Write(opening);
			Console.SetCursorPosition((_cursor.x * 10) + 13, (_cursor.y * 4) + 5);
			Console.Write(closing);
		}

		private void printBoard()
		{
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(this._gameScreen);
		}		
	}

	public interface IStep3Game
	{
		bool NoEmptySquares { get; }
		void Reset();
		global::ReducingComplexity.Shared.Piece[,] Squares { get; }
		void TakeTurn(global::ReducingComplexity.Shared.Point point);
		global::ReducingComplexity.Shared.Piece Winner { get; }
	}

	public interface IStep3View
	{
		ReducingComplexity.Shared.Point Cursor { get; }
		string Message { set; }
		void MoveCursor(int x, int y);
		void Reset();
		void UpdateBoard();
	}
}

using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Steps
{
	// Factor out non-domain logic into separate methods/modules

	public class Step2Controller : IController
	{
		private Step2View _view;
		private Piece _turn;
		private Piece[,] _squares;
		private Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }
		private Piece Winner
		{
			get
			{
				foreach (Point[] line in Constants.Lines)
				{
					if (IsWinningLineForPlayer(line, Piece.Player1)) return Piece.Player1;
					if (IsWinningLineForPlayer(line, Piece.Player2)) return Piece.Player2;
				}
				return NoEmptySquares ? Piece.Cat : Piece.Empty;
			}
		}
		private bool NoEmptySquares
		{
			get
			{
				bool foundEmptySquare = false;
				foreach (Piece square in _squares)
				{
					if (square == Piece.Empty)
					{
						foundEmptySquare = true;
					}
				}
				return !foundEmptySquare;
			}
		}

		public void Run(IComputerAI ai)
		{
			_view = new Step2View();
			reset();
			_view.Reset(Squares);
			bool running = true;
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				switch (keyInfo.Key)
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
						try
						{
							takeTurn(_view.Cursor);

							if (Winner != Piece.Empty || NoEmptySquares)
							{
								_view.PrintWinner(Winner);
							}
							else
							{
								takeTurn(ai.GetMove(Squares, Piece.Player2));

								if (Winner != Piece.Empty || NoEmptySquares)
								{
									_view.PrintWinner(Winner);
								}
							}
						}
						catch (Exception e)
						{
							_view.PrintMessage(e.Message);
						}
						break;
					case ConsoleKey.F3:
						reset();
						break;
					case ConsoleKey.Escape:
						running = false;
						break;
					default:
						break;
				}
			} while (running);
		}
		private void reset()
		{	// CHANGE: factor out code into reset()
			this._turn = Piece.Player1;
			this._squares = new Piece[3, 3] {
					{ Piece.Empty, Piece.Empty, Piece.Empty },
					{ Piece.Empty, Piece.Empty, Piece.Empty },
					{ Piece.Empty, Piece.Empty, Piece.Empty }
				};

			_view.Reset(Squares);
		}
		private void takeTurn(Point point)
		{
			if (!(Winner != Piece.Empty || NoEmptySquares))
			{
				if (point.y >= 0 && point.y <= 2 && point.x >= 0 && point.x <= 2)
				{
					if (_squares[point.y, point.x] == Piece.Empty)
					{
						this._squares[point.y, point.x] = _turn;
						_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
						_view.PrintPieces(Squares);
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
		private bool IsWinningLineForPlayer(Point[] line, Piece player)
		{
			return
				_squares[line[0].y, line[0].x] == player &&
				_squares[line[1].y, line[1].x] == player &&
				_squares[line[2].y, line[2].x] == player;
		}
	}

	public class Step2View
	{
		private Point _cursor = new Point(1, 1);
		public Point Cursor { get { return _cursor; } }
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

		public Step2View()
		{
			Console.CursorVisible = false;
		}

		public void Reset(Piece[,] squares)
		{
			Console.Clear();
			printBoard();
			PrintPieces(squares);
			printCursor();
			PrintMessage("Ready.");
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

		public void PrintMessage(string message)
		{
			if (message.Length > Console.WindowWidth - 7) message = message.Substring(0, Console.WindowWidth - 7);
			else if (message.Length < Console.WindowWidth - 7) message = message + new string(' ', (Console.WindowWidth - 7) - message.Length);
			Console.SetCursorPosition(7, 17);
			Console.Write(message);
		}

		public void PrintWinner(Piece winner)
		{
			if (winner == Piece.Cat)
			{
				PrintMessage("Cat's game!");
			}
			else
			{
				PrintMessage(winner.GetLabel() + " is the winner!");
			}
		}

		public void PrintPieces(Piece[,] squares)
		{	// CHANGE: factor out code into printPieces
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int y2 = (y * 4) + 5;
					int x2 = (x * 10) + 11;
					Console.SetCursorPosition(x2, y2);
					Console.Write(squares[y, x].GetLabel());
				}
			}
		}

		private void printCursor(bool show = true)
		{	// CHANGE: consolidate printCursor() and clearCursor()
			char opening = show ? '(' : ' ';
			char closing = show ? ')' : ' ';
			Console.SetCursorPosition((_cursor.x * 10) + 9, (_cursor.y * 4) + 5);
			Console.Write(opening);
			Console.SetCursorPosition((_cursor.x * 10) + 13, (_cursor.y * 4) + 5);
			Console.Write(closing);
		}

		private void printBoard()
		{	// CHANGE: factor out code into printBoard
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(this._gameScreen);
		}		
	}
}

using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Steps
{
	// Factor out duplicate code into separete methods

	public class Step1Controller : IController
	{
		private Piece _turn;
		private Piece[,] _squares;
		private Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }
		private Point _cursor = new Point(1, 1);
		private Piece Winner
		{
			get
			{
				foreach (Point[] line in Constants.Lines)
				{
					if (IsWinningLineForPlayer(line, Piece.Player1)) return Piece.Player1;
					if (IsWinningLineForPlayer(line, Piece.Player2)) return Piece.Player2;
				}
				return NoEmptySquares ? Piece.Cat : Piece.None;
			}
		}
		private bool NoEmptySquares
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
		private bool IsWinningLineForPlayer(Point[] line, Piece player)
		{
			return
				_squares[line[0].y, line[0].x] == player &&
				_squares[line[1].y, line[1].x] == player &&
				_squares[line[2].y, line[2].x] == player;
		}

		public void Run(IComputerAI ai)
		{
			Console.CursorVisible = false;
			reset();
			bool running = true;
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				switch (keyInfo.Key)
				{
					case ConsoleKey.UpArrow:
						moveCursor(0, -1);
						break;
					case ConsoleKey.DownArrow:
						moveCursor(0, 1);
						break;
					case ConsoleKey.LeftArrow:
						moveCursor(-1, 0);
						break;
					case ConsoleKey.RightArrow:
						moveCursor(1, 0);
						break;
					case ConsoleKey.Enter:
						try
						{
							takeTurn(_cursor);
							if (Winner != Piece.None || NoEmptySquares) // #001, #003:																																																																																																															_gameOver (bool property)
							{
								printWinner();
							}
							else
							{
								takeTurn(ai.GetMove(Squares, Piece.Player2));
								if (Winner != Piece.None || NoEmptySquares) // #001, #003:																																																																																																																					_gameOver (bool property)
								{
									printWinner();
								}
							}
						}
						catch (Exception e)
						{
							printMessage(e.Message);
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

		private void takeTurn(Point point)
		{
			if (!(Winner != Piece.None || NoEmptySquares)) // #001, #003, #004, #005, #013:																																																																																																																	_gameOver (bool property), _validate()
			{
				if (point.y >= 0 && point.y <= 2 && point.x >= 0 && point.x <= 2)
				{
					if (_squares[point.y, point.x] == Piece.None)
					{
						this._squares[point.y, point.x] = _turn;
						_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
						printPieces();
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
		private void reset()
		{	// CHANGE: factor out code into reset()
			this._turn = Piece.Player1;
			this._squares = new Piece[3, 3] {
					{ Piece.None, Piece.None, Piece.None },
					{ Piece.None, Piece.None, Piece.None },
					{ Piece.None, Piece.None, Piece.None }
				};

			Console.Clear();
			printBoard();
			printPieces();
			printCursor();
			printMessage("Ready.");
		}
		private void moveCursor(int x, int y)
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
		private void printPieces()
		{	// CHANGE: factor out code into printPieces
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int y2 = (y * 4) + 5;
					int x2 = (x * 10) + 11;
					Console.SetCursorPosition(x2, y2);
					Console.Write(Squares[y, x].GetLabel());
				}
			}
		}
		private void printMessage(string message)
		{
			if (message.Length > Console.WindowWidth - 7) message = message.Substring(0, Console.WindowWidth - 7);
			else if (message.Length < Console.WindowWidth - 7) message = message + new string(' ', (Console.WindowWidth - 7) - message.Length);
			Console.SetCursorPosition(7, 17);
			Console.Write(message);
		}
		private void printWinner()
		{
			if (Winner == Piece.Cat)
			{
				printMessage("Cat's game!");
			}
			else
			{
				printMessage(Winner.GetLabel() + " is the winner!");
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
	}
}

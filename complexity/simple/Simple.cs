using ReducingComplexity.Shared;
using ReducingComplexity.Simple.Interfaces;
using System;
using System.Collections.Generic;

namespace ReducingComplexity.Simple
{
	public class SimpleTicTacToeController : IController
	{
		private IGame game;
		private IView view;
		private IComputerAI ai;
		
		public void Run(IComputerAI computerAI)
		{
			ai = computerAI;
			game = new SimpleTicTacToeGame();
			view = new SimpleTicTacToeView(game);
			game.Reset();

			bool continueRunning = true;
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				continueRunning = OnKeyPress(keyInfo);
			} while (continueRunning);
		}

		public bool OnKeyPress(ConsoleKeyInfo keyInfo)
		{
			switch (keyInfo.Key)
			{
				case ConsoleKey.UpArrow:
					view.MoveCursor(-1, 0);
					break;
				case ConsoleKey.DownArrow:
					view.MoveCursor(1, 0);
					break;
				case ConsoleKey.LeftArrow:
					view.MoveCursor(0, -1);
					break;
				case ConsoleKey.RightArrow:
					view.MoveCursor(0, 1);
					break;
				case ConsoleKey.Enter:
					try
					{
						game.TakeTurn(view.GetCursor());
						if (!game.IsOver)
						{
							game.TakeTurn(ai.GetMove(game.Squares, Piece.Player2));
						}
					}
					catch (Exception e)
					{
						view.Message = e.Message;
					}
					break;
				case ConsoleKey.F3:
					game.Reset();
					break;
				case ConsoleKey.Escape:
					return false;
				default:
					break;
			}
			return true;
		}
	}

	public class SimpleTicTacToeGame : IGame
	{
		public event GameResetEventHandler GameReset;
		public event GameTurnTakenEventHandler TurnTaken;
		public event GameEndedEventHandler GameEnded;

		private Piece _turn;
		public Piece Turn { get { return _turn; } }

		private Piece[,] _squares;
		public Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }

		public bool IsOver
		{
			get { return Winner != Piece.None || NoEmptySquares; }
		}
		public bool NoEmptySquares
		{
			get
			{
				bool noEmptySquares = true;
				foreach (Piece square in _squares)
				{
					if (square == Piece.None)
					{
						noEmptySquares = false;
						break;
					}
				}
				return noEmptySquares;
			}
		}
		public Piece Winner
		{
			get
			{
				for (var i = 0; i < 8; i++)
				{
					if (
							_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player1 &&
							_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player1 &&
							_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player1
					)
					{
						return Piece.Player1;
					}
					if (
							_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player2 &&
							_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player2 &&
							_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player2
					)
					{
						return Piece.Player2;
					}
				}
				return NoEmptySquares ? Piece.Cat : Piece.None;
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

			if (GameReset != null) GameReset();
		}

		public void TakeTurn(Point point)
		{
			_validateMove(point);

			this._squares[point.y, point.x] = _turn;
			_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;

			if (TurnTaken != null)
			{
				TurnTaken(_turn);
			}
			if (IsOver && GameEnded != null)
			{
				GameEnded(Winner);
			}
		}

		private void _validateMove(Point point)
		{
			if (this.IsOver)
			{
				throw new Exception("The game is over! Press F3 to reset the game.");
			}
			if (this._squares[point.y, point.x] != Piece.None)
			{
				throw new Exception("That space is already taken. Try an empty space.");
			}
			if (point.y < 0 || point.y > 2 || point.x < 0 || point.x > 2)
			{
				throw new Exception("Something went horribly wrong.");
			}
		}
	}

	public class SimpleTicTacToeView : IView
	{
		private static int BOARD_Y_OFFSET = 3;
		private static int BOARD_X_OFFSET = 6;
		private static int MESSAGE_Y_OFFSET = BOARD_Y_OFFSET + 14;
		private static int MESSAGE_X_OFFSET = BOARD_X_OFFSET + 1;

		private IGame _game;
		private string _message = "Ready.";
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
				int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
				Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
				string formattedMessage = _message ?? "";
				if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
				else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
				Console.Write(formattedMessage);
			}
		}
		private Point _cursor = new Point(1, 1);

		#region BOARD SETUP
		private string gameScreen = 
			new string('\n', BOARD_Y_OFFSET) + 
			new string(' ', BOARD_X_OFFSET) +
			string.Join(
				"\n" + new string(' ', BOARD_X_OFFSET), 
				new string[] {
					"┌─────────┬─────────┬─────────┐ ",
					"│         │         │         │ Simple Tic Tac Toe Code",
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
		private Point[,] _piecePoints = new Point[,] {
			{ 
				new Point(BOARD_Y_OFFSET + 2,  BOARD_X_OFFSET + 5),  
				new Point(BOARD_Y_OFFSET + 2,  BOARD_X_OFFSET + 15),  
				new Point(BOARD_Y_OFFSET + 2,  BOARD_X_OFFSET + 25)  
			},
			{ 
				new Point(BOARD_Y_OFFSET + 6,  BOARD_X_OFFSET + 5), 
				new Point(BOARD_Y_OFFSET + 6,  BOARD_X_OFFSET + 15), 
				new Point(BOARD_Y_OFFSET + 6,  BOARD_X_OFFSET + 25)
			},
			{
				new Point(BOARD_Y_OFFSET + 10, BOARD_X_OFFSET + 5), 
				new Point(BOARD_Y_OFFSET + 10, BOARD_X_OFFSET + 15), 
				new Point(BOARD_Y_OFFSET + 10, BOARD_X_OFFSET + 25)
			},
		};
		#endregion

		public SimpleTicTacToeView(IGame game)
		{
			this._game = game;
			_game.GameReset += OnGameReset;
			_game.TurnTaken += OnTurnTaken;
			_game.GameEnded += OnGameEnded;
			Console.CursorVisible = false;
		}

		private void OnGameReset()
		{
			Console.Clear();
			_printBoard();
			_printPieces();
			_printCursor();
			Message = "Ready.";
		}
		
		private void OnTurnTaken(Piece nextTurn)
		{
			_printPieces();
		}
		
		private void OnGameEnded(Piece winner)
		{
			if (winner == Piece.Cat)
			{
				Message = "Cat's game!";
			}
			else
			{
				Message = winner.GetLabel() + " is the winner!";
			}
		}
		
		public Point GetCursor()
		{
			return this._cursor;
		}

		public void MoveCursor(int y, int x)
		{
			this._cursor.y += y;
			this._cursor.x += x;
			if (this._cursor.y < 0) this._cursor.y = 2;
			if (this._cursor.y > 2) this._cursor.y = 0;
			if (this._cursor.x < 0) this._cursor.x = 2;
			if (this._cursor.x > 2) this._cursor.x = 0;
			_printCursor();
		}

		private void _printBoard()
		{
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(this.gameScreen);
		}

		private void _printPieces()
		{
			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					Point p = _piecePoints[y, x];
					Console.SetCursorPosition(p.x, p.y);
					Console.Write(_game.Squares[y, x].GetLabel());
				}
			}
		}

		private void _printCursor()
		{
			foreach (Point p in _piecePoints)
			{
				Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
				bool isSelected = (p.y == cursorPoint.y && p.x == cursorPoint.x);
				Console.SetCursorPosition(p.x - 2, p.y);
				Console.Write(isSelected ? "(" : " ");
				Console.SetCursorPosition(p.x + 2, p.y);
				Console.Write(isSelected ? ")" : " ");
			}
		}
	}
}

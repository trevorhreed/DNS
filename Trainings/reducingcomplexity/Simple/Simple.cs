using ReducingComplexity.Shared;
using ReducingComplexity.Simple.Interfaces;
using System;
using System.Collections.Generic;

namespace ReducingComplexity.Simple
{
	public class SimpleController : IController
	{
		private SimpleGame _game;
		private SimpleView _view;
		private IComputerAI _ai;

		public void Run(IComputerAI ai)
		{
			this._ai = ai;
			_game = new SimpleGame();
			_view = new SimpleView(_game);
			ConsoleKey key;
			do
			{
				key = OnKeyPress();
			} while (key != ConsoleKey.Escape);
		}
		private ConsoleKey OnKeyPress()
		{
			ConsoleKey key = Console.ReadKey(true).Key;
			switch (key)
			{
				case ConsoleKey.F3:
					_game.Reset();
					_view.Reset();
					break;
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
						_game.TakeTurn(_view.Cursor);
						_view.UpdateBoard();
						if (!_game.GameOver)
						{
							_game.TakeTurn(_ai.GetMove(_game.Squares, Piece.Player2));
							_view.UpdateBoard();
						}
					}
					catch (Exception e)
					{
						_view.Message = e.Message;
					}
					break;
				default:
					break;
			}
			return key;
		}
	}

	public class SimpleGame : IGame
	{
		private Piece _turn;
		private Piece[,] _squares;
		public Piece[,] Squares { get { return _squares; } }
		public bool NoEmptySquares
		{
			get
			{
				// #001, #003: _noEmptySquares (bool proeprty)
				foreach (Piece square in _squares)
				{
					if (square == Piece.None)
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool GameOver
		{
			get
			{
				return Winner != Piece.None || NoEmptySquares;
			}
		}
		public Piece Winner
		{
			get
			{
				// #001, #003: _winner (Piece property)
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

		public SimpleGame()
		{
			Reset();
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
		public void TakeTurn(Point square)
		{
			_validateMove(square);
			this._squares[square.y, square.x] = _turn;
			_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
		}
		private void _validateMove(Point square)
		{
			if (GameOver) throw new InvalidOperationException("The game is over. Press F3 to restart.");
			if (_squares[square.y, square.x] != Piece.None) throw new InvalidOperationException("That space is already taken. Try an empty space.");
			if (square.y < 0 || square.y > 2 || square.x < 0 || square.x > 2) throw new InvalidOperationException("Something went horribly wrong.");
		}
	}

	public class SimpleView : IView
	{
		private static int BOARD_Y_OFFSET = 3;
		private static int BOARD_X_OFFSET = 6;
		private static int MESSAGE_Y_OFFSET = BOARD_Y_OFFSET + 14;
		private static int MESSAGE_X_OFFSET = BOARD_X_OFFSET + 1;

		private SimpleGame _game;
		private Point _cursor = new Point(1, 1);
		public Point Cursor
		{
			get
			{
				return _cursor;
			}
		}
		private string _message;
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
				string formattedMessage = value;
				if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
				else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
				Console.Write(formattedMessage);
			}
		}

		#region BOARD SETUP
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

		private string _gameScreen =
			new string('\n', BOARD_Y_OFFSET) +
			new string(' ', BOARD_X_OFFSET) +
			string.Join(
				"\n" + new string(' ', BOARD_X_OFFSET),
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

		public SimpleView(SimpleGame game)
		{
			this._game = game;
			Console.CursorVisible = false;
			Reset();
		}

		public void Reset()
		{
			Console.Clear();
			_printBoard();
			_printPieces();
			_printCursor();
			Message = "Ready.";
		}
		public void UpdateBoard()
		{
			_printPieces();
			if (_game.GameOver)
			{
				Message = _game.Winner == Piece.Cat
					? "Cat's game!"
					: _game.Winner.GetLabel() + " is the winner!";
			}
		}
		public void MoveCursor(int x, int y)
		{
			_eraseCursor();
			this._cursor.y += y;
			this._cursor.x += x;
			if (this._cursor.y < 0) this._cursor.y = 2;
			if (this._cursor.y > 2) this._cursor.y = 0;
			if (this._cursor.x < 0) this._cursor.x = 2;
			if (this._cursor.x > 2) this._cursor.x = 0;
			_printCursor();
		}
		private void _eraseCursor()
		{
			Point c = _piecePoints[this._cursor.y, this._cursor.x];
			Console.SetCursorPosition(c.x - 2, c.y);
			Console.Write(" ");
			Console.SetCursorPosition(c.x + 2, c.y);
			Console.Write(" ");
		}
		private void _printCursor()
		{
			Point c = _piecePoints[this._cursor.y, this._cursor.x];
			Console.SetCursorPosition(c.x - 2, c.y);
			Console.Write("(");
			Console.SetCursorPosition(c.x + 2, c.y);
			Console.Write(")");
		}
		private void _printBoard()
		{
			Console.Clear();
			Console.SetCursorPosition(0, 0);
			Console.WriteLine(this._gameScreen);
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
	}
}

using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Complex
{
	/* Ways To Reduce Complexity
	 * 
	 * Factor out duplicate code into separate methods (#001)
	 * Factor out non-domain logic into separate modules (#002)
	 * Factor out discrete domain logic into separate methods/modules (#003)
	 * Refactor methods using the following rules:
	 *		- Validate input, return or throw error on failure (#004)
	 *		- Validate domain data, return or throw error on failure (#005)
	 *		- Execute domain logic (#006)
	 *		- Use comments to explain why, not what (#007)
	 *		- Don't optimize at the expense of readability (#008)
	 *		- Declare variables right before the logic in which they are used (#009)
	 *		- Don't use the same variable for multiple purposes (#010)
	 *		- Don't try to minimize the number of method calls (#011)
	 *		- Allow multiple returns from a method if it improves readability or cyclomatic complexity (#012)
	 *		- Keep the happy path has flat as possible (#013)
	 *		- Separate levels of abstraction (#014)
	 */

	public class ComplexController : IController
	{
		private static int BOARD_Y_OFFSET = 3;
		private static int BOARD_X_OFFSET = 6;
		private static int MESSAGE_Y_OFFSET = BOARD_Y_OFFSET + 14;
		private static int MESSAGE_X_OFFSET = BOARD_X_OFFSET + 1;

		private Piece _turn;
		private Piece[,] _squares;
		private Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }
		private Point _cursor = new Point(1, 1);

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

		public void Run(IComputerAI ai)
		{
			Console.CursorVisible = false;
			{ // #001, #003:																																																																																																																										_resetGame()
				this._turn = Piece.Player1;
				this._squares = new Piece[3, 3] {
					{ Piece.None, Piece.None, Piece.None },
					{ Piece.None, Piece.None, Piece.None },
					{ Piece.None, Piece.None, Piece.None }
				};

				Console.Clear();

				{ // #003:																																																																																																																										 _printBoard()
					Console.SetCursorPosition(0, 0);
					Console.WriteLine(this._gameScreen);
				}

				for (int y = 0; y < 3; y++) // #001:																																																																																																																										 _printPieces()
				{
					for (int x = 0; x < 3; x++)
					{
						Point p = _piecePoints[y, x];
						Console.SetCursorPosition(p.x, p.y);
						Console.Write(Squares[y, x].GetLabel());
					}
				}

				{ // #001:																																																																																																																										 _printCursor()
					Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
					Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
					Console.Write("(");
					Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
					Console.Write(")");
				}

				{ // #001, #002:																																																																																																																										 _printMessage()
					int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
					Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
					string formattedMessage = "Ready.";
					if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
					else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
					Console.Write(formattedMessage);
				}
			}

			bool continueRunning = true;
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				switch (keyInfo.Key)
				{
					case ConsoleKey.UpArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							{ // #001:																																																																																																																										 _clearCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write(" ");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(" ");
							}
							this._cursor.y--;
							if (this._cursor.y < 0) this._cursor.y = 2;
							{ // #001:																																																																																																																										 _printCursor()
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
						break;
					case ConsoleKey.DownArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							{ // #001:																																																																																																																										 _clearCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write(" ");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(" ");
							}
							this._cursor.y++;
							if (this._cursor.y > 2) this._cursor.y = 0;
							{ // #001:																																																																																																																										 _printCursor()
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
						break;
					case ConsoleKey.LeftArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							{ // #001:																																																																																																																										 _clearCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write(" ");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(" ");
							}
							this._cursor.x--;
							if (this._cursor.x < 0) this._cursor.x = 2;
							{ // #001:																																																																																																																										 _printCursor()
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
						break;
					case ConsoleKey.RightArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							{ // #001:																																																																																																																										 _clearCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write(" ");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(" ");
							}
							this._cursor.x++;
							if (this._cursor.x > 2) this._cursor.x = 0;
							{ // #001:																																																																																																																										 _printCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write("(");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(")");
							}
						}
						break;
					case ConsoleKey.Enter:
						try
						{
							{ // #001, #003:																																																																																																																										 _takeTurn()

								bool noEmptySquares = true; // #001, #003:																																																																																																																									_noEmptySquares (bool proeprty)
								foreach (Piece square in _squares)
								{
									if (square == Piece.None)
									{
										noEmptySquares = false;
										break;
									}
								}

								Piece winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
								for (var i = 0; i < 8; i++)
								{
									if (
											_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player1 &&
											_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player1 &&
											_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player1
									)
									{
										winner = Piece.Player1;
									}
									if (
											_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player2 &&
											_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player2 &&
											_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player2
									)
									{
										winner = Piece.Player2;
									}
								}
								winner = noEmptySquares ? Piece.Cat : Piece.None;

								if (!(winner != Piece.None || noEmptySquares)) // #001, #003, #004, #005, #013:																																																																																																																	_gameOver (bool property), _validate()
								{
									if (_cursor.y >= 0 && _cursor.y <= 2 && _cursor.x >= 0 && _cursor.x <= 2)
									{
										if (_squares[_cursor.y, _cursor.x] == Piece.None)
										{
											this._squares[_cursor.y, _cursor.x] = _turn;
											_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;

											for (int y = 0; y < 3; y++) // #001:																																																																																																																										 _printPieces()
											{
												for (int x = 0; x < 3; x++)
												{
													Point p = _piecePoints[y, x];
													Console.SetCursorPosition(p.x, p.y);
													Console.Write(Squares[y, x].GetLabel());
												}
											}

											noEmptySquares = true;	// #001, #003:																																																																																																																										_noEmptySquares (bool proeprty)
											foreach (Piece square in _squares)
											{
												if (square == Piece.None)
												{
													noEmptySquares = false;
													break;
												}
											}

											winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
											for (var i = 0; i < 8; i++)
											{
												if (
														_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player1 &&
														_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player1 &&
														_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player1
												)
												{
													winner = Piece.Player1;
												}
												if (
														_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player2 &&
														_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player2 &&
														_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player2
												)
												{
													winner = Piece.Player2;
												}
											}
											if (winner == Piece.None)
											{
												winner = noEmptySquares ? Piece.Cat : Piece.None;
											}

											if (winner != Piece.None || noEmptySquares) // #001, #003:																																																																																																															_gameOver (bool property)
											{
												string message = "";
												if (winner == Piece.Cat)
												{
													message = "Cat's game!";
												}
												else
												{
													message = winner.GetLabel() + " is the winner!";
												}

												{ // #001, #002:																																																																																																																										 _printMessage()
													int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
													Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
													string formattedMessage = message;
													if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
													else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
													Console.Write(formattedMessage);
												}
											}
											else
											{
												{ // #001, #003:																																																																																																																										 _takeTurn()

													if (!(winner != Piece.None || noEmptySquares)) // #001, #003, #004, #005, #013:																																																																																																																			_gameOver (bool property), _validate(), 
													{
														Point aiMove = ai.GetMove(Squares, Piece.Player2);
														if (aiMove.y >= 0 && aiMove.y <= 2 && aiMove.x >= 0 && aiMove.x <= 2)
														{
															if (_squares[aiMove.y, aiMove.x] == Piece.None)
															{
																this._squares[aiMove.y, aiMove.x] = _turn;
																_turn = _turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;

																for (int y = 0; y < 3; y++) // #001:																																																																																																																										 _printPieces()
																{
																	for (int x = 0; x < 3; x++)
																	{
																		Point p = _piecePoints[y, x];
																		Console.SetCursorPosition(p.x, p.y);
																		Console.Write(Squares[y, x].GetLabel());
																	}
																}

																noEmptySquares = true; // #001, #003:																																																																																																																												_noEmptySquares (bool proeprty)
																foreach (Piece square in _squares)
																{
																	if (square == Piece.None)
																	{
																		noEmptySquares = false;
																		break;
																	}
																}

																winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
																for (var i = 0; i < 8; i++)
																{
																	if (
																			_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player1 &&
																			_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player1 &&
																			_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player1
																	)
																	{
																		winner = Piece.Player1;
																	}
																	if (
																			_squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player2 &&
																			_squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player2 &&
																			_squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player2
																	)
																	{
																		winner = Piece.Player2;
																	}
																}
																if (winner == Piece.None)
																{
																	winner = noEmptySquares ? Piece.Cat : Piece.None;
																}

																if (winner != Piece.None || noEmptySquares) // #001, #003:																																																																																																																					_gameOver (bool property)
																{
																	if (winner == Piece.Cat)
																	{
																		{ // #001, #002:																																																																																																																										 _printMessage()
																			int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
																			Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
																			string formattedMessage = "Cat's game!";
																			if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
																			else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
																			Console.Write(formattedMessage);
																		}
																	}
																	else
																	{
																		{ // #001, #002:																																																																																																																										 _printMessage()
																			int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
																			Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
																			string formattedMessage = winner.GetLabel() + " is the winner!";
																			if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
																			else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
																			Console.Write(formattedMessage);
																		}
																	}
																}
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
											}
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
						}
						catch (Exception e)
						{
							{ // #001, #002:																																																																																																																										_printMessage()
								int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
								Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
								string formattedMessage = e.Message;
								if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
								else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
								Console.Write(formattedMessage);
							}
						}
						break;
					case ConsoleKey.F3:
						{ // #001, #003:																																																																																																																										_resetGame()
							this._turn = Piece.Player1;
							this._squares = new Piece[3, 3] {
								{ Piece.None, Piece.None, Piece.None },
								{ Piece.None, Piece.None, Piece.None },
								{ Piece.None, Piece.None, Piece.None }
							};

							Console.Clear();

							{ // #003:																																																																																																																										 _printBoard()
								Console.SetCursorPosition(0, 0);
								Console.WriteLine(this._gameScreen);
							}

							for (int y = 0; y < 3; y++) // #001:																																																																																																																										 _printPieces()
							{
								for (int x = 0; x < 3; x++)
								{
									Point p = _piecePoints[y, x];
									Console.SetCursorPosition(p.x, p.y);
									Console.Write(Squares[y, x].GetLabel());
								}
							}

							{ // #001:																																																																																																																										 _printCursor()
								Point cursorPoint = _piecePoints[this._cursor.y, this._cursor.x];
								Console.SetCursorPosition(cursorPoint.x - 2, cursorPoint.y);
								Console.Write("(");
								Console.SetCursorPosition(cursorPoint.x + 2, cursorPoint.y);
								Console.Write(")");
							}

							{ // #001, #002:																																																																																																																										_printMessage()
								int WIDTH = Console.WindowWidth - MESSAGE_X_OFFSET;
								Console.SetCursorPosition(MESSAGE_X_OFFSET, MESSAGE_Y_OFFSET);
								string formattedMessage = "Ready.";
								if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
								else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
								Console.Write(formattedMessage);
							}
						}
						break;
					case ConsoleKey.Escape:
						continueRunning = false;
						break;
					default:
						break;
				}
			} while (continueRunning);
		}



























































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
	}
}

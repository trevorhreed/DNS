using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity.Reducing
{
	public class StartController : IController
	{
		private Piece _turn;
		private Piece[,] _squares;
		private Piece[,] Squares { get { return (Piece[,])this._squares.Clone(); } }
		private Point _cursor = new Point(1, 1);

		private void clearCursor()
		{
			Console.SetCursorPosition((_cursor.x * 10) + 9, (_cursor.y * 4) + 5);
			Console.Write(" ");
			Console.SetCursorPosition((_cursor.x * 10) + 13, (_cursor.y * 4) + 5);
			Console.Write(" ");
		}

		private void printCursor()
		{
			Console.SetCursorPosition((_cursor.x * 10) + 9, (_cursor.y * 4) + 5);
			Console.Write("(");
			Console.SetCursorPosition((_cursor.x * 10) + 13, (_cursor.y * 4) + 5);
			Console.Write(")");
		}

		private bool checkLine(int line, Piece player)
		{
			return
				_squares[Constants.Lines[line][0].y, Constants.Lines[line][0].x] == player &&
				_squares[Constants.Lines[line][1].y, Constants.Lines[line][1].x] == player &&
				_squares[Constants.Lines[line][2].y, Constants.Lines[line][2].x] == player;
		}

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

				{ // #003:																																																																																																																										 _printBoard()
					Console.Clear();
					Console.SetCursorPosition(0, 0);
					Console.WriteLine(this._gameScreen);
				}

				for (int y = 0; y < 3; y++) // #001:																																																																																																																										 _printPieces()
				{
					for (int x = 0; x < 3; x++)
					{
						int y2 = (y * 4) + 5;
						int x2 = (x * 10) + 11;
						Console.SetCursorPosition(x2, y2);
						Console.Write(Squares[y, x].GetLabel());
					}
				}

				printCursor();

				{ // #001, #002:																																																																																																																										 _printMessage()
					Console.SetCursorPosition(7, 17);
					string formattedMessage = "Ready.";
					if (formattedMessage.Length > Console.WindowWidth - 7) formattedMessage = formattedMessage.Substring(0, Console.WindowWidth - 7);
					else if (formattedMessage.Length < Console.WindowWidth - 7) formattedMessage = formattedMessage + new string(' ', (Console.WindowWidth - 7) - formattedMessage.Length);
					Console.Write(formattedMessage);
				}
			}

			bool running = true;
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				switch (keyInfo.Key)
				{
					case ConsoleKey.UpArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							clearCursor();
							this._cursor.y--;
							if (this._cursor.y < 0) this._cursor.y = 2;
							printCursor();
						}
						break;
					case ConsoleKey.DownArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							clearCursor();
							this._cursor.y++;
							if (this._cursor.y > 2) this._cursor.y = 0;
							printCursor();
						}
						break;
					case ConsoleKey.LeftArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							clearCursor();
							this._cursor.x--;
							if (this._cursor.x < 0) this._cursor.x = 2;
							printCursor();
						}
						break;
					case ConsoleKey.RightArrow:
						{ // #014:																																																																																																																										 _moveCursor(0, -1)
							clearCursor();
							this._cursor.x++;
							if (this._cursor.x > 2) this._cursor.x = 0;
							printCursor();
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
									}
								}

								Piece winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
								for (var i = 0; i < 8; i++)
								{
									if (checkLine(i, Piece.Player1))
									{
										winner = Piece.Player1;
									}
									if (checkLine(i, Piece.Player2))
									{
										winner = Piece.Player2;
									}
								}
								if (winner == Piece.None)
								{
									winner = noEmptySquares ? Piece.Cat : Piece.None;
								}

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
													int y2 = (y * 4) + 5;
													int x2 = (x * 10) + 11;
													Console.SetCursorPosition(x2, y2);
													Console.Write(Squares[y, x].GetLabel());
												}
											}

											noEmptySquares = true;	// #001, #003:																																																																																																																										_noEmptySquares (bool proeprty)
											foreach (Piece square in _squares)
											{
												if (square == Piece.None)
												{
													noEmptySquares = false;
												}
											}

											winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
											for (var i = 0; i < 8; i++)
											{
												if (checkLine(i, Piece.Player1))
												{
													winner = Piece.Player1;
												}
												if (checkLine(i, Piece.Player2))
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
													int WIDTH = Console.WindowWidth - 7;
													Console.SetCursorPosition(7, 17);
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
																		int y2 = (y * 4) + 5;
																		int x2 = (x * 10) + 11;
																		Console.SetCursorPosition(x2, y2);
																		Console.Write(Squares[y, x].GetLabel());
																	}
																}

																noEmptySquares = true; // #001, #003:																																																																																																																												_noEmptySquares (bool proeprty)
																foreach (Piece square in _squares)
																{
																	if (square == Piece.None)
																	{
																		noEmptySquares = false;
																	}
																}

																winner = Piece.None; // #001, #003:																																																																																																																									_winner (Piece property)
																for (var i = 0; i < 8; i++)
																{
																	if (checkLine(i, Piece.Player1))
																	{
																		winner = Piece.Player1;
																	}
																	if (checkLine(i, Piece.Player2))
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
																			int WIDTH = Console.WindowWidth - 7;
																			Console.SetCursorPosition(7, 17);
																			string formattedMessage = "Cat's game!";
																			if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
																			else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
																			Console.Write(formattedMessage);
																		}
																	}
																	else
																	{
																		{ // #001, #002:																																																																																																																										 _printMessage()
																			int WIDTH = Console.WindowWidth - 7;
																			Console.SetCursorPosition(7, 17);
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
								int WIDTH = Console.WindowWidth - 7;
								Console.SetCursorPosition(7, 17);
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
									int y2 = (y * 4) + 5;
									int x2 = (x * 10) + 11;
									Console.SetCursorPosition(x2, y2);
									Console.Write(Squares[y, x].GetLabel());
								}
							}

							printCursor();

							{ // #001, #002:																																																																																																																										_printMessage()
								int WIDTH = Console.WindowWidth - 7;
								Console.SetCursorPosition(7, 17);
								string formattedMessage = "Ready.";
								if (formattedMessage.Length > WIDTH) formattedMessage = formattedMessage.Substring(0, WIDTH);
								else if (formattedMessage.Length < WIDTH) formattedMessage = formattedMessage + new string(' ', WIDTH - formattedMessage.Length);
								Console.Write(formattedMessage);
							}
						}
						break;
					case ConsoleKey.Escape:
						running = false;
						break;
					default:
						break;
				}
			} while (running);
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

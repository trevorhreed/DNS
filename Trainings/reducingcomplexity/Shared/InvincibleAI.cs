using System;
using System.Collections.Generic;

namespace ReducingComplexity.Shared
{
	class InvincibleAI : IComputerAI
	{
		private Piece _computerPiece;
		private Piece _humanPiece;

		public Point GetMove(Piece[,] squares, Piece computer)
		{
			this._computerPiece = computer;
			this._humanPiece = _computerPiece == Piece.Player1 ? Piece.Player2 : Piece.Player1;
			return getStrongestMove(squares, _computerPiece).Square;
		}

		private ScoredMove getStrongestMove(Piece[,] squares, Piece turn, int depth = 0)
		{
			List<Point> legalMoves = getLegalMoves(squares);
			bool gameOver = _gameIsOver(squares);
			if (legalMoves.Count == 0 || gameOver)
			{
				int score = evaluateScore(squares) - depth;
				return new ScoredMove() { Score = score };
			}

			ScoredMove bestMove = new ScoredMove() { Score = (turn == _computerPiece ? -10000 : 10000) };
			foreach (Point move in legalMoves)
			{
				squares[move.y, move.x] = turn;
				ScoredMove nextMove = getStrongestMove(squares, nextTurn(turn), depth + 1);
				if (
					(turn == _computerPiece && nextMove.Score > bestMove.Score) ||
					(turn == _humanPiece && nextMove.Score < bestMove.Score)
					)
				{
					bestMove = new ScoredMove() { Score = nextMove.Score, Square = move };
				}
				squares[move.y, move.x] = Piece.Empty;
			}
			return bestMove;
		}

		private Piece nextTurn(Piece turn)
		{
			return turn == Piece.Player1 ? Piece.Player2 : Piece.Player1;
		}

		private List<Point> getLegalMoves(Piece[,] squares)
		{
			List<Point> legalMoves = new List<Point>();
			for (int row = 0; row < 3; row++)
			{
				for (int col = 0; col < 3; col++)
				{
					if (squares[row, col] == Piece.Empty)
					{
						legalMoves.Add(new Point() { y = row, x = col });
					}
				}
			}
			return legalMoves;
		}

		private int evaluateScore(Piece[,] squares)
		{
			int score = 0;
			foreach (Point[] line in Constants.Lines)
			{
				int aiCount = 0, oppCount = 0;
				foreach (Point p in line)
				{
					if (squares[p.y, p.x] == _computerPiece) aiCount++;
					if (squares[p.y, p.x] == _humanPiece) oppCount++;
				}
				if (aiCount > 0 && oppCount > 0) continue;
				else if (aiCount > 0)
				{
					score += (int)Math.Pow(10, aiCount - 1);
				}
				else
				{
					score += (int)Math.Pow(10, oppCount - 1) * -1;
				}
			}
			return score;
		}

		private bool _gameIsOver(Piece[,] squares)
		{
			bool existsWinner = false;
			for (var i = 0; i < 8; i++)
			{
				if (
					(
						squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player1 &&
						squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player1 &&
						squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player1
					) ||
					(
						squares[Constants.Lines[i][0].y, Constants.Lines[i][0].x] == Piece.Player2 &&
						squares[Constants.Lines[i][1].y, Constants.Lines[i][1].x] == Piece.Player2 &&
						squares[Constants.Lines[i][2].y, Constants.Lines[i][2].x] == Piece.Player2
					)
				)
				{
					existsWinner = true;
					break;
				}
			}

			bool existsEmptySquare = false;
			foreach (Piece square in squares)
			{
				if (square == Piece.Empty)
				{
					existsEmptySquare = true;
					break;
				}
			}

			return existsWinner || !existsEmptySquare;
		}

		private class ScoredMove
		{
			public Point Square { get; set; }
			public int Score { get; set; }
		}

	}
}

namespace ReducingComplexity.Shared
{
	public interface IComputerAI
	{
		Point GetMove(Piece[,] squares, Piece computer);
	}
}

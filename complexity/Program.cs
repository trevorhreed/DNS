using ReducingComplexity.Shared;

namespace ReducingComplexity
{
	class Program
	{
		static void Main(string[] args)
		{
			//new Simple.SimpleTicTacToeController().Run(new InvincibleAI());
			new Complex.ComplexController().Run(new InvincibleAI());
		}
	}
}

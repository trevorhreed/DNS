using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity
{
	class Program
	{
		static void Main(string[] args)
		{
			IComputerAI ai = new Shared.InvincibleAI();

			//new Simple.SimpleController().Run(ai);
			//new Complex.ComplexController().Run(ai);

			new Reducing.Step3Controller().Run(ai);
		}
	}
}

using ReducingComplexity.Shared;
using System;

namespace ReducingComplexity
{
	class Program
	{
		static void Main(string[] args)
		{
			IComputerAI ai = new Shared.InvincibleAI();
			new Steps.Step0Controller().Run(ai);
		}
	}
}

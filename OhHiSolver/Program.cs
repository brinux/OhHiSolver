namespace brinux.ohhisolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			/*var schema = new OhHiSchema(new int[12][]
			{
				new int[12] { 0, 0, 0, 0, 0, 0, 2, 2, 0, 2, 0, 0 },
				new int[12] { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
				new int[12] { 0, 0, 0, 2, 2, 0, 2, 0, 1, 0, 1, 0 },
				new int[12] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2 },
				new int[12] { 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0 },
				new int[12] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },
				new int[12] { 0, 2, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1 },
				new int[12] { 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0 },
				new int[12] { 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2 },
				new int[12] { 0, 2, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 },
				new int[12] { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0 },
				new int[12] { 0, 0, 0, 2, 0, 1, 0, 0, 2, 0, 2, 0 }
			});*/

			var schema = new OhHiSchema(new int[6][]
			{
				new int[6] { 0, 0, 2, 0, 0, 0 },
				new int[6] { 0, 0, 0, 0, 0, 0 },
				new int[6] { 0, 0, 0, 1, 1, 0 },
				new int[6] { 2, 0, 0, 1, 0, 2 },
				new int[6] { 2, 0, 0, 0, 0, 0 },
				new int[6] { 0, 0, 0, 0, 2, 2 }
			});

			SchemaPrinter.PrintSchema(schema);

			while (OhHiOptionsBasedSolver.Solve(schema))
			{
				SchemaPrinter.PrintSchema(schema);
			}

			/*while (OhHiRulesBasedSolver.Solve(schema))
			{
				SchemaPrinter.PrintSchema(schema);
			}*/

			Console.WriteLine(schema.IsSolved() ? "Solved!" : "The schema couldn't be solved.");
		}
	}
}
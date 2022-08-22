namespace brinux.ohhisolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var schema = new OhHiSchema(new int[12][]
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
			});

			SchemaPrinter.PrintSchema(schema);

			while (schema.Solve())
			{
				SchemaPrinter.PrintSchema(schema);
			}

			Console.WriteLine(schema.IsSolved() ? "Solved!" : "The schema couldn't be solved.");
		}
	}
}
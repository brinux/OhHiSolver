namespace brinux.ohhisolver
{
	public static class SchemaPrinter
	{
		public static void PrintSchema(OhHiSchema schema)
		{
			if (schema == null)
			{
				throw new NullReferenceException("The schema is undefined");
			}

			Console.Write("   ");
			for (int r = 0; r < schema.Size; r++)
			{
				Console.Write($"{ r + 1 }{(r < 10 ? " " : "")} ");
			}
			Console.WriteLine();

			for (int r = 0; r < schema.Size; r++)
			{
				Console.Write($"{ r + 1 }{( r + 1 < 10 ? " " : "" )} ");

				for (int c = 0; c < schema.Size; c++)
				{
					switch (schema.Cells[r][c])
					{
						case CellStatus.PrimaryColor:
							Console.BackgroundColor = ConsoleColor.Yellow;
							break;

						case CellStatus.SecondaryColor:
							Console.BackgroundColor = ConsoleColor.Blue;
							break;

						default:
							Console.BackgroundColor = ConsoleColor.Black;
							break;
					}

					Console.Write("   ");
				}

				Console.BackgroundColor = ConsoleColor.Black;

				Console.WriteLine();
			}

			Console.WriteLine();

			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}

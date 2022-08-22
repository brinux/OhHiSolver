namespace brinux.ohhisolver
{
	public class LineOptionsCalculator
	{
		public static List<CellStatus[]> GetMissingCellsOptions(int primaries, int secondaries)
		{
			var options = new List<CellStatus[]>();

			if (primaries > 0 && secondaries == 0)
			{
				var option = new CellStatus[primaries];

				for (int i = 0; i < primaries; i++)
				{
					option[i] = CellStatus.PrimaryColor;
				}

				options.Add(option);
			}
			else if (primaries == 0 && secondaries > 0)
			{
				var option = new CellStatus[secondaries];

				for (int i = 0; i < secondaries; i++)
				{
					option[i] = CellStatus.SecondaryColor;
				}

				options.Add(option);
			}
			else
			{
				var primaryOptions = GetMissingCellsOptions(primaries - 1, secondaries);

				foreach (var o in primaryOptions)
				{
					var option = new CellStatus[primaries + secondaries];

					option[0] = CellStatus.PrimaryColor;

					Array.Copy(o, 0, option, 1, o.Length);

					options.Add(option);
				}

				var secondaryOptions = GetMissingCellsOptions(primaries, secondaries - 1);

				foreach (var o in secondaryOptions)
				{
					var option = new CellStatus[primaries + secondaries];

					option[0] = CellStatus.SecondaryColor;

					Array.Copy(o, 0, option, 1, o.Length);

					options.Add(option);
				}
			}

			return options;
		}
	}
}
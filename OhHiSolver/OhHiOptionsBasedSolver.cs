namespace brinux.ohhisolver
{
	public static class OhHiOptionsBasedSolver
	{
		public static bool Solve(OhHiSchema schema)
		{
			var result = false;

			if (!result)
			{
				for (int r = 0; r < schema.Size; r++)
				{
					result = MandatoryOptionsInRow(schema, r);

					if (result)
					{
						break;
					}
				}
			}

			if (!result)
			{
				for (int c = 0; c < schema.Size; c++)
				{
					result = MandatoryOptionsInColumn(schema, c);

					if (result)
					{
						break;
					}
				}
			}

			return result;
		}

		public static bool MandatoryOptionsInRow(OhHiSchema schema, int r)
		{
			var applied = false;

			var missingPrimary = schema.Size / 2 - schema.CountPrimaryElementsInRow(r);
			var missingSecondary = schema.Size / 2 - schema.CountSecondaryElementsInRow(r);

			if (missingPrimary == 0 || missingSecondary == 0)
			{
				return applied;
			}

			var validOptions = new List<CellStatus[]>();

			var options = LineOptionsCalculator.GetMissingCellsOptions(missingPrimary, missingSecondary);

			foreach (var option in options)
			{
				var optionSchema = (OhHiSchema)schema.Clone();

				var optionIndex = 0;

				for (int c = 0; c < schema.Size; c++)
				{
					if (optionSchema.Cells[r][c] == CellStatus.Empty)
					{
						optionSchema.Cells[r][c] = option[optionIndex++];
					}
				}

				if (optionSchema.IsValid())
				{
					validOptions.Add(option);
				}
			}

			if (validOptions.Count > 0)
			{
				var mandatoryElements = new CellStatus[validOptions.First().Length];

				var initialized = false;

				foreach (var option in validOptions)
				{
					if (!initialized)
					{
						for (int i = 0; i < option.Length; i++)
						{
							mandatoryElements[i] = option[i];
						}

						initialized = true;
					}
					else
					{
						for (int i = 0; i < option.Length; i++)
						{
							if (mandatoryElements[i] != CellStatus.Empty && mandatoryElements[i] != option[i])
							{
								mandatoryElements[i] = CellStatus.Empty;
							}
						}
					}
				}

				int optionIndex = 0;

				for (int c = 0; c < schema.Size; c++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						if (mandatoryElements[optionIndex] != CellStatus.Empty)
						{
							schema.Cells[r][c] = mandatoryElements[optionIndex];

							applied = true;

							Console.WriteLine($"In row { r + 1}, considering all the possible options, it resulted cell { r + 1 }:{ c + 1 } had to be mandatorily set to { schema.Cells[r][c] }.");
						}

						optionIndex++;
					}
				}
			}

			return applied;
		}

		public static bool MandatoryOptionsInColumn(OhHiSchema schema, int c)
		{
			var applied = false;

			var missingPrimary = schema.Size / 2 - schema.CountPrimaryElementsInColumn(c);
			var missingSecondary = schema.Size / 2 - schema.CountSecondaryElementsInColumn(c);

			if (missingPrimary == 0 || missingSecondary == 0)
			{
				return applied;
			}

			var validOptions = new List<CellStatus[]>();

			var options = LineOptionsCalculator.GetMissingCellsOptions(missingPrimary, missingSecondary);

			foreach (var option in options)
			{
				var optionSchema = (OhHiSchema)schema.Clone();

				var optionIndex = 0;

				for (int r = 0; r < schema.Size; r++)
				{
					if (optionSchema.Cells[r][c] == CellStatus.Empty)
					{
						optionSchema.Cells[r][c] = option[optionIndex++];
					}
				}

				if (optionSchema.IsValid())
				{
					validOptions.Add(option);
				}
			}

			if (validOptions.Count > 0)
			{
				var mandatoryElements = new CellStatus[validOptions.First().Length];

				var initialized = false;

				foreach (var option in validOptions)
				{
					if (!initialized)
					{
						for (int i = 0; i < option.Length; i++)
						{
							mandatoryElements[i] = option[i];
						}

						initialized = true;
					}
					else
					{
						for (int i = 0; i < option.Length; i++)
						{
							if (mandatoryElements[i] != CellStatus.Empty && mandatoryElements[i] != option[i])
							{
								mandatoryElements[i] = CellStatus.Empty;
							}
						}
					}
				}

				int optionIndex = 0;

				for (int r = 0; r < schema.Size; r++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						if (mandatoryElements[optionIndex] != CellStatus.Empty)
						{
							schema.Cells[r][c] = mandatoryElements[optionIndex];

							applied = true;

							Console.WriteLine($"In column { c + 1 }, considering all the possible options, it resulted cell { r + 1 }:{ c + 1 } had to be mandatorily set to { schema.Cells[r][c] }.");
						}

						optionIndex++;
					}
				}
			}

			return applied;
		}
	}
}
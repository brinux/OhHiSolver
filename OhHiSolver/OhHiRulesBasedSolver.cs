namespace brinux.ohhisolver
{
	public static class OhHiRulesBasedSolver
	{
		public static bool Solve(OhHiSchema schema)
		{
			var result = false;

			for (int r = 0; r < schema.Size; r++)
			{
				result = SolveRow(schema, r);

				if (result)
				{
					break;
				}
			}

			if (!result)
			{
				for (int c = 0; c < schema.Size; c++)
				{
					result = SolveColumn(schema, c);

					if (result)
					{
						break;
					}
				}
			}
			
			return result;
		}

		public static bool SolveRow(OhHiSchema schema, int r)
		{
			return
				CheckForColorCompletellySetInRow(schema, r) ||
				CheckForTwoEqualConsecutiveElementsInRow(schema, r) ||
				CheckForTwoSameColorElementsWithStepInRow(schema, r) ||
				AvoidDuplicatedRows(schema, r);
		}

		public static bool SolveColumn(OhHiSchema schema, int c)
		{
			return
				CheckForColorCompletellySetInColumn(schema, c) ||
				CheckForTwoEqualConsecutiveElementsInColumn(schema, c) ||
				CheckForTwoSameColorElementsWithStepInColumn(schema, c) ||
				AvoidDuplicatedColumns(schema, c);
		}

		public static bool CheckForTwoSameColorElementsWithStepInRow(OhHiSchema schema, int r)
		{
			var applied = false;

			if (schema.Size >= 4)
			{
				for (int c = 0; c < schema.Size - 2; c++)
				{
					if (schema.Cells[r][c] != CellStatus.Empty &&
						schema.Cells[r][c + 1] == CellStatus.Empty &&
						schema.Cells[r][c + 2] == schema.Cells[r][c])
					{
						schema.Cells[r][c + 1] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
							CellStatus.SecondaryColor :
							CellStatus.PrimaryColor;

						applied = true;

						Console.WriteLine($"In row { r + 1 }, cell { c + 2 } has been set to { schema.Cells[r][c + 1] } since is in between two { schema.Cells[r][c] } cells.");

						break;
					}
				}
			}

			return applied;
		}

		public static bool CheckForTwoSameColorElementsWithStepInColumn(OhHiSchema schema, int c)
		{
			var applied = false;

			if (schema.Size >= 4)
			{
				for (int r = 0; r < schema.Size - 2; r++)
				{
					if (schema.Cells[r][c] != CellStatus.Empty &&
						schema.Cells[r + 1][c] == CellStatus.Empty &&
						schema.Cells[r + 2][c] == schema.Cells[r][c])
					{
						schema.Cells[r + 1][c] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
							CellStatus.SecondaryColor :
							CellStatus.PrimaryColor;

						applied = true;

						Console.WriteLine($"In column { c + 1 }, cell { r + 2 } has been set to { schema.Cells[r + 1][c] } since is in between two { schema.Cells[r][c] } cells.");

						break;
					}
				}
			}

			return applied;
		}

		public static bool CheckForTwoEqualConsecutiveElementsInRow(OhHiSchema schema, int r)
		{
			var applied = false;

			if (schema.Size >= 4)
			{
				for (int c = 0; c < schema.Size - 1; c++)
				{
					if (schema.Cells[r][c] != CellStatus.Empty && schema.Cells[r][c + 1] == schema.Cells[r][c])
					{
						if (c - 1 >= 0 && schema.Cells[r][c - 1] == CellStatus.Empty)
						{
							schema.Cells[r][c - 1] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In row { r + 1 }, cell { c } has been set to { schema.Cells[r][c - 1] } since is before two { schema.Cells[r][c] } consecutive cells.");
						}


						if (c + 2 < schema.Size && schema.Cells[r][c + 2] == CellStatus.Empty)
						{
							schema.Cells[r][c + 2] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In row { r + 1 }, cell { c + 3 } has been set to { schema.Cells[r][c + 2] } since is after two { schema.Cells[r][c] } consecutive cells.");
						}

						if (applied)
						{
							break;
						}
					}
				}
			}

			return applied;
		}

		public static bool CheckForTwoEqualConsecutiveElementsInColumn(OhHiSchema schema, int c)
		{
			var applied = false;

			if (schema.Size >= 4)
			{
				for (int r = 0; r < schema.Size - 1; r++)
				{
					if (schema.Cells[r][c] != CellStatus.Empty && schema.Cells[r + 1][c] == schema.Cells[r][c])
					{
						if (r - 1 >= 0 && schema.Cells[r - 1][c] == CellStatus.Empty)
						{
							schema.Cells[r - 1][c] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In column { c + 1 }, cell { r } has been set to { schema.Cells[r - 1][c] } since is before two { schema.Cells[r][c] } consecutive cells.");
						}


						if (r + 2 < schema.Size && schema.Cells[r + 2][c] == CellStatus.Empty)
						{
							schema.Cells[r + 2][c] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In column { c + 1 }, cell { r + 3 } has been set to { schema.Cells[r + 1][c] } since is before two { schema.Cells[r][c] } consecutive cells.");
						}

						if (applied)
						{
							break;
						}
					}
				}
			}

			return applied;
		}

		public static bool AvoidDuplicatedRows(OhHiSchema schema, int row)
		{
			var applied = false;

			if (schema.CountSetElementsInRow(row) == schema.Size - 2)
			{
				for (int r = 0; r < schema.Size; r++)
				{
					if (r != row && schema.CountSetElementsInRow(r) == schema.Size)
					{
						var emptyColumns = new List<int>();
						bool matchingRows = true;

						for (int c = 0; c < schema.Size; c++)
						{
							if (schema.Cells[row][c] == CellStatus.Empty || schema.Cells[row][c] == schema.Cells[r][c])
							{
								if (schema.Cells[row][c] == CellStatus.Empty)
								{
									emptyColumns.Add(c);
								}
							}
							else
							{
								matchingRows = false;

								break;
							}
						}

						if (matchingRows)
						{
							foreach (var c in emptyColumns)
							{
								schema.Cells[row][c] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
									CellStatus.SecondaryColor :
									CellStatus.PrimaryColor;

								Console.WriteLine($"Row { row + 1 } was lacking 2 elements, and was the same as row { r + 1 }. Because of this, cell { row + 1 }:{ c + 1 } was set to { schema.Cells[row][c] }, as the oppositve of cell { r + 1 }:{ c + 1 }.");
							}

							applied = true;

							break;
						}
					}
				}
			}

			return applied;
		}

		public static bool AvoidDuplicatedColumns(OhHiSchema schema, int column)
		{
			var applied = false;

			if (schema.CountSetElementsInColumn(column) == schema.Size - 2)
			{
				for (int c = 0; c < schema.Size; c++)
				{
					if (c != column && schema.CountSetElementsInColumn(c) == schema.Size)
					{
						var emptyRows = new List<int>();
						bool matchingColumns = true;

						for (int r = 0; r < schema.Size; r++)
						{
							if (schema.Cells[r][column] == CellStatus.Empty || schema.Cells[r][column] == schema.Cells[r][c])
							{
								if (schema.Cells[r][column] == CellStatus.Empty)
								{
									emptyRows.Add(r);
								}
							}
							else
							{
								matchingColumns = false;

								break;
							}
						}

						if (matchingColumns)
						{
							foreach (var r in emptyRows)
							{
								schema.Cells[r][column] = schema.Cells[r][c] == CellStatus.PrimaryColor ?
									CellStatus.SecondaryColor :
									CellStatus.PrimaryColor;

								Console.WriteLine($"Column { column + 1 } was lacking 2 elements, and was the same as column { c + 1 }. Because of this, cell { r + 1 }:{ column + 1 } was set to { schema.Cells[r][column] }, as the oppositve of cell { r + 1 }:{ column + 1 }.");
							}

							applied = true;

							break;
						}
					}
				}
			}

			return applied;
		}

		public static bool CheckForColorCompletellySetInRow(OhHiSchema schema, int r)
		{
			var applied = false;

			var primaries = schema.CountPrimaryElementsInRow(r);
			var secondaries = schema.CountSecondaryElementsInRow(r);

			if (primaries == schema.Size / 2 && secondaries != schema.Size / 2)
			{
				for (int c = 0; c < schema.Size; c++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						schema.Cells[r][c] = CellStatus.SecondaryColor;

						Console.WriteLine($"Since all { CellStatus.PrimaryColor } cells of row { r + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.SecondaryColor }.");
					}
				}

				applied = true;
			}
			else if (secondaries == schema.Size / 2 && primaries != schema.Size / 2)
			{
				for (int c = 0; c < schema.Size; c++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						schema.Cells[r][c] = CellStatus.PrimaryColor;

						Console.WriteLine($"Since all { CellStatus.SecondaryColor } cells of row { r + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.PrimaryColor }.");
					}
				}

				applied = true;
			}

			return applied;
		}

		public static bool CheckForColorCompletellySetInColumn(OhHiSchema schema, int c)
		{
			var applied = false;

			var primaries = schema.CountPrimaryElementsInColumn(c);
			var secondaries = schema.CountSecondaryElementsInColumn(c);

			if (primaries == schema.Size / 2 && secondaries != schema.Size / 2)
			{
				for (int r = 0; r < schema.Size; r++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						schema.Cells[r][c] = CellStatus.SecondaryColor;

						Console.WriteLine($"Since all { CellStatus.PrimaryColor } cells of column { c + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.SecondaryColor }.");
					}
				}

				applied = true;
			}
			else if (secondaries == schema.Size / 2 && primaries != schema.Size / 2)
			{
				for (int r = 0; r < schema.Size; r++)
				{
					if (schema.Cells[r][c] == CellStatus.Empty)
					{
						schema.Cells[r][c] = CellStatus.PrimaryColor;

						Console.WriteLine($"Since all { CellStatus.SecondaryColor } cells of column { c + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.PrimaryColor }.");
					}
				}

				applied = true;
			}

			return applied;
		}
	}
}
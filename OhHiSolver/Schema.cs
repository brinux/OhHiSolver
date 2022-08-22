namespace brinux.ohhisolver
{
	public class Schema : ICloneable
	{
		public int Size { get; }
		public CellStatus[][] Cells { get; private set; }

		public Schema(int size)
		{
			if (size < 2)
			{
				throw new ArgumentException("The size of the schema cannot be lower than 2.");
			}
			else if (size % 2 != 0)
			{
				throw new ArgumentException("The size of the schema must be even.");
			}

			Size = size;

			ResetSchema();
		}

		public Schema(int[][] cells)
		{
			if (cells == null)
			{
				throw new NullReferenceException("The provided init schema is null.");
			}

			for (int r = 0; r < cells.Length; r++)
			{
				if (cells[r].Length != cells.Length)
				{
					throw new ArgumentException("The number of rows and columns of the schema must be the same.");
				}
				else
				{
					for (int c = 0; c < Size; c++)
					{
						if (cells[r][c] < 0 || cells[r][c] > 2)
						{
							throw new ArgumentException("The init schema contains values not in the range 0 - 2.");
						}
					}
				}
			}

			if (cells.Length < 2)
			{
				throw new ArgumentException("The size of the schema cannot be lower than 2.");
			}
			else if (cells.Length % 2 != 0)
			{
				throw new ArgumentException("The size of the schema must be even.");
			}

			Size = cells.Length;

			ResetSchema();

			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					Cells[r][c] = (CellStatus) cells[r][c];
				}
			}
		}

		public bool Solve()
		{
			var result = false;

			for (int r = 0; r < Size; r++)
			{
				result = SolveRow(r);

				if (result)
				{
					break;
				}
			}

			if (!result)
			{
				for (int c = 0; c < Size; c++)
				{
					result = SolveColumn(c);

					if (result)
					{
						break;
					}
				}
			}

			if (!result)
			{
				for (int r = 0; r < Size; r++)
				{
					result = MandatoryOptionsInRow(r);

					if (result)
					{
						break;
					}
				}

				if (!result)
				{
					for (int c = 0; c < Size; c++)
					{
						result = MandatoryOptionsInColumn(c);

						if (result)
						{
							break;
						}
					}
				}
			}

			return result;
		}

		private bool SolveRow(int r)
		{
			return
				CheckForColorCompletellySetInRow(r) ||
				CheckForTwoEqualConsecutiveElementsInRow(r) ||
				CheckForTwoSameColorElementsWithStepInRow(r) ||
				AvoidDuplicatedRows(r);
		}

		private bool SolveColumn(int c)
		{
			return
				CheckForColorCompletellySetInColumn(c) ||
				CheckForTwoEqualConsecutiveElementsInColumn(c) ||
				CheckForTwoSameColorElementsWithStepInColumn(c) ||
				AvoidDuplicatedColumns(c);
		}

		private bool CheckForTwoSameColorElementsWithStepInRow(int r)
		{
			var applied = false;

			if (Size >= 4)
			{
				for (int c = 0; c < Size - 3; c++)
				{
					if (Cells[r][c] != CellStatus.Empty &&
						Cells[r][c + 1] == CellStatus.Empty &&
						Cells[r][c + 2] == Cells[r][c])
					{
						Cells[r][c + 1] = Cells[r][c] == CellStatus.PrimaryColor ?
							CellStatus.SecondaryColor :
							CellStatus.PrimaryColor;

						applied = true;

						Console.WriteLine($"In row { r + 1 }, cell { c+ 2 } has been set to { Cells[r][c + 1] } since is in between two { Cells[r][c] } cells.");

						break;
					}
				}
			}

			return applied;
		}

		private bool CheckForTwoSameColorElementsWithStepInColumn(int c)
		{
			var applied = false;

			if (Size >= 4)
			{
				for (int r = 0; r < Size - 3; r++)
				{
					if (Cells[r][c] != CellStatus.Empty &&
						Cells[r + 1][c] == CellStatus.Empty &&
						Cells[r + 2][c] == Cells[r][c])
					{
						Cells[r + 1][c] = Cells[r][c] == CellStatus.PrimaryColor ?
							CellStatus.SecondaryColor :
							CellStatus.PrimaryColor;

						applied = true;

						Console.WriteLine($"In column { c + 1 }, cell { r + 2 } has been set to { Cells[r + 1][c] } since is in between two { Cells[r][c] } cells.");

						break;
					}
				}
			}

			return applied;
		}

		private bool CheckForTwoEqualConsecutiveElementsInRow(int r)
		{
			var applied = false;

			if (Size >= 4)
			{
				for (int c = 0; c < Size - 1; c++)
				{
					if (Cells[r][c] != CellStatus.Empty && Cells[r][c + 1] == Cells[r][c])
					{
						if (c - 1 >= 0 && Cells[r][c - 1] == CellStatus.Empty)
						{
							Cells[r][c - 1] = Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;
							
							applied = true;

							Console.WriteLine($"In row { r + 1 }, cell { c } has been set to { Cells[r][c - 1] } since is before two { Cells[r][c] } consecutive cells.");
						}


						if (c + 2 < Size && Cells[r][c + 2] == CellStatus.Empty)
						{
							Cells[r][c + 2] = Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In row { r + 1 }, cell { c + 3 } has been set to { Cells[r][c + 2] } since is after two { Cells[r][c] } consecutive cells.");
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

		private bool CheckForTwoEqualConsecutiveElementsInColumn(int c)
		{
			var applied = false;

			if (Size >= 4)
			{
				for (int r = 0; r < Size - 1; r++)
				{
					if (Cells[r][c] != CellStatus.Empty && Cells[r + 1][c] == Cells[r][c])
					{
						if (r - 1 >= 0 && Cells[r - 1][c] == CellStatus.Empty)
						{
							Cells[r - 1][c] = Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In column { c + 1 }, cell { r } has been set to { Cells[r - 1][c] } since is before two { Cells[r][c] } consecutive cells.");
						}


						if (r + 2 < Size && Cells[r + 2][c] == CellStatus.Empty)
						{
							Cells[r + 2][c] = Cells[r][c] == CellStatus.PrimaryColor ?
								CellStatus.SecondaryColor :
								CellStatus.PrimaryColor;

							applied = true;

							Console.WriteLine($"In column { c + 1 }, cell { r + 3 } has been set to { Cells[r + 1][c] } since is before two { Cells[r][c] } consecutive cells.");
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

		private bool AvoidDuplicatedRows(int row)
		{
			var applied = false;

			if (CountSetElementsInRow(row) == Size - 2)
			{
				for (int r = 0; r < Size; r++)
				{
					if (r != row && CountSetElementsInRow(r) == Size)
					{
						var emptyColumns = new List<int>();
						bool matchingRows = true;

						for (int c = 0; c < Size; c++)
						{
							if (Cells[row][c] == CellStatus.Empty || Cells[row][c] == Cells[r][c])
							{
								if (Cells[row][c] == CellStatus.Empty)
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
								Cells[row][c] = Cells[r][c] == CellStatus.PrimaryColor ?
									CellStatus.SecondaryColor :
									CellStatus.PrimaryColor;

								Console.WriteLine($"Row { row + 1 } was lacking 2 elements, and was the same as row { r + 1 }. Because of this, cell { row + 1 }:{ c + 1 } was set to { Cells[row][c] }, as the oppositve of cell { r + 1 }:{ c + 1 }.");
							}

							applied = true;

							break;
						}
					}
				}
			}

			return applied;
		}

		private bool AvoidDuplicatedColumns(int column)
		{
			var applied = false;

			if (CountSetElementsInColumn(column) == Size - 2)
			{
				for (int c = 0; c < Size; c++)
				{
					if (c != column && CountSetElementsInColumn(c) == Size)
					{
						var emptyRows = new List<int>();
						bool matchingColumns = true;

						for (int r = 0; r < Size; r++)
						{
							if (Cells[r][column] == CellStatus.Empty || Cells[r][column] == Cells[r][c])
							{
								if (Cells[r][column] == CellStatus.Empty)
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
								Cells[r][column] = Cells[r][c] == CellStatus.PrimaryColor ?
									CellStatus.SecondaryColor :
									CellStatus.PrimaryColor;

								Console.WriteLine($"Column { column + 1 } was lacking 2 elements, and was the same as column { c + 1 }. Because of this, cell { r + 1 }:{ column + 1 } was set to { Cells[r][column] }, as the oppositve of cell { r + 1 }:{ column + 1 }.");
							}

							applied = true;

							break;
						}
					}
				}
			}

			return applied;
		}

		private bool CheckForColorCompletellySetInRow(int r)
		{
			var applied = false;

			var primaries = CountPrimaryElementsInRow(r);
			var secondaries = CountSecondaryElementsInRow(r);

			if (primaries == Size / 2 && secondaries != Size / 2)
			{
				for (int c = 0; c < Size; c++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						Cells[r][c] = CellStatus.SecondaryColor;

						Console.WriteLine($"Since all { CellStatus.PrimaryColor } cells of row { r + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.SecondaryColor }.");
					}
				}

				applied = true;
			}
			else if (secondaries == Size / 2 && primaries != Size / 2)
			{
				for (int c = 0; c < Size; c++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						Cells[r][c] = CellStatus.PrimaryColor;

						Console.WriteLine($"Since all { CellStatus.SecondaryColor } cells of row { r + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.PrimaryColor }.");
					}
				}

				applied = true;
			}

			return applied;
		}

		private bool CheckForColorCompletellySetInColumn(int c)
		{
			var applied = false;

			var primaries = CountPrimaryElementsInColumn(c);
			var secondaries = CountSecondaryElementsInColumn(c);

			if (primaries == Size / 2 && secondaries != Size / 2)
			{
				for (int r = 0; r < Size; r++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						Cells[r][c] = CellStatus.SecondaryColor;

						Console.WriteLine($"Since all { CellStatus.PrimaryColor } cells of column { c + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.SecondaryColor }.");
					}
				}

				applied = true;
			}
			else if (secondaries == Size / 2 && primaries != Size / 2)
			{
				for (int r = 0; r < Size; r++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						Cells[r][c] = CellStatus.PrimaryColor;

						Console.WriteLine($"Since all { CellStatus.SecondaryColor } cells of column { c + 1 } were already set, cell { r + 1 }:{ c + 1 } was set to { CellStatus.PrimaryColor }.");
					}
				}

				applied = true;
			}

			return applied;
		}

		private bool MandatoryOptionsInRow(int r)
		{
			var applied = false;

			var missingPrimary = Size / 2 - CountPrimaryElementsInRow(r);
			var missingSecondary = Size / 2 - CountSecondaryElementsInRow(r);

			if (missingPrimary == 0 || missingSecondary == 0)
			{
				return applied;
			}

			var validOptions = new List<CellStatus[]>();

			var options = LineOptionsCalculator.GetMissingCellsOptions(missingPrimary, missingSecondary);

			foreach (var option in options)
			{
				var optionSchema = (Schema) Clone();

				var optionIndex = 0;

				for (int c = 0; c < Size; c++)
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

				for (int c = 0; c < Size; c++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						if (mandatoryElements[optionIndex] != CellStatus.Empty)
						{
							Cells[r][c] = mandatoryElements[optionIndex];

							applied = true;

							Console.WriteLine($"In row { r + 1}, considering all the possible options, it resulted cell { r + 1 }:{ c + 1 } had to be mandatorily set to { Cells[r][c] }.");
						}

						optionIndex++;
					}
				}
			}

			return applied;
		}

		private bool MandatoryOptionsInColumn(int c)
		{
			var applied = false;

			var missingPrimary = Size / 2 - CountPrimaryElementsInColumn(c);
			var missingSecondary = Size / 2 - CountSecondaryElementsInColumn(c);

			if (missingPrimary == 0 || missingSecondary == 0)
			{
				return applied;
			}

			var validOptions = new List<CellStatus[]>();

			var options = LineOptionsCalculator.GetMissingCellsOptions(missingPrimary, missingSecondary);

			foreach (var option in options)
			{
				var optionSchema = (Schema)Clone();

				var optionIndex = 0;

				for (int r = 0; r < Size; r++)
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

				for (int r = 0; r < Size; r++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						if (mandatoryElements[optionIndex] != CellStatus.Empty)
						{
							Cells[r][c] = mandatoryElements[optionIndex];

							applied = true;

							Console.WriteLine($"In column { c + 1 }, considering all the possible options, it resulted cell { r + 1 }:{ c + 1 } had to be mandatorily set to { Cells[r][c] }.");
						}

						optionIndex++;
					}
				}
			}

			return applied;
		}

		public bool IsSolved()
		{
			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					if (Cells[r][c] == CellStatus.Empty)
					{
						return false;
					}
				}
			}

			return IsValid();
		}

		public bool IsValid()
		{
			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size - 2; c++)
				{
					if (Cells[r][c] != CellStatus.Empty && Cells[r][c] == Cells[r][c + 1] && Cells[r][c] == Cells[r][c + 2])
					{
						return false;
					}
				}

				if (CountPrimaryElementsInRow(r) > Size / 2 || CountSecondaryElementsInRow(r) > Size / 2)
				{
					return false;
				}

				if (CountSecondaryElementsInRow(r) == Size)
				{
					for (int i = r + 1; i < Size; i++)
					{
						if (CountSetElementsInRow(i) == Size)
						{
							var match = true;

							for (int c = 0; c < Size; c++)
							{
								if (Cells[r][c] != Cells[r][i])
								{
									match = false;
									break;
								}
							}

							if (match)
							{
								return false;
							}
						}
					}
				}
			}

			for (int c = 0; c < Size; c++)
			{
				for (int r = 0; r < Size - 2; r++)
				{
					if (Cells[r][c] != CellStatus.Empty && Cells[r][c] == Cells[r + 1][c] && Cells[r][c] == Cells[r + 2][c])
					{
						return false;
					}
				}

				if (CountPrimaryElementsInColumn(c) > Size / 2 || CountSecondaryElementsInColumn(c) > Size / 2)
				{
					return false;
				}

				if (CountSecondaryElementsInColumn(c) == Size)
				{
					for (int i = c + 1; i < Size; i++)
					{
						if (CountSetElementsInColumn(i) == Size)
						{
							var match = true;

							for (int r = 0; r < Size; r++)
							{
								if (Cells[r][c] != Cells[r][i])
								{
									match = false;
									break;
								}
							}

							if (match)
							{
								return false;
							}
						}
					}
				}
			}

			return true;
		}

		private int CountSetElementsInRow(int r)
		{
			return Size - CountElementsInRow(r, CellStatus.Empty);
		}

		private int CountSetElementsInColumn(int c)
		{
			return Size - CountElementsInColumn(c, CellStatus.Empty);
		}

		private int CountPrimaryElementsInRow(int r)
		{
			return CountElementsInRow(r, CellStatus.PrimaryColor);
		}

		private int CountPrimaryElementsInColumn(int c)
		{
			return CountElementsInColumn(c, CellStatus.PrimaryColor);
		}

		private int CountSecondaryElementsInRow(int r)
		{
			return CountElementsInRow(r, CellStatus.SecondaryColor);
		}

		private int CountSecondaryElementsInColumn(int c)
		{
			return CountElementsInColumn(c, CellStatus.SecondaryColor);
		}

		private int CountElementsInRow(int r, CellStatus status)
		{
			int count = 0;

			for (int c = 0; c < Size; c++)
			{
				if (Cells[r][c] == status)
				{
					count++;
				}
			}

			return count;
		}

		private int CountElementsInColumn(int c, CellStatus status)
		{
			int count = 0;

			for (int r = 0; r < Size; r++)
			{
				if (Cells[r][c] == status)
				{
					count++;
				}
			}

			return count;
		}

		private void ResetSchema()
		{
			Cells = new CellStatus[Size][];

			for (int i = 0; i < Size; i++)
			{
				Cells[i] = new CellStatus[Size];
			}
		}

		public object Clone()
		{
			var schema = new Schema(Size);

			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					schema.Cells[r][c] = Cells[r][c];
				}
			}

			return schema;
		}
	}
}
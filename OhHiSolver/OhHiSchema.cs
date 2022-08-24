namespace brinux.ohhisolver
{
	public class OhHiSchema : ICloneable
	{
		public int Size { get; }
		public CellStatus[][] Cells { get; private set; }

		public OhHiSchema(int size)
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

		public OhHiSchema(int[][] cells)
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

		public int CountSetElementsInRow(int r)
		{
			return Size - CountElementsInRow(r, CellStatus.Empty);
		}

		public int CountSetElementsInColumn(int c)
		{
			return Size - CountElementsInColumn(c, CellStatus.Empty);
		}

		public int CountPrimaryElementsInRow(int r)
		{
			return CountElementsInRow(r, CellStatus.PrimaryColor);
		}

		public int CountPrimaryElementsInColumn(int c)
		{
			return CountElementsInColumn(c, CellStatus.PrimaryColor);
		}

		public int CountSecondaryElementsInRow(int r)
		{
			return CountElementsInRow(r, CellStatus.SecondaryColor);
		}

		public int CountSecondaryElementsInColumn(int c)
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
			var schema = new OhHiSchema(Size);

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
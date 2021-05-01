using System;

namespace Nordic.Abstractions.Data
{
    public class Matrix<T>
    {
        // -- fields

        /*
         * generic jagged array:
         * KEIN 2D-Array, weil diese langsamer sind, in manchen Fällen innere Schleifen benötigen, wo eine jagged array keins braucht UND
         * weil man leicht eine 3-Ecks Matrix erstellen kann, da es keine zeilenweisen Abhängigkeiten gibt.
         */
        protected T[][] _matrix;


        // -- properties

        /// <summary>
        /// Gets the behavior of iterating the matrix and if the elements on the main diagonal will be executed (true) or not (false).
        /// </summary>
        public bool IterateMainDiagonal { get; private set; }

        /// <summary>
        /// Gets the number of rows. To set this property perform the Init method.
        /// </summary>
        public int RowsCount { get; private set; }

        /// <summary>
        /// Gets the number of columns. To set this property perform the Init method.
        /// </summary>
        public int ColsCount { get; private set; }

        // -- indexer 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public T this[int row, int col]
        {
            get
            {
                return _matrix[row][col];
            }
        }

        // -- constructors

        #region constructor (2)

        public Matrix(bool iterateMainDiagonal = true)
        {
            IterateMainDiagonal = iterateMainDiagonal;
        }

        public Matrix(int rows, int cols, bool iterateMainDiagonal = true) : this(iterateMainDiagonal)
        {
            Init(rows, cols);
        }
        #endregion

        // -- methods

        /// <summary>
        /// Initializes the matrix (new) and sets all items to its default(T) value.
        /// Existing values will be overwritten.
        /// </summary>
        /// <param name="size">The size of the matrix</param>
        public void Init(int size)
        {
            Init(size, size);
        }

        /// <summary>
        /// Initializes the matrix (new) and sets all items to the given value.
        /// Existing values will be overwritten.
        /// </summary>
        /// <param name="size">The size of the matrix</param>
        /// <param name="value">the initial value for each item</param>
        public void Init(int size, T value)
        {
            Init(size, size, value);
        }

        /// <summary>
        /// (Re-)Initializes the matrix and sets all elements with default()T values of the defined type.
        /// Existent elements will be removed.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        public void Init(int rows, int cols)
        {
            Init(rows, cols, default);
        }

        /// <summary>
        /// (Re-)Initializes the matrix and sets all elements to the value of the defined type.
        /// Existent elements will be removed.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        /// <param name="value">The initial value for each element.</param>
        public void Init(int rows, int cols, T value)
        {
            RowsCount = rows;
            ColsCount = cols;

            _matrix = new T[RowsCount][];
            for (int r = 0; r < RowsCount; r++)
            {
                _matrix[r] = new T[ColsCount];
                for (int c = 0; c < ColsCount; c++)
                {
                    _matrix[r][c] = value;
                }
            }
        }

        /// <summary>
        /// The call of this method will cause that all Each iterations will skip elements,
        /// where the index of the row and the column are equal.
        /// </summary>
        public void SkipMainDiagonal()
        {
            IterateMainDiagonal = false;
        }

        /// <summary>
        /// The call of this method will cause that all Each iterations will execute functions on elements,
        /// where the index of the row and the column are equal.
        /// </summary>
        public void TakeMainDiagonal()
        {
            IterateMainDiagonal = true;
        }

        /// <summary>
        /// Runs a function on each element in the matrix.
        /// </summary>
        /// <param name="function">The delegate funktion needs the parameters  row:int, column:int, the value T and
        /// returns the the manipulated value of T of the matrix.</param>
        public virtual void Each(Func<int, int, T, T> function)
        {
            for (int r = 0; r < RowsCount; r++)
            {
                for (int c = 0; c < ColsCount; c++)
                {
                    if (!IterateMainDiagonal && r == c)
					{
                        continue;
					}
                    _matrix[r][c] = function(r, c, _matrix[r][c]);
                }
            }
        }

        /// <summary>
        /// Gets a single element from the matrix after a validation of the input parameters.
        /// </summary>
        /// <param name="rows">The index of the row.</param>
        /// <param name="cols">The index of the columns.</param>
        /// <returns>The value of the der Matrix</returns>
        public T Get(int row, int col)
        {
            Validate(row, col);
            return _matrix[row][col];
        }

        public T[] GetRow(int row)
        {
            ValidateRow(row);
            return _matrix[row];
        }

        public T[] GetCol(int col)
        {
            ValidateCol(col);

            T[] colArray = new T[RowsCount];
            for (int r = 0; r < RowsCount; r++)
            {
                colArray[r] = _matrix[r][col];
            }
            return colArray;
        }

        /// <summary>
        /// Ruft einen einzelnen Wert der Matrix ab. Die Hauptdiagonale beinhaltet stets die default(T) Werte des festgelegten Typs.
        /// </summary>
        /// <param name="row">Die Zeile</param>
        /// <param name="col">Die Spalte</param>
        /// <param name="value">Der zu setzende Wert vom Typ der Matrix</param>
        public void Set(int row, int col, T value)
        {
            Validate(row, col);
            _matrix[row][col] = value;
        }

        /// <summary>
        /// Prüft, ob der generische Datentyp eine Zahl ist.
        /// </summary>
        /// <returns>Bei Erfolg 'true', andernfalls 'false'</returns>
        public bool IsNumeric()
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public override string ToString() => $"Rows: {RowsCount} x Cols: {ColsCount}";

        // --- private methods

        private void Validate(int row, int col)
        {
            ValidateRow(row);
            ValidateCol(col);
        }

        private void ValidateRow(int row)
        {
            if (row < 0 || row > RowsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }
        }

        private void ValidateCol(int col)
        {
            if (col < 0 || col > ColsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(col));
            }
        }

    

    }
}

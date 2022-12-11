using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class Board
    {
        internal const int k_MinSize = 4;
        internal const int k_MaxSize = 6;
        private short m_CupleLeftMatchCounter;
        private short m_Height;
        private short m_Width;
        private CellTypeOfBoard[,] m_BoardToPlay;

        internal Board(short i_Height, short i_Width)
        {
            resetTheBoard(i_Height, i_Width);
        }

        private void resetTheBoard(short i_Height, short i_Width)
        {
            m_CupleLeftMatchCounter = (short)(i_Height * i_Width / 2);
            m_BoardToPlay = new CellTypeOfBoard[i_Height, i_Width];
            m_Height = i_Height;
            m_Width = i_Width;
            fillTheBoard();
        }

        private List<short> inputsToBoard()
        {
            List<short> inputsToBoard = new List<short>(m_Width * m_Height);

            for (short loopIndex = 0; loopIndex < (m_Width * m_Height) / 2; loopIndex++)
            {
                inputsToBoard.Add(loopIndex);
                inputsToBoard.Add(loopIndex);
            }

            return inputsToBoard;
        }

        private void fillTheBoard()
        {
            List<short> dataToFill = inputsToBoard();
            Random randIndex = new Random();
            int indexToRand;

            for (int outterIndex = 0; outterIndex < m_Height; outterIndex++)
            {
                for (int innerIndex = 0; innerIndex < m_Width; innerIndex++)
                {
                    indexToRand = randIndex.Next(0, dataToFill.Count);
                    m_BoardToPlay[outterIndex, innerIndex] = new CellTypeOfBoard(dataToFill[indexToRand]);
                    dataToFill.RemoveAt(indexToRand);
                }
            }
        }

        internal void ResetTheBoardToNewSize(short i_NewHeight, short i_NewWidht)
        {
            resetTheBoard(i_NewHeight, i_NewWidht);
        }

        internal short Height
        {
            get
            {
                return m_Height;
            }
        }

        internal short Width
        {
            get
            {
                return m_Width;
            }
        }

        internal short CupleCounter
        {
            get
            {
                return m_CupleLeftMatchCounter;
            }
        }

        internal short this[short i_Row, short i_Col]
        {
            get
            {
                return m_BoardToPlay[i_Row, i_Col].Data;
            }
        }

        internal bool CheckIfCellDiscovered(short i_RowIndex, short i_ColIndex)
        {
            bool v_IfCellDiscovered = false;

            v_IfCellDiscovered = m_BoardToPlay[i_RowIndex, i_ColIndex].Discovered;

            return v_IfCellDiscovered;
        }

        internal void SetCellToDiscoveredByIndex(short i_RowIndex, short i_ColIndex)
        {
            m_BoardToPlay[i_RowIndex, i_ColIndex].Discovered = true;
        }

        internal void SetDiscoveredStatusToFalse(short i_RowIndex, short i_ColIndex)
        {
            m_BoardToPlay[i_RowIndex, i_ColIndex].Discovered = false;
        }

        internal bool CheckIfMatchCells(short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            bool v_CheckIfMatch = false;

            v_CheckIfMatch = m_BoardToPlay[i_FirstRow, i_FirstCol].Data == m_BoardToPlay[i_SecondRow, i_SecondCol].Data;
            if (v_CheckIfMatch)
            {
                CupleWasFound();
            }

            return v_CheckIfMatch;
        }

        internal void CupleWasFound()
        {
            m_CupleLeftMatchCounter--;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class ComputerPlayer
    {
        internal const string k_ComputerName = "Computer";
        private short m_Score;
        private AIcells[] m_AImemory;
        private List<string> m_AvaliabelsTurns;

        internal ComputerPlayer(short i_Height, short i_Width)
        {
            resetComputerPlayer(i_Height, i_Width);
        }

        private void resetComputerPlayer(short i_Height, short i_Width)
        {
            m_Score = 0;
            m_AImemory = new AIcells[i_Height * i_Width];
            m_AvaliabelsTurns = new List<string>(i_Height * i_Width);
            resetTheMemory();
            ResetAvaliableTurn(i_Height, i_Width);
        }

        private void resetTheMemory()
        {
            for (int index = 0; index < m_AImemory.Length; index++)
            {
                m_AImemory[index] = new AIcells(AIcells.k_DefultValueToAiCells, AIcells.k_DefultValueToAiCells, AIcells.k_DefultValueToAiCells);
            }
        }

        internal short Score
        {
            get
            {
                return m_Score;
            }
        }

        internal void AddOneToScore()
        {
            m_Score++;
        }

        internal void AddValueToMemory(short i_Data, short i_RowIndex, short i_ColIndex)
        {
            if (!findIfDataExistsInMemory(i_Data, i_RowIndex, i_ColIndex))
            {
                if (m_AImemory[i_Data * 2].Data == AIcells.k_DefultValueToAiCells)
                {
                    m_AImemory[i_Data * 2].Data = i_Data;
                    m_AImemory[i_Data * 2].Row = i_RowIndex;
                    m_AImemory[i_Data * 2].Col = i_ColIndex;
                }
                else
                {
                    m_AImemory[(i_Data * 2) + 1].Data = i_Data;
                    m_AImemory[(i_Data * 2) + 1].Row = i_RowIndex;
                    m_AImemory[(i_Data * 2) + 1].Col = i_ColIndex;
                }
            }
        }

        internal void RemoveCupleFromValidComputerTurns(string i_FirstIndexToRemove, string i_SecondIndexToRemove)
        {
            m_AvaliabelsTurns.Remove(i_FirstIndexToRemove);
            m_AvaliabelsTurns.Remove(i_SecondIndexToRemove);
        }

        internal void GetRandomIndexesForComputerTurn(out string o_FirstStrIndexes, out short o_FirstRow, out short o_FirstCol, out string o_SeocndStrIndexes, out short o_SecondRow, out short o_SecondCol)
        {
            int firstIndexToRand, secondIndexToRand;

            firstIndexToRand = GameSystem.s_RandomChoice.Next(0, m_AvaliabelsTurns.Count);
            secondIndexToRand = GameSystem.s_RandomChoice.Next(0, m_AvaliabelsTurns.Count);
            while (firstIndexToRand == secondIndexToRand)
            {
                secondIndexToRand = GameSystem.s_RandomChoice.Next(0, m_AvaliabelsTurns.Count);
            }

            o_FirstStrIndexes = m_AvaliabelsTurns[firstIndexToRand];
            o_FirstCol = (short)(o_FirstStrIndexes[GameSystem.k_ColPositionInChoise] - 'A');
            o_FirstRow = (short)(char.GetNumericValue(o_FirstStrIndexes[GameSystem.k_RowPositionInChoise]) - 1);
            o_SeocndStrIndexes = m_AvaliabelsTurns[secondIndexToRand];
            o_SecondCol = (short)(o_SeocndStrIndexes[GameSystem.k_ColPositionInChoise] - 'A');
            o_SecondRow = (short)(char.GetNumericValue(o_SeocndStrIndexes[GameSystem.k_RowPositionInChoise]) - 1);
        }

        private void ResetAvaliableTurn(short i_RowIndex, short i_ColIndex)
        {
            string cellToAdd;

            for (int outterLoop = 0; outterLoop < i_RowIndex; outterLoop++)
            {
                for (int innerLoop = 0; innerLoop < i_ColIndex; innerLoop++)
                {
                    cellToAdd = string.Concat((char)(innerLoop + 'A'), (char)(outterLoop + '1'));
                    m_AvaliabelsTurns.Add(cellToAdd);
                }
            }
        }

        private bool IfCellInputAllreadyExistInMemory(short i_Data, short i_RowIndex, short i_ColIndex)
        {
            bool v_isExistInMemory = false;
            int dataInMemory = m_AImemory[i_Data * 2].Data;
            int rowInMemory = m_AImemory[i_Data * 2].Row;
            int colInMemory = m_AImemory[i_Data * 2].Col;

            if (dataInMemory == i_Data)
            {
                if (rowInMemory == i_RowIndex && colInMemory == i_ColIndex)
                {
                    v_isExistInMemory = true;
                }
            }

            return v_isExistInMemory;
        }

        private bool findIfDataExistsInMemory(short i_Data, short i_RowIndex, short i_ColIndex)
        {
            bool v_isExistInMemory = true;
            int dataInMemory = m_AImemory[i_Data * 2].Data;

            if (dataInMemory == AIcells.k_DefultValueToAiCells || !IfCellInputAllreadyExistInMemory(i_Data, i_RowIndex, i_ColIndex))
            {
                v_isExistInMemory = false;
            }

            return v_isExistInMemory;
        }

        private bool validTurnInAvaliableComputerTruns(short i_RowToCheck, short i_ColToCheck)
        {
            bool v_IfValidTurn = false;
            string cellToCheck = string.Concat((char)(i_ColToCheck + 'A'), (char)(i_RowToCheck + '1'));

            v_IfValidTurn = m_AvaliabelsTurns.Contains(cellToCheck);

            return v_IfValidTurn;
        }

        internal void RemoveFromComputerMemory(short i_DataToRemove)
        {
            m_AImemory[2 * i_DataToRemove].Data = AIcells.k_DefultValueToAiCells;
            m_AImemory[(2 * i_DataToRemove) + 1].Data = AIcells.k_DefultValueToAiCells;
        }

        internal bool HaveCupleMatchInMemory(out short o_RowFirst, out short o_ColFirst, out short o_RowSecond, out short o_ColSecond)
        {
            bool v_HaveMatchInMemory = false;

            o_RowFirst = o_ColFirst = o_RowSecond = o_ColSecond = AIcells.k_DefultValueToAiCells;
            for (int index = 0; index < m_AImemory.Length; index += 2)
            {
                if (m_AImemory[index].Data == m_AImemory[index + 1].Data && m_AImemory[index].Data != AIcells.k_DefultValueToAiCells)
                {
                    v_HaveMatchInMemory = true;
                    o_RowFirst = m_AImemory[index].Row;
                    o_ColFirst = m_AImemory[index].Col;
                    o_RowSecond = m_AImemory[index + 1].Row;
                    o_ColSecond = m_AImemory[index + 1].Col;
                    m_AImemory[index].Data = AIcells.k_DefultValueToAiCells;
                    m_AImemory[index + 1].Data = AIcells.k_DefultValueToAiCells;
                    break;
                }
            }

            return v_HaveMatchInMemory;
        }

        internal bool CheckIfMatchByData(short i_DataToFind, short i_RowToCompare, short i_ColToCompare, ref short io_GetRow, ref short io_GetCol)
        {
            bool v_HaveMatchByData = false;

            if (i_DataToFind == m_AImemory[i_DataToFind * 2].Data && !IfCellInputAllreadyExistInMemory(i_DataToFind, i_RowToCompare, i_ColToCompare))
            {
                io_GetRow = m_AImemory[i_DataToFind * 2].Row;
                io_GetCol = m_AImemory[i_DataToFind * 2].Col;
                v_HaveMatchByData = true;
            }

            return v_HaveMatchByData;
        }

        internal void ResetComputerToNewGame(short i_NewHeight, short i_NewWidth)
        {
            resetComputerPlayer(i_NewHeight, i_NewWidth);
        }

        internal void ResetScore()
        {
            m_Score = 0;
        }
    }
}
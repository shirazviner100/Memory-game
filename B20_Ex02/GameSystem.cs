using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class GameSystem
    {
        public enum ePlayer
        {
            ComputerPlayer = 0,
            HummanPlayer
        }

        public enum ePlayerTurn
        {
            FirstPlayer,
            SecondPlayer
        }

        internal const short k_ColPositionInChoise = 0;
        internal const short k_RowPositionInChoise = 1;
        internal const short k_GameOver = 0;
        public static Random s_RandomChoice = new Random();
        private Board m_GameBoard;
        private HumanPlayer m_FirstPlayer;
        private SecondPlayer m_SecondPlayer;

        public GameSystem(short i_Height, short i_Width, string i_FirstPlayerName, string i_SecondPlayerName, ePlayer i_WhoIsTheSecondPlaying)
        {
            m_GameBoard = new Board(i_Height, i_Width);
            m_FirstPlayer = new HumanPlayer(i_FirstPlayerName);
            m_SecondPlayer = new SecondPlayer(i_SecondPlayerName, i_Height, i_Width, i_WhoIsTheSecondPlaying);
        }

        internal HumanPlayer FirstPlayer
        {
            get
            {
                return m_FirstPlayer;
            }
        }

        internal SecondPlayer SecondGamePlayer
        {
            get
            {
                return m_SecondPlayer;
            }
        }

        internal Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        private void convertFromShortToStr(out string i_StrToConvert, short i_RowIndex, short i_ColIndex)
        {
            i_StrToConvert = string.Concat((char)(i_ColIndex + 'A'), (char)(i_RowIndex + '1'));
        }

        private bool randIfAiIsPlay()
        {
            bool v_AIplay = false;
            int choiceFromRand = GameSystem.s_RandomChoice.Next(0, 3);

            if (choiceFromRand != 0)
            {
                v_AIplay = true;
            }

            return v_AIplay;
        }

        public bool MakeComputerTurn(out short o_FirstRow, out short o_FirstCol, out short o_SecondRow, out short o_SecondCol)
        {
            bool v_IfPlayByMemory = true, v_IfComputerFoundMatch = false;
            short saveCupleCounter = m_GameBoard.CupleCounter;

            v_IfPlayByMemory = randIfAiIsPlay();
            if (v_IfPlayByMemory)
            {
                v_IfComputerFoundMatch = aiComputerMove(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
            }
            else
            {
                v_IfComputerFoundMatch = PlayWithoutAiMoves(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
            }

            if (v_IfComputerFoundMatch)
            {
                m_SecondPlayer.AddOneToScore();
                m_GameBoard.SetCellToDiscoveredByIndex(o_FirstRow, o_FirstCol);
                m_GameBoard.SetCellToDiscoveredByIndex(o_SecondRow, o_SecondCol);
                if (saveCupleCounter == m_GameBoard.CupleCounter)
                {
                    m_GameBoard.CupleWasFound();
                }
            }
            else
            {
                addTurnToAiMemory(o_FirstRow, o_FirstCol);
                addTurnToAiMemory(o_SecondRow, o_SecondCol);
            }

            return v_IfComputerFoundMatch;
        }

        private void addTurnToAiMemory(short i_RowIndex, short i_ColIndex)
        {
            short valueToAdd = m_GameBoard[i_RowIndex, i_ColIndex];

            m_SecondPlayer.Computer.AddValueToMemory(valueToAdd, i_RowIndex, i_ColIndex);
        }

        private bool PlayWithoutAiMoves(out short o_FirstRow, out short o_FirstCol, out short o_SecondRow, out short o_SecondCol)
        {
            string firstStrIndex, secondStrIndex;
            bool v_IfMatch = false;

            m_SecondPlayer.Computer.GetRandomIndexesForComputerTurn(out firstStrIndex, out o_FirstRow, out o_FirstCol, out secondStrIndex, out o_SecondRow, out o_SecondCol);
            v_IfMatch = m_GameBoard.CheckIfMatchCells(o_FirstRow, o_FirstCol, o_SecondRow, o_SecondCol);
            if (v_IfMatch)
            {
                m_SecondPlayer.Computer.RemoveCupleFromValidComputerTurns(firstStrIndex, secondStrIndex);
            }

            return v_IfMatch;
        }

        private bool asCupleInComputerMemoryMove(out short o_FirstRow, out short o_FirstCol, out short o_SecondRow, out short o_SecondCol)
        {
            bool v_HasCupleInMemory = false;
            string firstStrIndex, secondStrIndex;

            v_HasCupleInMemory = m_SecondPlayer.Computer.HaveCupleMatchInMemory(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
            if (v_HasCupleInMemory)
            {
                convertFromShortToStr(out firstStrIndex, o_FirstRow, o_FirstCol);
                convertFromShortToStr(out secondStrIndex, o_SecondRow, o_SecondCol);
                m_SecondPlayer.Computer.RemoveCupleFromValidComputerTurns(firstStrIndex, secondStrIndex);
            }

            return v_HasCupleInMemory;
        }

        private bool aiComputerMove(out short o_FirstRow, out short o_FirstCol, out short o_SecondRow, out short o_SecondCol)
        {
            bool v_AsMatch = false;

            v_AsMatch = asCupleInComputerMemoryMove(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
            if (!v_AsMatch)
            {
                if (randIfAiIsPlay())
                {
                    v_AsMatch = makeComputerTurnByOneRandom(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
                }
                else
                {
                    v_AsMatch = PlayWithoutAiMoves(out o_FirstRow, out o_FirstCol, out o_SecondRow, out o_SecondCol);
                }
            }

            return v_AsMatch;
        }

        private bool makeComputerTurnByOneRandom(out short o_FirstRow, out short o_FirstCol, out short o_SecondRow, out short o_SecondCol)
        {
            string firstStrIndex, secondStrIndex;
            short dataInCell;
            bool v_HasMatch = false;

            m_SecondPlayer.Computer.GetRandomIndexesForComputerTurn(out firstStrIndex, out o_FirstRow, out o_FirstCol, out secondStrIndex, out o_SecondRow, out o_SecondCol);
            dataInCell = m_GameBoard[o_FirstRow, o_FirstCol];
            v_HasMatch = m_SecondPlayer.Computer.CheckIfMatchByData(dataInCell, o_FirstRow, o_FirstCol, ref o_SecondRow, ref o_SecondCol);
            if (v_HasMatch)
            {
                convertFromShortToStr(out secondStrIndex, o_SecondRow, o_SecondCol);
                m_SecondPlayer.Computer.RemoveCupleFromValidComputerTurns(firstStrIndex, secondStrIndex);
            }
            else
            {
                v_HasMatch = m_GameBoard.CheckIfMatchCells(o_FirstRow, o_FirstCol, o_SecondRow, o_SecondCol);
                if (v_HasMatch)
                {
                    m_SecondPlayer.Computer.RemoveCupleFromValidComputerTurns(firstStrIndex, secondStrIndex);
                }
            }

            return v_HasMatch;
        }

        public void ResetTheGame(short i_NewHeight, short i_NewWidth)
        {
            m_GameBoard.ResetTheBoardToNewSize(i_NewHeight, i_NewWidth);
            m_FirstPlayer.ResetScore();
            m_SecondPlayer.ResetScore();
            m_SecondPlayer.ResetIfComputerPlay(i_NewHeight, i_NewWidth);
        }

        public bool CheckIfMatchAndAddScore(ePlayerTurn i_PlayerIndex, short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            bool v_CheckIfMatch = false;

            v_CheckIfMatch = GameBoard.CheckIfMatchCells(i_FirstRow, i_FirstCol, i_SecondRow, i_SecondCol);
            if (v_CheckIfMatch)
            {
                if (i_PlayerIndex == ePlayerTurn.FirstPlayer)
                {
                    FirstPlayer.AddOneToScore();
                    if (m_SecondPlayer.CheckIfComputer())
                    {
                        m_SecondPlayer.Computer.RemoveFromComputerMemory(m_GameBoard[i_FirstRow, i_FirstCol]);
                        removeFromComputerMemory(i_FirstRow, i_FirstCol, i_SecondRow, i_SecondCol);
                    }
                }
                else
                {
                    SecondGamePlayer.AddOneToScore();
                }
            }
            else
            {
                GameBoard.SetDiscoveredStatusToFalse(i_FirstRow, i_FirstCol);
                GameBoard.SetDiscoveredStatusToFalse(i_SecondRow, i_SecondCol);
            }

            return v_CheckIfMatch;
        }

        public bool CheckIfValidColChoise(string i_ChoiseToCheck, out short o_ColIndex)
        {
            bool v_IsValidRow = false;
            char boardLowerLimit = 'A', boardUpperLimit = (char)('A' + GameBoard.Width);
            o_ColIndex = -1; // defult value if the char isn`t number

            if (char.IsLetter(i_ChoiseToCheck[GameSystem.k_ColPositionInChoise]))
            {
                o_ColIndex = (short)(i_ChoiseToCheck[GameSystem.k_ColPositionInChoise] - boardLowerLimit);
                if (o_ColIndex < GameBoard.Width && o_ColIndex >= 0)
                {
                    v_IsValidRow = true;
                }
            }

            return v_IsValidRow;
        }

        public void InsertNewDataToComputerMemory(short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            short dataInCell;

            dataInCell = GameBoard[i_FirstRow, i_FirstCol];
            SecondGamePlayer.AddVlaueToComputerMemory(dataInCell, i_FirstRow, i_FirstCol);
            dataInCell = GameBoard[i_SecondRow, i_SecondCol];
            SecondGamePlayer.AddVlaueToComputerMemory(dataInCell, i_SecondRow, i_SecondCol);
        }

        public short GetValueByCellIndex(short i_RowIndex, short i_ColIndex)
        {
            return m_GameBoard[i_RowIndex, i_ColIndex];
        }

        public bool IfGameOver()
        {
            bool v_IfGameOver = true;

            v_IfGameOver = m_GameBoard.CupleCounter == GameSystem.k_GameOver;

            return v_IfGameOver;
        }

        public short GetFirstPlayerScore()
        {
            return FirstPlayer.Score;
        }

        public short GetSecondPlayerScore()
        {
            return SecondGamePlayer.Score;
        }

        public bool CheckIfMatchByIndexs(short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            bool v_IfMatch = false;

            v_IfMatch = m_GameBoard[i_FirstRow, i_FirstCol] == m_GameBoard[i_SecondRow, i_SecondCol];

            return v_IfMatch;
        }

        private void removeFromComputerMemory(short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            string firstIndex, secondIndex;

            firstIndex = string.Concat((char)(i_FirstCol + 'A'), (char)(i_FirstRow + '1'));
            secondIndex = string.Concat((char)(i_SecondCol + 'A'), (char)(i_SecondRow + '1'));

            m_SecondPlayer.Computer.RemoveCupleFromValidComputerTurns(firstIndex, secondIndex);
        }
    }
}
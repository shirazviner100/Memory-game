using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class SecondPlayer
    {
        private bool m_IsComputerPlayer = false;
        private HumanPlayer m_SecondHummanPlayer = null;
        private ComputerPlayer m_SecondPlayerComputer = null;

        internal SecondPlayer(string i_UserName, short i_Height, short i_Width, GameSystem.ePlayer i_WhoIsTheSecondPlayer)
        {
            if (i_WhoIsTheSecondPlayer == GameSystem.ePlayer.HummanPlayer)
            {
                m_SecondHummanPlayer = new HumanPlayer(i_UserName);
            }
            else
            {
                m_IsComputerPlayer = true;
                m_SecondPlayerComputer = new ComputerPlayer(i_Height, i_Width);
            }
        }

        internal void AddOneToScore()
        {
            if (m_IsComputerPlayer)
            {
                m_SecondPlayerComputer.AddOneToScore();
            }
            else
            {
                m_SecondHummanPlayer.AddOneToScore();
            }
        }

        internal bool CheckIfComputer()
        {
            return m_IsComputerPlayer;
        }

        internal string Name
        {
            get
            {
                if (m_IsComputerPlayer)
                {
                    return ComputerPlayer.k_ComputerName;
                }

                return m_SecondHummanPlayer.Name;
            }
        }

        internal ComputerPlayer Computer
        {
            get
            {
                return m_SecondPlayerComputer;
            }
        }

        internal void AddVlaueToComputerMemory(short i_DataToAdd, short i_RowIndex, short i_ColIdex)
        {
            m_SecondPlayerComputer.AddValueToMemory(i_DataToAdd, i_RowIndex, i_ColIdex);
        }

        internal short Score
        {
            get
            {
                if (m_IsComputerPlayer)
                {
                    return m_SecondPlayerComputer.Score;
                }

                return m_SecondHummanPlayer.Score;
            }
        }

        internal void ResetIfComputerPlay(short i_NewHeight, short i_NewWidth)
        {
            if (m_IsComputerPlayer)
            {
                m_SecondPlayerComputer.ResetComputerToNewGame(i_NewHeight, i_NewWidth);
            }
        }

        internal void ResetScore()
        {
            if (m_IsComputerPlayer)
            {
                m_SecondPlayerComputer.ResetScore();
            }
            else
            {
                m_SecondHummanPlayer.ResetScore();
            }
        }
    }
}
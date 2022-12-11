using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class HumanPlayer
    {
        internal string m_PlayerName;
        internal short m_Score;

        internal HumanPlayer(string i_PlayerName)
        {
            m_PlayerName = i_PlayerName;
            m_Score = 0;
        }

        internal string Name
        {
            get
            {
                return m_PlayerName;
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

        internal void ResetScore()
        {
            m_Score = 0;
        }
    }
}
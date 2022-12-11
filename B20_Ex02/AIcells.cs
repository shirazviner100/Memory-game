using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal class AIcells
    {
        internal const short k_DefultValueToAiCells = -1;
        private short m_ObjectToRemember;
        private short m_RowIndex;
        private short m_ColIndex;

        internal AIcells(short i_CellInput, short i_RowIndex, short i_ColIndex)
        {
            m_ObjectToRemember = i_CellInput;
            m_ColIndex = i_ColIndex;
            m_RowIndex = i_RowIndex;
        }

        internal short Data
        {
            get
            {
                return m_ObjectToRemember;
            }

            set
            {
                m_ObjectToRemember = value;
            }
        }

        internal short Row
        {
            get
            {
                return m_RowIndex;
            }

            set
            {
                m_RowIndex = value;
            }
        }

        internal short Col
        {
            get
            {
                return m_ColIndex;
            }

            set
            {
                m_ColIndex = value;
            }
        }
    }
}
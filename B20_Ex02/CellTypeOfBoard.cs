using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    internal struct CellTypeOfBoard
    {
        internal short m_DataInCell;
        internal bool m_IsDiscovered;

        internal CellTypeOfBoard(short i_DataToInsert)
        {
            m_DataInCell = i_DataToInsert;
            m_IsDiscovered = false;
        }

        internal short Data
        {
            get
            {
                return m_DataInCell;
            }
        }

        internal bool Discovered
        {
            get
            {
                return m_IsDiscovered;
            }

            set
            {
                m_IsDiscovered = value;
            }
        }
    }
}
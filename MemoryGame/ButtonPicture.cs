using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame
{
    public class ButtonPicture : PictureBox
    {
        private bool m_IfDiscovered = false;
        private short m_RowIndex;
        private short m_ColIndex;

        public ButtonPicture(short i_Row, short i_Col) : base()
        {
            m_RowIndex = i_Row;
            m_ColIndex = i_Col;
        }

        public short Row
        {
            get
            {
                return m_RowIndex;
            }
        }

        public short Col
        {
            get
            {
                return m_ColIndex;
            }
        }

        public bool Discovered
        {
            get
            {
                return m_IfDiscovered;
            }

            set
            {
                m_IfDiscovered = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class FormGameSetting : Form
    {
        public const byte k_MaxLenght = 6;
        public const byte k_MinLenght = 4;
        private short m_Height;
        private short m_Width;

        public FormGameSetting()
        {
            m_Height = k_MinLenght;
            m_Width = k_MinLenght;
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFirstPlayer.Text))
            {
                if (textBoxSecondPlayer.Enabled == true)
                {
                    if (!string.IsNullOrEmpty(textBoxSecondPlayer.Text))
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        DialogResult = DialogResult.No;
                    }
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                DialogResult = DialogResult.No;
            }

            this.Close();
        }

        private void buttonAgainstAFriend_Click(object sender, EventArgs e)
        {
            this.textBoxSecondPlayer.Enabled = true;
        }

        public string FirstPlayerName
        {
            get
            {
                return textBoxFirstPlayer.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                string nameToReturn = null;

                if (textBoxSecondPlayer.Enabled == true)
                {
                    nameToReturn = textBoxSecondPlayer.Text;
                }
                else
                {
                    nameToReturn = "Computer";
                }

                return nameToReturn;
            }
        }

        public bool IsComputerPlaying
        {
            get
            {
                bool v_IsComputer = false;

                v_IsComputer = textBoxSecondPlayer.Enabled == false;

                return v_IsComputer;
            }
        }

        public short Height
        {
            get
            {
                return m_Height;
            }
        }

        public short Width
        {
            get
            {
                return m_Width;
            }
        }

        private void buttonSize_Click(object sender, EventArgs e)
        {
            if (Width >= k_MaxLenght)
            {
                m_Width = k_MinLenght;
                m_Height++;
            }
            else
            {
                m_Width++;
            }

            if (Height > k_MaxLenght)
            {
                m_Height = k_MinLenght;
            }

            if(Height % 2 == 1 && Width % 2 == 1)
            {
                m_Width++;
            }

            this.buttonSize.Text = string.Concat(m_Height.ToString(), " X ", m_Width.ToString());
        }
    }
}

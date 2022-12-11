using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using B20_Ex02;

namespace MemoryGame
{
    public class FormMemoryGame : Form
    {
        private const byte k_MaxIconsQuantity = 18;
        private static Random s_RandIconIndex = new Random();
        private readonly FormGameSetting r_FormSetting;
        private List<PictureBox> m_IconsArray;
        private bool m_IsFirstButton = true;
        private ButtonPicture m_FirstButtonClicked;
        private ButtonPicture m_SecondButtonClicked;
        private B20_Ex02.GameSystem m_GameSystem;
        private System.ComponentModel.IContainer components = null;
        private List<ButtonPicture> m_ButtonsGame;
        private Label m_LabelFirstPlayer;
        private Label m_LabelCurrentPlayer;
        private Label m_LabelSecondPlayer;
        private System.Windows.Forms.Timer m_TimerToPictureButton;
        private System.Windows.Forms.Timer m_TimerToGameOver;

        public FormMemoryGame()
        {
            r_FormSetting = new FormGameSetting();
            initializeSetting();
            m_IconsArray = new List<PictureBox>(IconsQuantity / 2);
            initArryOfRandomIcons();
            initGameSystem();
            m_ButtonsGame = new List<ButtonPicture>(r_FormSetting.Width * r_FormSetting.Height);
            InitializeComponent();
        }

        protected override void Dispose(bool i_Disposing)
        {
            if (i_Disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(i_Disposing);
        }

        private void initPlayerLabels()
        {
            m_LabelFirstPlayer.Text = string.Format("{0}: {1} pair(s)", r_FormSetting.FirstPlayerName, m_GameSystem.GetFirstPlayerScore());
            m_LabelCurrentPlayer.BackColor = m_LabelFirstPlayer.BackColor;
            m_LabelCurrentPlayer.Text = string.Format("Current Player: {0}", r_FormSetting.FirstPlayerName);
            m_LabelSecondPlayer.Text = string.Format("{0}: {1} pair(s)", r_FormSetting.SecondPlayerName, m_GameSystem.GetSecondPlayerScore());
        }

        private void initTheGame()
        {
            m_IconsArray = new List<PictureBox>(IconsQuantity / 2);
            ButtonPicture currentButtonToChange;

            foreach(System.Windows.Forms.Control control in this.Controls)
            {
                if(control is ButtonPicture)
                {
                    currentButtonToChange = control as ButtonPicture;
                    currentButtonToChange.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                    currentButtonToChange.Image = null;
                    currentButtonToChange.Discovered = false;
                }
            }

            m_FirstButtonClicked = null;
            m_SecondButtonClicked = null;
            m_IsFirstButton = true;
            m_GameSystem.ResetTheGame(r_FormSetting.Height, r_FormSetting.Width);
            initPlayerLabels();
            initArryOfRandomIcons();
        }

        private void initGameOverMessageBox()
        {
            string message = getWiningMessage();
            string Title = "Game Over";
            MessageBoxButtons buttonToMessageBox = MessageBoxButtons.YesNo;
            DialogResult messageResult;

            messageResult = MessageBox.Show(message, Title, buttonToMessageBox);
            if (messageResult == DialogResult.Yes)
            {
                initTheGame();
            }
            else
            {
                this.Close();
            }
        }

        private string getWiningMessage()
        {
            string message;

            if (m_GameSystem.GetFirstPlayerScore() > m_GameSystem.GetSecondPlayerScore())
            {
                message = string.Format(
                    "Congratulations {0} you won{1}Do you want to start new game?",
                    r_FormSetting.FirstPlayerName, 
                    Environment.NewLine);
            }
            else if (m_GameSystem.GetFirstPlayerScore() < m_GameSystem.GetSecondPlayerScore())
            {
                message = string.Format(
                  "Congratulations {0} you won{1}Do you want to start new game?",
                  r_FormSetting.SecondPlayerName, 
                  Environment.NewLine);
            }
            else
            {
                message = string.Format(
                 "Congratulations you both won{0}Do you want to start new game?",
                 Environment.NewLine);
            }

            return message;
        }

        private void initGameSystem()
        {
            if (r_FormSetting.IsComputerPlaying)
            {
                m_GameSystem = new GameSystem(r_FormSetting.Height, r_FormSetting.Width, r_FormSetting.FirstPlayerName, r_FormSetting.SecondPlayerName, GameSystem.ePlayer.ComputerPlayer);
            }
            else
            {
                m_GameSystem = new GameSystem(r_FormSetting.Height, r_FormSetting.Width, r_FormSetting.FirstPlayerName, r_FormSetting.SecondPlayerName, GameSystem.ePlayer.HummanPlayer);
            }
        }

        private void initializeSetting()
        {
            r_FormSetting.ShowDialog();
            if (r_FormSetting.DialogResult == DialogResult.Cancel)
            {
                Environment.Exit(0);
            }

            while(r_FormSetting.DialogResult != DialogResult.OK)
            {
                r_FormSetting.ShowDialog();
                if (r_FormSetting.DialogResult == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }
            }
        }

        public int IconsQuantity
        {
            get
            {
                return r_FormSetting.Height * r_FormSetting.Width;
            }
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.components = new System.ComponentModel.Container();
            m_TimerToPictureButton = new System.Windows.Forms.Timer(this.components);
            m_TimerToGameOver = new System.Windows.Forms.Timer(this.components);
            ButtonPicture buttonToAdd;
            short row, col, leftSize = 12, upperSize = 12;
            int boardWidth, boardHeight;

            row = r_FormSetting.Height;
            col = r_FormSetting.Width;
            this.SuspendLayout();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.m_TimerToPictureButton.Interval = 1200;
            this.m_TimerToPictureButton.Tick += new System.EventHandler(this.timerToPictureButton_Tick);
            m_TimerToGameOver.Interval = 1600;
            m_TimerToGameOver.Tick += timerToGameOver_Tick;
            for (short outterLoop = 0; outterLoop < row; outterLoop++)
            {
                for (short innerLoop = 0; innerLoop < col; innerLoop++)
                {
                    buttonToAdd = new ButtonPicture(outterLoop, innerLoop);
                    buttonToAdd.Name = string.Concat("PictureButton", (char)('A' + innerLoop), outterLoop + 1);
                    buttonToAdd.Size = new System.Drawing.Size(100, 100);
                    buttonToAdd.TabIndex = outterLoop + innerLoop;
                    buttonToAdd.BackgroundImageLayout = ImageLayout.Center;
                    buttonToAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                    buttonToAdd.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                    buttonToAdd.Top = upperSize;
                    buttonToAdd.Left = leftSize;
                    buttonToAdd.Click += buttonPicture_Click;
                    leftSize += 112;
                    this.Controls.Add(buttonToAdd);
                    m_ButtonsGame.Add(buttonToAdd);
                }

                upperSize += 112;
                leftSize = 12;
            }

            m_LabelCurrentPlayer = new Label();
            m_LabelCurrentPlayer.AutoSize = true;
            m_LabelCurrentPlayer.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            m_LabelCurrentPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 177);
            m_LabelCurrentPlayer.Name = "labelCurrentPlayer";
            m_LabelCurrentPlayer.Text = "Current Player: ";
            m_LabelCurrentPlayer.Top = upperSize;
            m_LabelCurrentPlayer.Left = 12;
            m_LabelFirstPlayer = new Label();
            upperSize += 30;
            m_LabelFirstPlayer.AutoSize = true;
            m_LabelFirstPlayer.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            m_LabelFirstPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 177);
            m_LabelFirstPlayer.Name = "labelFirstPlayer";
            m_LabelFirstPlayer.Text = "First";
            m_LabelFirstPlayer.Top = upperSize;
            m_LabelFirstPlayer.Left = 12;
            m_LabelSecondPlayer = new Label();
            upperSize += 30;
            m_LabelSecondPlayer.AutoSize = true;
            m_LabelSecondPlayer.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            m_LabelSecondPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 177);
            m_LabelSecondPlayer.Name = "labelSecondPlayer";
            m_LabelSecondPlayer.Text = "Second";
            m_LabelSecondPlayer.Top = upperSize;
            m_LabelSecondPlayer.Left = 12;
            upperSize += 30;

            this.Controls.Add(m_LabelCurrentPlayer);
            this.Controls.Add(m_LabelFirstPlayer);
            this.Controls.Add(m_LabelSecondPlayer);
            boardWidth = (col * 112) + 12;
            boardHeight = upperSize;
            this.ClientSize = new System.Drawing.Size(boardWidth, boardHeight);
            initPlayerLabels();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.MaximizeBox = false;
        }

        private void timerToGameOver_Tick(object i_Sender, EventArgs i_EventArgument)
        {
            m_TimerToGameOver.Stop();
            initGameOverMessageBox();
        }

        private List<PictureBox> initIconsArray()
        {
            List<PictureBox> iconArray = new List<PictureBox>(k_MaxIconsQuantity);
            int idPictureByRand, upperLimitRand = 10, lowerLimitRand = 5;
            PictureBox pictureToAdd;

            for(int loopIndex = 0; loopIndex < k_MaxIconsQuantity; loopIndex++)
            {
                idPictureByRand = s_RandIconIndex.Next(lowerLimitRand, upperLimitRand);
                if(idPictureByRand == 86 || idPictureByRand == 97)
                {
                    idPictureByRand += 20;
                }

                pictureToAdd = new PictureBox();
                pictureToAdd.Size = new Size(100, 100);
                pictureToAdd.Load(string.Concat(@"https://picsum.photos/id/", idPictureByRand, "/80/80.jpg"));
                iconArray.Add(pictureToAdd);
                upperLimitRand += 5;
                lowerLimitRand += 5;
            }

            return iconArray;
        }

        private void initArryOfRandomIcons()
        {
            int indexByRandom;
            List<PictureBox> iconList = initIconsArray();

            for(int index = 0; index < IconsQuantity / 2; index++)
            {
                indexByRandom = s_RandIconIndex.Next(iconList.Count);
                m_IconsArray.Add(iconList[indexByRandom]);
                iconList.RemoveAt(indexByRandom);
            }
        }

        private void makeTurnForFirstButton(ButtonPicture i_Sender)
        {
            m_FirstButtonClicked = i_Sender;
            m_IsFirstButton = false;
            i_Sender.Image = m_IconsArray[m_GameSystem.GetValueByCellIndex(i_Sender.Row, i_Sender.Col)].Image;
        }

        private void makeTurnForSecondButton(ButtonPicture i_Sender)
        {
            m_SecondButtonClicked = i_Sender;
            m_IsFirstButton = true;
            i_Sender.Image = m_IconsArray[m_GameSystem.GetValueByCellIndex(i_Sender.Row, i_Sender.Col)].Image;
            this.m_TimerToPictureButton.Start();
        }

        private void timerWasTIcked()
        {
            GameSystem.ePlayerTurn playerTurn;

            playerTurn = getPlayerTurn();
            if (playerTurn == GameSystem.ePlayerTurn.FirstPlayer)
            {
                timerTickedByHumman(playerTurn);
            }
            else if(!r_FormSetting.IsComputerPlaying)
            {
                timerTickedByHumman(playerTurn);
            }
            else if(!m_IsFirstButton)
            {
                computerSecondButton();
            }
            else
            {
                timerTickedByComputer(playerTurn);
            }
        }

        private void changeCupleBackroung()
        {
            if(getPlayerTurn() == GameSystem.ePlayerTurn.FirstPlayer)
            {
                m_FirstButtonClicked.BackColor = m_LabelFirstPlayer.BackColor;
                m_SecondButtonClicked.BackColor = m_LabelFirstPlayer.BackColor;
            }
            else
            {
                m_FirstButtonClicked.BackColor = m_LabelSecondPlayer.BackColor;
                m_SecondButtonClicked.BackColor = m_LabelSecondPlayer.BackColor;
            }
        }

        private void timerTickedByComputer(GameSystem.ePlayerTurn i_PlayerTurn)
        {
            bool v_IfCupleMatch = false;
            short firstRow = m_FirstButtonClicked.Row, firstCol = m_FirstButtonClicked.Col;
            short secondRow = m_SecondButtonClicked.Row, secondCol = m_SecondButtonClicked.Col;

            v_IfCupleMatch = m_GameSystem.CheckIfMatchByIndexs(firstRow, firstCol, secondRow, secondCol);
            if (v_IfCupleMatch)
            {
                m_FirstButtonClicked.Discovered = true;
                m_SecondButtonClicked.Discovered = true;
                changeCupleBackroung();
                updatePlayerScoreLabel(GameSystem.ePlayerTurn.SecondPlayer);
                if (m_GameSystem.IfGameOver())
                {
                    m_TimerToGameOver.Start();
                }
                else
                {
                    m_FirstButtonClicked = null;
                    m_SecondButtonClicked = null;
                    makeComputerButtonsTurn();
                }
            }
            else
            {
                changeLableValue(GameSystem.ePlayerTurn.SecondPlayer);
                m_FirstButtonClicked.Image = null;
                m_SecondButtonClicked.Image = null;
                m_FirstButtonClicked = null;
                m_SecondButtonClicked = null;
            }
        }

        private void updatePlayerScoreLabel(GameSystem.ePlayerTurn i_PlayerTurn)
        {
            if(i_PlayerTurn == GameSystem.ePlayerTurn.FirstPlayer)
            {
                m_LabelFirstPlayer.Text = string.Format("{0}: {1} pairs", r_FormSetting.FirstPlayerName, m_GameSystem.GetFirstPlayerScore());
            }
            else
            {
                m_LabelSecondPlayer.Text = string.Format("{0}: {1} pairs", r_FormSetting.SecondPlayerName, m_GameSystem.GetSecondPlayerScore());
            }
        }

        private void timerTickedByHumman(GameSystem.ePlayerTurn i_PlayerTurn)
        {
            short firstRow, firstCol, secondRow, secondCol;

            firstRow = m_FirstButtonClicked.Row;
            firstCol = m_FirstButtonClicked.Col;
            secondRow = m_SecondButtonClicked.Row;
            secondCol = m_SecondButtonClicked.Col;
            if (m_GameSystem.CheckIfMatchAndAddScore(i_PlayerTurn, firstRow, firstCol, secondRow, secondCol))
            {
                m_FirstButtonClicked.Discovered = true;
                m_SecondButtonClicked.Discovered = true;
                changeCupleBackroung();
                updatePlayerScoreLabel(i_PlayerTurn);
                if (m_GameSystem.IfGameOver())
                {
                    m_TimerToGameOver.Start();
                }
            }
            else
            {
                changeLableValue(i_PlayerTurn);
                m_FirstButtonClicked.Image = null;
                m_SecondButtonClicked.Image = null;
            }

            m_FirstButtonClicked = null;
            m_SecondButtonClicked = null;
            if (r_FormSetting.IsComputerPlaying && m_LabelCurrentPlayer.BackColor != m_LabelFirstPlayer.BackColor)
            {
                makeComputerButtonsTurn();
            }
        }

        private void changeLableValue(GameSystem.ePlayerTurn i_PlayerTurn)
        {
            if(i_PlayerTurn == GameSystem.ePlayerTurn.FirstPlayer)
            {
                m_LabelFirstPlayer.Text = string.Format("{0}: {1} pairs", r_FormSetting.FirstPlayerName, m_GameSystem.GetFirstPlayerScore());
                m_LabelCurrentPlayer.BackColor = m_LabelSecondPlayer.BackColor;
                m_LabelCurrentPlayer.Text = string.Format("Current Player: {0}", r_FormSetting.SecondPlayerName);
            }
            else
            {
                m_LabelSecondPlayer.Text = string.Format("{0}: {1} pairs", r_FormSetting.SecondPlayerName, m_GameSystem.GetSecondPlayerScore());
                m_LabelCurrentPlayer.BackColor = m_LabelFirstPlayer.BackColor;
                m_LabelCurrentPlayer.Text = string.Format("Current Player: {0}", r_FormSetting.FirstPlayerName);
            }
        }

        private void makeComputerButtonsTurn()
        {
            short firstRow, firstCol, secondRow, secondCol;

            m_GameSystem.MakeComputerTurn(out firstRow, out firstCol, out secondRow, out secondCol);
            convertComputerButtonsByIndexs(firstRow, firstCol, secondRow, secondCol);
            m_FirstButtonClicked.Image = m_IconsArray[m_GameSystem.GetValueByCellIndex(firstRow, firstCol)].Image;
            m_IsFirstButton = false;
            m_TimerToPictureButton.Start();
        }

        private void computerSecondButton()
        {
            short rowIndex = m_SecondButtonClicked.Row, colIndex = m_SecondButtonClicked.Col;

            m_IsFirstButton = true;
            m_SecondButtonClicked.Image = m_IconsArray[m_GameSystem.GetValueByCellIndex(rowIndex, colIndex)].Image;
            this.m_TimerToPictureButton.Start();
        }

        private void convertComputerButtonsByIndexs(short i_FirstRow, short i_FirstCol, short i_SecondRow, short i_SecondCol)
        {
            ButtonPicture currentButtonInLoop;

            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                if (control is ButtonPicture)
                {
                    currentButtonInLoop = control as ButtonPicture;
                    if (currentButtonInLoop.Row == i_FirstRow && currentButtonInLoop.Col == i_FirstCol)
                    {
                        m_FirstButtonClicked = currentButtonInLoop;
                    }
                    else if (currentButtonInLoop.Row == i_SecondRow && currentButtonInLoop.Col == i_SecondCol)
                    {
                        m_SecondButtonClicked = currentButtonInLoop;
                    }

                    if(m_FirstButtonClicked != null && m_SecondButtonClicked != null)
                    {
                        break;
                    }
                }
            }
        }

        private GameSystem.ePlayerTurn getPlayerTurn()
        {
            GameSystem.ePlayerTurn playerTrun;

            if (m_LabelCurrentPlayer.BackColor == Color.FromArgb(192, 255, 192))
            {
                playerTrun = GameSystem.ePlayerTurn.FirstPlayer;
            }
            else
            {
                playerTrun = GameSystem.ePlayerTurn.SecondPlayer;
            }

            return playerTrun;
        }

        private void buttonPicture_Click(object i_Sender, EventArgs i_EventArgument)
        {
            if (!(getPlayerTurn() == GameSystem.ePlayerTurn.SecondPlayer && r_FormSetting.IsComputerPlaying))
            {
                if (m_FirstButtonClicked == null || m_SecondButtonClicked == null)
                {
                    ButtonPicture senderButtonPicture = i_Sender as ButtonPicture;

                    if (!senderButtonPicture.Discovered)
                    {
                        if (m_IsFirstButton)
                        {
                            makeTurnForFirstButton(senderButtonPicture);
                        }
                        else
                        {
                            makeTurnForSecondButton(senderButtonPicture);
                        }
                    }
                }
            }
        }

        private void timerToPictureButton_Tick(object i_Sender, EventArgs i_EventArgument)
        {
            this.m_TimerToPictureButton.Stop();
            timerWasTIcked();
        }
    }
}

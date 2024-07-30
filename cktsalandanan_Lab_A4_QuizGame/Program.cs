/*
 * Name: Salandanan, Christian Kerby T.
 * Date: November 17, 2023
 * Description: A Quiz Game program that practices the concept of OOP specifically Class structures and implementation of Winforms GUI.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cktsalandanan_Lab_A4_QuizGame
{
    internal class Program
    {
        static void Main()
        {
            GenFrame ui = new GenFrame(); //Creating instance of the GUI class
            Application.Run(ui); //running the GUI
        }
    }

    class GenFrame : Form //GUI class inheriting Form class
    {
        Questions item = new Questions(); //created a new instance of question class.
        Player player; 

        private Panel levelPanel;
        public string selectedLevel;

        private Panel gamePanel;
        Label questionType; 
        public static Label questionLabel;
        Label feedbackLabel;
        Button choice1Btn;
        Button choice2Btn;
        Button choice3Btn;
        Label scoreLabel;

        private Panel playerPanel;
        public TextBox userNameField;
        private Button submitButton;

        private Panel scoreBoardPanel;
        public Label playerLabel;
        public Button replayBtn;
        public Button exitBtn;

        public static List<string> occurredQuestions = new List<string>(); //checks if the question to be display already occurred
        public string[] questionBank = new string[25];
        public string[,] playerStats = new string[10, 2];
        public int levelScore; //holds the value of score per level
        public int gameScore;
        public int questionNum = 0; //to monitor sa questions and display question number

        public GenFrame() //constructor, start of sequence, creating the window 
        {
            Text = "Quiz Game";
            Size = new Size(380, 500); //setting size for window
            FormBorderStyle = FormBorderStyle.FixedDialog; //making the window edge curve
            InitializeLevelPanel(); //after calling constructor, the constructor will invoke the levelpanel method 
        }

        public void InitializeLevelPanel() //this method will create the panel and controls on the level panel
        {
            levelPanel = new Panel();
            levelPanel.Size = new Size(380, 500);
            levelPanel.BackColor = Color.CornflowerBlue; //sets background panel color
            Controls.Add(levelPanel); //adding the panel to window controls

            Label gameTitle = new Label(); //create instance for label gametitle
            gameTitle.Text = "Quiz Game"; //set the label text to Quiz Game
            gameTitle.Size = new Size(180, 30); //sets the label size
            gameTitle.Font = new Font("Arial", 20, FontStyle.Bold); //sets the label font style and size
            gameTitle.Location = new Point(105, 30); //sets the location of theb label on the panel

            Label levelLabel = new Label(); 
            levelLabel.Text = "Select Difficulty";
            levelLabel.Size = new Size(180, 25);
            levelLabel.Font = new Font("Arial", 16);
            levelLabel.Location = new Point(100, 170);

            Button easyBtn = new Button();
            easyBtn.Text = "EASY";
            easyBtn.Size = new Size(100, 30);
            easyBtn.Font = new Font("Arial", 10);
            easyBtn.Location = new Point(20, 250);

            Button aveBtn = new Button();
            aveBtn.Text = "AVERAGE";
            aveBtn.Size = new Size(100, 30);
            aveBtn.Font = new Font("Arial", 10);
            aveBtn.Location = new Point(130, 250);

            Button diffBtn = new Button();
            diffBtn.Text = "DIFFICULT";
            diffBtn.Size = new Size(100, 30);
            diffBtn.Font = new Font("Arial", 10);
            diffBtn.Location = new Point(240, 250);

            easyBtn.Click += btnLevelHandler; //this 3 lines of code handles the events when the user clicked any of the 3 buttons,
            aveBtn.Click += btnLevelHandler; //when clicked, the btnLevelHandler method will be invoke
            diffBtn.Click += btnLevelHandler;

            levelPanel.Controls.Add(gameTitle); //line 110 to 114, adding the controls to the level panel after adding the level panel to window controls
            levelPanel.Controls.Add(levelLabel);
            levelPanel.Controls.Add(easyBtn);
            levelPanel.Controls.Add(aveBtn);
            levelPanel.Controls.Add(diffBtn);
        }

        public void btnLevelHandler(object sender, EventArgs e) //this method will do what the clicked buttons intended to do
        {
            Button clickedButton = sender as Button; //this checks and cast the sender, if the sender is a button then it will create an object button that references to the object sender

            if (clickedButton != null) //if not null meaning successfully casted, the text property of the button will be pass as an argument to the loadLevel method
                loadLevel(clickedButton.Text);
        }

        public void loadLevel(string level) //this method will load the selected difficulty
        {
            Questions items = new Questions();
            player = new Player(); //instantiated a new player instance when loading the selected game diff, meaning theres a new player/round

            questionNum = 0; //resets the question num 
            gameScore = 0; //resets the score
            occurredQuestions.Clear(); //clears the list so that when another round for the same diff played, all of the questions can be displayed again

            switch (level)
            {
                case "EASY": selectedLevel = items.EasyQ; levelScore = 1; break; //this switch statement sets the string for selected level and the score for that level
                case "AVERAGE": selectedLevel = items.AveQ; levelScore = 2; break;
                case "DIFFICULT": selectedLevel = items.DiffQ; levelScore = 3; break;
            }

            questionBank = Questions.PopulateQuestions(selectedLevel); //passsess the selected level string as argument for this method

            levelPanel.Visible = false; //this is the transition after the the player selected a level, the level panel will not be visible while the gamepanel will be visible
            InitializeGamePanel(); //claing the function to initialize gamepanel itself and its controls
            gamePanel.Visible = true;
        }

        public void InitializeGamePanel() //this methods creates the gamepanel and controls 
        {
            gamePanel = new Panel();
            gamePanel.Size = new Size(380, 500);
            gamePanel.BackColor = Color.CornflowerBlue;
            Controls.Add(gamePanel);

            questionType = new Label(); //this label instance will display the question number
            questionType.Size = new Size(180,50);
            questionType.Font = new Font("Arial",20);
            questionType.Location = new Point(105,60);

            scoreLabel = new Label(); //this instance is for the label that will display score in the game round
            scoreLabel.ForeColor = Color.DarkBlue;
            scoreLabel.Size = new Size(170, 25);
            scoreLabel.Font = new Font("Arial", 14);
            scoreLabel.Location = new Point(5,5);

            questionLabel = new Label(); //this instance is for the label that will display the question per round
            questionLabel.BackColor = Color.Beige;
            questionLabel.BorderStyle = BorderStyle.FixedSingle;
            questionLabel.Size = new Size(180, 35);
            questionLabel.Font = new Font("Arial", 25);
            questionLabel.Location = new Point(90, 120);

            feedbackLabel = new Label(); //this instance is for the label that will display the feedback after the player selected a button for answer choices
            feedbackLabel.ForeColor = Color.LightYellow;
            feedbackLabel.Size = new Size(215,45);
            feedbackLabel.Font = new Font("Arial", 14);
            feedbackLabel.Location = new Point(70, 180);
            
            choice1Btn = new Button(); //instance to display and hold the answer choice 1
            choice1Btn.Size = new Size(355, 30);
            choice1Btn.Font = new Font("Arial", 14);
            choice1Btn.Location = new Point(5, 250);

            choice2Btn = new Button(); //instance to display and hold the answer choice 2
            choice2Btn.Size = new Size(355, 30);
            choice2Btn.Font = new Font("Arial", 14);
            choice2Btn.Location = new Point(5, 290);

            choice3Btn = new Button(); //instance to display and hold the answer choice 3
            choice3Btn.Size = new Size(355, 30);
            choice3Btn.Font = new Font("Arial", 14);
            choice3Btn.Location = new Point(5, 330);

            choice1Btn.Click += btnAnsHandler; //lines 194 to 196, these lines will invoke the method if the button answer choices were clicked
            choice2Btn.Click += btnAnsHandler;
            choice3Btn.Click += btnAnsHandler;

            gamePanel.Controls.Add(scoreLabel);
            gamePanel.Controls.Add(questionType);
            gamePanel.Controls.Add(questionLabel);
            gamePanel.Controls.Add(feedbackLabel);
            gamePanel.Controls.Add(choice1Btn);
            gamePanel.Controls.Add(choice2Btn);
            gamePanel.Controls.Add(choice3Btn);

            UpdateQuestions(); //this method will set the values for the question label and 3 answer button choices
        }

        public void btnAnsHandler(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button; 

            if (clickedButton != null)
            {
                ++questionNum; //when clicked, the questionnum will increment
                player.DetermineScore(item, clickedButton.Text, levelScore); //using player instance to invoke the determine score method to check the value of the button that was clicked, the button value and levelscore is passed as an argument
                UpdateQuestions(); //when the answer button was clicked, this method will update the set of questions to be display in the question label and 3 button choices
            }

            if (questionNum < questionBank.Length / 5) // if question num is less than the quesbank len divided by 5 which is equals to the number of questions per level, the gamepanel will retain its visibility.
            {
                gamePanel.Visible = true;
            }
            else //else if the questionum is not less than the numbers of questions per level, the transition begins. set the visibility of gamepanel to false and invoke the creation of player panel and set its panel visibility to true
            {
                gamePanel.Visible = false;
                InitializePlayerPanel();
                playerPanel.Visible = true;
            }
        }

        public void UpdateQuestions() //this method updates the questionLabel text value and buttons text value when called
        {
            item.GenerateQuestion(questionBank); //this method generates questions, passed the question bank as an argument
            scoreLabel.Text = $"Current Score: {player.Score}"; //updates the text of the score label to display the current player score.
            questionType.Text = $"Question {questionNum + 1}"; //updates the question number label 
            feedbackLabel.Text = player.FeedBack; //sets the feedback for correct and incorrect answers
            Questions.DisplayQuestions(item); //invokes the static method to set and update the questionLabel text
            choice1Btn.Text = item.Choices[0]; //index the 1st element in the choices array and display it in the choice 1 btn
            choice2Btn.Text = item.Choices[1]; //index the 2nd element in the choices array and display it in the choice 2 btn
            choice3Btn.Text = item.Choices[2]; //index the 3rd element in the choices array and display it in the choice 3 btn
            occurredQuestions.Add(item.Question); //when the selected question was displayed, it will be added to the list so that it wont be selected on the generatequestion method again
        }

        public void InitializePlayerPanel() //after the gamepanel, this method if invoked will create instances for player panel
        {
            playerPanel = new Panel();
            playerPanel.Size = new Size(380, 500);
            playerPanel.BackColor = Color.CornflowerBlue;
            Controls.Add(playerPanel);

            Label completionLabel = new Label();
            completionLabel.Text = "Well Played!";
            completionLabel.Size = new Size(220,45);
            completionLabel.Font = new Font("Arial", 20);
            completionLabel.Location = new Point(90, 120);

            userNameField = new TextBox(); //textbox instance that will be accountable in asking and receiving the player's username
            userNameField.Text = "Enter Player Name"; //a prompt in the textbox, when click it will disappear, will explain later
            userNameField.ForeColor = Color.LightGray;
            userNameField.Font = new Font("Arial",20);
            userNameField.Size = new Size(320,40);
            userNameField.Location = new Point(20,250);

            submitButton = new Button(); //button to submit name and to initialize next panel 
            submitButton.Text = "SUBMIT";
            submitButton.Font = new Font("Arial",15);
            submitButton.Size = new Size(320,35);
            submitButton.Location = new Point(20,300);

            userNameField.Click += clearField; //this line, if the user clicked the textbox to enter their username, the text value of the textbox will set to empty string, giving the effect of clear
            submitButton.Click += playerNameHandler; //method for when the submit button is clicked

            playerPanel.Controls.Add(completionLabel);
            playerPanel.Controls.Add(userNameField);
            playerPanel.Controls.Add(submitButton);
        }

        public void clearField(object sender, EventArgs e) 
        {
            userNameField.Text = ""; //this is the clear effect for textbox, assigning empty string when clicked
        }

        public void playerNameHandler(object sender, EventArgs e)
        {
            if (userNameField != null) //if username textbox value is not null then assign the value to,
            {
                player.UserName = userNameField.Text; //player username
            }

            playerPanel.Visible = false; //another transition block. After submitting the username and assigning it to username property, scoreboard panel will be visible
            player.DisplayHighScores(playerStats); //passing the playerstats array to the displayhighscores method to be updated with player names and scores
            InitializeScoreboardPanel(); //invoking method to initialize scoreboard panels
            scoreBoardPanel.Visible = true;
        }

        public void InitializeScoreboardPanel() //method that will initialize all scoreboard panel controls and panel itself
        {
            scoreBoardPanel = new Panel();
            scoreBoardPanel.Size = new Size(380, 500);
            scoreBoardPanel.BackColor = Color.CornflowerBlue;
            Controls.Add(scoreBoardPanel);

            Label scoreBoardTitle = new Label();
            scoreBoardTitle.Text = "TOP 10 PLAYER LEADERBOARDS";
            scoreBoardTitle.Font = new Font("Arial",14);
            scoreBoardTitle.Size = new Size(350,30);
            scoreBoardTitle.Location = new Point(20, 20);

            playerLabel = new Label(); //this instance label will be the one to display the usernames and scores
            playerLabel.Font = new Font("Arial", 14);
            playerLabel.Size = new Size(322, 300);
            playerLabel.Location = new Point(20, 50);

            replayBtn = new Button(); //button instance for replay
            replayBtn.Text = "PLAY AGAIN";
            replayBtn.Font = new Font("Arial", 15);
            replayBtn.Size = new Size(320, 35);
            replayBtn.Location = new Point(20, 360);

            exitBtn = new Button(); //button instance for exit
            exitBtn.Text = "EXIT GAME";
            exitBtn.Font = new Font("Arial", 15);
            exitBtn.Size = new Size(320, 35);
            exitBtn.Location = new Point(20, 400);

            replayBtn.Click += gameBtnHandler; //when clicked, eventhandler methods will be called
            exitBtn.Click += gameBtnHandler;

            scoreBoardPanel.Controls.Add(scoreBoardTitle);
            scoreBoardPanel.Controls.Add(playerLabel);
            scoreBoardPanel.Controls.Add(replayBtn);
            scoreBoardPanel.Controls.Add(exitBtn);

            UpdatePlayerStats(); //updates to assign values to playerlabel text that will display the set of names and scores
        }

        public void UpdatePlayerStats() //assigning usernames and score to playerlabel text property for display
        {
            string players = ""; //empty string for concatenation

            for (int row = 0; row < playerStats.GetLength(0); row++) //for loop in accessing the 2d array rows
            {
                if (playerStats[row, 0] != null) // if the slot in the 2d array is not null and contains a value
                {
                    players += $"\nPLAYER: {playerStats[row, 0]} | SCORE: {playerStats[row, 1]}"; //add to empty string to print
                }
                else
                {
                    continue; //else if null continue
                }
            }

            playerLabel.Text = players; //concatenated string will be assigned as value for playerLabel text property
        }

        public void gameBtnHandler(object sender, EventArgs e) //this method handles operation for buttons on scoreboard panel
        {
            Button clickedButton = sender as Button; //checking and casting if button is the sender

            if (clickedButton != null) 
            {
                if (clickedButton.Text.Equals("PLAY AGAIN")) //if the value of text property of the selected button is play again,
                {
                    scoreBoardPanel.Visible = false; //set visibility of the last panel to false
                    InitializeLevelPanel(); //call method to initialize level panel again
                    levelPanel.Visible = true; //set visibility of level panel to be true
                }
                else
                {
                    Close(); //else if button value is EXIT GAME then close the game ui
                }
            }
        }
    }

    class Questions
    {
        private readonly string easyQ = "10+15=;10;25;20;25;54-19=;36;25;35;35;25*4=;100;75;50;100;35/6=;5;6;7;5;35%6=;5;6;7;5";
        private readonly string aveQ = "10+15-9=;15;16;17;16;15-3*4=;48;3;27;3;25/3+5=;14;15;13;13;27+15*3=;72;126;88;72;35+6%4=;1;37;7;37";
        private readonly string diffQ = "10+15-9*3=;15;-2;17;-2;15-3*4+7=;48;10;27;10;25/3+5*3=;14;15;23;23;27+15*3-5=;65;126;88;65;35+6%4-10=;1;27;7;27";
        private string[] choices;
        private string question;
        private string correctAnswer;

        public Questions()
        {
            choices = new string[4]; //upon calling the constructor, the creation of the array happens
        }

        public string[] Choices { get { return choices; } set { choices = value; } }

        public string EasyQ { get { return easyQ; } }

        public string AveQ { get { return aveQ; } }

        public string DiffQ { get { return diffQ; } }

        public string Question { get { return question; } set { question = value; } }

        public string CorrectAnswer { get { return correctAnswer; } set { correctAnswer = value; } }

        public static string[] PopulateQuestions(string bankItems) //accepts argument of questionBank from GenFrame
        {
            return bankItems.Split(';'); //splits and return an array with splitted values
        }

        public void GenerateQuestion(string[] bankToDisplay) //generates question randomly with restrictions
        {
            Random randNum = new Random();
            int randGenNum, ansIndex;

            do
            {
                randGenNum = (randNum.Next(1, 6) - 1) * 5; //generates index 1-6 then subract result with 1 to get 0-5 then multiply result to get each questions index
                Question = bankToDisplay[randGenNum]; //gets the string value question and then assign to question property

            } while (GenFrame.occurredQuestions.Contains(Question)  && GenFrame.occurredQuestions.Count < bankToDisplay.Length / 5); //while the question is in the list and the count of list is not 5, select new question

            for (ansIndex = 0; ansIndex <= 3; ansIndex++) //for loop for populating choices with choices of answer from 1-3 starting from the index of selected question
                Choices[ansIndex] = bankToDisplay[randGenNum + ansIndex + 1]; //increment to skip the question string

            CorrectAnswer = bankToDisplay[randGenNum + 4]; //assign the correct answer to property, adding 4 to get the index of the correct answer per question
        }

        public static void DisplayQuestions(Questions item)
        {
            GenFrame.questionLabel.Text = item.Question; //sets the question to be display in the questionlabel text property
        }
    }

    class Player
    {
        private int score;
        private string userName;
        private string feedBack;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string FeedBack
        {
            get { return feedBack; }
            set { feedBack = value; }
        }

        public void DetermineScore(Questions item, string userAns, int levelScore) //accepts the stringvalue of button selected as parameter
        {
            if (userAns.Equals(item.CorrectAnswer)) //checks if the asnwer is match with correct answer
            {
                feedBack = $"You are Correct!\nThe Answer is {item.CorrectAnswer}."; //sets prompt
                score += levelScore; //increment score based on the passed level score, 1 , 2 , or 3
            }
            else
            {
                feedBack = $"Your choice is Incorrect!\nThe Correct Answer is {item.CorrectAnswer}.";
            }
        }

        public void DisplayHighScores(string[,] playerStats) //method populate the 2d array with username and score value
        {
            for (int row = 0; row < playerStats.GetLength(0); row++) //access the rows of 2d array
            {
                if (userName.Equals(playerStats[row, 0]))//checks if the username exists in the array if yes update the record
                {
                    int newScore = int.Parse(playerStats[row, 1]) + score; //get the string value score and convert to int to add the current score
                    playerStats[row, 1] = newScore.ToString(); //after adding, set the new score to the array slot 
                    break;
                }
                else if (playerStats[row, 0] == null) //if the username is new, it will activate else condition which will find a null value slot in 2d array and add new record
                {
                    playerStats[row, 0] = userName;
                    playerStats[row, 1] = score.ToString();
                    break;
                }
            }
        }
    }
}




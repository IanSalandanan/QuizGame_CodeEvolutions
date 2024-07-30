/*
 * Name: Salandanan, Christian Kerby T.
 * Date: November 14, 2023
 * Description: A Quiz Game program that practices the concept of OOP specifically Class structures.
 */

using System.Collections;

namespace cktsalandanan_Lab_A3_QuizGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitiateGame(); //invoke the method that serves as the main body of the code
        }

        public static void InitiateGame()
        {
            Questions questions = new Questions(); //instantiate question class
            Player player = new Player(); //instantiate player class

            string[] questionBank = new string[25];
            bool active = true, valid;
            string choice, exitOption;

            while (active)
            {
                Player.Score = 0; //resetting score to 0 each game loop for another player

                do
                {
                    Console.Write("Hello Player, Please select your preferred Game Difficulty\n\n[ EASY | AVERAGE | DIFFICULT ]: ");
                    choice = Console.ReadLine().ToUpper();

                    switch (choice) //this switch statement will decide what string to pass based on the input of the user 
                    {
                        case "EASY":
                            questionBank = Questions.PopulateQuestions(questions.EasyQ); //invoking the PopulateQuestion method, returns an array with values of separated string, assigned as the values for questionBank
                            Questions.DisplayQuestions(questionBank, 1); //calls the static displayQuestion method and passing the questionbank and level score
                            valid = true;
                            break;
                        case "AVERAGE":
                            questionBank = Questions.PopulateQuestions(questions.AveQ);
                            Questions.DisplayQuestions(questionBank, 2);
                            valid = true;
                            break;
                        case "DIFFICULT":
                            questionBank = Questions.PopulateQuestions(questions.DiffQ);
                            Questions.DisplayQuestions(questionBank, 3);
                            valid = true;
                            break;
                        default:
                            Console.WriteLine("\nInvalid Choice!\n"); //validation for user input
                            valid = false;
                            break;
                    }

                } while (!valid);

                player.DisplayHighScores(); //invoke displayhighscores method to display 2d array playerstats

                Console.Write("Would you like to play again? [ YES | NO ]: "); //exitoption
                exitOption = Console.ReadLine().ToUpper();

                if (exitOption.Equals("YES")) active = true;
                else if (exitOption.Equals("NO")) active = false;
                else Console.WriteLine("\nInvalid Choice!\n");
            }
        }
    }

    class Player //Player class containing all properties and methods that are relevant to the player
    {
        private string[,] playerStats; //initiated all player properties
        private string userName;
        private static int score;
        public Player()
        {
            playerStats = new string[10, 2]; //when constructor is called the array player stats will be created
        }
        public string[,] PlayerStats //accessors to get and set values to initated properties
        {
            get { return playerStats; }
            set { playerStats = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public static int Score
        {
            get { return score; }
            set { score = value; }
        }

        public void DetermineScore(Questions questions, int levelScore) //this method will ask for the player's answer and check if its correct, adds score if yes
        {
            Console.Write("\nEnter your answer: ");
            string userAns = Console.ReadLine();

            if (userAns.Equals(questions.CorrectAnswer)) //compares user ans with the correct answer property from question class
            {
                Console.WriteLine($"\nYou are Correct!\n\nThe Answer is {questions.CorrectAnswer}.\n");
                score += levelScore; //increments score with passed levelscore 1, 2, or 3
            }
            else
            {
                Console.WriteLine($"\nYour choice is Incorrect!\n\nThe Correct Answer is {questions.CorrectAnswer}.\n");
            }
            Console.WriteLine($"Current Score: {score}"); //displays current score
        }

        public void DisplayHighScores() //this method adds the player record and display the leaderboards
        {
            Console.Write("Enter your Username: "); //asks for the player name
            userName = Console.ReadLine();

            for (int row = 0; row < PlayerStats.GetLength(0); row++) //for loop for accessing the rows of the 2d array
            {
                if (userName.Equals(PlayerStats[row, 0])) //checks if the username exists if yes then update the record
                {
                    int newScore = int.Parse(PlayerStats[row, 1]) + score; //gets the value and convert to int then add the new score
                    PlayerStats[row, 1] = newScore.ToString(); //adding the newly updated score 
                    break;
                }
                else if (PlayerStats[row, 0] == null) //if not equals to an existing player then find a null slot in array and add the new player record
                {
                    PlayerStats[row, 0] = userName;
                    PlayerStats[row, 1] = score.ToString();
                    break;
                }
            }

            Console.WriteLine("\nTOP 10 PLAYER LEADERBOARDS");

            for (int row = 0; row < PlayerStats.GetLength(0); row++)
            {
                if (PlayerStats[row, 0] != null) //if the slot in 2d array has elements then print
                {
                    Console.WriteLine($"\nPLAYER: {PlayerStats[row, 0]}\tSCORE: {PlayerStats[row, 1]}\n");
                }
                else
                {
                    continue; //else continue
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
            choices = new string[4];
        }
        public string[] Choices
        {
            get { return choices; }
            set { choices = value; }
        }

        public string EasyQ
        {
            get { return easyQ; }
        }

        public string AveQ
        {
            get { return aveQ; }
        }

        public string DiffQ
        {
            get { return diffQ; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string CorrectAnswer
        {
            get { return correctAnswer; }
            set { correctAnswer = value; }
        }

        public static string[] PopulateQuestions(string bankItems) // a static method that accepts a string argument and split the string then return an array with splitted string as its values
        {
            return bankItems.Split(';');
        }

        public static void DisplayQuestions(string[] bankToDisplay, int levelScore) //method for selecting random questions and display question and choices
        {
            Player answer = new();
            Questions question = new();

            ArrayList occurredQuestions = new();
            Random randNum = new();
            int randGenNum, ansIndex, questionNum = 0;

            do
            {
                randGenNum = (randNum.Next(1, 6) - 1) * 5; //generates number from 1-6 then subract 1 to get 0-5 then multiply by 5 to get each questions index 

                question.Question = bankToDisplay[randGenNum]; //assign the question string to question property

                if (occurredQuestions.Contains(question.Question)) //if the question string value is in the list, loop again to select a new question
                    continue;

                occurredQuestions.Add(question.Question); //after selecting a new question that does not exist in the list, add it to the list
                question.CorrectAnswer = bankToDisplay[randGenNum + 4]; //assign the correct answer to correct answer property

                for (ansIndex = 0; ansIndex < 3; ansIndex++) //for loop to index the 3 choices and add it to choices array
                    question.Choices[ansIndex] = bankToDisplay[randGenNum + ansIndex + 1]; //+1 to skip the question string

                Console.WriteLine($"\nQuestion #{++questionNum}\n"); //displaying question num and question
                Console.WriteLine($"{question.Question}\n");

                foreach (string choice in question.Choices) //displaying choices
                    Console.WriteLine(choice);

                answer.DetermineScore(question,levelScore); //passing the instance of question class and level score to be used in the determine score method

            } while (occurredQuestions.Count < bankToDisplay.Length / 5); //while the count of the list is not equals to len of questionbank divide by 5, loop. This means that all 5 questions are still not in the occurredQuestions list
        }
    }
}
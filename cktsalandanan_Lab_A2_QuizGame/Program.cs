/*
 * Name: Salandanan, Christian Kerby T.
 * Date: October 16, 2023
 * Description: A Quiz Game program that simulates each level of difficulty, the program involves the use of methods, array, random, and 2D Array.
 */

using System.Collections;
using System.Runtime.CompilerServices;

namespace cktsalandanan_Lab_A2_QuizGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string easyQ = "10+15=;10;25;20;25;54-19=;36;25;35;35;25*4=;100;75;50;100;35/6=;5;6;7;5;35%6=;5;6;7;5";
            string aveQ = "10+15-9=;15;16;17;16;15-3*4=;48;3;27;3;25/3+5=;14;15;13;13;27+15*3=;72;126;88;72;35+6%4=;1;37;7;37";
            string diffQ = "10+15-9*3=;15;-2;17;-2;15-3*4+7=;48;10;27;10;25/3+5*3=;14;15;23;23;27+15*3-5=;65;126;88;65;35+6%4-10=;1;27;7;27";

            string[] questionBank = new string[25];
            string[,] playerStats = new string[10,2];

            bool active = true, valid = false;
            string choice, exitOption;
            int totalScore = 0;

            while (active) //Main Loop of the Game
            {
                do //Inner Loop of the Game, for validating the selected difficulty
                {
                    totalScore = 0; //Reset the totalScore for another player everytime the game loops

                    Console.Write("Hello Player, Please select your preferred Game Difficulty\n\n[ EASY | AVERAGE | DIFFICULT ]: ");
                    choice = Console.ReadLine().ToUpper(); //ToUpper() to directly convert the input into uppercase

                    switch (choice) //Switch case for every game difficulty, passing the right string coordinated to the user's selected difficulty.
                    {
                        case "EASY":
                            PopulateQuestionBank(ref questionBank, easyQ);
                            DisplayQuestion(questionBank, 1, ref totalScore);
                            valid = true;
                            break;
                        case "AVERAGE":
                            PopulateQuestionBank(ref questionBank, aveQ);
                            DisplayQuestion(questionBank, 2, ref totalScore);
                            valid = true;
                            break;
                        case "DIFFICULT":
                            PopulateQuestionBank(ref questionBank, diffQ);
                            DisplayQuestion(questionBank, 3, ref totalScore);
                            valid = true;
                            break;
                        default:
                            Console.WriteLine("\nInvalid Choice!\n");
                            valid = false;
                            break;
                    }

                } while (!valid);

                DisplayHighScores(playerStats, totalScore); //Invoke the method DisplayHighScores() to display players' stats. 2D Array and totalScore arguments

                Console.Write("Would you like to play again? [ YES | NO ]: "); //ExitOption for the game.
                exitOption = Console.ReadLine().ToUpper();

                if (exitOption.Equals("YES")) active = true;
                else if (exitOption.Equals("NO")) active = false;
                else Console.WriteLine("\nInvalid Choice!\n");
            }
        }

        static void PopulateQuestionBank(ref string[] bankToPopulate, string bankItems)
        {
            bankToPopulate = bankItems.Split(';'); //Split the string and append the items to the questionBank
        }

        static void DisplayQuestion(string[] bankToDisplay, int levelScore ,ref int totalScore)
        {
            ArrayList occurredQuestions = new ArrayList();
            Random randNum = new Random();
            int randGenNum, ansIndex, questionNum = 0;

            do
            {
                randGenNum = randNum.Next(1,6) - 1; //Generate a random number, however I subract 1 every randGenNum to include 0 and 5 
                randGenNum *= 5; //After generating a number, I designed the algo to immediately multiply the randGenNum to 5 so that I can directly index the 5 given questions.

                if (occurredQuestions.Contains(bankToDisplay.GetValue(randGenNum))) //This line will check if the string question indexed by randGenNum is already in the ArrayList.
                    continue; //If the question string has occured and is in the ArrayList, then this continue line will skip the bottom lines and generate a random number again.

                occurredQuestions.Add(bankToDisplay.GetValue(randGenNum)); //If it is the question string's 1st occurrence then the algo will add the string to the ArrayList.

                Console.WriteLine($"\nQuestion #{++questionNum}\n"); //Print the question number;

                for (ansIndex = 0; ansIndex < 4; ansIndex++) //This loop will print the answer choices, 4 to avoid printing the slot of the correct answer.
                {
                    Console.WriteLine(bankToDisplay[randGenNum + ansIndex]); //Increment to the current question string index to print all the answer choices
                }

                DetermineScore(bankToDisplay, randGenNum, levelScore, ref totalScore); //Invoke the DetermineScore() method to check if the answer is correct and return the totalScore.

            } while (occurredQuestions.Count < bankToDisplay.Length / 5); //To continuously loop the game round, I compare the ArrayList' number of items to the Array's number of questions by dividing the length with the number of questions
        }

        static int DetermineScore(string[] bankToCheck, int randGenNum, int levelScore, ref int totalScore)
        {
            string userAns;

            Console.Write("\nEnter your answer: ");
            userAns = Console.ReadLine();

            if (userAns.Equals(bankToCheck.GetValue(randGenNum + 4))) //Plus 4 to check the last index of each question, which holds the correct answer.
            {
                Console.WriteLine($"\nYou are Correct!\n\nThe Answer is {bankToCheck.GetValue(randGenNum + 4)}.\n");
                totalScore += levelScore; //Based on the passed argument for each difficulty, this line will determine the score for each difficulty.
            }
            else
            {
                Console.WriteLine($"\nYour choice is Incorrect!\n\nThe Correct Answer is {bankToCheck.GetValue(randGenNum + 4)}.\n");
            }
            Console.WriteLine($"Current Score: {totalScore}"); //Display current score

            return totalScore;
        }

        static void DisplayHighScores(string[,] playerStats, int totalScore)
        {
            Console.Write("\nEnter your Username: "); //Ask for the player's userName
            string userName = Console.ReadLine();

            for (int row = 0;  row < playerStats.GetLength(0); row++) //Loop for populating the playerStats Array
            {
                if (userName.Equals(playerStats[row,0])) //Check if the user exists in the LeaderBoards, if yes then overwrite their existing score.
                {
                    int newScore = int.Parse(playerStats[row, 1]) + totalScore; //Get the existing score, convert and add with new score.
                    playerStats[row,1] = newScore.ToString(); //Convert to string and add the new score.
                    break;
                }
                else if (playerStats[row,0] == null) //Check if the slot is null, if yes then add a new player record. 
                {
                    playerStats[row, 0] = userName;
                    playerStats[row, 1] = totalScore.ToString();
                    break;
                }
            }

            Console.WriteLine("\nTOP 10 PLAYER LEADERBOARDS");

            for (int row = 0; row < playerStats.GetLength(0); row++) //Display Player Scores, a loop that based on the number of rows of the 2D Array
            {
                if (playerStats[row, 0] != null) //Check if the slot is not null
                {
                    Console.WriteLine($"\nPLAYER: {playerStats[row, 0]}\tSCORE: {playerStats[row, 1]}\n");
                }
                else //Else if null, skip.
                {
                    continue;
                }
            }

        }
    }
}
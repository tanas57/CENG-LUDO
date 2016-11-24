﻿using System;
namespace Project2___Ludo
{
    class Ludo
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(71, 22);
            string[] Board = new string[88]; // GAME ARRAY
            for (int j = 0; j < 88; j++) Board[j] = "*";   // EMPTY GAME AREAS
            for (int j = 56; j <= 71; j++) Board[j] = "0"; // FINISH INDEX AREAS
            string[] PawnsNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" }; // PAWNS NAMES
            for (int j = 0; j < 16; j++) { Board[72 + j] = PawnsNames[j]; } // LOCATED PAWNS NAMES
            // || \\    VARIABLES   // || \\
            int[] PawnStartingIndex = { 0, 0, 0, 0, 14, 14, 14, 14, 28, 28, 28, 28, 42, 42, 42, 42 }; // EACH PAWNS STARTING INDEX
            int[] PawnHomeFirstIndex = { 72, 72, 72, 72, 76, 76, 76, 76, 80, 80, 80, 80, 84, 84, 84, 84 }; // PAWS HOME'S FIRST INDEX
            int[] PawnLastIndex = { 55, 55, 55, 55, 13, 13, 13, 13, 27, 27, 27, 27, 41, 41, 41, 41 }; // PAWS THAT CAN PLAY LAST INDEX
            int[] PawnHomeIndex = { 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87 }; // PAWNS HOME INDEXS
            int[] FinishPlaceFirstIndex = { 56, 56, 56, 56, 60, 60, 60, 60, 64, 64, 64, 64, 68, 68, 68, 68 }; // PAWS FINISH AREA'S FIRST INDEXS
            char[] PenaltiesAndRewards = { ')', ')', ')', '(', '(', '(', '<', '<', '>', '>', 'X' }; // REWARDS AND PENALTIES
            int[] pawns = new int[16]; // PAWS VALUES
            int[] WaitIndexValues = new int[16]; // WAIT PENALTIES INDEX VALUES
            bool[] WaitGamer = new bool[4];      // WHICH PLAYER WILL WAIT INFORMATIONS ARRAY
            int[] PlayIndexValues = new int[16]; // PLAY REWARDS INDEX VALUES
            bool[] pawnTours = new bool[16];     // IF COMPUTER'S PAWNS INDEX GREATER THAN 55 PAWNTOURS[İ] TRUE
            Random RD = new Random(); int rand = 0;
            int round = 1; int player = 1; int dice = 1; string pawn = "";
            bool LegalMove = false, LegalMove2 = false, NewDice = true; bool InvalidMove = false;
            int temporary = 0; int PawnINDEX = 0; bool GameFinish = false;
            int controlIndex = 72; int control1 = 0, control2 = 0, NoLegalMoveCount = 0;
            bool ComPawnOwner = false, ComPawnOther = false, PlayAgain = false, ComPawnStartPointOtherPlayersPawnHere = false; int ComTemp = 0; int Winner = 0;
            // || \\    REWARDS AND PENALTIES LOCATED   // || \\
            for (int i = 0; i < PenaltiesAndRewards.Length; i++)
            {
                rand = RD.Next(3, 56);
                control1 = rand + 3; control2 = rand - 3;
                if (!(rand == 14 || rand == 15 || rand == 16 || rand == 28 || rand == 29 || rand == 30 || rand == 42 || rand == 43 || rand == 44))
                {
                    if (control1 >= 1 && control1 <= 56 && control2 >= 1 && control2 <= 56)
                    {
                        if (Board[rand] == "*" && Board[control1] == "*" && Board[control2] == "*" && (rand != 14 || rand != 28 || rand != 42)) Board[rand] = PenaltiesAndRewards[i].ToString();
                        else i--;
                    }
                    else i--;
                }
                else i--;
            }
            do
            {
                // is there legal move, do not change dice.
                if (NewDice == false) { NewDice = true; } // legalmove true => newdice false
                else { dice = RD.Next(1, 7); }
                // || \\    WAIT COMMAND CONTROL   // || \\
                for (int j = 0; j <= 3; j++)
                {
                    if (player == 5) player = 4; else if (player == 0) player = 1;
                    if (WaitGamer[player - 1] == true)
                    {
                        WaitGamer[player - 1] = false;
                        player++;
                    }
                }
                for (int k = 0; k < 16; k++)
                {
                    if (WaitIndexValues[k] > 0) // PAWN 
                    {
                        if (WaitIndexValues[k] != pawns[k])
                        {
                            Board[WaitIndexValues[k]] = "<";
                            WaitIndexValues[k] = 0;
                        }
                    }
                }
                // || \\    PLAY ONE MORE TIMES CONTROL   // || \\
                for (int l = 0; l < 16; l++)
                {
                    if (PlayIndexValues[l] > 0) // PAWN WAS STAYED ANY '>' REWARD.
                    {
                        if (PlayIndexValues[l] != pawns[l])
                        {
                            Board[PlayIndexValues[l]] = ">";
                            PlayIndexValues[l] = 0;
                        }
                    }
                }
                Console.Clear(); // CLEAR SCREEN
                // || \\    DRAW GAME SCREEN   // || \\
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  Player1                       "); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("Player2");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  A B C D"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     ┌───────┐        "); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine(" E F G H");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ╔═════╗"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[12] + " " + Board[13] + " " + Board[14] + " │        "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(" ╔═════╗       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Round: " + round);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ║ " + Board[72] + " " + Board[73] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[11] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[60]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[15] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("║ " + Board[76] + " " + Board[77] + " ║       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Turn : Player" + player);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ║ " + Board[74] + " " + Board[75] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[10] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[61]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[16] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("║ " + Board[78] + " " + Board[79] + " ║       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Dice : " + dice);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ╚═════╝"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[9] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[62]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[17] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("╚═════╝       "); Console.ForegroundColor = ConsoleColor.White; if (player == 1) Console.WriteLine("Enter Pawn: "); else Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("              │ " + Board[8] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[63]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" " + Board[18] + " │                ");
                Console.WriteLine("  ┌───────────┘ " + Board[7] + "   " + Board[19] + " └───────────┐");
                Console.WriteLine("  │ " + Board[0] + " " + Board[1] + " " + Board[2] + " " + Board[3] + " " + Board[4] + " " + Board[5] + " " + Board[6] + "   " + Board[20] + " " + Board[21] + " " + Board[22] + " " + Board[23] + " " + Board[24] + " " + Board[25] + " " + Board[26] + " │");
                Console.Write("  │ " + Board[55] + " "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(Board[56] + " " + Board[57] + " " + Board[58] + " " + Board[59]); Console.ForegroundColor = ConsoleColor.White; Console.Write("     " + dice + "     "); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(Board[67] + " " + Board[66] + " " + Board[65] + " " + Board[64] + " "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(Board[27] + " │");
                Console.WriteLine("  │ " + Board[54] + " " + Board[53] + " " + Board[52] + " " + Board[51] + " " + Board[50] + " " + Board[49] + " " + Board[48] + "   " + Board[34] + " " + Board[33] + " " + Board[32] + " " + Board[31] + " " + Board[30] + " " + Board[29] + " " + Board[28] + " │");
                Console.WriteLine("  └───────────┐ " + Board[47] + "   " + Board[35] + " ┌───────────┘");
                Console.Write("              │ " + Board[46] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[71]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" " + Board[36] + " │                ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ╔═════╗"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[45] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[70]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[37] + " │       "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("  ╔═════╗");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ║ " + Board[84] + " " + Board[85] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[44] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[69]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[38] + " │   "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("      ║ " + Board[80] + " " + Board[81] + " ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ║ " + Board[86] + " " + Board[87] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[43] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[68]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[39] + " │     "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("    ║ " + Board[82] + " " + Board[83] + " ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ╚═════╝     "); Console.ForegroundColor = ConsoleColor.White; Console.Write("│ " + Board[42] + " " + Board[41] + " " + Board[40] + " │    "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("     ╚═════╝");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  M N O P"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     └───────┘"); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("         I J K L");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  Player4"); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("                       Player3"); Console.ForegroundColor = ConsoleColor.White;
                if (GameFinish == true) break;
                // || \\    DRAW GAME SCREEN   // || \\
                switch (player)
                {
                    case 1: controlIndex = 72; break;
                    case 2: controlIndex = 76; break;
                    case 3: controlIndex = 80; break;
                    case 4: controlIndex = 84; break;
                } // for each player's first pawn, gives starting index.
                int ComPawn = 0;
                // all pawns is home and dice is six, player can play 1 pawn.
                if (Board[controlIndex] != "*" && Board[controlIndex + 1] != "*" && Board[controlIndex + 2] != "*" && Board[controlIndex + 3] != "*")
                {
                    if (dice == 6)
                    {
                        if (player == 1)
                        {
                            Console.SetCursorPosition(58, 5);
                            pawn = Console.ReadLine().ToUpper(); // select pawn
                            if (!(pawn == "A" || pawn == "B" || pawn == "C" || pawn == "D")) { InvalidMove = true; }
                        }
                        else
                        {
                            ComPawn = RD.Next(controlIndex, controlIndex + 3);
                            pawn = Board[ComPawn];
                        }
                    }
                    else { Console.SetCursorPosition(46, 5); Console.Write("            "); }
                }
                else // any pawn is on the game board, select this.
                {
                    if (player == 1)
                    {
                        Console.SetCursorPosition(58, 5);
                        pawn = Console.ReadLine().ToUpper();
                        if (!(pawn == "A" || pawn == "B" || pawn == "C" || pawn == "D")) { InvalidMove = true; }
                        for(byte u = 0; u < 16; u++)
                        {
                            if(PawnsNames[u] == pawn)
                            {
                                if (Board[PawnHomeIndex[u]] != "*") InvalidMove = true;
                            }
                        }
                    }
                    else
                    {
                        int[] ComPawnPoint = { 0, 0, 0, 0 };
                        int[] ComPawnIndex = { 0, 0, 0, 0 };
                        int PawnCount = 0, PointCount=0;
                        for (int j = 0; j < 4; j++)
                        {
                            ComPawn = controlIndex + j - 72; // CURRENT PAWN
                            ComTemp = pawns[ComPawn] + dice; // CURRENT PAWN VALUE + DICE
                            ComPawnIndex[j] = ComPawn; // IT USES CHOSING THE BEST PAWN
                            ComPawnOwner = false; ComPawnOther = false; ComPawnStartPointOtherPlayersPawnHere = false; // RESET CONTROL VARIABLES

                            if (Board[PawnHomeIndex[ComPawn]] == "*") // PAWN IS AT GAME BOARD
                            {
                                PawnCount++; // PAWN COUNTER AT THE GAME BOARD
                                // ACCORDING TO RELATED PAWN'S FINISH POINT, IT CALCULATES
                                //if (ComTemp > 55) ComTemp = ComTemp - 56;

                                if (player == 2 && ComTemp > 13 && pawnTours[ComPawn] == true)
                                { // 60 61 62 63
                                    if (ComTemp >= 60 && ComTemp <= 63) ComTemp = 59 + (ComTemp - 13);
                                }
                                else if (player == 3 && ComTemp > 27 && pawnTours[ComPawn] == true)
                                { // 64 65 66 67
                                    if (ComTemp >= 64 && ComTemp <= 67) ComTemp = 63 + (ComTemp - 27);
                                }
                                else if (player == 4 && ComTemp > 41 && pawnTours[ComPawn] == true)
                                {// 68 69 70 71
                                    if (ComTemp >= 68 && ComTemp <= 71) ComTemp = 67 + (ComTemp - 41);
                                }
                                // GRADE CURRENT PAWN
                                if (ComTemp > 55)
                                {
                                    ComTemp = ComTemp - 56;
                                    PointCount = (56 - PawnStartingIndex[ComPawn]) + ComTemp;
                                }
                                else if (pawnTours[ComPawn])
                                    PointCount = (56 - PawnStartingIndex[ComPawn]) + ComTemp;
                                else PointCount = ComTemp - PawnStartingIndex[ComPawn];
                                
                                if (pawns[ComPawn] >= FinishPlaceFirstIndex[ComPawn] && pawns[ComPawn] <= FinishPlaceFirstIndex[ComPawn] + 3) // PAWN IS AT FINISH POINT
                                {
                                    if (ComTemp >= FinishPlaceFirstIndex[ComPawn] && ComTemp <= FinishPlaceFirstIndex[ComPawn] + 3) // PAWN CAN PLAY AT FINISH POINT
                                        PointCount += 4;   // IN FINISH POINT PAWN THAT CAN PLAY
                                    else
                                    {
                                        PawnCount--;
                                        PointCount = 0;
                                    } // PAWN CAN NOT PLAY, COUNTER AND THIS POINT RESET
                                }
                                else // AT GAME BOARD 0 - 55 INDEX
                                {
                                    // SOME CONTROLS
                                    for (int i = 0; i < 16; i++)
                                    {
                                        if (Board[ComTemp] == PawnsNames[i]) // CONTROL PLAYER'S PAWS.
                                        {
                                            if ((i + 72) >= PawnHomeFirstIndex[ComPawn] && (i + 72) <= PawnHomeFirstIndex[ComPawn] + 3) // owner pawn
                                                ComPawnOwner = true;
                                            else ComPawnOther = true;
                                        }
                                        if (Board[PawnStartingIndex[ComPawn]] == PawnsNames[i]) // CONTROL CURRENT PAWN START POINT
                                        {
                                            if (!((i + 72) >= PawnHomeFirstIndex[ComPawn] && (i + 72) <= PawnHomeFirstIndex[ComPawn] + 3)) // other players pawn
                                                ComPawnStartPointOtherPlayersPawnHere = true;
                                        }
                                    }

                                    if (ComPawnOther == true) // OTHER PLAYER'S PAWN IN FOLLOWING SECTION, IT CAN BE BROKEN
                                    {
                                        ComPawnOwner = false;
                                        PointCount += 8;
                                    }
                                    else
                                    {
                                        if (ComPawnOwner == true) // OWNER PAWN IN HERE, CAN NOT PLAY.
                                        {
                                            ComPawnOwner = false;
                                            PointCount = 0; // eğer kendi taşı varsa oynamasın istiyorum 0 puan
                                        }
                                        else
                                        {
                                            if (Board[ComTemp] == ")") PointCount += 7;        // REWARDS 1 : GO 3 STEP FORWARD
                                            else if (Board[ComTemp] == ">") PointCount += 6;   // REWARDS 2 : PLAY ONE MORE TIMES
                                            else if (Board[ComTemp] == "*") PointCount += 5;   // NORMAL
                                            else if (Board[ComTemp] == "(") PointCount += 3;   // PENALTY 1 : GO 3 STEP BACK
                                            else if (Board[ComTemp] == "<") PointCount += 2;   // PENALTY 2 : WAIT ONE ROUND
                                            else if (Board[ComTemp] == "X") PointCount += 1;   // PENALTY 3 : GO PAWN HOME
                                            else { if (ComTemp >= PawnHomeFirstIndex[ComPawn] && ComTemp <= PawnHomeFirstIndex[ComPawn]) PointCount += 10; } // PAWN CAN ENTER FINISH POINT
                                        }
                                    }
                                    if (ComTemp > FinishPlaceFirstIndex[ComPawn] + 3) PointCount = 0; // OUT OF BOARD.
                                }
                            }
                            else { PointCount = 0; ComPawnIndex[j] = 0; } // IS NOT GAME
                            ComPawnPoint[j] = PointCount;
                            PointCount = 0;
                        }
                        // PAWN CHOOSES
                        if (PawnCount == 1) // JUST ONE PAWN CAN BE PLAYED, CHOOSES IT
                        {
                            for (int j = 0; j < 4; j++) if (ComPawnPoint[j] != 0) { pawn = PawnsNames[ComPawnIndex[j]]; }
                        }
                        else
                        for (int i = 0; i < 4; i++) if ((ComPawnPoint[i] >= ComPawnPoint[0]) && ComPawnPoint[i] != 0) pawn = PawnsNames[ComPawnIndex[i]]; // BETWEEN MORE AND PAWNS,ONE

                        bool NewPawn = false; byte say = 0;
                        // IF COMPUTER CAN NOT PLAY ANY PAWN, TURN++ | CONTROL CAN IT PLAY NEW PAWN ?
                        for (int a = 0; a < 4; a++) for (int b = 0; b < 56; b++) if (Board[b] == PawnsNames[controlIndex + a - 72]) say++;

                        if (say >= 1) say = 0;
                        else NewPawn = true;

                        if (dice == 6 && (NewPawn == true || (Board[PawnStartingIndex[ComPawn]] == "*" || ComPawnStartPointOtherPlayersPawnHere == true)))
                        {   // ENTER NEW PAWN.
                            for (int i = 0; i < 16; i++)
                            {
                                ComPawn = RD.Next(controlIndex, controlIndex + 4);
                                if (Board[ComPawn] != "*") pawn = Board[ComPawn];
                            }   if (NewPawn == true) NewPawn = false;
                        }
                    }
                }
                if (LegalMove) // legal move control.
                {
                    pawn = "";
                    Console.SetCursorPosition(46, 5);
                    Console.Write("No legal move        ");
                    NewDice = false;
                    LegalMove = false;
                    NoLegalMoveCount++;
                    Console.ReadKey();
                }
                else if (InvalidMove == true)
                {
                    InvalidMove = false;
                    PlayAgain = true;
                    NewDice = false;
                    Console.SetCursorPosition(46, 5);
                    Console.Write("Invalid move         ");
                    pawn = "";
                    //Console.ReadKey();
                }
                else
                {   // pawn is selecteD top of pawns informations. and plays.. Sometimes, player can not play with any pawn
                    if (pawn != "")
                    {
                        // GET PAWN INDEX
                        for (int a = 0; a < 16; a++)
                        {
                            if (PawnsNames[a] == pawn) { PawnINDEX = a; break; }
                        }
                        temporary = pawns[PawnINDEX];
                        if (Board[PawnHomeIndex[PawnINDEX]] == pawn) // is selected pawn home?
                        {
                            if (dice == 6)
                            { // if dice 6, player can play one times.
                                if (Board[PawnStartingIndex[PawnINDEX]] == "*") // is starting point empty?
                                {
                                    Board[PawnStartingIndex[PawnINDEX]] = pawn;
                                    Board[PawnHomeIndex[PawnINDEX]] = "*";
                                    temporary = PawnStartingIndex[PawnINDEX]; // selected Pawns index
                                }
                                else // check the other players pawns. there can be own pawns or other players pawns control it.
                                {
                                    for (int i = 0; i < 16; i++)
                                    {
                                        if (Board[PawnStartingIndex[PawnINDEX]] == PawnsNames[i])
                                        {
                                            if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3) LegalMove2 = true; // owner pawn
                                            else
                                            { // other player's pawn PawnsNames[i] brokes
                                                Board[PawnHomeIndex[i]] = Board[PawnStartingIndex[PawnINDEX]]; // broken pawn goes home!
                                                Board[PawnStartingIndex[PawnINDEX]] = pawn; // start index new values.
                                                Board[PawnHomeIndex[PawnINDEX]] = "*"; // on the start index pawn homeIndex => *
                                                temporary = PawnStartingIndex[PawnINDEX];
                                                pawns[i] = PawnStartingIndex[PawnINDEX]; // broken pawns value => 0
                                                pawnTours[i] = false;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // selected pawn is on the game board.
                            int temp = temporary + dice;
                            int LocatePawnIndex = temporary;
                            int EnterFinishPoint = 0;
                            if (player == 1 && temp > 55) // pawns can enter to finish point ! PLAYER 1
                            {
                                if (temp >= 56 && temp <= 59) EnterFinishPoint = 1;
                                else if(temp >59) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 2 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; }
                            else if (player == 3 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; }
                            else if (player == 4 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; }

                            if (player == 2 && temp > 13 && pawnTours[PawnINDEX] == true)
                            { // 60 61 62 63
                                temp = 59 + (temp - 13);
                                if (temp >= 60 && temp <= 63) EnterFinishPoint = 1;
                                else if (temp > 63) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 3 && temp > 27 && pawnTours[PawnINDEX] == true)
                            { // 64 65 66 67
                                temp = 63 + (temp - 27);
                                if (temp >= 64 && temp <= 67) EnterFinishPoint = 1;
                                else if (temp > 67) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 4 && temp > 41 && pawnTours[PawnINDEX] == true)
                            {// 68 69 70 71
                                temp = 67 + (temp - 41);
                                if (temp >= 68 && temp <= 71) EnterFinishPoint = 1;
                                else if (temp > 71) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            // PAWN CAN ENTER TO FINISH POINT.
                            if (EnterFinishPoint == 1)
                            {
                                EnterFinishPoint = 0;

                                if ((temporary) >= FinishPlaceFirstIndex[PawnINDEX] && (temporary) <= FinishPlaceFirstIndex[PawnINDEX] + 3) // IN FINISH PLACE, PAWNS CAN CHANGE ITS LOCATION
                                {
                                    if ((temp) >= FinishPlaceFirstIndex[PawnINDEX] && (temp) <= FinishPlaceFirstIndex[PawnINDEX] + 3)
                                    {
                                        Board[LocatePawnIndex] = "0";
                                        Board[temp] = pawn;
                                        temporary = temp;
                                    }
                                    else
                                    {
                                        LegalMove2 = true;
                                        temporary = LocatePawnIndex;
                                    }
                                }
                                else
                                {
                                    if (Board[temp] == "0") // SLOT IS EMPTY
                                    {
                                        Board[LocatePawnIndex] = "*";
                                        Board[temp] = pawn;
                                        pawnTours[PawnINDEX] = false;
                                        temporary = temp; // it is okey.
                                    }
                                    else LegalMove2 = true; // SLOT IS FULL, ANY PAWN İS HERE
                                }
                            }
                            else if(EnterFinishPoint == 2)
                            {
                                Console.SetCursorPosition(45, 6);
                                Console.Write("No legal move        ");
                                LegalMove2 = true;
                                Console.ReadKey();
                            }
                            else
                            {
                                bool ProblemControl = false;
                                // CONTROL INDEX, IS THERE OTHER PLAYERS PAWNS OR YOURS ?
                                for (int i = 0; i < 16; i++)
                                {
                                    if (Board[temp] == PawnsNames[i])
                                    {
                                        if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                        {   // owner pawn
                                            ProblemControl = true;
                                            LegalMove2 = true;
                                        }
                                        else
                                        { // other player's pawn
                                            ProblemControl = true;
                                            Board[PawnHomeIndex[i]] = Board[temp]; // broken pawn goes home!
                                            Board[temp] = pawn; // start index new values.
                                            Board[temporary] = "*"; // on the start index pawn homeIndex => *
                                            pawns[i] = PawnStartingIndex[PawnINDEX]; // broken pawns value => 0
                                            pawnTours[i] = false;
                                            temporary = temp;
                                        }
                                        break;
                                    }
                                }
                                if (ProblemControl == false)
                                {
                                    // 3 STEP FORWARD COMMAND
                                    if (Board[temp] == ")")
                                    {
                                        bool IsThereProblem = false;
                                        int WorkTemp = temp;
                                        temp = temp + 3;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Go 3 step forward");
                                        Console.ReadKey();
                                        if ((temp) > PawnLastIndex[PawnINDEX]) // IF REWARDS IS AT BEFORE RELATED PAWN'S FINISH POINT
                                        {
                                            IsThereProblem = true;
                                            if (player == 2 && temp > 13 && pawnTours[PawnINDEX] == true) temp = 59 + (temp - 13);
                                            else if (player == 3 && temp > 27 && pawnTours[PawnINDEX] == true) temp = 63 + (temp - 27);
                                            else if (player == 4 && temp > 41 && pawnTours[PawnINDEX] == true) temp = 67 + (temp - 41);

                                            if ((temp) >= FinishPlaceFirstIndex[PawnINDEX] && (temp) <= FinishPlaceFirstIndex[PawnINDEX] + 3) // +3 STEP => EMPTY?
                                            {
                                                if (Board[temp] == "0") // IS SLOT EMPTY?
                                                {
                                                    Board[temp] = pawn;
                                                    Board[temporary] = "*";
                                                    temporary = temp;
                                                }
                                                else LegalMove2 = true;
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < 16; i++)
                                            {
                                                if (Board[temp] == PawnsNames[i]) // PAWN + 3.INDEX CONTROL
                                                {
                                                    if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                                    {   // owner pawn
                                                        IsThereProblem = true;
                                                        LegalMove2 = true;
                                                    }
                                                    else
                                                    { // other player's pawn
                                                      // PawnsNames[i] . taşı kırıcaz.
                                                        IsThereProblem = true;
                                                        pawns[i] = PawnStartingIndex[i];                    // broken pawns value => starting value.
                                                        Board[PawnHomeIndex[i]] = Board[temp + 3];          // broken pawn goes home!
                                                        pawnTours[i] = false;
                                                        Board[temp] = pawn;                                 // start index new values.
                                                        Board[temporary] = "*";                             // on the start index pawn homeIndex => *
                                                        temporary = temp;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (IsThereProblem == false)
                                        {
                                            Board[temporary] = "*";  // before not adding dice => *
                                            temporary = temp;        // add dice
                                            Board[temporary] = pawn; // after adding dice => current pawn
                                        }
                                    }// 3 STEP BACK COMMAND 
                                    else if (Board[temp] == "(")
                                    {
                                        bool IsThereProblem = false;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Go 3 step back");
                                        Console.ReadKey();
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (Board[temp - 3] == PawnsNames[i]) // PAWN - 3.INDEX CONTROL
                                            {
                                                if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                                {   // owner pawn
                                                    IsThereProblem = true;
                                                    LegalMove2 = true;
                                                    // goto LegalMoveControl;
                                                }
                                                else
                                                { // other player's pawn
                                                  // PawnsNames[i] . taşı kırıcaz.
                                                    IsThereProblem = true;
                                                    pawns[i] = PawnStartingIndex[i];         // broken pawns value => starting value.
                                                    Board[PawnHomeIndex[i]] = Board[temp - 3];         // broken pawn goes home!
                                                    pawnTours[i] = false;
                                                    Board[temp - 3] = pawn;                 // start index new values.
                                                    Board[temporary] = "*";                  // on the start index pawn homeIndex => *
                                                    temporary = temp - 3;
                                                }
                                                break;
                                            }
                                        }
                                        if (IsThereProblem == false)
                                        {
                                            Board[temporary] = "*";  // before not adding dice => *
                                            temporary = temp - 3;    // add dice
                                            Board[temporary] = pawn; // after adding dice => current pawn
                                        }
                                    } // PLAY ONE MORE TIMES
                                    else if (Board[temp] == ">")
                                    {
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("play one more time");
                                        PlayIndexValues[PawnINDEX] = temp;
                                        Board[temporary] = "*";
                                        Board[temp] = pawn;
                                        temporary = temp;
                                        PlayAgain = true;
                                        Console.ReadKey();
                                    } // WAIT ONE ROUND
                                    else if (Board[temp] == "<")
                                    {
                                        WaitIndexValues[PawnINDEX] = temp;
                                        WaitGamer[player - 1] = true;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Wait one round!");
                                        Board[temporary] = "*";
                                        Board[temp] = pawn;
                                        temporary = temp;
                                        Console.ReadKey();
                                    }   // bir el bekleyecek
                                    else if (Board[temp] == "X")
                                    {
                                        bool IsThereProblem = false;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Pawn goes to home");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (Board[PawnStartingIndex[PawnINDEX]] == PawnsNames[i]) // STARTING INDEX CONTROL
                                            {
                                                if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                                {
                                                    // owner pawn
                                                    IsThereProblem = true;
                                                    LegalMove2 = true;
                                                    temporary = PawnStartingIndex[PawnINDEX];   // IF in starting index any own pawn stays, current own pawn goes to home
                                                    Board[PawnHomeIndex[PawnINDEX]] = pawn;
                                                    Board[temporary] = "*";
                                                }
                                                else
                                                { // other player's pawn
                                                  // PawnsNames[i] . taşı kırıcaz.
                                                    IsThereProblem = true;
                                                    Board[PawnHomeIndex[i]] = Board[temporary];          // broken pawn goes home!
                                                    pawns[i] = PawnStartingIndex[i];            // broken pawns value => starting index
                                                    pawnTours[i] = false;
                                                    Board[PawnStartingIndex[PawnINDEX]] = pawn; // start index new values.
                                                    Board[temporary] = "*";                     // on the start index pawn homeIndex => *
                                                    temporary = PawnStartingIndex[PawnINDEX];
                                                }
                                                break;
                                            }
                                        }
                                        if (IsThereProblem == false)
                                        {
                                            Board[PawnStartingIndex[PawnINDEX]] = pawn;
                                            Board[temporary] = "*";
                                            temporary = PawnStartingIndex[PawnINDEX];
                                        }
                                        Console.ReadKey();
                                    } // başlangıca döncek
                                    else if (Board[temp] == "*")
                                    {
                                        Board[temp] = pawn;
                                        Board[temporary] = "*";
                                        temporary = temp;
                                    }
                                }

                            }
                        }
                        if (LegalMove2 == true)
                        {
                            Console.SetCursorPosition(46, 5);
                            Console.Write("No legal move");
                            NoLegalMoveCount++;
                            NewDice = false;
                            LegalMove2 = false;
                            Console.ReadKey();
                        }
                        Console.SetCursorPosition(38, 9);
                        if (player != 1) Console.Write("==> Player {0} played pawn {1}", player, pawn);
                        pawn = "";
                    }
                }
                pawns[PawnINDEX] = temporary; // CURRENT PAWN'S VALUE transfers pawns array
                Console.ReadKey();
                if (((Board[56] != "0" && Board[57] != "0" && Board[58] != "0" && Board[59] != "0")
                    || (Board[60] != "0" && Board[61] != "0" && Board[62] != "0" && Board[63] != "0")
                    || (Board[64] != "0" && Board[65] != "0" && Board[66] != "0" && Board[67] != "0")
                    || (Board[68] != "0" && Board[69] != "0" && Board[70] != "0" && Board[71] != "0")))
                { GameFinish = true; Winner = player; }

                if (PlayAgain == true) { player--; PlayAgain = false; }
                if (dice == 6) player--;
                if (NoLegalMoveCount >= 2)
                {
                    LegalMove = false;
                    LegalMove2 = false;
                    NoLegalMoveCount = 0;
                    player++;
                }
                player++;
                if (player > 4) { player = 1; round++; }
            }
            while (true);
            Console.SetCursorPosition(38, 9);
            Console.WriteLine("     Player {0} Win !    ", Winner);
            Console.SetCursorPosition(0, 20);
            Console.ReadLine();
        }
    }
}
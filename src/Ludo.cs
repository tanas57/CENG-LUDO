using System;
namespace Project2___Ludo
{
    class Ludo
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(71, 22);
            char[] Board = new char[88]; // GAME ARRAY
            char pawn = ' '; char empty = '.'; char slot = '0';
            for (byte j = 0; j < 88; j++) Board[j] = empty;   // EMPTY GAME AREAS
            for (byte j = 56; j <= 71; j++) Board[j] = slot; // FINISH INDEX AREAS
            char[] PawnsNames = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P' }; // PAWNS NAMES
            for (byte j = 0; j < 16; j++) { Board[72 + j] = PawnsNames[j]; } // LOCATED PAWNS NAMES
            // || \\    VARIABLES   // || \\
            byte[] PawnStartingIndex = { 0, 0, 0, 0, 14, 14, 14, 14, 28, 28, 28, 28, 42, 42, 42, 42 }; // EACH PAWNS STARTING INDEX
            byte[] PawnHomeFirstIndex = { 72, 72, 72, 72, 76, 76, 76, 76, 80, 80, 80, 80, 84, 84, 84, 84 }; // PAWS HOME'S FIRST INDEX
            byte[] PawnLastIndex = { 55, 55, 55, 55, 13, 13, 13, 13, 27, 27, 27, 27, 41, 41, 41, 41 }; // PAWS THAT CAN PLAY LAST INDEX
            byte[] PawnHomeIndex = { 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87 }; // PAWNS HOME INDEXS
            byte[] FinishPlaceFirstIndex = { 56, 56, 56, 56, 60, 60, 60, 60, 64, 64, 64, 64, 68, 68, 68, 68 }; // PAWS FINISH AREA'S FIRST INDEXS
            char[] PenaltiesAndRewards = { ')', ')', ')', '(', '(', '(', '<', '<', '>', '>', 'X' }; // REWARDS AND PENALTIES
            int[] pawns = new int[16]; // PAWS VALUES
            int[] WaitIndexValues = new int[16]; // WAIT PENALTIES INDEX VALUES
            bool[] WaitGamer = new bool[4];      // WHICH PLAYER WILL WAIT INFORMATIONS ARRAY
            int[] PlayIndexValues = new int[16]; // PLAY REWARDS INDEX VALUES
            bool[] pawnTours = new bool[16];     // IF COMPUTER'S PAWNS INDEX GREATER THAN 55 PAWNTOURS[İ] TRUE
            Random RD = new Random(); int rand = 0;
            byte round = 1; byte player = 1; int dice = 1;
            bool LegalMove = false, NewDice = true, InvalidMove = false;
            int temporary = 0; int PawnINDEX = 0; bool GameFinish = false;
            byte controlIndex = 72; int control1 = 0, control2 = 0, NoLegalMoveCount = 0;
            bool ComPawnOwner = false, ComPawnOther = false, PlayAgain = false, ComPawnStartPointOtherPlayersPawnHere = false; int ComTemp = 0; byte Winner = 0;
            bool DebugWait = false, DebugPlayer = false; // DEBUG MODES
            // || \\    REWARDS AND PENALTIES LOCATED   // || \\
            for (byte i = 0; i < PenaltiesAndRewards.Length; i++)
            {
                rand = RD.Next(3, 56);
                control1 = rand + 3; control2 = rand - 3;  // Control values is used to control current random number's +3 and -3 index
                // starting points must be empty
                if (!(rand == 14 || rand == 15 || rand == 16 || rand == 28 || rand == 29 || rand == 30 || rand == 42 || rand == 43 || rand == 44))
                {
                    // control values can not bigger 56 and can not smaller 0
                    if (control1 >= 1 && control1 <= 56 && control2 >= 1 && control2 <= 56)
                    {
                        // In index of Controls and random number's must be empty
                        if (Board[rand] == empty && Board[control1] == empty && Board[control2] == empty && (rand != 14 || rand != 28 || rand != 42)) Board[rand] = PenaltiesAndRewards[i];
                        else i--;
                    }
                    else i--;
                }
                else i--;
            }
            do
            {
                // is there legal move, do not change dice.
                if (NewDice == false) { NewDice = true; } // if there is any invalid move, it must not make a new dice
                else { dice = RD.Next(1, 7); }
                // || \\    WAIT COMMAND CONTROL   // || \\
                for (byte j = 0; j <= 3; j++)
                {
                    if (player == 5) player = 4; else if (player == 0) player = 1; // Some bugs fixed. sometimes, player may be five and zero.
                    if (WaitGamer[player - 1] == true) // is current gamer taking wait penalty?
                    {
                        WaitGamer[player - 1] = false;
                        player++;
                    }
                }
                for (byte k = 0; k < 16; k++)
                {
                    // until pawn is stayed wait penalty, wait penalty's index is kept temprory wait array
                    if (WaitIndexValues[k] > 0) // PAWN 
                    {
                        if (WaitIndexValues[k] != pawns[k]) // related pawn index is changed. Wait penalty go back its place
                        {
                            Board[WaitIndexValues[k]] = '<';
                            WaitIndexValues[k] = 0;
                        }
                    }
                }
                // || \\    PLAY ONE MORE TIMES CONTROL   // || \\
                for (byte l = 0; l < 16; l++)
                {
                    // until pawn is stayed play one more time reward, play reward's index is kept temprory playIndexValues array
                    if (PlayIndexValues[l] > 0) // PAWN WAS STAYED ANY '>' REWARD.
                    {
                        if (PlayIndexValues[l] != pawns[l]) // related pawn index is changed. Play one more times rewards go back its place.
                        {
                            Board[PlayIndexValues[l]] = '>';
                            PlayIndexValues[l] = 0;
                        }
                    }
                }
                Console.Clear(); // CLEAR SCREEN
                switch (player) // for each player's first pawn, gives starting index.
                {
                    case 1: controlIndex = 72; break;
                    case 3: controlIndex = 80; break;
                    case 2: controlIndex = 76; break;
                    case 4: controlIndex = 84; break;
                }
                // || \\    DRAW GAME SCREEN   // || \\
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  Player1                       "); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("Player2");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  A B C D"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     ┌───────┐        "); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine(" E F G H");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ╔═════╗"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[12] + " " + Board[13] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[14]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" │        "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(" ╔═════╗       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Round: " + round);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ║ " + Board[72] + " " + Board[73] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[11] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[60]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[15] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("║ " + Board[76] + " " + Board[77] + " ║       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Turn : Player" + player);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ║ " + Board[74] + " " + Board[75] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[10] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[61]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[16] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("║ " + Board[78] + " " + Board[79] + " ║       "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine("Dice : " + dice);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("  ╚═════╝"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[9] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[62]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[17] + " │         "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("╚═════╝       "); Console.ForegroundColor = ConsoleColor.White; if (player == 1 && DebugPlayer == false) Console.WriteLine("Enter Pawn: "); else Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("              │ " + Board[8] + " "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write(Board[63]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" " + Board[18] + " │                ");
                Console.WriteLine("  ┌───────────┘ " + Board[7] + "   " + Board[19] + " └───────────┐");
                Console.Write("  │ "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(Board[0]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" " + Board[1] + " " + Board[2] + " " + Board[3] + " " + Board[4] + " " + Board[5] + " " + Board[6] + "   " + Board[20] + " " + Board[21] + " " + Board[22] + " " + Board[23] + " " + Board[24] + " " + Board[25] + " " + Board[26] + " │");
                Console.Write("  │ " + Board[55] + " "); Console.ForegroundColor = ConsoleColor.Red; Console.Write(Board[56] + " " + Board[57] + " " + Board[58] + " " + Board[59]); Console.ForegroundColor = ConsoleColor.White; Console.Write("     " + dice + "     "); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(Board[67] + " " + Board[66] + " " + Board[65] + " " + Board[64] + " "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(Board[27] + " │");
                Console.Write("  │ " + Board[54] + " " + Board[53] + " " + Board[52] + " " + Board[51] + " " + Board[50] + " " + Board[49] + " " + Board[48] + "   " + Board[34] + " " + Board[33] + " " + Board[32] + " " + Board[31] + " " + Board[30] + " " + Board[29] + " "); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(Board[28]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" │");
                Console.WriteLine("  └───────────┐ " + Board[47] + "   " + Board[35] + " ┌───────────┘");
                Console.Write("              │ " + Board[46] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[71]); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(" " + Board[36] + " │                ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ╔═════╗"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[45] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[70]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[37] + " │       "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("  ╔═════╗");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ║ " + Board[84] + " " + Board[85] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[44] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[69]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[38] + " │   "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("      ║ " + Board[80] + " " + Board[81] + " ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ║ " + Board[86] + " " + Board[87] + " ║"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     │ " + Board[43] + " "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[68]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[39] + " │     "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("    ║ " + Board[82] + " " + Board[83] + " ║");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  ╚═════╝     "); Console.ForegroundColor = ConsoleColor.White; Console.Write("│ "); Console.ForegroundColor = ConsoleColor.Green; Console.Write(Board[42]); Console.ForegroundColor = ConsoleColor.White; Console.Write(" " + Board[41] + " " + Board[40] + " │    "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("     ╚═════╝");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  M N O P"); Console.ForegroundColor = ConsoleColor.White; Console.Write("     └───────┘"); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("         I J K L");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  Player4"); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("                       Player3"); Console.ForegroundColor = ConsoleColor.White;
                if (GameFinish == true) break; // is game over ? broke loop
                int ComPawn = 0;
                string data = "";
                // ARE ALL PAWNS AT HOME ?
                if (Board[controlIndex] != empty && Board[controlIndex + 1] != empty && Board[controlIndex + 2] != empty && Board[controlIndex + 3] != empty)
                {
                    if (dice == 6) // CAN ENTER NEW PAWN
                    {
                        if (player == 1 && DebugPlayer == false) // If player is one, and debug mode must be close
                        {
                            Console.SetCursorPosition(58, 5);
                            data = Console.ReadLine(); // user must chose a pawn
                            if (data.Length == 1) pawn = Convert.ToChar(data.ToUpper());
                            else InvalidMove = true;
                            if (!(pawn == 'A' || pawn == 'B' || pawn == 'C' || pawn == 'D')) { InvalidMove = true; }
                        }
                        else
                        {
                            // current player enter new a pawn according to it's home index
                            // EXAMPLE : Player2's first pawn index is 76 so controlIndex=76 random(76,79)
                            ComPawn = RD.Next(controlIndex, controlIndex + 3);
                            pawn = Board[ComPawn];
                        }
                    }
                    else { Console.SetCursorPosition(46, 5); Console.Write("No legal move            "); }
                }
                else // any pawn is on the game board, select this.
                {
                    //player must choose a pawn which is at game board.
                    if (player == 1 && DebugPlayer == false)
                    {
                        Console.SetCursorPosition(58, 5);
                        data = Console.ReadLine(); // Choosen pawn name
                        if (data.Length == 1) pawn = Convert.ToChar(data.ToUpper()); 
                        else InvalidMove = true;
                        // Invalid move control
                        if (!(pawn == 'A' || pawn == 'B' || pawn == 'C' || pawn == 'D')) { InvalidMove = true; }
                        for (byte u = 0; u < 16; u++)
                        {
                            if (PawnsNames[u] == pawn) // choosen pawn
                            {   // is choosen pawn at game board ?
                                if (Board[PawnHomeIndex[u]] != empty && dice != 6) InvalidMove = true;
                            }
                        }
                    }
                    else
                    {
                        int[] ComPawnPoint = { 0, 0, 0, 0 }; // computer pawns point indexs
                        int[] ComPawnIndex = { 0, 0, 0, 0 }; // computer pawns indexs
                        int PawnCount = 0, PointCount = 0;
                        for (byte j = 0; j < 4; j++) // Control each pawn
                        {
                            ComPawn = controlIndex + j - 72; // CURRENT PAWN
                            ComTemp = pawns[ComPawn] + dice; // CURRENT PAWN VALUE + DICE
                            ComPawnIndex[j] = ComPawn;       // IT USES CHOSING THE BEST PAWN
                            ComPawnOwner = false; ComPawnOther = false; ComPawnStartPointOtherPlayersPawnHere = false; // RESET CONTROL VARIABLES
                            if (Board[PawnHomeIndex[ComPawn]] == empty) // PAWN IS AT GAME BOARD
                            {
                                PawnCount++; // PAWN COUNTER AT THE GAME BOARD
                                // ACCORDING TO RELATED PAWN'S FINISH POINT, IT CALCULATES
                                if (player == 2 && ComTemp > 13 && pawnTours[ComPawn] == true)
                                { // 60 61 62 63 => finishindexs. Change values if current pawn value is between 60 and 63
                                    if (ComTemp >= 60 && ComTemp <= 63) ComTemp = 59 + (ComTemp - 13);
                                }
                                else if (player == 3 && ComTemp > 27 && pawnTours[ComPawn] == true)
                                { // 64 65 66 67
                                    // if current pawn index is between finish indexs, temp changes that 63 + current pawn index - current pawn starting index
                                    if (ComTemp >= 64 && ComTemp <= 67) ComTemp = 63 + (ComTemp - 27);
                                }
                                else if (player == 4 && ComTemp > 41 && pawnTours[ComPawn] == true)
                                {// 68 69 70 71
                                    if (ComTemp >= 68 && ComTemp <= 71) ComTemp = 67 + (ComTemp - 41);
                                }
                                // GRADE CURRENT PAWN => // How much current pawns moved ? 
                                if (ComTemp > 55)
                                {
                                    ComTemp = ComTemp - 56; // if pawn index is greater than 55, pawn's index must be reset. For 
                                    PointCount = (56 - PawnStartingIndex[ComPawn]) + ComTemp; // How much current pawns moved ? 
                                }
                                else if (pawnTours[ComPawn]) // did pawn get a tour ? index 55 => 0
                                    PointCount = (56 - PawnStartingIndex[ComPawn]) + ComTemp;
                                else PointCount = ComTemp - PawnStartingIndex[ComPawn]; // current pawn index - starting index
                                // SMALL GRADE PAWN. According to current pawn how much it moved ?
                                if (PointCount >= 0 && PointCount < 14) PointCount = 1; else if (PointCount >= 14 && PointCount < 28) PointCount = 2;
                                if (PointCount >= 28 && PointCount < 42) PointCount = 3; else if (PointCount >= 42 && PointCount < 56) PointCount = 4;

                                if (pawns[ComPawn] >= FinishPlaceFirstIndex[ComPawn] && pawns[ComPawn] <= FinishPlaceFirstIndex[ComPawn] + 3) // PAWN IS AT FINISH POINT
                                {
                                    if (ComTemp >= FinishPlaceFirstIndex[ComPawn] && ComTemp <= FinishPlaceFirstIndex[ComPawn] + 3 && Board[ComTemp] == slot) // PAWN CAN PLAY AT FINISH POINT
                                        PointCount += 4;
                                    else
                                    {
                                        PawnCount--; // temp is bigger than finishpoint so it is out of board. he can not play.
                                        PointCount = 0;
                                    } // PAWN CAN NOT PLAY, COUNTER AND THIS POINT RESET
                                }
                                else // AT GAME BOARD 0 - 55 INDEX
                                {
                                    // SOME CONTROLS
                                    for (byte i = 0; i < 16; i++)
                                    {
                                        // Accourding to Pawn index + dice, Is any pawn in Board's related index ?
                                        if (Board[ComTemp] == PawnsNames[i]) // CONTROL PLAYER'S PAWS.
                                        {
                                            // Index current pawn's is any of home index, following index is owner pawn
                                            if ((i + 72) >= PawnHomeFirstIndex[ComPawn] && (i + 72) <= PawnHomeFirstIndex[ComPawn] + 3) // owner pawn
                                                ComPawnOwner = true;
                                            else ComPawnOther = true; // or not other player's pawn is here
                                        }
                                        if (Board[PawnStartingIndex[ComPawn]] == PawnsNames[i]) // CONTROL CURRENT PAWN START POINT
                                        {
                                            // is current pawn's starting point empty ? 
                                            if (!((i + 72) >= PawnHomeFirstIndex[ComPawn] && (i + 72) <= PawnHomeFirstIndex[ComPawn] + 3)) // other players pawn
                                                ComPawnStartPointOtherPlayersPawnHere = true; // other player's pawn is here, he can broke this
                                        }
                                    }

                                    if (ComPawnOther == true) // OTHER PLAYER'S PAWN IN FOLLOWING SECTION, IT CAN BE BROKEN
                                    {
                                        ComPawnOwner = false; PointCount += 8;
                                    }
                                    else
                                    {
                                        if (ComPawnOwner == true) // OWNER PAWN IN HERE, CAN NOT PLAY.
                                        {
                                            ComPawnOwner = false;
                                            PointCount = 0;     // next section is owner pawn, do not move it
                                        } // There is not own pawn, and other player's pawn, other possibilities
                                        else
                                        {
                                            if (Board[ComTemp] == ')')        PointCount += 7;   // REWARDS 1 : GO 3 STEP FORWARD
                                            else if (Board[ComTemp] == '>')   PointCount += 6;   // REWARDS 2 : PLAY ONE MORE TIMES
                                            else if (Board[ComTemp] == empty) PointCount += 5;   // NORMAL
                                            else if (Board[ComTemp] == '(')   PointCount += 3;   // PENALTY 1 : GO 3 STEP BACK
                                            else if (Board[ComTemp] == '<')   PointCount += 2;   // PENALTY 2 : WAIT ONE ROUND
                                            else if (Board[ComTemp] == 'X')   PointCount += 1;   // PENALTY 3 : GO PAWN HOME
                                            else {
                                                if (ComTemp >= PawnHomeFirstIndex[ComPawn] && 
                                                    ComTemp <= PawnHomeFirstIndex[ComPawn] + 3 && Board[ComTemp] == slot)
                                                    PointCount += 10; // PAWN CAN ENTER FINISH POINT
                                            } 
                                        }
                                    }
                                    // IS ANY PAWN NEAREST CURRENT PAWN ?
                                    for (byte y = 6; y <= 0; y--)
                                    {
                                        int OtherPawsCon = 0;
                                        OtherPawsCon = ComTemp - y;
                                        // look current pawn's back six index. WHY ? because any player's pawn can be in current pawn's back
                                        if (ComTemp - y < 0) OtherPawsCon = 56 - ComTemp - y;
                                        for (byte pwn = 0; pwn < 16; pwn++)
                                        {
                                            if (Board[OtherPawsCon] == PawnsNames[pwn]) 
                                                // there is a pawn which is nearby current pawn
                                                if (!((pwn + 72) >= PawnHomeFirstIndex[PawnINDEX] && (pwn + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)) PointCount += 5;
                                        }
                                    }
                                    if (ComTemp > FinishPlaceFirstIndex[ComPawn] + 3) PointCount = 0; // OUT OF BOARD. Can not play this
                                }
                            }
                            else { PointCount = 0; ComPawnIndex[j] = 0; } // This pawn is not at game board
                            ComPawnPoint[j] = PointCount;
                            PointCount = 0; // RESET Status
                        }
                        // PAWN CHOOSES
                        if (PawnCount == 1) // JUST ONE PAWN CAN BE PLAYED, CHOOSES IT
                        {
                            for (int j = 0; j < 4; j++) if (ComPawnPoint[j] != 0) { pawn = PawnsNames[ComPawnIndex[j]]; }
                        }
                        else
                            // BETWEEN MORE AND PAWNS. Choose the bigger point of pawns
                            for (int i = 0; i < 4; i++) if ((ComPawnPoint[i] >= ComPawnPoint[0]) && ComPawnPoint[i] != 0) pawn = PawnsNames[ComPawnIndex[i]]; 

                        bool NewPawn = false; byte say = 0;
                        // IF COMPUTER CAN NOT PLAY ANY PAWN, TURN++ | CONTROL CAN IT PLAY NEW PAWN ?
                        for (int a = 0; a < 4; a++) for (int b = 0; b < 56; b++) if (Board[b] == PawnsNames[controlIndex + a - 72]) say++;

                        if (say >= 1) say = 0; 
                        else NewPawn = true; // if computer can not play with any pawn, computer must enter new pawn
                        // DICE MUST BE SIX, NEW PAWN CAN BE TRUE, OR , STARTING INDEX CAN BE EMPTY OR OTHER PLAYERS PAWN.
                        if (dice == 6 && (NewPawn == true || (Board[PawnStartingIndex[ComPawn]] == empty || ComPawnStartPointOtherPlayersPawnHere == true)))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                ComPawn = RD.Next(controlIndex, controlIndex + 4); // random new pawn join in game board
                                if (Board[ComPawn] != empty) { pawn = Board[ComPawn]; break; } // until it choose any pawn
                            }
                            if (NewPawn == true) NewPawn = false; // reset
                        }
                        if (pawn == ' ') LegalMove = true; // if AI can not choose pawn, there is not legal move
                    }
                }
                // CONTROL INVALID MOVE
                if (InvalidMove == true)
                {
                    InvalidMove = false;
                    PlayAgain = true; // play again
                    NewDice = false;  // dice is not changing
                    Console.SetCursorPosition(46, 5);
                    Console.Write("Invalid move         ");
                    pawn = ' ';
                }
                else if (LegalMove == true)
                {
                    LegalMove = false;
                    Console.SetCursorPosition(46, 5);
                    Console.Write("No legal move         ");
                    pawn = ' ';
                }
                else
                {   // pawn is selecteD top of pawns informations. and plays.. Sometimes, player can not play with any pawn
                    if (pawn != ' ')
                    {
                        // GET PAWN INDEX by name
                        for (byte a = 0; a < 16; a++)
                        {
                            if (PawnsNames[a] == pawn) { PawnINDEX = a; break; }
                        }
                        temporary = pawns[PawnINDEX]; // current pawn's index num
                        if (Board[PawnHomeIndex[PawnINDEX]] == pawn) // is selected pawn home?
                        {
                            if (dice == 6)
                            { // if dice 6, player can play one times.
                                if (Board[PawnStartingIndex[PawnINDEX]] == empty) // is starting point empty?
                                {
                                    Board[PawnStartingIndex[PawnINDEX]] = pawn; // startinindex changes with pawn's name
                                    Board[PawnHomeIndex[PawnINDEX]] = empty;    // current pawn's index is it's startingindex
                                    temporary = PawnStartingIndex[PawnINDEX]; // selected Pawns index
                                }
                                else // check the other players pawns. there can be own pawns or other players pawns control it.
                                {
                                    for (byte i = 0; i < 16; i++)
                                    {
                                        if (Board[PawnStartingIndex[PawnINDEX]] == PawnsNames[i])
                                        {
                                            // there is a owner pawn
                                            if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3) LegalMove = true; // owner pawn
                                            else
                                            { // other player's pawn PawnsNames[i] brokes
                                                Board[PawnHomeIndex[i]] = Board[PawnStartingIndex[PawnINDEX]]; // broken pawn goes home!
                                                Board[PawnStartingIndex[PawnINDEX]] = pawn; // start index new values.
                                                Board[PawnHomeIndex[PawnINDEX]] = empty; // on the start index pawn homeIndex => *
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
                            int temp = temporary + dice; // current pawn index + dice
                            int LocatePawnIndex = temporary;
                            int EnterFinishPoint = 0; 
                            if (player == 1 && temp > 55) // pawns can enter to finish point ! PLAYER 1
                            {
                                if (temp >= 56 && temp <= 59) EnterFinishPoint = 1; // pawn can enter finish point
                                else if (temp > 59) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 2 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; } // if player 2,3,4's pawns index is greater than 55, current pawn tour is true
                            else if (player == 3 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; }
                            else if (player == 4 & temp > 55) { temp = temp - 56; pawnTours[PawnINDEX] = true; }
                            // current pawn tours must be true and index bigger than 13
                            if (player == 2 && temp > 13 && pawnTours[PawnINDEX] == true)
                            { // 60 61 62 63
                                temp = 59 + (temp - 13);
                                if (temp >= 60 && temp <= 63) EnterFinishPoint = 1; // pawn can enter finish point
                                else if (temp > 63) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 3 && temp > 27 && pawnTours[PawnINDEX] == true)
                            { // 64 65 66 67
                                temp = 63 + (temp - 27);
                                if (temp >= 64 && temp <= 67) EnterFinishPoint = 1;// pawn can enter finish point
                                else if (temp > 67) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            else if (player == 4 && temp > 41 && pawnTours[PawnINDEX] == true)
                            {// 68 69 70 71
                                temp = 67 + (temp - 41);
                                if (temp >= 68 && temp <= 71) EnterFinishPoint = 1;// pawn can enter finish point
                                else if (temp > 71) EnterFinishPoint = 2;     // ERROR OUT OF BOARD
                            }
                            // PAWN CAN ENTER TO FINISH POINT.
                            if (EnterFinishPoint == 1)
                            {
                                EnterFinishPoint = 0;
                                // Can current pawn enter finish point ?
                                if ((temporary) >= FinishPlaceFirstIndex[PawnINDEX] && (temporary) <= FinishPlaceFirstIndex[PawnINDEX] + 3) 
                                {
                                    // pawn's new value is not bigger than it's finish place indexs
                                    if ((temp) >= FinishPlaceFirstIndex[PawnINDEX] && (temp) <= FinishPlaceFirstIndex[PawnINDEX] + 3)
                                    {   // pawn can move in finish point,
                                        Board[LocatePawnIndex] = slot;  // previus index makes slot's symbo
                                        Board[temp] = pawn; // pawn moves to new index
                                        temporary = temp; // pawn index updates
                                    }
                                    else
                                    {
                                        LegalMove = true; // there is owner pawn in previus index, can not play it
                                        temporary = LocatePawnIndex; // reset pawn index previus value
                                    }
                                }
                                else
                                {
                                    if (Board[temp] == slot) // SLOT IS EMPTY
                                    {
                                        Board[LocatePawnIndex] = empty; // previus index makes empty
                                        Board[temp] = pawn; // pawn moves to finish index
                                        pawnTours[PawnINDEX] = false; // pawn mission complete
                                        temporary = temp; // it is okey.
                                    }
                                    else LegalMove = true; // SLOT IS FULL, ANY PAWN İS HERE
                                }
                            }
                            else if (EnterFinishPoint == 2) // if there is out of board
                                LegalMove = true;
                            else
                            {
                                bool ProblemControl = false;
                                // CONTROL INDEX, IS THERE OTHER PLAYERS PAWNS OR YOURS ?
                                for (int i = 0; i < 16; i++)
                                {
                                    if (Board[temp] == PawnsNames[i])
                                    {
                                        if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                        {   // owner pawn is here
                                            ProblemControl = true;
                                            LegalMove = true;
                                        }
                                        else
                                        { // other player's pawn
                                            ProblemControl = true;
                                            Board[PawnHomeIndex[i]] = Board[temp]; // broken pawn goes home!
                                            Board[temp] = pawn; // start index new values.
                                            Board[temporary] = empty; // on the start index pawn homeIndex => *
                                            pawns[i] = PawnStartingIndex[PawnINDEX]; // broken pawns value => 0
                                            pawnTours[i] = false;
                                            temporary = temp;
                                        }
                                        break;
                                    }
                                }
                                if (ProblemControl == false)
                                {   // 3 STEP FORWARD COMMAND
                                    if (Board[temp] == ')')
                                    {
                                        bool IsThereProblem = false;
                                        int WorkTemp = temp;
                                        temp = temp + 3;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Go 3 step forward");
                                        if ((temp) > PawnLastIndex[PawnINDEX] && pawnTours[PawnINDEX]) // IF REWARDS IS AT BEFORE RELATED PAWN'S FINISH POINT
                                        {   // pawn goes to finish point if there is empty
                                            IsThereProblem = true;
                                            if (player == 2 && temp > 13 && pawnTours[PawnINDEX] == true) temp = 59 + (temp - 13);      // 13 + dice index converts to finish index (60-63)
                                            else if (player == 3 && temp > 27 && pawnTours[PawnINDEX] == true) temp = 63 + (temp - 27); // 27 + dice index converts to finish index (64-67)
                                            else if (player == 4 && temp > 41 && pawnTours[PawnINDEX] == true) temp = 67 + (temp - 41); // 41 + dice index converts to finish index (68-71)

                                            if ((temp) >= FinishPlaceFirstIndex[PawnINDEX] && (temp) <= FinishPlaceFirstIndex[PawnINDEX] + 3) // +3 STEP => EMPTY?
                                            {
                                                if (Board[temp] == slot) // IS SLOT EMPTY?
                                                {
                                                    Board[temp] = pawn;     // slot's new value is pawn
                                                    Board[temporary] = empty; // pawn's old index is empty
                                                    temporary = temp; // pawn new index updates
                                                }
                                                else LegalMove = true;
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
                                                        LegalMove = true;
                                                    }
                                                    else
                                                    { // other player's pawn
                                                        IsThereProblem = true;
                                                        pawns[i] = PawnStartingIndex[i];                    // broken pawns value => starting value.
                                                        Board[PawnHomeIndex[i]] = Board[temp];              // broken pawn goes home!
                                                        pawnTours[i] = false;
                                                        Board[temp] = pawn;                                 // start index new values.
                                                        Board[temporary] = empty;                             // on the start index pawn homeIndex => *
                                                        temporary = temp;
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                        if (IsThereProblem == false)
                                        {
                                            Board[temporary] = empty;  // before not adding dice => *
                                            temporary = temp;        // add dice
                                            Board[temporary] = pawn; // after adding dice => current pawn
                                        }
                                    }// 3 STEP BACK COMMAND 
                                    else if (Board[temp] == '(')
                                    {
                                        bool IsThereProblem = false;
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Go 3 step back");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (Board[temp - 3] == PawnsNames[i]) // PAWN - 3.INDEX CONTROL
                                            {
                                                if ((i + 72) >= PawnHomeFirstIndex[PawnINDEX] && (i + 72) <= PawnHomeFirstIndex[PawnINDEX] + 3)
                                                {   // owner pawn
                                                    IsThereProblem = true;
                                                    LegalMove = true;
                                                }
                                                else
                                                { // other player's pawn
                                                    IsThereProblem = true;
                                                    pawns[i] = PawnStartingIndex[i];         // broken pawns value => starting value.
                                                    Board[PawnHomeIndex[i]] = Board[temp - 3];         // broken pawn goes home!
                                                    pawnTours[i] = false;
                                                    Board[temp - 3] = pawn;                 // start index new values.
                                                    Board[temporary] = empty;                  // on the start index pawn homeIndex => *
                                                    temporary = temp - 3;
                                                }
                                                break;
                                            }
                                        }
                                        if (IsThereProblem == false)
                                        {
                                            Board[temporary] = empty;  // before not adding dice => *
                                            temporary = temp - 3;    // add dice
                                            Board[temporary] = pawn; // after adding dice => current pawn
                                        }
                                    } // PLAY ONE MORE TIMES
                                    else if (Board[temp] == '>')
                                    {
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("play one more time");
                                        PlayIndexValues[PawnINDEX] = temp;  // play reward's index tranfers to play temprorary array
                                        Board[temporary] = empty;           // previous index makes empty
                                        Board[temp] = pawn;                 // new index updates pawns name
                                        temporary = temp;                   // pawn index updates
                                        PlayAgain = true;                   // play one more time
                                    } // WAIT ONE ROUND
                                    else if (Board[temp] == '<')
                                    {
                                        WaitIndexValues[PawnINDEX] = temp;  // wait reward's index transfers to wait temprorary array
                                        WaitGamer[player - 1] = true;       // player which will wait is defined
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Wait one round!");
                                        Board[temporary] = empty;           // previous index makes empty
                                        Board[temp] = pawn;                 // new index updates with pawn name
                                        temporary = temp;                   // pawn index updates
                                    } // Pawn goes to home
                                    else if (Board[temp] == 'X')
                                    {
                                        Console.SetCursorPosition(45, 6);
                                        Console.Write("Pawn goes to home");
                                        Board[PawnHomeIndex[PawnINDEX]] = pawn;    // current pawn goes to it's home
                                        Board[temporary] = empty;                  // previous pawn place makes empty
                                        temporary = PawnStartingIndex[PawnINDEX];  // pawn index updates
                                    } // normal playing
                                    else if (Board[temp] == empty)
                                    {
                                        Board[temp] = pawn;         // following section is current pawn name
                                        Board[temporary] = empty;   // previous location makes empty
                                        temporary = temp;           // pawn index updates
                                    }
                                }
                            }
                        }
                        if (LegalMove == true) // There is a no legal move
                        {
                            Console.SetCursorPosition(46, 5);
                            Console.Write("No legal move            ");
                            LegalMove = false;
                        }
                        if (InvalidMove) { InvalidMove = false; NewDice = false; } // dice must not change
                        Console.SetCursorPosition(38, 9);
                        if (player != 1) Console.Write("==> Player {0} played pawn {1}", player, pawn);
                        pawn = ' ';
                    }
                }
                pawns[PawnINDEX] = temporary; // CURRENT PAWN'S VALUE transfers pawns array
                if (((Board[56] != slot && Board[57] != slot && Board[58] != slot && Board[59] != slot)
                    || (Board[60] != slot && Board[61] != slot && Board[62] != slot && Board[63] != slot)// CONTROL PAWNS FINISH INDEX
                    || (Board[64] != slot && Board[65] != slot && Board[66] != slot && Board[67] != slot)// ANY PLAYER FINISHS THE GAME
                    || (Board[68] != slot && Board[69] != slot && Board[70] != slot && Board[71] != slot)))// BREAK THE LOOP
                { GameFinish = true; Winner = player; }
                if (PlayAgain) { player--; PlayAgain = false; } // Play one more times
                if (dice == 6) player--; // Play one more times
                if (NoLegalMoveCount >= 2) { LegalMove = false; NoLegalMoveCount = 0; player++; } // if computer makes two more no legal move, the turn passes next player
                if (DebugWait) System.Threading.Thread.Sleep(30);
                else Console.ReadKey();
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
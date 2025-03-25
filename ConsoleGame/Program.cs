namespace ConsoleGame
{
    internal class Program
    {
        // 2차원 좌표를 설정하는 구조체
        struct Position
        {
            // x 좌표와 y 좌표를 설정, 좌상단을 기준으로 0, 0
            public int x;
            public int y;
        }
        // 게임의 상태를 체크할 열거형, 플레이중 0, 실패 1, 클리어 2
        enum GameState
        {
            Play,
            Fail,
            Clear,
        }
        static void Main(string[] args)
        {
            // 플레이어 위치를 저장할 Position 구조체 타입 변수 playerPos 와 초기 위치를 저장할 initPos
            // 키입력을 받아 저장해놓을 ConsoleKey 열거형 변수 key
            // 게임 상태를 받아 저장해놓을 GameState 열거형 변수 gameState 
            // 아이템 갯수를 체크할 ItemCount
            // 움직임 횟수를 체크할 moveCount
            // 움직임 횟수의 제한을 체크할 moveLimit
            // 인 게임 텍스트의 변화를 저장하고 출력할 문자열 text
            // 맵 구조를 저장할 2차원 char 배열 map
            // - 빈 공간 : ' ', 벽 : '#', 목적지 : Goal의 'G', 함정 : Trap의 'T', 아이템 : Item의 'I'
            // 변수타입 | 변수명   | 변수 내용
            Position playerPos = new Position();
            Position initPos = new Position();
            ConsoleKey key = new ConsoleKey();
            GameState gameState = new GameState();
            int itemCount = 0;
            int itemRequire = 0;
            int moveCount = 0;
            int moveLimit = 60;
            string text = "";
            char[,] map = new char[15, 15]
            {
                { '#','#','#','#','#', '#','#','#','#','#', '#','#','#','#','#'},
                { '#',' ',' ','#',' ', ' ',' ',' ',' ',' ', ' ',' ',' ',' ','#'},
                { '#',' ',' ','#',' ', ' ',' ','#',' ',' ', ' ',' ','I',' ','#'},
                { '#',' ',' ','#',' ', 'I',' ','#',' ',' ', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', 'I',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#','T','#', ' ',' ','#',' ','#'},
                { '#',' ',' ',' ',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ',' ',' ', '#',' ',' ',' ','#', ' ','T','#',' ','#'},
                { '#',' ','I',' ',' ', '#',' ',' ',' ','#', ' ',' ',' ',' ','#'},
                { '#','T',' ',' ',' ', ' ',' ',' ',' ',' ', ' ',' ',' ','G','#'},
                { '#','#','#','#','#', '#','#','#','#','#', '#','#','#','#','#'},
            };
            char[,] initMap = new char[15, 15]
            {
                { '#','#','#','#','#', '#','#','#','#','#', '#','#','#','#','#'},
                { '#',' ',' ','#',' ', ' ',' ',' ',' ',' ', ' ',' ',' ',' ','#'},
                { '#',' ',' ','#',' ', ' ',' ','#',' ',' ', ' ',' ','I',' ','#'},
                { '#',' ',' ','#',' ', 'I',' ','#',' ',' ', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', 'I',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ','#',' ', '#',' ','#','T','#', ' ',' ','#',' ','#'},
                { '#',' ',' ',' ',' ', '#',' ','#',' ','#', ' ',' ','#',' ','#'},
                { '#',' ',' ',' ',' ', '#',' ',' ',' ','#', ' ','T','#',' ','#'},
                { '#',' ','I',' ',' ', '#',' ',' ',' ','#', ' ',' ',' ',' ','#'},
                { '#','T',' ',' ',' ', ' ',' ',' ',' ',' ', ' ',' ',' ','G','#'},
                { '#','#','#','#','#', '#','#','#','#','#', '#','#','#','#','#'},
            };
            // 0. 기본적인 설정을 한다. 함수 종료 시 플레이어 위치와 맵 구조를 받아간다.
            Start(ref playerPos, ref initPos, ref gameState, ref itemRequire, map);
            // 게임이 끝날 때 까지 무한반복
            while (gameState == 0)
            {
                // 1. 맵, 플레이어, 텍스트를 출력한다.
                Render(playerPos, map, text, itemRequire - itemCount, moveCount, moveLimit);
                // 2. 입력을 받는다
                key = Input();
                // 2-1. 그것이 R키일 경우 맵을 리셋하여 처음 상태로 되돌린다.
                if (key == ConsoleKey.R)
                {
                    // 2-2. 혹시나 잘못 눌러 리셋하는 사태를 방지하고자 재확인하는 텍스트 출력
                    RenderText("리셋하시겠습니까? (Y / N)                   ", map);
                    // 2-3. 리셋 의도가 확실하여 리셋 진행
                    if (Input() == ConsoleKey.Y)
                    {
                        Reset(ref playerPos, ref map, ref moveCount, ref itemCount, initPos, initMap);
                    }
                }
                // 3. 입력된 key 값에 따른 플레이어 위치 변환. 플레이어의 위치에 따른 게임 상태 변환
                Update(key, ref playerPos, ref text, map, ref itemCount, ref moveCount, ref gameState, itemRequire);
                if (moveCount > moveLimit)
                {
                    gameState = GameState.Fail;
                    text = "움직임 횟수 초과!                          ";
                }
            }
            // 게임이 끝날 경우 클리어/실패 여부에 따른 게임 엔딩 출력
            End(gameState, map, text);
        }
        // 게임 시작 설정
        // 바꿔야 할 변수 : 플레이어 위치, 게임 상태
        // 설정 해 줘야 할 변수 : 플레이어 초기 위치
        static void Start(ref Position playerPos, ref Position initPos, ref GameState gameState, ref int itemRequire, char[,] map)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            playerPos.x = 1;
            playerPos.y = 1;
            initPos.x = playerPos.x;
            initPos.y = playerPos.y;
            foreach (char c in map)
            {
                if (c == 'I')
                {
                    itemRequire++;
                }
            }
            Console.WriteLine("한붓 미로!");
            Console.WriteLine("아무 키나 눌러 게임을 진행하세요");
            Console.ReadKey(true);
            gameState = 0;
            Console.Clear();
        }
        // 게임 출력 설정
        // 게임 출력은 미로 화면에서 두줄 띄운 곳에서 출력함
        static void Render(Position playerPos, char[,] map, string text, int itemCount, int moveCount, int moveLimit)
        {
            // 콘솔창 반짝거림 해소용 커서 위치 좌상단으로 초기화
            Console.SetCursorPosition(0, 0);
            // 맵과 플레이어를 순서대로 출력
            RenderMap(map);
            RenderPlayer(playerPos);
            // 상태에 따라 결과 출력
            RenderText($"아이템 획득 : {itemCount}, 남은 움직임 횟수 : {moveLimit - moveCount}          \n{text}", map);
        }
        // 맵 출력
        static void RenderMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j])
                    {
                        case '#':
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case 'G':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case 'T':
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            break;
                        case 'I':

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 'X':
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                    }
                    Console.Write(map[i, j]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        // 플레이어 출력
        static void RenderPlayer(Position playerPos)
        {
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('P');
            Console.ResetColor();
        }
        static void RenderText(string text, char[,] map)
        {
            Console.SetCursorPosition(0, map.GetLength(1));
            Console.WriteLine("====================");
            Console.WriteLine(text);
        }
        // 키값을 입력
        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }
        // 입력받은 키 값에 따른 처리 작업 실행
        static void Update(ConsoleKey key, ref Position playerPos, ref string text, char[,] map, ref int itemCount, ref int moveCount, ref GameState gameState, int itemRequire)
        {
            Move(key, ref playerPos, ref text, map, ref itemCount, ref moveCount, ref gameState, itemRequire);
        }
        // 입력받은 키의 방향에 따른 도착 위치 targetPos 값 지정
        // 지나간 곳에는 다시 못지나가게 기존 위치에 'X'를 추가
        // targetPos 갑에 뭐가 있는지에 따라 아래의 행동을 진행
        // 빈 공간이면 전진.
        // 게임 오버 조건 :  1. 함정('T') 밟음, 2. 왔던 길('X') 밟음
        // 클리어 조건 : 아이템 다 먹고 목적지('G')로 가기

        static void Move(ConsoleKey key, ref Position playerPos, ref string text, char[,] map, ref int itemCount, ref int moveCount, ref GameState gameState, int itemRequire)
        {
            Position targetPos;
            map[playerPos.y, playerPos.x] = 'X';
            moveCount++;
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    targetPos.x = playerPos.x - 1;
                    targetPos.y = playerPos.y;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    targetPos.x = playerPos.x + 1;
                    targetPos.y = playerPos.y;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y - 1;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y + 1;
                    break;
                default:
                    return;
            }

            // 움직이는 방향에 골이 있을 때
            if (map[targetPos.y, targetPos.x] == 'G')
            {
                // 골에 도착했고 아이템을 전부 먹은 상태여야 클리어 가능
                if (itemRequire - itemCount == 0)
                {
                    text = "클리어! 골에 도착했습니다!                         \n\n아무 키나 눌러 진행";
                    playerPos.x = targetPos.x;
                    playerPos.y = targetPos.y;
                    gameState = GameState.Clear;
                }
                else
                {
                    text = "실패... 아이템을 전부 먹어야 클리어가 가능합니다                      ";
                    gameState = GameState.Fail;
                }
            }
            // 움직이는 방향에 함정이 있을 때
            else if (map[targetPos.y, targetPos.x] == 'T')
            {
                text = "실패... 함정을 밟았습니다.                            \n\n아무 키나 눌러 진행";
                playerPos.x = targetPos.x;
                playerPos.y = targetPos.y;
                gameState = GameState.Fail;
            }
            // 움직이는 방향에 아이템이 있을 때
            else if (map[targetPos.y, targetPos.x] == 'I')
            {
                text = "아이템 획득!";
                itemCount++;
                playerPos.x = targetPos.x;
                playerPos.y = targetPos.y;
            }
            // 움직이는 방향에 빈칸일 때
            else if (map[targetPos.y, targetPos.x] == ' ')
            {
                text = "                                                    ";
                playerPos.x = targetPos.x;
                playerPos.y = targetPos.y;
            }
            // 움직이는 방향이 왔던 길인 경우
            else if (map[targetPos.y, targetPos.x] == 'X')
            {
                text = "실패... 왔던 길로 되돌아갈 수 없습니다.                        \n\n아무 키나 눌러 진행";
                gameState = GameState.Fail;
                // 아무것도 안함
            }
            // 움직이는 방향에 벽일 때
            else if (map[targetPos.y, targetPos.x] == '#')
            {
                // 벽이 있음에도 진행할려고 하면 moveCount가 증가하는 이슈 발생
                // 해결하기 위해 moveCount를 하나 감소
                moveCount--;
                text = "";
            }
        }
        // 게임 상태 리셋
        // 캐릭터의 위치와 맵 상태, 움직임 제약 횟수까지 초기화한다.
        static void Reset(ref Position playerPos, ref char[,] map, ref int moveCount, ref int itemCount, Position initPos, char[,] initMap)
        {
            ResetPlayer(ref playerPos, initPos, ref moveCount, ref itemCount);
            ResetMap(ref map, initMap);
        }
        // 캐릭터 위치, 정보들 리셋
        static void ResetPlayer(ref Position playerPos, Position initPos, ref int moveCount, ref int itemCount)
        {
            playerPos.x = initPos.x;
            playerPos.y = initPos.y;
            moveCount = -1;
            itemCount = 0;
        }
        // 맵 리셋
        static void ResetMap(ref char[,] map, char[,] initMap)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != initMap[i, j])
                    {
                        map[i, j] = initMap[i, j];
                    }
                }
            }
        }
        // 게임 끝
        // 어떻게 끝났는지에 따라 다른 값을 출력
        static void End(GameState gameState, char[,] map, string text)
        {
            string endText = text;
            RenderText(endText, map);
            Input();
            if (gameState == GameState.Clear)
            {
                endText = "***게임 클리어!***                            ";
            }
            if (gameState == GameState.Fail)
            {
                endText = "___게임 실패___                              ";
            }
            RenderText(endText, map);
        }
    }
}

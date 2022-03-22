using System;

namespace Unity_CSharp
{
    class TextRPG_Choose_Job
    {
        enum JobType
        {
            None = 0,
            Knight = 1,
            Archer = 2,
            Mage = 3
        }

        struct Player
        {
            public int hp;
            public int attack;
        }

        enum MonsterType
        {
            None = 0,
            Slime = 1,
            Orc = 2,
            Skeleton = 3
        }
        struct Monster
        {
            public int hp;
            public int attack;
        }

        static JobType ChooseClass()
        {
            Console.WriteLine("직업을 선택하세요!");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");

            JobType choice = JobType.None; // 다양한 범위의 컨텍스트에 적용하기 위해 가장 위에 선언

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    choice = JobType.Knight;
                    break;
                case "2":
                    choice = JobType.Archer;
                    break;
                case "3":
                    choice = JobType.Mage;
                    break;
            }

            return choice;
        }

        static void CreatePlayer(JobType choice, out Player player)
        {
            // 기사(100/10), 궁수(75/12), 법사(50/15)
            switch (choice)
            {
                case JobType.Knight:
                    player.hp = 100;
                    player.attack = 10;
                    break;
                case JobType.Archer:
                    player.hp = 75;
                    player.attack = 12;
                    break;
                case JobType.Mage:
                    player.hp = 50;
                    player.attack = 15;
                    break;
                default:
                    player.hp = 0;
                    player.attack = 0;
                    break;
            }
        }

        static void CreateRandomMonster(out Monster monster)
        {
            Random rand = new Random();
            int randMonster = rand.Next(1, 4); // 1~3 사이의 숫자

            // 랜덤으로 1~3 몬스터 중 하나를 리스폰
            switch (randMonster)
            {
                case (int)MonsterType.Slime:
                    Console.WriteLine("슬라임이 스폰되었습니다.");
                    monster.hp = 20;
                    monster.attack = 2;
                    break;
                case (int)MonsterType.Orc:
                    Console.WriteLine("오크가 스폰되었습니다.");
                    monster.hp = 40;
                    monster.attack = 4;
                    break;
                case (int)MonsterType.Skeleton:
                    Console.WriteLine("스켈레톤이 스폰되었습니다.");
                    monster.hp = 30;
                    monster.attack = 2;
                    break;
                default:
                    monster.hp = 0;
                    monster.attack = 0;
                    break;
            }
        }

        static void Fight(ref Player player, ref Monster monster)
        {
            while (true)
            {
                // 플레이어가 몬스터 공격
                monster.hp -= player.attack;
                if (monster.hp <= 0)
                {
                    Console.WriteLine("승리했습니다.");
                    Console.WriteLine($"남은 체역 : {player.hp}");
                    break;
                }

                // 몬스터 반격
                player.hp -= monster.attack;
                if (player.hp <= 0)
                {
                    Console.WriteLine("패배했습니다.");
                    break;
                }
            }
        }

        static void EnterField(ref Player player)
        {
            while (true)
            {
                Console.WriteLine("필드에 접속했습니다.");

                // 랜덤으로 1~3 몬스터 중 하나를 리스폰
                Monster monster;
                CreateRandomMonster(out monster);

                Console.WriteLine("[1] 전투 모드로 돌입");
                Console.WriteLine("[2] 일정 확률로 마을로 도망");

                string input = Console.ReadLine();
                if (input == "1")
                {
                    Fight(ref player, ref monster);
                }
                else if (input == "2")
                {
                    // 33%
                    Random rand = new Random();
                    int randValue = rand.Next(0, 101);
                    if (randValue <= 33)
                    {
                        Console.WriteLine("도망에 성공했습니다.");
                        break;
                    }
                    else
                    {
                        Fight(ref player, ref monster);
                    }
                }
            }
        }

        static void EnterGame(ref Player player)
        {
            while (true)
            {
                Console.WriteLine("마을에 접속했습니다.");
                Console.WriteLine("[1] 필드로 간다");
                Console.WriteLine("[2] 로비로 돌아가기");

                string input = Console.ReadLine();
                if (input == "1")
                {
                    EnterField(ref player);
                }
                else if (input == "2")
                {
                    break; // 여기에서 break 가 걸리면 while문을 빠져나옴
                }

                //switch (input)
                //{
                //    case "1":
                //        // EnterField();
                //        break;
                //    case "2":
                //        return; // return시 switch문이 아닌 함수 자체를 빠져나옴
                //}
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                JobType choice = ChooseClass(); // 다양한 범위의 컨텍스트에 적용하기 위해 가장 위에 선언                
                if (choice != JobType.None)
                {
                    // 캐릭터 생성
                    Player player;
                    CreatePlayer(choice, out player);

                    EnterGame(ref player);
                }
            }
        }
    }
}
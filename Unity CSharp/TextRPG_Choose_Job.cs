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

        static JobType ChooseClass()
        {
            Console.WriteLine("직업을 선택하세요!");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 기사");

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

        static void Main(string[] args)
        {          
            while (true)
            {
                JobType choice = ChooseClass(); // 다양한 범위의 컨텍스트에 적용하기 위해 가장 위에 선언                
                if (choice != JobType.None)
                {
                    // 캐릭터 생성
                    Player player;

                    //10개
                    CreatePlayer(choice, out player);

                    Console.WriteLine($"HP{player.hp} Attack{player.attack}");              
                                      
                    // 필드로 가서 몬스터랑 싸운다
                    break;
                }
            }            
        }
    }
}
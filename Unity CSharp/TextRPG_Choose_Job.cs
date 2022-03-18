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

        static void Main(string[] args)
        {          
            while (true)
            {
                JobType choice = ChooseClass(); // 다양한 범위의 컨텍스트에 적용하기 위해 가장 위에 선언                
                if (choice != JobType.None)
                {
                    break;
                }
            }            
        }
    }
}
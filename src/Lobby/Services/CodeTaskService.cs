using Lobby.Models;

namespace Lobby.Services
{
    public class CodeTaskService
    {
        public List<CodeTask> CodeTasks { get; set; } = new List<CodeTask>()
        {
            new CodeTask
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Додавання двох чисел!",
                Description = "Написати функцію function addNumbers(a, b) яка б в результаті видавла суму двох чисел.",
                Tests = new List<CodeTaskTest>
                {
                    new CodeTaskTest 
                    { 
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "10",
                        InputData = "addNumbers(5, 5)"
                        
                    },
                    new CodeTaskTest
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "15",
                        InputData = "addNumbers(10, 5)"

                    },
                    new CodeTaskTest
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "0",
                        InputData = "addNumbers(5, -5)"

                    }
                }
            },
            new CodeTask
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Віднімання двох чисел!",
                Description = "Написати функцію function divisionNumbers(a, b) яка б в результаті видавла різницю двох чисел.",
                Tests = new List<CodeTaskTest>
                {
                    new CodeTaskTest
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "0",
                        InputData = "divisionNumbers(5, 5)"

                    },
                    new CodeTaskTest
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "5",
                        InputData = "divisionNumbers(10, 5)"

                    },
                    new CodeTaskTest
                    {
                        Id = Guid.NewGuid().ToString(),
                        ExpectedResult = "10",
                        InputData = "divisionNumbers(5, -5)"

                    }
                }
            }
        };

        public CodeTask GetRandomTask()
        {
            var rand = new Random();
            return CodeTasks[rand.Next(CodeTasks.Count)];
        }

    }
}

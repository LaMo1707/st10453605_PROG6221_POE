using System.Collections.Generic;

namespace ST10453605_PROG6221_POE
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectOption { get; set; }
        public string Explanation { get; set; }
    }
}

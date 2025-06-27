using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ST10453605_PROG6221_POE
{
    public partial class MainWindow : Window
    {
        private string userName = "User";
        private string userInterest;
        private string lastTopic;

        private enum ConversationState
        {
            InitialGreeting,
            AwaitingName,
            NormalConversation
        }
        private ConversationState currentState = ConversationState.InitialGreeting;


        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            { "password", new List<string>
                {
                    "Strong passwords are essential! Use a mix of letters, numbers, and symbols — and never reuse them across sites.",
                    "Using a password manager is a great way to stay organised and secure. Keep it up!",
                    "Avoid personal info like birthdays or names in passwords. Stay unpredictable!"
                }
            },
            { "phishing", new List<string>
                {
                    "Phishing attempts often come with urgent messages. Pause and verify before clicking any links.",
                    "Always check the sender’s email address carefully. Scammers love to impersonate trusted brands.",
                    "If in doubt, don’t click! Better to visit the site directly than follow suspicious links."
                }
            },
            { "scam", new List<string>
                {
                    "Online scams are everywhere. Stay alert and don’t share personal info unless you're 100% sure.",
                    "Scammers often create fake urgency. Don’t rush into anything — take a moment to double-check.",
                    "Protect your financial info by ignoring unexpected messages requesting payments or passwords."
                }
            },
            { "privacy", new List<string>
                {
                    "It’s smart to care about privacy! Check your app permissions regularly and limit data sharing.",
                    "Use privacy settings on social media to control who sees what. You're in charge of your info!",
                    "Consider tools like VPNs and encrypted messaging apps to strengthen your privacy online."
                }
            },
            { "safe browsing", new List<string>
                {
                    "Always look for 'https://' in URLs — it means the site is secure!",
                    "Avoid downloading files from unknown sources, even if the site looks legit.",
                    "Update your browser and antivirus software regularly for the safest online experience."
                }
            }
        };

        private Dictionary<string, string> sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "worried", "It’s totally okay to feel worried. Cybersecurity can be overwhelming — I’m here to guide you. 😊" },
            { "curious", "I love your curiosity! That’s the first step to becoming more cyber-aware. Let’s explore!" },
            { "frustrated", "I'm sorry you're feeling frustrated. Let's take it one step at a time — you've got this!" },
            { "confused", "That’s okay! Cyber topics can be tricky. I'm happy to explain anything more clearly." }
        };

        public ObservableCollection<CyberTask> Tasks = new ObservableCollection<CyberTask>();
        public Queue<string> Logs = new Queue<string>();
        public List<QuizQuestion> QuizQuestions = new List<QuizQuestion>();
        private int currentQuizIndex = 0;
        private int quizScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            TaskList.ItemsSource = Tasks;
            LoadQuizQuestions();
            DisplayQuizQuestion();

            ChatHistory.Items.Add("Bot: Hello! I'm your Cybersecurity Assistant. What's your name?");
            currentState = ConversationState.AwaitingName;
        }

        private string HandleUserInput(string input)
        {
            string normalizedQuery = input.ToLower().Trim();

            switch (currentState)
            {
                case ConversationState.AwaitingName:
                    string[] words = input.Split(new char[] { ' ', ',', '.', '!' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length > 0)
                    {
                        userName = words[0];
                        currentState = ConversationState.NormalConversation;
                        return $"Nice to meet you, {userName}! How can I help you with cybersecurity today?";
                    }
                    else
                    {
                        return "I didn't catch that. Could you please tell me your name?";
                    }

                case ConversationState.NormalConversation:
                    foreach (var sentiment in sentimentResponses.Keys)
                    {
                        if (normalizedQuery.Contains(sentiment))
                            return sentimentResponses[sentiment];
                    }

                    if (normalizedQuery.Contains("i'm interested in"))
                    {
                        userInterest = ExtractInterest(normalizedQuery);
                        return $"That’s awesome, {userName}! I’ll remember that you’re interested in {userInterest}. 🌐";
                    }

                    foreach (var keyword in keywordResponses.Keys)
                    {
                        if (normalizedQuery.Contains(keyword))
                        {
                            lastTopic = keyword;
                            return GetRandomResponse(keywordResponses[keyword]);
                        }
                    }

                    if ((normalizedQuery.Contains("more info") || normalizedQuery.Contains("tell me more")) && lastTopic != null)
                    {
                        return GetRandomResponse(keywordResponses[lastTopic]);
                    }

                    if (userInterest != null && normalizedQuery.Contains("tip"))
                    {
                        return $"As someone interested in {userInterest}, here’s a good tip: stay informed and review your account settings regularly!";
                    }

                    if (normalizedQuery.Contains("task"))
                        return "You can manage cybersecurity tasks in the 'Tasks' tab.";

                    if (normalizedQuery.Contains("quiz"))
                        return "You can test your knowledge in the 'Quiz' tab.";

                    if (normalizedQuery.Contains("log") || normalizedQuery.Contains("what have you done"))
                    {
                        ActivityLog.Items.Clear();
                        foreach (var log in Logs.Reverse())
                            ActivityLog.Items.Add(log);
                        return "Here is your recent activity log.";
                    }

                    return GetRandomResponse(new List<string>
                    {
                        "Hmm, I’m not quite sure I understood that. Could you please rephrase?",
                        "I didn’t catch that — try asking about scams, passwords, or privacy tips!",
                        "Let’s stick to cybersecurity topics. Want to know more about phishing or password safety?",
                        "That’s interesting! Could you tell me more or ask a different way?"
                    });

                default:
                    return "An unexpected error occurred in conversation flow.";
            }
        }

        private string ExtractInterest(string input)
        {
            return input.Replace("i'm interested in", "").Trim(new char[] { '.', '!', '?', ' ' });
        }

        private string GetRandomResponse(List<string> responses)
        {
            Random rand = new Random();
            return responses[rand.Next(responses.Count)];
        }

        private void LogAction(string message)
        {
            if (Logs.Count >= 10) Logs.Dequeue();
            Logs.Enqueue($"{DateTime.Now:T} - {message}");
            ActivityLog.Items.Add($"{DateTime.Now:T} - {message}");
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = ChatInput.Text;
            if (string.IsNullOrWhiteSpace(input)) return;

            ChatHistory.Items.Add("You: " + input);
            string response = HandleUserInput(input);
            ChatHistory.Items.Add("Bot: " + response);
            ChatInput.Clear();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var task = new CyberTask
            {
                Title = TaskTitleInput.Text,
                Description = TaskDescInput.Text,
                Reminder = ReminderInput.Text
            };
            Tasks.Add(task);
            LogAction($"Task added: {task.Title} (Reminder: {task.Reminder})");

            TaskTitleInput.Clear();
            TaskDescInput.Clear();
            ReminderInput.Clear();
        }

        private void LoadQuizQuestions()
        {
            QuizQuestions.Add(new QuizQuestion
            {
                Question = "What is the primary purpose of a firewall?",
                Options = new List<string> { "To block spam emails", "To prevent unauthorized access to a network", "To speed up internet connection", "To encrypt data" },
                CorrectOption = "To prevent unauthorized access to a network",
                Explanation = "A firewall acts as a barrier between a trusted internal network and untrusted external networks, controlling incoming and outgoing network traffic."
            });

            QuizQuestions.Add(new QuizQuestion
            {
                Question = "Which of the following is a common sign of a phishing email?",
                Options = new List<string> { "Perfect grammar and spelling", "A generic greeting (e.g., 'Dear Customer')", "A legitimate sender email address", "No links or attachments" },
                CorrectOption = "A generic greeting (e.g., 'Dear Customer')",
                Explanation = "Phishing emails often use generic greetings because they are sent to a large number of recipients, making it impractical to personalize each one."
            });

            QuizQuestions.Add(new QuizQuestion
            {
                Question = "What does 'MFA' stand for in cybersecurity?",
                Options = new List<string> { "Malware File Analysis", "Multi-Factor Authentication", "Mainframe Access", "Managed Firewall Appliance" },
                CorrectOption = "Multi-Factor Authentication",
                Explanation = "Multi-Factor Authentication (MFA) requires users to provide two or more verification factors to gain access to a resource, like an application, online account, or VPN."
            });

            QuizQuestions.Add(new QuizQuestion
            {
                Question = "What is ransomware?",
                Options = new List<string> { "Software that optimizes computer performance", "Malware that encrypts files and demands payment for decryption", "A tool for secure file sharing", "A type of antivirus software" },
                CorrectOption = "Malware that encrypts files and demands payment for decryption",
                Explanation = "Ransomware is a type of malicious software that encrypts a victim's files. The attacker then demands a ransom from the victim to restore access to the data upon payment."
            });

            QuizQuestions.Add(new QuizQuestion
            {
                Question = "Which of these is NOT a good practice for creating a strong password?",
                Options = new List<string> { "Using a combination of uppercase and lowercase letters", "Including numbers and symbols", "Making it at least 12 characters long", "Using easily guessable personal information" },
                CorrectOption = "Using easily guessable personal information",
                Explanation = "Avoid using personal information like birthdays, names, or common words, as these are easy for attackers to guess or find."
            });
        }

        private void DisplayQuizQuestion()
        {
            if (QuizQuestions.Count == 0)
            {
                QuizQuestionText.Text = "No quiz questions loaded. Please add questions to the quiz.";
                QuizAnswers.ItemsSource = null;
                QuizFeedback.Text = string.Empty;
                return;
            }

            if (currentQuizIndex >= QuizQuestions.Count)
            {
                QuizQuestionText.Text = $"Quiz finished! Your score: {quizScore} out of {QuizQuestions.Count}.";
                QuizAnswers.ItemsSource = null;
                QuizFeedback.Text = "Well done! You've completed the quiz.";
                LogAction($"Quiz completed. Score: {quizScore}/{QuizQuestions.Count}");
                return;
            }

            QuizQuestion currentQuestion = QuizQuestions[currentQuizIndex];
            QuizQuestionText.Text = currentQuestion.Question;
            QuizAnswers.ItemsSource = currentQuestion.Options;
            QuizAnswers.SelectedItem = null;
            QuizFeedback.Text = string.Empty;
        }

        private void QuizAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuizAnswers.SelectedItem == null || currentQuizIndex >= QuizQuestions.Count) return;

            QuizQuestion currentQuestion = QuizQuestions[currentQuizIndex];
            string selectedAnswer = QuizAnswers.SelectedItem.ToString();

            if (selectedAnswer == currentQuestion.CorrectOption)
            {
                QuizFeedback.Text = "Correct! " + currentQuestion.Explanation;
                quizScore++;
            }
            else
            {
                QuizFeedback.Text = $"Incorrect. The correct answer was: {currentQuestion.CorrectOption}. {currentQuestion.Explanation}";
            }
            QuizAnswers.IsEnabled = false;
        }

        private void NextQuizQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (QuizAnswers.SelectedItem == null && currentQuizIndex < QuizQuestions.Count)
            {
                QuizFeedback.Text = "Please select an answer before proceeding.";
                return;
            }

            currentQuizIndex++;
            QuizAnswers.IsEnabled = true;
            DisplayQuizQuestion();
        }
    }
}

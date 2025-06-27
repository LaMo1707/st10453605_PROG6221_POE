# Cybersecurity Chatbot

This is a simple WPF application designed to act as a cybersecurity assistant. It features a chatbot interface, a task management system, a cybersecurity quiz, and an activity log.

## Features

*   **Chatbot:** Interact with an AI assistant that provides information and tips on various cybersecurity topics like passwords, phishing, scams, privacy, and safe browsing. It includes basic natural language processing (NLP) to understand user intent and sentiment.
*   **Task Management:** Add and track cybersecurity-related tasks with titles, descriptions, and reminders.
*   **Cybersecurity Quiz:** Test your knowledge with a multiple-choice quiz on fundamental cybersecurity concepts.
*   **Activity Log:** View a real-time log of interactions and actions within the application.

## Technologies Used

*   **C#:** The primary programming language for the application logic.
*   **WPF (Windows Presentation Foundation):** Used for building the graphical user interface.
*   **.NET Framework 4.8:** The target framework for the project.

## Getting Started

### Prerequisites

*   Visual Studio 2019 or later (with .NET desktop development workload installed)
*   .NET Framework 4.8 Developer Pack

### Installation and Running


1.  **Open in Visual Studio:**
    Open the `ST10453605_PROG6221_POE.sln` file in Visual Studio.

2.  **Build the Solution:**
    Go to `Build` > `Build Solution` (or press `Ctrl+Shift+B`).

3.  **Run the Application:**
    Press `F5` or click the `Start` button in Visual Studio to run the application.

## How to Use

### Chat Tab

*   Type your questions or statements in the `ChatInput` text box at the bottom.
*   Click the `Send` button to send your message.
*   The chatbot will respond in the `ChatHistory` area.
*   **Initial Greeting:** The bot will first ask for your name. Type your name (or any word) and press Send to proceed to normal conversation.
*   Try asking about: "password", "phishing", "scam", "privacy", "safe browsing".
*   You can also express sentiments like "I'm worried", "I'm curious", "I'm frustrated", "I'm confused".
*   Ask for "more info" or "tell me more" after a topic to get another response on the same topic.
*   Ask for a "tip" if you've previously expressed an interest (e.g., "I'm interested in data protection").
*   You can also ask about "task", "quiz", or "log" to get information about other tabs.

### Tasks Tab

*   Enter a `Task Title`, `Task Description`, and `Reminder` in the respective text boxes.
*   Click `Add Task` to add the task to the list.
*   Tasks will be displayed in the `ListView` below.

### Quiz Tab

*   Read the `QuizQuestionText`.
*   Select an answer from the `ListBox` of options.
*   The `QuizFeedback` will tell you if your answer was correct and provide an explanation.
*   Click `Next Question` to proceed to the next quiz question.

### Activity Log Tab

*   This tab displays a real-time log of significant actions and interactions within the application, such as tasks being added or quizzes being completed.

## Project Structure

*   `MainWindow.xaml`: Defines the main window's user interface.
*   `MainWindow.xaml.cs`: Contains the C# code-behind for the main window, including chatbot logic, task management, and quiz functionality.
*   `App.xaml`, `App.xaml.cs`: Standard WPF application entry point.
*   `Models/CyberTask.cs`: Defines the `CyberTask` data structure.
*   `Models/QuizQuestion.cs`: Defines the `QuizQuestion` data structure.
*   `Properties/`: Contains assembly information and resources.


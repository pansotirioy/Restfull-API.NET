using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using System.ClientModel;

namespace Assignment.Service
{
    public class AiRoutingService : IAiRoutingService
    {
        private readonly ChatClient _chatClient;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly string _model;

        public AiRoutingService(IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            // Pull API key from config
            var apiKey = config["OpenAI:ApiKey"]
                ?? throw new ArgumentNullException("OpenAI:ApiKey is missing");

            _model = config["OpenAI:ModelName"] ?? "gpt-5";

            // Initialize ChatClient (OpenAI .NET SDK)
            _chatClient = new ChatClient(_model, apiKey);
        }

        public async Task<string> DetectDepartment(string message,int Conversation = 0)
        {
            ClientResult<ChatCompletion> completion;
            List<ChatMessage>? messages;
            if (Conversation == 0)
            {
                // System instructions
                // Fetch departments from the database
                await using var context = _contextFactory.CreateDbContext();

                var departmentsList = await context.Departments.ToListAsync();

                // Convert to string (e.g., list each department with name and contact info)
                var departmentsText = string.Join("\n", departmentsList.Select(d => $"{d.Name}: {d.Phone}"));

                // Build the system prompt
                var systemPrompt = $"""
                You are an AI assistant that helps classify intent and provide customers with contact information to the appropriate department in your Certificate business. 

                Only choose between the departments listed below. 

                You are friendly and professional.

                When a user provides a message, analyze the content to determine which department is most appropriate to handle their request.

                If unsure use "Customer Service Department" as the default department with the appropriate phone.

                After determining the correct department, provide the contact phone number for that department.

                Below is the list of departments and their contact information:
                """ + departmentsText;

                // Send to OpenAI and get chat completion
                messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(message)
                };

                completion = await _chatClient.CompleteChatAsync(messages);
            }
            else
            {
                messages = new List<ChatMessage>
                {
                    new UserChatMessage(message)
                };
                completion = await _chatClient.CompleteChatAsync(messages);
            }

            // Return the text of the AI response
            return completion.Value.Content[0].Text;
        }
    }
}

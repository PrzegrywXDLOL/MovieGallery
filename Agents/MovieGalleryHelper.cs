using Microsoft.Extensions.AI;
using MovieGallery.Models;

namespace MovieGallery.Agents
{
    public class MovieGalleryHelper
    {
        private readonly IChatClient _chatClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieGalleryHelper(IChatClient chatClient, IHttpClientFactory httpClientFactory)
        {
            _chatClient = chatClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AskAsync(string query, IEnumerable<ChatTurn>? history)
        {
            var moviesData = await GetMoviesDataFromApiAsync();

            var messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, $"""
                You are a helpful assistant in a Movie Gallery that provides information about movies.
                Be friendly, informative, and concise in your responses. Your response should be short and to the point, 
                providing only the necessary information. Don't use "**" or any other wierd formatting in your responses. When providing titles of movies use "", 
                use the exact title as it appears in the movie data. If a user asks for information about a movie that is not in the movie data, 
                politely inform them that you don't have information about that movie and suggest they ask about another movie. When asked about reviews, 
                provide a brief summary of the review and the average rating if available but never give informations about reviewers. You can also quote from the reviews if available.
                Try to make a feeling of excitement and enthusiasm in your responses and make the user feel like they are talking to a movie expert not a robot.
                Also, if the user asks for help in any other topic, politely inform them that you can only provide information about movies and 
                suggest they ask a different question related to movies. Don't provide any information about other topics or solve problems unrelated to movies.
                Use the following movie data to answer questions (JSON): {moviesData}
                """)
            };

            foreach (var turn in (history?.TakeLast(10) ?? Enumerable.Empty<ChatTurn>()))
            {
                if (string.IsNullOrWhiteSpace(turn.Content)) continue;

                var content = turn.Content.Length > 2000
                    ? turn.Content[..2000]
                    : turn.Content;

                if (turn.Role == "user")
                    messages.Add(new ChatMessage(ChatRole.User, content));
                else if (turn.Role == "assistant")
                    messages.Add(new ChatMessage(ChatRole.Assistant, content));
            }

            messages.Add(new ChatMessage(ChatRole.User, query));

            try
            {
                var response = await _chatClient.GetResponseAsync(messages,
                    new ChatOptions
                    {
                        MaxOutputTokens = 1500,
                    });
                return response.Text;
            
            }catch (Exception){
                return "Sorry, I couldn't process your request at the moment. Please try again later.";
            }

        }

        private async Task<string> GetMoviesDataFromApiAsync()
        {
            var client = _httpClientFactory.CreateClient("MoviesApi");
            var response = await client.GetAsync("/api/Movies");
            return await response.Content.ReadAsStringAsync();
        }
    }
}

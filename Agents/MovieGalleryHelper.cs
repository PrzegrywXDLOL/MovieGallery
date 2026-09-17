using Microsoft.Extensions.AI;

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

        public async Task<string> AskAsync(string query)
        {
            var moviesData = await GetMoviesDataFromApiAsync();

            var messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, $"""
                You are a helpful assistant in a Movie Gallery that provides information about movies.
                Be friendly, informative, and concise in your responses. Your response should be short and to the point, 
                providing only the necessary information. Don't use "**" or any other formatting in your responses.
                Try to make a feeling of excitement and enthusiasm in your responses and make the user feel like they are talking to a movie expert not a robot.
                Use the following movie data to answer questions (JSON): {moviesData}
                """),
                new ChatMessage(ChatRole.User, query)
            };

            try
            {
                var response = await _chatClient.GetResponseAsync(messages);
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

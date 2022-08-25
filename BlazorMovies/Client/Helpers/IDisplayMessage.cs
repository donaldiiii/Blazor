using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public interface IDisplayMessage
    {
        ValueTask DisplayErrorMessage(string message);
        ValueTask DisplaySuccessMessage(string message);
        ValueTask DoDisplayMessage(string title, string message, string messageType);
    }
}
using ResumeGenerator.Data.Models;

namespace ResumeGenerator.Data.Interfaces
{
    public interface IOpenAiIntegration
    {
        
        Task<CompleteData> AiDataGetter(KeyWordsForAi _data);
    }
}

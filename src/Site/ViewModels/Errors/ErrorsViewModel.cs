using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.Errors
{
    public class ErrorsViewModel : BaseViewModel
    {
        public int ErrorCode { get; }

        public ErrorsViewModel(BaseViewModelConfig config, int errorCode) : base(config)
        {
            ErrorCode = errorCode;
        }

        public override Task<string> Title()
        {
            return Task.FromResult("Ошибка");
        }
    }
}

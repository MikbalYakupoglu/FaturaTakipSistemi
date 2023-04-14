using AspNetCoreHero.ToastNotification.Abstractions;

namespace FaturaTakip.Business.Aspects
{
    public class ReferenceFactory
    {
        private readonly INotyfService _notyf;

        public ReferenceFactory(INotyfService notyf)
        {
            _notyf = notyf;
        }

        public INotyfService GetNotyfReference()
        {
            return _notyf;
        }
    }
}

using Envivo.Fresnel.UiCore.Commands;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ExplorerController : ApiController
    {
        private GetObjectCommand _GetObjectCommand;
        private GetPropertyCommand _GetPropertyCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private InvokeMethodCommand _InvokeMethodCommand;
        private CollectionAddCommand _CollectionAddCommand;

        public ExplorerController
            (
            GetObjectCommand getObjectCommand,
            GetPropertyCommand getPropertyCommand,
            SetPropertyCommand setPropertyCommand,
            InvokeMethodCommand invokeMethodCommand,
            CollectionAddCommand collectionAddCommand
            )
        {
            _GetObjectCommand = getObjectCommand;
            _GetPropertyCommand = getPropertyCommand;
            _SetPropertyCommand = setPropertyCommand;
            _InvokeMethodCommand = invokeMethodCommand;
            _CollectionAddCommand = collectionAddCommand;
        }

        [HttpPost]
        public GetPropertyResponse GetObject([FromBody]GetObjectRequest id)
        {
            var request = id;
            var response = _GetObjectCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GetPropertyResponse GetObjectProperty([FromBody]GetPropertyRequest id)
        {
            var request = id;
            var response = _GetPropertyCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetProperty([FromBody]SetPropertyRequest id)
        {
            var request = id;
            var response = _SetPropertyCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var response = _InvokeMethodCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse AddItemToCollection([FromBody]CollectionRequest id)
        {
            var request = id;
            var response = _CollectionAddCommand.Invoke(request);
            return response;
        }

    }
}
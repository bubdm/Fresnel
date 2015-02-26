using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ExplorerController : ApiController
    {
        private CreateAndSetPropertyCommand _CreateAndSetPropertyCommand;
        private GetObjectCommand _GetObjectCommand;
        private GetPropertyCommand _GetPropertyCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private InvokeMethodCommand _InvokeMethodCommand;
        private CollectionAddCommand _CollectionAddCommand;
        private CollectionRemoveCommand _CollectionRemoveCommand;
        private SaveChangesCommand _SaveChangesCommand;

        public ExplorerController
            (
            IEnumerable<ICommand> commands
            )
        {
            _CreateAndSetPropertyCommand = commands.OfType<CreateAndSetPropertyCommand>().Single();
            _GetObjectCommand = commands.OfType<GetObjectCommand>().Single();
            _GetPropertyCommand = commands.OfType<GetPropertyCommand>().Single(); ;
            _SetPropertyCommand = commands.OfType<SetPropertyCommand>().Single(); ;
            _InvokeMethodCommand = commands.OfType<InvokeMethodCommand>().Single(); ;
            _CollectionAddCommand = commands.OfType<CollectionAddCommand>().Single(); ;
            _CollectionRemoveCommand = commands.OfType<CollectionRemoveCommand>().Single(); ;
            _SaveChangesCommand = commands.OfType<SaveChangesCommand>().Single();
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
        public BaseCommandResponse CreateAndSetProperty([FromBody]CreateAndSetPropertyRequest id)
        {
            var request = id;
            var response = _CreateAndSetPropertyCommand.Invoke(request);
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
        public GenericResponse AddNewItemToCollection([FromBody]CollectionAddNewRequest id)
        {
            var request = id;
            var response = _CollectionAddCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse AddItemsToCollection([FromBody]CollectionAddRequest id)
        {
            var request = id;
            var response = _CollectionAddCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse RemoveItemFromCollection([FromBody]CollectionRemoveRequest id)
        {
            var request = id;
            var response = _CollectionRemoveCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SaveChanges([FromBody]SaveChangesRequest id)
        {
            var result = _SaveChangesCommand.Invoke(id);
            return result;
        }

    }
}
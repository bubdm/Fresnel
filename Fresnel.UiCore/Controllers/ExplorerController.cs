using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ExplorerController : ApiController
    {
        private Lazy<CreateAndSetPropertyCommand> _CreateAndSetPropertyCommand;
        private Lazy<GetObjectCommand> _GetObjectCommand;
        private Lazy<GetPropertyCommand> _GetPropertyCommand;
        private Lazy<SetPropertyCommand> _SetPropertyCommand;
        private Lazy<SetParameterCommand> _SetParameterCommand;
        private Lazy<InvokeMethodCommand> _InvokeMethodCommand;
        private Lazy<CollectionAddCommand> _CollectionAddCommand;
        private Lazy<CollectionRemoveCommand> _CollectionRemoveCommand;
        private Lazy<SaveChangesCommand> _SaveChangesCommand;
        private Lazy<CancelChangesCommand> _CancelChangesCommand;
        private Lazy<SearchPropertyCommand> _SearchPropertyCommand;
        private Lazy<SearchParameterCommand> _SearchParameterCommand;

        public ExplorerController
            (
            Lazy<CreateAndSetPropertyCommand> createAndSetPropertyCommand,
            Lazy<GetObjectCommand> getObjectCommand,
            Lazy<GetPropertyCommand> getPropertyCommand,
            Lazy<SetPropertyCommand> setPropertyCommand,
            Lazy<SetParameterCommand> setParameterCommand,
            Lazy<InvokeMethodCommand> invokeMethodCommand,
            Lazy<CollectionAddCommand> collectionAddCommand,
            Lazy<CollectionRemoveCommand> collectionRemoveCommand,
            Lazy<SaveChangesCommand> saveChangesCommand,
            Lazy<CancelChangesCommand> cancelChangesCommand,
            Lazy<SearchPropertyCommand> searchPropertyCommand,
            Lazy<SearchParameterCommand> searchParameterCommand
            )
        {
            _CreateAndSetPropertyCommand = createAndSetPropertyCommand;
            _GetObjectCommand = getObjectCommand;
            _GetPropertyCommand = getPropertyCommand;
            _SetPropertyCommand = setPropertyCommand;
            _SetParameterCommand = setParameterCommand;
            _InvokeMethodCommand = invokeMethodCommand;
            _CollectionAddCommand = collectionAddCommand;
            _CollectionRemoveCommand = collectionRemoveCommand;
            _SaveChangesCommand = saveChangesCommand;
            _CancelChangesCommand = cancelChangesCommand;
            _SearchPropertyCommand = searchPropertyCommand;
            _SearchParameterCommand = searchParameterCommand;
        }

        [HttpPost]
        public GetPropertyResponse GetObject([FromBody]GetObjectRequest id)
        {
            var request = id;
            var response = _GetObjectCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GetPropertyResponse GetObjectProperty([FromBody]GetPropertyRequest id)
        {
            var request = id;
            var response = _GetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetProperty([FromBody]SetPropertyRequest id)
        {
            var request = id;
            var response = _SetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public BaseCommandResponse CreateAndSetProperty([FromBody]CreateAndSetPropertyRequest id)
        {
            var request = id;
            var response = _CreateAndSetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetParameter([FromBody]SetParameterRequest id)
        {
            var request = id;
            var response = _SetParameterCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var response = _InvokeMethodCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public CollectionAddResponse AddNewItemToCollection([FromBody]CollectionAddNewRequest id)
        {
            var request = id;
            var response = _CollectionAddCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse AddItemsToCollection([FromBody]CollectionAddRequest id)
        {
            var request = id;
            var response = _CollectionAddCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse RemoveItemFromCollection([FromBody]CollectionRemoveRequest id)
        {
            var request = id;
            var response = _CollectionRemoveCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public SaveChangesResponse SaveChanges([FromBody]SaveChangesRequest id)
        {
            var result = _SaveChangesCommand.Value.Invoke(id);
            return result;
        }

        [HttpPost]
        public CancelChangesResponse CancelChanges([FromBody]CancelChangesRequest id)
        {
            var result = _CancelChangesCommand.Value.Invoke(id);
            return result;
        }

        public SearchResponse SearchPropertyObjects(SearchPropertyRequest id)
        {
            var fullyQualifiedName = id;
            var result = _SearchPropertyCommand.Value.Invoke(id);
            return result;
        }

        public SearchResponse SearchParameterObjects(SearchParameterRequest id)
        {
            var fullyQualifiedName = id;
            var result = _SearchParameterCommand.Value.Invoke(id);
            return result;
        }
    }
}
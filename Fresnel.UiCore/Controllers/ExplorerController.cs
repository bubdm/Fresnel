using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ExplorerController : ApiController
    {
        private CreateAndSetPropertyCommand _CreateAndSetPropertyCommand;
        private GetObjectCommand _GetObjectCommand;
        private GetPropertyCommand _GetPropertyCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private SetParameterCommand _SetParameterCommand;
        private InvokeObjectMethodCommand _InvokeObjectMethodCommand;
        private CollectionAddCommand _CollectionAddCommand;
        private CollectionRemoveCommand _CollectionRemoveCommand;
        private SaveChangesCommand _SaveChangesCommand;
        private CancelChangesCommand _CancelChangesCommand;
        private SearchPropertyCommand _SearchPropertyCommand;
        private SearchParameterCommand _SearchParameterCommand;

        public ExplorerController
            (
            CreateAndSetPropertyCommand createAndSetPropertyCommand,
            GetObjectCommand getObjectCommand,
            GetPropertyCommand getPropertyCommand,
            SetPropertyCommand setPropertyCommand,
            SetParameterCommand setParameterCommand,
            InvokeObjectMethodCommand invokeMethodCommand,
            CollectionAddCommand collectionAddCommand,
            CollectionRemoveCommand collectionRemoveCommand,
            SaveChangesCommand saveChangesCommand,
            CancelChangesCommand cancelChangesCommand,
            SearchPropertyCommand searchPropertyCommand,
            SearchParameterCommand searchParameterCommand
            )
        {
            _CreateAndSetPropertyCommand = createAndSetPropertyCommand;
            _GetObjectCommand = getObjectCommand;
            _GetPropertyCommand = getPropertyCommand;
            _SetPropertyCommand = setPropertyCommand;
            _SetParameterCommand = setParameterCommand;
            _InvokeObjectMethodCommand = invokeMethodCommand;
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
        public GenericResponse SetParameter([FromBody]SetParameterRequest id)
        {
            var request = id;
            var response = _SetParameterCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var response = _InvokeObjectMethodCommand.Invoke(request);
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
        public SaveChangesResponse SaveChanges([FromBody]SaveChangesRequest id)
        {
            var result = _SaveChangesCommand.Invoke(id);
            return result;
        }

        [HttpPost]
        public CancelChangesResponse CancelChanges([FromBody]CancelChangesRequest id)
        {
            var result = _CancelChangesCommand.Invoke(id);
            return result;
        }

        public SearchResponse SearchPropertyObjects(SearchPropertyRequest id)
        {
            var fullyQualifiedName = id;
            var result = _SearchPropertyCommand.Invoke(id);
            return result;
        }

        public SearchResponse SearchParameterObjects(SearchParameterRequest id)
        {
            var fullyQualifiedName = id;
            var result = _SearchParameterCommand.Invoke(id);
            return result;
        }
    }
}
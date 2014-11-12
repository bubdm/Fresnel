using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.BasicTypes
{
    /// <summary>
    /// A set of string Path properties
    /// </summary>
    public class FilePathValues
    {

        private string _PathValue = string.Empty;

        public Guid ID { get; internal set; }

        /// <summary>
        /// This is a path to an existing file.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.None)]
        public string NormalFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to an existing file.
        /// Clicking the hover button will open an "Open File" dialog at the correct folder.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.OpenFile)]
        public string OpenFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to a new (non-existent) file.
        /// Clicking the hover button will open an "Save File" dialog the correct folder.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.SaveFile)]
        public string SaveFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to a folder.
        /// Clicking the hover button will open an "Open Folder" dialog at the correct folder.
        /// You can drag a Windows folder onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.FolderBrowser)]
        public string FolderPath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to an image, and will show the image if possible.
        /// The image tool tip will show the file location.
        /// Clicking the hover button will open an "Open Folder" dialog at the correct folder.
        /// You can drag a Windows image file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.OpenFile, IsImage = true)]
        public string ImageFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to a file.
        /// Clicking the hover button will open an "Open Folder" dialog at the correct folder.
        /// The file dialog will show a 'DOC' filter and 'All files' filter
        /// You can drag a Windows image file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.OpenFile, Filter = "DOC files|*.doc|All files (*.*)|*.*")]
        public string FilePathWithFilter
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

    }
}

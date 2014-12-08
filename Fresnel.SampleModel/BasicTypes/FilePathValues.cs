using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of string Path properties
    /// </summary>
    public class FilePathValues
    {

        private string _PathValue = string.Empty;

        public virtual  Guid ID { get; set; }

        /// <summary>
        /// This is a path to an existing file.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [FilePath(DialogType = FileDialogType.None)]
        public virtual string NormalFilePath
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
        public virtual string OpenFilePath
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
        public virtual string SaveFilePath
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
        public virtual string FolderPath
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
        public virtual string ImageFilePath
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
        public virtual string FilePathWithFilter
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

    }
}

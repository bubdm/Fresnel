using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of string Path properties
    /// </summary>
    public class FilePathValues
    {
        private string _PathValue = string.Empty;

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// This is a path to an existing file.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [DataType(DataType.Url)]
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
        [DataType(DataType.Url)]
        [Dialog(DialogType.OpenFile)]
        public string OpenFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to a new (non-existent) file.
        /// Clicking the hover button will open an "Save File" dialog at the correct folder.
        /// You can drag a Windows file onto this location.
        /// </summary>
        [DataType(DataType.Url)]
        [Dialog(DialogType.SaveFile)]
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
        [DataType(DataType.Url)]
        [Dialog(DialogType.FolderBrowser)]
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
        [DataType(DataType.ImageUrl)]
        [Dialog(DialogType.FolderBrowser)]
        public string ImageFilePath
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }

        /// <summary>
        /// This is a path to a file.
        /// Clicking the hover button will open an "Open Folder" dialog at the correct folder.
        /// The file dialog will show a 'DOC' filter and 'TXT' filter
        /// You can drag a Windows image file onto this location.
        /// </summary>
        [DataType(DataType.Url)]
        [Dialog(DialogType.OpenFile, Filter = "*.doc|*.txt")]
        public string FilePathWithFilter
        {
            get { return _PathValue; }
            set { _PathValue = value; }
        }
    }
}
using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a string Property that represents a file (or folder) Path
    /// </summary>
    public class FilePathConfiguration : StringConfiguration
    {
        private bool _IsImage;

        public FilePathConfiguration()
            : base()
        {
            base.MaxLength = 260;
            this.IsImage = false;
            //this.DialogType = FileDialogType.None;
        }

        /// <summary>
        /// Determines if the file is an image. The image format may be one of the following: BMP, JPG, GIF, PNG, TIF, WMF, and EMF.
        /// </summary>
        /// <value></value>
        public bool IsImage
        {
            get { return _IsImage; }
            set
            {
                _IsImage = value;
                if (_IsImage)
                {
                    this.Filter = "Image Files|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;*.WMF;*.EMF|All files|*.*";
                    //this.DialogType = FileDialogType.OpenFile;
                }
            }
        }

        /// <summary>
        /// The default extension to be used in the file dialog
        /// </summary>
        /// <value></value>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// The filter string to be used in the file dialog
        /// </summary>
        /// <value></value>
        public string Filter { get; set; }
    }
}
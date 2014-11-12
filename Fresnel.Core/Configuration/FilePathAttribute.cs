using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a string Property that represents a file (or folder) Path
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class FilePathAttribute : StringAttribute
    {

        private bool _IsImage;

        public FilePathAttribute()
            : base()
        {
            base.MaxLength = 260;
            this.IsImage = false;
            this.DialogType = FileDialogType.None;
        }

        /// <summary>
        /// Determines what type of Dialog box should be used for this path
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FileDialogType DialogType { get; set; }

        /// <summary>
        /// Determines if the file is an image. The image format may be one of the following: BMP, JPG, GIF, PNG, TIF, WMF, and EMF.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsImage
        {
            get { return _IsImage; }
            set
            {
                _IsImage = value;
                if (_IsImage)
                {
                    this.Filter = "Image Files|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;*.WMF;*.EMF|All files|*.*";
                    this.DialogType = FileDialogType.OpenFile;
                }
            }
        }

        /// <summary>
        /// The default extension to be used in the file dialog
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// The filter string to be used in the file dialog
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Filter { get; set; }

    }

}

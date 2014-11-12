using System;


namespace Envivo.Fresnel.Core.Configuration
{
    [Serializable()]
    public enum FileDialogType
    {
        /// <summary>
        /// A dialog is not shown
        /// </summary>
        None,

        /// <summary>
        /// An OpenFileDialog is shown
        /// </summary>
        OpenFile,

        /// <summary>
        /// A SaveFileDialog is shown
        /// </summary>
        SaveFile,

        /// <summary>
        /// A FolderBrowserDialog is shown
        /// </summary>
        FolderBrowser
    }

}

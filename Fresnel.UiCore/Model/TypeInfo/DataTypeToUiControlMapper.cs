using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    public class DataTypeToUiControlMapper
    {

        public UiControlType Convert(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.CreditCard:
                    return UiControlType.Text;

                case DataType.Currency:
                    return UiControlType.Currency;

                case DataType.Custom:
                    return UiControlType.None;

                case DataType.Date:
                    return UiControlType.Date;

                case DataType.DateTime:
                    return UiControlType.DateTimeLocal;

                case DataType.Duration:
                    return UiControlType.TimeDuration;

                case DataType.EmailAddress:
                    return UiControlType.Email;

                case DataType.Html:
                    return UiControlType.Html;

                case DataType.ImageUrl:
                    return UiControlType.Image;

                case DataType.MultilineText:
                    return UiControlType.TextArea;

                case DataType.Password:
                    return UiControlType.Password;

                case DataType.PhoneNumber:
                    return UiControlType.Telephone;

                case DataType.PostalCode:
                    return UiControlType.PostalCode;

                case DataType.Text:
                    return UiControlType.Text;

                case DataType.Time:
                    return UiControlType.Time;

                case DataType.Upload:
                    return UiControlType.Upload;

                case DataType.Url:
                    return UiControlType.Url;

            }

            return UiControlType.None;
        }

    }
}
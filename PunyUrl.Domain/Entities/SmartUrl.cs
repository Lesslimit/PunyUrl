using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using PunyUrl.Domain.Converters;

#pragma warning disable 618

namespace PunyUrl.Domain.Entities
{
    [TypeConverter(typeof(SmartUrlConverter))]
    public class SmartUrl : Uri
    {
        public static SmartUrl Empty => new SmartUrl();

        public bool IsEmpty { get; private set; }

        #region Constructors

        private SmartUrl() : base(":empty")
        {
            IsEmpty = true;
        }

        public SmartUrl(string uriString) : base(uriString)
        {
        }

        public SmartUrl(string uriString, bool dontEscape) : base(uriString, dontEscape)
        {
        }

        public SmartUrl(Uri baseUri, string relativeUri, bool dontEscape) : base(baseUri, relativeUri, dontEscape)
        {
        }

        public SmartUrl(string uriString, UriKind uriKind) : base(uriString, uriKind)
        {
        }

        public SmartUrl(Uri baseUri, string relativeUri) : base(baseUri, relativeUri)
        {
        }

        public SmartUrl(Uri baseUri, Uri relativeUri) : base(baseUri, relativeUri)
        {
        }

        public SmartUrl(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        #endregion

        public static SmartUrl Parse(string value)
        {
            if (value == null)
            {
                return Empty;
            }

            Uri uri;
            return TryCreate(value, UriKind.Absolute, out uri) ? new SmartUrl(uri.AbsoluteUri) : Empty;
        }

        public static implicit operator string(SmartUrl smartUrl)
        {
            return smartUrl.OriginalString;
        }
    }
}
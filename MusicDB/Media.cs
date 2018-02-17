using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MusicDB.Entities.Metadata;
using MusicDB.ExternalData.Helpers;

namespace MusicDB.Entities
{
    public partial class Media : BaseEntity
    {
        // FileName
        string _fileName = null;

        public string FileName { get => _fileName;

            set
            {
                string fileName = null;
                string extension = null;

                fileName = Path.GetFileName(value);
                extension = Path.GetExtension(value);

                if (_fileName == null && _extension == null)
                {
                    _fileName = fileName;
                    _extension = extension;
                }
                else if(_extension != null)
                {
                    _fileName = fileName.Replace(extension, _extension);
                }
            }
        }

        // MimeType
        string _mimeType = "image/png";

        public string MimeType { get => _mimeType; set => _mimeType = value; }

        string _extension = null;

        // binary content of media
        byte[] _mediaBinary = null;

        public byte[] MediaBinary {

            get
            {
                if (Image != null)
                {
                    return Helpers.ImageHelper.ImageToByteArray(Image);
                }

                return null;
            }

            set => _mediaBinary = value;
        }

        string _mediaURL = null;

        // image 
        Bitmap _image = null;

        public Bitmap Image {
            get
            {
                if(_mediaURL != null && _image == null)
                {
                    MusicDB.ExternalData.Connectors.Helpers.XmlHttpRequest xmlHttp = new ExternalData.Connectors.Helpers.XmlHttpRequest();

                    try
                    {

                        _image = xmlHttp.GetImage(_mediaURL);

                    }
                    catch (Exception ex)
                    {
                        //Something is wrong with Format -- Maybe required Format is not 
                        // applicable here
                    }
                }

                return _image;
            }

            set => _image = value;
        }

        // MediaType
        Metadata.MediaType _mediaType = null;

        public MediaType MediaType { get => _mediaType; set => _mediaType = value; }

        // MediaType
        Metadata.MediaFileType _mediaFileType = new MediaFileType(MediaFileType.MediaFileEnum.Image);

        public MediaFileType FileType { get => _mediaFileType; set => _mediaFileType = value; }

        BaseEntity _baseEntity = null;

        public BaseEntity BaseEntity { get => _baseEntity;  }


        // Contructor by base entity 
        Media(BaseEntity baseEntity, MediaType mediaType) : base(0)
        {
            _baseEntity = baseEntity;
            _mediaType = mediaType;
        }

        // constructor by media URL
        public Media(BaseEntity baseEntity, MediaType mediaType, string strMediaURL) : this(baseEntity, mediaType)
        {
            _mediaURL = strMediaURL;

            FileName = BasicHelper.GetURLFilename(strMediaURL);
            _extension = BasicHelper.GetURLFilenameExtension(strMediaURL);

        }

        /// <summary>
        /// Saves Image to File
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            string extension = ExternalData.Helpers.BasicHelper.GetURLFilenameExtension(fileName);
            Bitmap bitmap = Image;
            fileName = fileName.Replace(extension, _extension);

            if (bitmap != null)
            {
                if (bitmap != null)
                {
                    bitmap.Save(BasicHelper.StripNonAlphaNumeric(fileName), ImageFormat.Png);
                }
            }
        }



    }
}

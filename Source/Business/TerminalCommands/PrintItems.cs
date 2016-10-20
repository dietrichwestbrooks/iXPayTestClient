using System;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands
{
    public class PrintItems
    {
        #region Private Fields

        private List<PrintCommandSetTypeFaceTypeFace> _typeFaces = new List<PrintCommandSetTypeFaceTypeFace>();
        private List<string> _textLines = new List<string>();
        private HorizontalJustificationsEnum _barcodeJustification;
        private PrintCommandPrintBarCodeTextPosition _barcodeTextPosition;
        private bool _barcodeTextPositionSpecified;
        private PrintCommandPrintBarCodeEncoding _barcodeEncoding;
        private bool _barcodeEncodingSpecified;
        private int _barcodeWidth;
        private bool _barcodeWidthSpecified;
        private int _barcodeHeight;
        private bool _barcodeHeightSpecified;
        private bool _fillCut;
        private bool _fullCutSpecified;
        private bool _eject;
        private bool _ejectSpecified;
        private bool _barcodeJustificationSpecified;
        private HorizontalJustificationsEnum _textJustification;
        private HorizontalJustificationsEnum _imageJustification;
        private HorizontalJustificationsEnum _downloadedImageJustification;
        private BarCodeSymbologiesEnum _barcodeSymbology;
        private bool _resetTypeFacesToDefaults;
        private bool _textLineFeed;
        private string _imageFileName;
        private string _barcodeCharacters;
        private int _downloadedImageId;
        private string _fontName; 

        #endregion

        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        public void AddTypeFace(string id, bool value)
        {
            TypeFaceEnum typeFace;

            if(!Enum.TryParse(id, true, out typeFace))
                throw new ArgumentException($"Invalid type face: {id}", nameof(id));

            _typeFaces.Add(new PrintCommandSetTypeFaceTypeFace { Id = typeFace, Value = value });
        }

        public PrintCommandSetTypeFaceTypeFace[] TypeFaces => _typeFaces.ToArray();

        public bool ResetTypeFacesToDefaults
        {
            get { return _resetTypeFacesToDefaults; }
            set { _resetTypeFacesToDefaults = value; }
        }

        public void AddTextLine(string line)
        {
            _textLines.Add(line);
        }

        public string[] TextLines => _textLines.ToArray();

        public string TextJustification
        {
            get { return _textJustification.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _textJustification))
                    throw new ArgumentException($"Invalid justification: {value}");
            }
        }

        public bool TextLineFeed
        {
            get { return _textLineFeed; }
            set { _textLineFeed = value; }
        }

        public string ImageFileName
        {
            get { return _imageFileName; }
            set { _imageFileName = value; }
        }

        public string ImageJustification
        {
            get { return _imageJustification.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _imageJustification))
                    throw new ArgumentException($"Invalid justification: {value}");
            }
        }

        public string BarcodeJustification
        {
            get { return _barcodeJustification.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _barcodeJustification))
                    throw new ArgumentException($"Invalid justification: {value}");
                _barcodeJustificationSpecified = true;
            }
        }

        public string BarcodeCharacters
        {
            get { return _barcodeCharacters; }
            set { _barcodeCharacters = value; }
        }

        public string BarcodeSymbology
        {
            get { return _barcodeSymbology.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _barcodeSymbology))
                    throw new ArgumentException($"Invalid symbologies: {value}");
            }
        }

        public string BarcodeTextPosition
        {
            get { return _barcodeTextPosition.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _barcodeTextPosition))
                    throw new ArgumentException($"Invalid position: {value}");
                _barcodeTextPositionSpecified = true;
            }
        }

        public string BarcodeEncoding
        {
            get { return _barcodeEncoding.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _barcodeEncoding))
                    throw new ArgumentException($"Invalid position: {value}");
                _barcodeEncodingSpecified = true;
            }
        }

        public int BarcodeWidth
        {
            get { return _barcodeWidth; }
            set
            {
                _barcodeWidth = value;
                _barcodeWidthSpecified = true;
            }
        }

        public int BarcodeHeight
        {
            get { return _barcodeHeight; }
            set
            {
                _barcodeHeight = value;
                _barcodeHeightSpecified = true;
            }
        }

        public int DownloadedImageId
        {
            get { return _downloadedImageId; }
            set { _downloadedImageId = value; }
        }

        public string DownloadedImageJustification
        {
            get { return _downloadedImageJustification.ToString("F"); }
            set
            {
                if (!Enum.TryParse(value, true, out _downloadedImageJustification))
                    throw new ArgumentException($"Invalid justification: {value}");
            }
        }

        public bool FillCut
        {
            get { return _fillCut; }
            set
            {
                _fillCut = value;
                _fullCutSpecified = true;
            }
        }

        public bool Eject
        {
            get { return _eject; }
            set
            {
                _eject = value;
                _ejectSpecified = true;
            }
        }

        public object[] ToArray()
        {
            List<object> items = new List<object>();

            if (_ejectSpecified || _fullCutSpecified)
            {
                var printCutPaper = new PrintCommandCutPaper();

                if (_ejectSpecified)
                {
                    printCutPaper.Eject = _eject;
                    printCutPaper.EjectSpecified = true;
                }

                if (_fullCutSpecified)
                {
                    printCutPaper.FullCut = _fillCut;
                    printCutPaper.FullCutSpecified = true;
                }

                items.Add(printCutPaper);
            }

            if (!string.IsNullOrWhiteSpace(BarcodeCharacters))
            {
                var printBarCode = new PrintCommandPrintBarCode
                    {
                        Characters = _barcodeCharacters,
                        Symbology = _barcodeSymbology
                    };

                if (_barcodeJustificationSpecified)
                {
                    printBarCode.Justification = _barcodeJustification;
                    printBarCode.JustificationSpecified = true;
                }

                if (_barcodeTextPositionSpecified)
                {
                    printBarCode.TextPosition = _barcodeTextPosition;
                    printBarCode.TextPositionSpecified = true;
                }

                if (_barcodeEncodingSpecified)
                {
                    printBarCode.Encoding = _barcodeEncoding;
                    printBarCode.EncodingSpecified = true;
                }

                if (_barcodeWidthSpecified)
                {
                    printBarCode.Width = _barcodeWidth;
                    printBarCode.WidthSpecified = true;
                }

                if (_barcodeHeightSpecified)
                {
                    printBarCode.Height = _barcodeHeight;
                    printBarCode.HeightSpecified = true;
                }

                items.Add(printBarCode);
            }

            if (DownloadedImageId > 0)
            {
                var printDownloadedImage = new PrintCommandPrintDownloadedImage
                    {
                        Id = _downloadedImageId,
                        Justification = _downloadedImageJustification
                    };

                items.Add(printDownloadedImage);
            }

            if (!string.IsNullOrWhiteSpace(ImageFileName))
            {
                var printImage = new PrintCommandPrintImage
                    {
                        FileName = _imageFileName,
                        Justification = _imageJustification
                    };

                items.Add(printImage);
            }

            if (_textLines.Any())
            {
                var printText = new PrintCommandPrintText
                    {
                        Justification = _textJustification,
                        LineFeed = _textLineFeed,
                        Text = new PrintCommandPrintTextText {Text = _textLines.ToArray()}
                    };

                items.Add(printText);
            }

            if (!string.IsNullOrWhiteSpace(FontName))
            {
                var printSetFont = new PrintCommandSetFont
                    {
                        Font = new PrintCommandSetFontFont {FontName = _fontName}
                    };

                items.Add(printSetFont);
            }

            if (_typeFaces.Any())
            {
                var printSetTypeFace = new PrintCommandSetTypeFace
                    {
                        ResetToDefaults = _resetTypeFacesToDefaults,
                        TypeFace = _typeFaces.ToArray()
                    };

                items.Add(printSetTypeFace);
            }

            return items.ToArray();
        }
    }
}

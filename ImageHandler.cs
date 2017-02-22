using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    public class ImageHandler
    {
        private string _bitmapPath;
        private Bitmap _currentBitmap;
        private Bitmap _bitmapbeforeProcessing;
        private Bitmap _bitmapPrevCropArea;

        public ImageHandler()
        {

        }

        public Bitmap CurrentBitmap
        {
            get 
            {
                if (_currentBitmap == null)
                    _currentBitmap = new Bitmap(1, 1);
                return _currentBitmap; 
            }
            set { _currentBitmap = value; }
        }

        public Bitmap BitmapBeforeProcessing
        {
            get { return _bitmapbeforeProcessing; }
            set { _bitmapbeforeProcessing = value; }
        }

        public string BitmapPath
        {
            get { return _bitmapPath; }
            set { _bitmapPath = value; }
        }

        public enum ColorFilterTypes
        {
            Red,
            Green,
            Blue
        };

        public void ResetBitmap()
        {
            if (_currentBitmap != null && _bitmapbeforeProcessing != null)
            {
                Bitmap temp = (Bitmap)_currentBitmap.Clone();
                _currentBitmap = (Bitmap)_bitmapbeforeProcessing.Clone();
                _bitmapbeforeProcessing = (Bitmap)temp.Clone();
            }
        }

        public void SaveBitmap(string saveFilePath)
        {
            _bitmapPath = saveFilePath;
            if (System.IO.File.Exists(saveFilePath))
                System.IO.File.Delete(saveFilePath);
            _currentBitmap.Save(saveFilePath);
        }

        public void ClearImage()
        {
            _currentBitmap = new Bitmap(1, 1);
        }

        public void RestorePrevious()
        {
            _bitmapbeforeProcessing = _currentBitmap;
        }

        public void SetColorFilter(ColorFilterTypes colorFilterType)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int nPixelR = 0;
                    int nPixelG = 0;
                    int nPixelB = 0;
                    if (colorFilterType == ColorFilterTypes.Red)
                    {
                        nPixelR = c.R;
                        nPixelG = c.G - 255;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == ColorFilterTypes.Green)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G;
                        nPixelB = c.B - 255;
                    }
                    else if (colorFilterType == ColorFilterTypes.Blue)
                    {
                        nPixelR = c.R - 255;
                        nPixelG = c.G - 255;
                        nPixelB = c.B;
                    }

                    nPixelR = Math.Max(nPixelR, 0);
                    nPixelR = Math.Min(255, nPixelR);

                    nPixelG = Math.Max(nPixelG, 0);
                    nPixelG = Math.Min(255, nPixelG);

                    nPixelB = Math.Max(nPixelB, 0);
                    nPixelB = Math.Min(255, nPixelB);

                    bmap.SetPixel(i, j, Color.FromArgb((byte)nPixelR, (byte)nPixelG, (byte)nPixelB));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void SetGamma(double red, double green, double blue)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            byte[] redGamma = CreateGammaArray(red);
            byte[] greenGamma = CreateGammaArray(green);
            byte[] blueGamma = CreateGammaArray(blue);
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j, Color.FromArgb(redGamma[c.R], greenGamma[c.G], blueGamma[c.B]));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        public void SetBrightness(int brightness)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    int cR = c.R + brightness;
                    int cG = c.G + brightness;
                    int cB = c.B + brightness;

                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;

                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;

                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void SetContrast(double contrast)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void SetGrayscale()
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void SetInvert()
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth != 0 && newHeight != 0)
            {
                Bitmap temp = (Bitmap)_currentBitmap;
                Bitmap bmap = new Bitmap(newWidth, newHeight, temp.PixelFormat);

                double nWidthFactor = (double)temp.Width / (double)newWidth;
                double nHeightFactor = (double)temp.Height / (double)newHeight;

                double fx, fy, nx, ny;
                int cx, cy, fr_x, fr_y;
                Color color1 = new Color();
                Color color2 = new Color();
                Color color3 = new Color();
                Color color4 = new Color();
                byte nRed, nGreen, nBlue;

                byte bp1, bp2;

                for (int x = 0; x < bmap.Width; ++x)
                {
                    for (int y = 0; y < bmap.Height; ++y)
                    {

                        fr_x = (int)Math.Floor(x * nWidthFactor);
                        fr_y = (int)Math.Floor(y * nHeightFactor);
                        cx = fr_x + 1;
                        if (cx >= temp.Width) cx = fr_x;
                        cy = fr_y + 1;
                        if (cy >= temp.Height) cy = fr_y;
                        fx = x * nWidthFactor - fr_x;
                        fy = y * nHeightFactor - fr_y;
                        nx = 1.0 - fx;
                        ny = 1.0 - fy;

                        color1 = temp.GetPixel(fr_x, fr_y);
                        color2 = temp.GetPixel(cx, fr_y);
                        color3 = temp.GetPixel(fr_x, cy);
                        color4 = temp.GetPixel(cx, cy);

                        // Blue
                        bp1 = (byte)(nx * color1.B + fx * color2.B);

                        bp2 = (byte)(nx * color3.B + fx * color4.B);

                        nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Green
                        bp1 = (byte)(nx * color1.G + fx * color2.G);

                        bp2 = (byte)(nx * color3.G + fx * color4.G);

                        nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Red
                        bp1 = (byte)(nx * color1.R + fx * color2.R);

                        bp2 = (byte)(nx * color3.R + fx * color4.R);

                        nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        bmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, nRed, nGreen, nBlue));
                    }
                }
                _currentBitmap = (Bitmap)bmap.Clone();
            }
        }

        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            bmap.RotateFlip(rotateFlipType);
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void Crop(int xPosition, int yPosition, int width, int height)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (xPosition + width > _currentBitmap.Width)
                width = _currentBitmap.Width - xPosition;
            if (yPosition + height > _currentBitmap.Height)
                height = _currentBitmap.Height - yPosition;
            Rectangle rect = new Rectangle(xPosition, yPosition, width, height);
            _currentBitmap = (Bitmap)bmap.Clone(rect, bmap.PixelFormat);
        }

        public void DrawOutCropArea(int xPosition, int yPosition, int width, int height)
        {
            _bitmapPrevCropArea = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)_bitmapPrevCropArea.Clone();
            Graphics gr = Graphics.FromImage(bmap);
            Brush cBrush = new Pen(Color.FromArgb(150, Color.White)).Brush;
            Rectangle rect1 = new Rectangle(0, 0, _currentBitmap.Width, yPosition);
            Rectangle rect2 = new Rectangle(0, yPosition, xPosition, height);
            Rectangle rect3 = new Rectangle(0, (yPosition + height), _currentBitmap.Width, _currentBitmap.Height);
            Rectangle rect4 = new Rectangle((xPosition + width), yPosition, (_currentBitmap.Width - xPosition - width), height);
            gr.FillRectangle(cBrush, rect1);
            gr.FillRectangle(cBrush, rect2);
            gr.FillRectangle(cBrush, rect3);
            gr.FillRectangle(cBrush, rect4);
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void RemoveCropAreaDraw()
        {
            _currentBitmap = (Bitmap)_bitmapPrevCropArea.Clone();
        }

        public void InsertText(string text, int xPosition, int yPosition, string fontName, float fontSize, string fontStyle, string colorName1, string colorName2)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Graphics gr = Graphics.FromImage(bmap);
            if (string.IsNullOrEmpty(fontName))
                fontName = "Times New Roman";
            if (fontSize.Equals(null))
                fontSize = 10.0F;
            Font font = new Font(fontName, fontSize);
            if (!string.IsNullOrEmpty(fontStyle))
            {
                FontStyle fStyle = FontStyle.Regular;
                switch (fontStyle.ToLower())
                {
                    case "bold":
                        fStyle = FontStyle.Bold;
                        break;
                    case "italic":
                        fStyle = FontStyle.Italic;
                        break;
                    case "underline":
                        fStyle = FontStyle.Underline;
                        break;
                    case "strikeout":
                        fStyle = FontStyle.Strikeout;
                        break;

                }
                font = new Font(fontName, fontSize, fStyle);
            }
            if (string.IsNullOrEmpty(colorName1))
                colorName1 = "Black";
            if (string.IsNullOrEmpty(colorName2))
                colorName2 = colorName1;
            Color color1 = Color.FromName(colorName1);
            Color color2 = Color.FromName(colorName2);
            int gW = (int)(text.Length * fontSize);
            gW = gW == 0 ? 10 : gW;
            LinearGradientBrush LGBrush = new LinearGradientBrush(new Rectangle(0, 0, gW, (int)fontSize), color1, color2, LinearGradientMode.Vertical);
            gr.DrawString(text, font, LGBrush, xPosition, yPosition);
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void InsertImage(string imagePath, int xPosition, int yPosition)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Graphics gr = Graphics.FromImage(bmap);
            if (!string.IsNullOrEmpty(imagePath))
            {
                Bitmap i_bitmap = (Bitmap)Bitmap.FromFile(imagePath);
                Rectangle rect = new Rectangle(xPosition, yPosition, i_bitmap.Width, i_bitmap.Height);
                gr.DrawImage(Bitmap.FromFile(imagePath), rect);
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        public void InsertShape(string shapeType, int xPosition, int yPosition, int width, int height, string colorName)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Graphics gr = Graphics.FromImage(bmap);
            if (string.IsNullOrEmpty(colorName))
                colorName = "Black";
            Pen pen = new Pen(Color.FromName(colorName));
            switch (shapeType.ToLower())
            {
                case "filledellipse":
                    gr.FillEllipse(pen.Brush, xPosition, yPosition, width, height);
                    break;
                case "filledrectangle":
                    gr.FillRectangle(pen.Brush, xPosition, yPosition, width, height);
                    break;
                case "ellipse":
                    gr.DrawEllipse(pen, xPosition, yPosition, width, height);
                    break;
                case "rectangle":
                default:
                    gr.DrawRectangle(pen, xPosition, yPosition, width, height);
                    break;
               
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }
        public void LinifyByPriority(int horizontalNodesCount, int verticalNodesCount, int lineThickness, int stepCount, ImageProcessing process)
        {
            Random rand = new Random();
            int loop = stepCount;
            Bitmap temp = (Bitmap)_currentBitmap;
            //calculate border nodes
            int hInterval = temp.Width / (horizontalNodesCount - 1);
            int vInterval = temp.Height / (verticalNodesCount - 1);
            int xMin = 0;
            int yMin = 0;
            int xMax = hInterval * (horizontalNodesCount - 1);
            int yMax = vInterval * (verticalNodesCount - 1);
            List<XYCoordinates> borderNodes = new List<XYCoordinates>();

            for (int y = 0; y < temp.Height; y += vInterval)
            {
                borderNodes.Add(new XYCoordinates(xMin, y));
                borderNodes.Add(new XYCoordinates(xMax, y));
            }

            for (int x = 0; x < temp.Width; x += hInterval)
            {
                borderNodes.Add(new XYCoordinates(x, yMin));
                borderNodes.Add(new XYCoordinates(x, yMax));
            }

            foreach (XYCoordinates c in borderNodes)
            {
                Console.Out.WriteLine(c.x + " " + c.y);
            }
            Console.Out.WriteLine(borderNodes.Count);

            //get call n choose 2 pairs
            List<Tuple<XYCoordinates, XYCoordinates>> coordinatePairs = new List<Tuple<XYCoordinates, XYCoordinates>>();
            while (borderNodes.Count > 0)
            {
                XYCoordinates coordinate1 = borderNodes[0];
                borderNodes.RemoveAt(0);
                foreach (XYCoordinates c in borderNodes)
                {
                    if (coordinate1.x != c.x && coordinate1.y != c.y)
                    {
                        coordinatePairs.Add(Tuple.Create(coordinate1, c));
                    }
                }
            }
            StreamWriter file =
            new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\pairs.txt", false);
            foreach (Tuple<XYCoordinates, XYCoordinates> pair in coordinatePairs)
            {
                file.WriteLine(pair.Item1.x + "," + pair.Item1.y + " : " + pair.Item2.x + "," + pair.Item2.y);
            }
            file.WriteLine(coordinatePairs.Count);
            file.Flush();
            file.Close();
            //   Bitmap bmpLinified = new Bitmap(temp.Width, temp.Height);
            _currentBitmap = new Bitmap(temp.Width, temp.Height);

            StreamWriter steps_file =
           new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + @"\steps.txt", false);
            // traverse though each coodidate pair, get sum
            int tempPairScore;
            byte[] rgbValues;
            byte[] rgbValues2;
            int depth;
            int width;
            int scoreSum;
            int candidateIndex = 0;
            XYCoordinates darkestXY = new XYCoordinates(0,0);
            int reject = 0;
            for (int l = 0; l < loop; l++)
            {
                Color beforedd = temp.GetPixel(darkestXY.x, darkestXY.y);
                candidateIndex = 0;
                //  rgbValues = RgbValues(temp);
                rgbValues = RgbValues2((Bitmap)temp.Clone());
                depth = System.Drawing.Bitmap.GetPixelFormatSize(temp.PixelFormat);
                width = _currentBitmap.Width;

                //find darkest point
                darkestXY = DarkestPoint(rgbValues, _currentBitmap.Width, _currentBitmap.Height, depth);
                scoreSum = PairScore(rgbValues, depth, width, coordinatePairs[0].Item1, coordinatePairs[0].Item2, darkestXY); // the lower the higher likely candidate (darker)
                for (int i = 0; i < coordinatePairs.Count; i++)
                {
                    tempPairScore = PairScore(rgbValues, depth, width, coordinatePairs[i].Item1, coordinatePairs[i].Item2, darkestXY);

                    if (tempPairScore < scoreSum)
                    {
                        candidateIndex = i;
                        scoreSum = tempPairScore;
                    }


                }
                if (scoreSum > 255)
                {
                    temp.SetPixel(darkestXY.x, darkestXY.y, Color.White);
                    l--;
                    reject++;
                    Console.WriteLine(darkestXY.x+" , "+darkestXY.y+" Total Rejected:"+reject);
                }
                else {
                    steps_file.WriteLine(coordinatePairs[candidateIndex].Item1.x + "," + coordinatePairs[candidateIndex].Item1.y + " : " + coordinatePairs[candidateIndex].Item2.x + "," + coordinatePairs[candidateIndex].Item2.y);
                    Console.Out.WriteLine(coordinatePairs[candidateIndex].Item1.x + "," + coordinatePairs[candidateIndex].Item1.y + " : " + coordinatePairs[candidateIndex].Item2.x + "," + coordinatePairs[candidateIndex].Item2.y + " "+l+"/" + loop);
                    // draw candidate, 
                    Color black = new Color();
                    black = Color.FromArgb(0, 0, 0);
                    Color white = new Color();
                    white = Color.FromArgb(255, 255, 255);
                    line(_currentBitmap, coordinatePairs[candidateIndex].Item1, coordinatePairs[candidateIndex].Item2, black);
                    // subtract from original
                //    Color before = temp.GetPixel(darkestXY.x, darkestXY.y);
                    //           Color before2 = GetPixel(rgbValues, depth, width, darkestXY.x, darkestXY.y);
                    line(temp, coordinatePairs[candidateIndex].Item1, coordinatePairs[candidateIndex].Item2, white);
                    //                rgbValues = RgbValues2(temp);
                   // Color after = temp.GetPixel(darkestXY.x, darkestXY.y);
                    //            Color after2 = GetPixel(rgbValues, depth, width, darkestXY.x, darkestXY.y);
                    coordinatePairs.RemoveAt(candidateIndex);
                    //invalidate
                    // process.Invalidate();
                }
            }//end if (int l=.....
            steps_file.Flush();
            steps_file.Close();
            Console.WriteLine("Total Rejected:" + reject);

        }
        public XYCoordinates DarkestPoint(byte[] rgbValues, int width, int height, int depth)
        {
            int low = 255;
            int average;
            int r, g, b;
            int lx = 0;
            int ly=0;
            Color c;
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    c = GetPixel(rgbValues, depth, width, x, y);
                    average = (c.R + c.G + c.B) / 3;
                    if (average < low)
                    {
                        low = average;
                        lx = x;
                        ly = y;
                    }
                }

            }
            return new XYCoordinates(lx, ly);
        }
        public void Linify(int horizontalNodesCount, int verticalNodesCount, int lineThickness, int stepCount, ImageProcessing process)
        {
            Random rand = new Random();
            int loop = stepCount;
            Bitmap temp = (Bitmap)_currentBitmap;
            //calculate border nodes
            int hInterval = temp.Width / (horizontalNodesCount - 1);
            int vInterval = temp.Height / (verticalNodesCount - 1);
            int xMin = 0;
            int yMin = 0;
            int xMax = hInterval* (horizontalNodesCount - 1);
            int yMax = vInterval*(verticalNodesCount-1);
            List<XYCoordinates> borderNodes = new List<XYCoordinates>();

            for (int y = 0; y < temp.Height; y += vInterval)
            {
                borderNodes.Add(new XYCoordinates(xMin, y));
                borderNodes.Add(new XYCoordinates(xMax, y));
            }

            for (int x = 0; x < temp.Width; x += hInterval)
            {
                borderNodes.Add(new XYCoordinates(x, yMin));
                borderNodes.Add(new XYCoordinates(x, yMax));
            }
            /*
            for (int x = 0; x < temp.Width; x += hInterval)
            {
                for (int y = 0; y < temp.Height; y += vInterval)
                {
                    if(x==xMin )
                    borderNodes.Add(new XYCoordinates(x, y));
                }
            }
            */


            foreach (XYCoordinates c in borderNodes)
            {
                Console.Out.WriteLine(c.x + " " + c.y);
            }
            Console.Out.WriteLine(borderNodes.Count);

            //get call n choose 2 pairs
            List<Tuple<XYCoordinates, XYCoordinates>> coordinatePairs = new List<Tuple<XYCoordinates, XYCoordinates>>();
            while (borderNodes.Count > 0)
            {
                XYCoordinates coordinate1 = borderNodes[0];
                borderNodes.RemoveAt(0);
                foreach (XYCoordinates c in borderNodes)
                {
                    if (coordinate1.x != c.x && coordinate1.y != c.y)
                    {
                        coordinatePairs.Add(Tuple.Create(coordinate1, c));
                    }
                }
            }
            StreamWriter file =
            new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory()+@"\pairs.txt", false);
            foreach (Tuple<XYCoordinates, XYCoordinates> pair in coordinatePairs)
            {
                file.WriteLine(pair.Item1.x + "," + pair.Item1.y + " : " + pair.Item2.x + "," + pair.Item2.y);
            }
            file.WriteLine(coordinatePairs.Count);
            file.Flush();
            file.Close();
            //   Bitmap bmpLinified = new Bitmap(temp.Width, temp.Height);
            _currentBitmap = new Bitmap(temp.Width, temp.Height);

            StreamWriter steps_file =
           new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory()+@"\steps.txt", false);
            // traverse though each coodidate pair, get sum
            int tempPairScore;
            byte[] rgbValues;
            int depth;
            int width;
            int scoreSum;
            Task<int>[] taskArray;
            for (int l = 0; l < loop; l++)
            {
                taskArray = new Task<int>[coordinatePairs.Count];
                int candidateIndex = 0;
                rgbValues = RgbValues(temp);
                depth = System.Drawing.Bitmap.GetPixelFormatSize(temp.PixelFormat);
                width = _currentBitmap.Width;
                //        List<Tuple<int,int>> scoreList = new List<Tuple<int,int>>();
                scoreSum = PairScore(rgbValues, depth, width, coordinatePairs[0].Item1, coordinatePairs[0].Item2); // the lower the higher likely candidate (darker)
                for (int i = 0; i < coordinatePairs.Count; i++)
                {
                    tempPairScore= PairScore(rgbValues, depth, width, coordinatePairs[i].Item1, coordinatePairs[i].Item2);
            //     taskArray[i] = Task<int>.Factory.StartNew(() => PairScore(rgbValues,depth,width, coordinatePairs[i].Item1, coordinatePairs[i].Item2));
            //              tempPairScore = PairScore(temp, coordinatePairs[i].Item1, coordinatePairs[i].Item2);
            //                  scoreList.Add(Tuple.Create(tempPairScore, i));
                               if (tempPairScore < scoreSum)
                               {
                                   candidateIndex = i;
                                   scoreSum = tempPairScore;
                               }
             

        }

                /*scoreSum = taskArray[0].Result;
                for (int i = 1; i < taskArray.Length; i++)
                {
                    try
                    {
                        taskArray[i].Wait();
                        if (taskArray[i].Result < scoreSum)
                        {
                            candidateIndex = i;
                            scoreSum = taskArray[i].Result;
                        }
                    }
                    catch (AggregateException e)
                    {

                    }

                }
                */
                //    scoreList.Sort();
                steps_file.WriteLine(coordinatePairs[candidateIndex].Item1.x + "," + coordinatePairs[candidateIndex].Item1.y + " : " + coordinatePairs[candidateIndex].Item2.x + "," + coordinatePairs[candidateIndex].Item2.y);
                Console.Out.WriteLine(coordinatePairs[candidateIndex].Item1.x + "," + coordinatePairs[candidateIndex].Item1.y + " : " + coordinatePairs[candidateIndex].Item2.x + "," + coordinatePairs[candidateIndex].Item2.y);
                // draw candidate, 
                Color black = new Color();
                black = Color.FromArgb(0, 0, 0);
                Color white = new Color();
                white = Color.FromArgb(255, 255, 255);
                line(_currentBitmap, coordinatePairs[candidateIndex].Item1, coordinatePairs[candidateIndex].Item2,black);
                // subtract from original
                line(temp, coordinatePairs[candidateIndex].Item1, coordinatePairs[candidateIndex].Item2,white);
                coordinatePairs.RemoveAt(candidateIndex);
                //invalidate
               // process.Invalidate();
            }//end if (int l=.....
            steps_file.Flush();
            steps_file.Close();

        }
        public byte[] RgbValues2(Bitmap bmp)
        {
            Bitmap b = (Bitmap)bmp.Clone();

            BitmapData bData = b.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, b.PixelFormat);

            /* GetBitsPerPixel just does a switch on the PixelFormat and returns the number */
            int bitsPerPixel =  Image.GetPixelFormatSize(bData.PixelFormat);

            /*the size of the image in bytes */
            int size = bData.Stride * bData.Height;

            /*Allocate buffer for image*/
            byte[] data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);

            /*
            for (int i = 0; i < size; i += bitsPerPixel / 8)
            {
                double magnitude = 1 / 3d * (data[i] + data[i + 1] + data[i + 2]);

                //data[i] is the first of 3 bytes of color

            }
            */

            /* This override copies the data back into the location specified */
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);

            b.UnlockBits(bData);
            b.Dispose();
            return data;
        }
        byte[] RgbValues(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);
                        // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes  = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            bmp.UnlockBits(bmpData);
            return rgbValues;
        }
        public int PairScore(Bitmap bmp, XYCoordinates c1, XYCoordinates c2)
        {
            return lineSum(bmp, c1.x, c1.y, c2.x, c2.y);
        }
        public int PairScore(byte[] rgbValues,int depth,int width, XYCoordinates c1, XYCoordinates c2,XYCoordinates darkestXY)
        {
            int result = 255;
            try
            {
               result =  lineSum(rgbValues, depth, width, darkestXY, c1.x, c1.y, c2.x, c2.y);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
            return result;
        }
        public int PairScore(byte[] rgbValues, int depth, int width, XYCoordinates c1, XYCoordinates c2)
        {
            int result = 255;
            try
            {
                result = lineSum(rgbValues, depth, width, c1.x, c1.y, c2.x, c2.y);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
            return result;
        }
        public int lineSum(byte[] rgbValues,int depth,int width, XYCoordinates darkestXY, int x, int y, int x2, int y2)
        {
            Boolean ifPassThroughDarkest = false;
            int count = 0;
            int sum = 0;
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                //  putpixel(x, y, color);
                if(x==darkestXY.x && y == darkestXY.y)
                {
                    ifPassThroughDarkest = true;
                }
                Color c = GetPixel(rgbValues, depth,width, x, y);
                sum += (c.R + c.G + c.B) / 3;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else {
                    x += dx2;
                    y += dy2;
                }
                count++;
            }
            if (ifPassThroughDarkest)
            {
                return sum / count;
            }
            else
            {
                return 256;
            }
         //   return sum / count;
        }
        public int lineSum(byte[] rgbValues, int depth, int width, int x, int y, int x2, int y2)
        {
            Boolean ifPassThroughDarkest = false;
            int count = 0;
            int sum = 0;
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                //  putpixel(x, y, color);
                Color c = GetPixel(rgbValues, depth, width, x, y);
                sum += (c.R + c.G + c.B) / 3;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else {
                    x += dx2;
                    y += dy2;
                }
                count++;
            }
               return sum / count;
        }
        public int lineSum(Bitmap bmp, int x, int y, int x2, int y2)
        {
            int count = 0;
            int sum = 0;
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                //  putpixel(x, y, color);
                Color c = bmp.GetPixel(x, y);
                sum += (c.R + c.G + c.B) / 3;
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else {
                    x += dx2;
                    y += dy2;
                }
                count++;
            }
            return sum/count;
        }
        public void line(Bitmap bmp, XYCoordinates c1, XYCoordinates c2, Color c)
        {
            line(bmp, c1.x, c1.y, c2.x, c2.y,c);
        }
        public void line(Bitmap bmp, int x, int y, int x2, int y2, Color color)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                //  putpixel(x, y, color);
           //     Console.Out.WriteLine(x+","+y);
                bmp.SetPixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else {
                    x += dx2;
                    y += dy2;
                }
            }
         //   Console.Out.WriteLine("");
        }
        public Color GetPixel(byte[] rgbValues, int depth,int width, int x, int y)
        {
            Color clr = Color.Empty;

            // Get color components count
            int cCount = depth / 8;

            // Get start index of the specified pixel
            int i = ((y * width) + x) * cCount;

            if (i > rgbValues.Length - cCount)
                throw new IndexOutOfRangeException();

            if (depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = rgbValues[i];
                byte g = rgbValues[i + 1];
                byte r = rgbValues[i + 2];
                byte a = rgbValues[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = rgbValues[i];
                byte g = rgbValues[i + 1];
                byte r = rgbValues[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = rgbValues[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }
    }
}

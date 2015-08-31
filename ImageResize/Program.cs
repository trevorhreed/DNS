using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageResize
{
	class Program
	{
		private const int ORIGINAL_WIDTH = 1280;
		private const int ORIGINAL_HEIGHT = 720;
		private const int THUMBNAIL_WIDTH = 320;
		private const int THUMBNAIL_HEIGHT = 180;

		private const string DIRECTORY = @"C:\projects\vs\dns\ImageResize\images\";
		private static string[] FILES = {
			"starwars1.jpg",
			"starwars2.jpg",
			"steve1.jpg",
			"steve2.jpg"
		};

		static void Main(string[] args)
		{
			foreach (string file in FILES)
			{
				resize(file);
			}

		}

		static void resize(string file)
		{
			Bitmap original = new Bitmap(DIRECTORY + file);
			if (original.Width != ORIGINAL_WIDTH || original.Height != ORIGINAL_HEIGHT) throw new ArgumentException("Image must be 1280x720!");

			ImageAttributes imgAttrs = new ImageAttributes();
			imgAttrs.SetWrapMode(WrapMode.TileFlipXY);
			Rectangle thumbnailRect = new Rectangle(0, 0, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
			Bitmap target = new Bitmap(THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
			Graphics copy = Graphics.FromImage(target);
			copy.InterpolationMode = InterpolationMode.HighQualityBicubic;
			copy.CompositingQuality = CompositingQuality.Default;
			copy.CompositingMode = CompositingMode.SourceCopy;

			copy.DrawImage(original, thumbnailRect, 0, 0, ORIGINAL_WIDTH, ORIGINAL_HEIGHT, GraphicsUnit.Pixel, imgAttrs);

			//copy.DrawImage(original, 0, 0, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);

			int extensionStart = file.LastIndexOf('.');
			target.Save(DIRECTORY + file.Substring(0, extensionStart) + "-thumbnail" + file.Substring(extensionStart));
		}
	}
}

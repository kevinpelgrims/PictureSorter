namespace PictureSorter
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ImageSorter
    {
        private static readonly Regex RegEx = new Regex(":");

        public static IEnumerable<string> GetImagePathsFromFolder(string path, bool recursive)
        {
            var images = new List<string>();
            var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                images.AddRange(Directory.GetFiles(path, String.Format("*.{0}", filter), searchOption));
            }
            return images;
        }

        // Retrieve the datetime without loading the whole image
        // Source: http://stackoverflow.com/questions/180030/how-can-i-find-out-when-a-picture-was-actually-taken-in-c-sharp-running-on-vista
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                System.Drawing.Imaging.PropertyItem propItem = myImage.GetPropertyItem(36867);
                if (myImage.PropertyIdList.Any(x => x == 36867)) 
                {
                    string dateTaken = RegEx.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
                return DateTime.MinValue;
            }
        }

        public static void CreateImageDirectory(string path, int year, int month)
        {
            var fullPath = String.Format(@"{0}\{1}\{2}", path, year, GetFormattedMonth(month));
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public static void MoveImageToDirectory(string path, int year, int month, string imageName)
        {
            var fullPathOld = String.Format(@"{0}\{1}", path, imageName);
            var fullPathNew = String.Format(@"{0}\{1}\{2}\{3}", path, year, GetFormattedMonth(month), imageName);
            if (File.Exists(fullPathOld))
            {
                File.Move(fullPathOld, fullPathNew);
            }
        }

        private static string GetFormattedMonth(int month)
        {
            return month.ToString().PadLeft(2, '0');
        }
    }
}

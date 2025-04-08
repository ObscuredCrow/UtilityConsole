using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityConsole.Ext
{
    public static class RotateImage
    {
        public static string FileType = string.Empty;
        public static string RotateAmount = string.Empty;
        public static string AppendName = string.Empty;

        public static void Process() {
            if (!Core.RunAgain) {
                Core.Path = string.Empty;
                FileType = string.Empty;

                Console.WriteLine("What is the starting directory?");
                Core.Path = Console.ReadLine();

                Console.WriteLine("What type of files are being changed?");
                FileType = "." + Console.ReadLine();

                Console.WriteLine("What is the rotation degrees?");
                RotateAmount = Console.ReadLine();

                if (string.IsNullOrEmpty(RotateAmount)) {
                    Console.WriteLine("A rotation degree must be supplied. Aborting...");
                    return;
                }

                Console.WriteLine("What would you like to append to the file name?");
                AppendName = Console.ReadLine();

                Core.RunAgain = false;
            }

            if (Directory.Exists(Core.Path)) {
                foreach (var file in Directory.GetFiles(Core.Path, "*", SearchOption.AllDirectories)) {
                    FileInfo info = new FileInfo(file);
                    if (!info.Name.Contains(FileType)) {
                        Console.WriteLine($"Error - Ignoring: File Type Doesn't Match: {info.FullName}");
                        continue;
                    }

                    var image = Image.FromFile(info.FullName);
                    var bitmap = new Bitmap(image);
                    image.Dispose();

                    var newBitmap = Rotate(bitmap, float.Parse(RotateAmount));
                    bitmap.Dispose();

                    var newName = $"{info.FullName.Replace($"{info.Name}", $"{info.Name.Replace(".png", "")}{AppendName}{FileType}")}";

                    try {
                        if (File.Exists(newName)) {
                            File.Delete(newName);
                        }

                        newBitmap.Save(newName, ImageFormat.Png);
                        newBitmap.Dispose();

                        Console.WriteLine($"{info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                    catch {
                        Console.WriteLine($"Error - Ignoring: New Path Already Exists: {info.FullName.Replace(Core.Path, "")} => {newName.Replace(Core.Path, "")}");
                    }
                }

                Console.WriteLine($"Rotation Complete!");
            }
            else {
                Console.WriteLine($"Not a valid path: {Core.Path}");
            }
        }

        private static Bitmap Rotate(Bitmap bmp, float angle) {
            float alpha = angle;

            //edit: negative angle +360
            while (alpha < 0)
                alpha += 360;

            float gamma = 90;
            float beta = 180 - angle - gamma;

            float c1 = bmp.Height;
            float a1 = (float)(c1 * Math.Sin(alpha * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));
            float b1 = (float)(c1 * Math.Sin(beta * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));

            float c2 = bmp.Width;
            float a2 = (float)(c2 * Math.Sin(alpha * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));
            float b2 = (float)(c2 * Math.Sin(beta * Math.PI / 180) / Math.Sin(gamma * Math.PI / 180));

            int width = Convert.ToInt32(b2 + a1);
            int height = Convert.ToInt32(b1 + a2);

            Bitmap rotatedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(rotatedImage)) {
                g.TranslateTransform(rotatedImage.Width / 2, rotatedImage.Height / 2); //set the rotation point as the center into the matrix
                g.RotateTransform(angle); //rotate
                g.TranslateTransform(-rotatedImage.Width / 2, -rotatedImage.Height / 2); //restore rotation point into the matrix
                g.DrawImage(bmp, new Point((width - bmp.Width) / 2, (height - bmp.Height) / 2)); //draw the image on the new bitmap
            }
            return rotatedImage;
        }
    }
}

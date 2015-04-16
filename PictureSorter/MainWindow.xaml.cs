using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace PictureSorter
{
    using System.IO;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            var folderResult = folderDialog.ShowDialog();

            if (folderResult == System.Windows.Forms.DialogResult.OK)
            {
                FolderTextBox.Text = folderDialog.SelectedPath;
            }
        }

        private void MoveImagesButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(FolderTextBox.Text))
            {
                MoveImages(FolderTextBox.Text);
            }
            else
            {
                MessageBox.Show("Select a folder or paste a valid path in the textbox");
            }
        }

        private void MoveImages(string path)
        {
            StatusTextBlock.Text = "Sorting images in " + path;
            foreach (var image in ImageSorter.GetImagePathsFromFolder(path, false))
            {
                var dateTime = ImageSorter.GetDateTakenFromImage(image);
                ImageSorter.CreateImageDirectory(path, dateTime.Year, dateTime.Month);
                ImageSorter.MoveImageToDirectory(path, dateTime.Year, dateTime.Month, Path.GetFileName(image));
            }
            StatusTextBlock.Text = StatusTextBlock.Text + "\nDone!";
        }
    }
}

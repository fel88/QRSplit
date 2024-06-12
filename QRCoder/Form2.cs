﻿using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRSplit
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            Text = "File split: " + ofd.FileName;
            var bts = File.ReadAllBytes(ofd.FileName);
            var b64 = Convert.ToBase64String(bts);
            images.Clear();
            for (int i = 0; i < b64.Length; i += blockSize)
            {
                var last = b64.Length - i;
                last = Math.Min(blockSize, last);
                var sub = b64.Substring(i, last);
                var qq = RenderQrCode(sub);
                images.Add(qq);
            }
            if (images.Any())
                pictureBox1.Image = images[0];
        }

        List<Bitmap> images = new List<Bitmap>();


        private Bitmap RenderQrCode(string text, string level = "L")
        {

            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, eccLevel))
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                var img = qrCode.GetGraphic(20, GetPrimaryColor(), GetBackgroundColor(),
                    GetIconBitmap());
                //  pictureBox1.BackgroundImage = img;
                //  this.pictureBox1.Size = new System.Drawing.Size(pictureBox1.Width, pictureBox1.Height);
                //  //Set the SizeMode to center the image.
                //  this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

                //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                return img;
            }
        }
        private Color GetPrimaryColor()
        {
            return Color.Black;
        }

        private Color GetBackgroundColor()
        {
            return Color.White;
        }
        private Bitmap GetIconBitmap()
        {

            return null;

        }

        int blockSize = 2900;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddNumericField("blockSize", "Block size", blockSize, 2900, 128, 0);
            if (!d.ShowDialog())
                return;

            blockSize = d.GetIntegerNumericField("blockSize");

        }

        int imageIdx = 0;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            next();
        }
        public void next()
        {
            if (images.Count == 0)
                return;

            if (imageIdx < images.Count - 1)
                imageIdx++;

            toolStripStatusLabel1.Text = $"{imageIdx + 1} / {images.Count}";
            pictureBox1.Image = images[imageIdx];
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (imageIdx > 0)
                imageIdx--;

            toolStripStatusLabel1.Text = $"{imageIdx + 1} / {images.Count}";
            pictureBox1.Image = images[imageIdx];
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            next();
        }
    }
}
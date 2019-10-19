using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using QRCoder;

namespace Cert2QR_for_OVPN
{
    public partial class frmQRProfilePrinter : Form
    {
        //QrCodeEncodingOptions options;
        //BarcodeWriter writer;
        String nombreUsuario = String.Empty;
       
        StringFormat sf = new StringFormat();
        List<perfilPorBloques> listaPerfiles = new List<perfilPorBloques>();

        public frmQRProfilePrinter()
        {
            InitializeComponent();
        }

        private System.Drawing.Image crearQR(String textoAcodificar, int width, int height)
        {
            options.Width = width;
            options.Height = height;
            writer.Options = options;

            writer.Format = ZXing.BarcodeFormat.QR_CODE;
            return new Bitmap(writer.Write(textoAcodificar));
        }

        private void frmQRProfilePrinter_Load(object sender, EventArgs e)
        {
            options = new QrCodeEncodingOptions
            {
                //DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 354,
                Height = 354,
                Margin = 0
                
                
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;

            openFileDialog1.Filter = "Ficheros OpenVPN (*.ovpn)|*.ovpn";
            openFileDialog1.FileName = "";

         

            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            folderBrowserDialog1.ShowNewFolderButton = true;



        }



        //private void btnPrintCert_Click(object sender, EventArgs e)
        //{
        //    PrintDocument pd = new PrintDocument();
        //    pd.PrintPage += (s, args) =>
        //    {
        //        System.Drawing.Image i = crearQR(lecturaCERT, args.MarginBounds.Width, args.MarginBounds.Height);

        //        args.Graphics.DrawImage(i, (args.PageBounds.Width - i.Width) / 2, (args.PageBounds.Height - i.Height) / 2, i.Width, i.Height);

        //        args.Graphics.DrawString("CERTIFICADO DEL USUARIO " + nombreUsuario.ToUpper() + " PARA RED SEGURA MOE.\nImpreso en " + DateTime.Now.ToShortDateString() + Environment.NewLine +  "Este QR es exclusivo para el usuario "+nombreUsuario,
        //             new System.Drawing.Font(FontFamily.GenericMonospace, 12.0F, FontStyle.Bold),
        //            Brushes.Black,
        //            new RectangleF(0, 100, args.PageBounds.Width, 100),
        //            sf);
        //        args.Graphics.DrawString("DIFUSIÓN LIMITADA",
        //           new System.Drawing.Font(FontFamily.GenericMonospace, 12.0F, FontStyle.Bold),
        //          Brushes.Black,
        //          new RectangleF(0, 50, args.PageBounds.Width, 50),
        //          sf);
        //    };
        //    PrintDialog pdi = new PrintDialog();
        //    pdi.Document = pd;
        //    if (pdi.ShowDialog() == DialogResult.OK)
        //    {
        //        pd.Print();
        //    }
        //}


        private void btnSelectOVPNfile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                
                tbSelectedOVPNfile.Text = openFileDialog1.FileName;
                infoLecturaCA.Text = "Comprobada(s) CA - OK";
                infoLecturaCert.Text = "Comprobado(s) CERT - OK";
                infoLecturaKey.Text = "Comprobado(s) KEY - OK";
                infoLecturaConfig.Text = "Comprobado(s) Configuración - OK";
                bool flagConfigOK = true;
                bool flagCAOK = true;
                bool flagCertOK = true;
                bool flagKeyOK = true;
                try
                {
                    foreach (string filename in openFileDialog1.FileNames)
                    {
                        String lecturaCONFIG = String.Empty;
                        String lecturaCA = String.Empty;
                        String lecturaCERT = String.Empty;
                        String lecturaKEY = String.Empty;
                        String lecturaCOMPLETA = System.IO.File.ReadAllText(filename);
                        //CA
                        infoLecturaCA.Visible = true;
                        int positionIniCA = lecturaCOMPLETA.IndexOf("<ca>");
                        int numCharsCA = lecturaCOMPLETA.IndexOf("</ca>") + 5 - positionIniCA;
                        lecturaCA = lecturaCOMPLETA.Substring(positionIniCA, numCharsCA);

                        //CERT
                        infoLecturaCert.Visible = true;
                        int positionIniCert = lecturaCOMPLETA.IndexOf("<cert>");
                        int numCharsCERT = lecturaCOMPLETA.IndexOf("</cert>") + 7 - positionIniCert;
                        lecturaCERT = lecturaCOMPLETA.Substring(positionIniCert, numCharsCERT);

                        //KEY
                        infoLecturaKey.Visible = true;
                        int positionIniKEY = lecturaCOMPLETA.IndexOf("<key>");
                        int numCharsKEY = lecturaCOMPLETA.IndexOf("</key>") + 6 - positionIniKEY;
                        lecturaKEY = lecturaCOMPLETA.Substring(positionIniKEY, numCharsKEY);

                        //CONFIG
                        infoLecturaConfig.Visible = true;
                        lecturaCONFIG = lecturaCOMPLETA.Remove(positionIniCA, numCharsCA);
                        positionIniCert = lecturaCONFIG.IndexOf("<cert>");
                        lecturaCONFIG = lecturaCONFIG.Remove(positionIniCert, numCharsCERT);
                        positionIniKEY = lecturaCONFIG.IndexOf("<key>");
                        lecturaCONFIG = lecturaCONFIG.Remove(positionIniKEY, numCharsKEY);
                        perfilPorBloques perfil = new perfilPorBloques(Path.GetFileNameWithoutExtension(filename), lecturaCONFIG, lecturaCA, lecturaCERT, lecturaKEY);
                        listaPerfiles.Add(perfil);
                        
                    }
                    if (flagCAOK) { }
                    if (flagCertOK) { }
                    if (flagConfigOK)
                    {

                    }
                    if (flagKeyOK)
                    {

                    }
                    if (flagKeyOK && flagConfigOK && flagCertOK && flagCAOK)
                    {
                       
                        btnCertKeyUser.Enabled = true;
                        //btnSaveServerConfigCert.Enabled = true;
                        
                    }
                }
                catch (Exception ex)
                {
                   
                    btnCertKeyUser.Enabled = false;
                    btnSaveServerConfigCert.Enabled = false;
                    MessageBox.Show("Error leyendo configuración VPN del fichero. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(openFileDialog1.FileName);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btnSaveServerConfigCert_Click(object sender, EventArgs e)
        //{
        //    folderBrowserDialog1.Description = "Ruta para guardar configuración y CA del servidor";
        //    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        string pathFichero = Path.Combine(folderBrowserDialog1.SelectedPath, "1.Configuración de servidor" + ".pdf");

        //        SaveAsPDF(pathFichero, "CONFIGURACIÓN de Servidor VPN - ATREVO v1", "Impreso el " + DateTime.Now.ToShortDateString(), listaPerfiles[0].Config,null); //Acceso para Terminales REmotos por Vpn para Oes


        //        pathFichero = Path.Combine(folderBrowserDialog1.SelectedPath, "2.Certificado del servidor" + ".pdf");


        //        SaveAsPDF(pathFichero, "CERTIFICADO del Servidor VPN - ATREVO v1", "Impreso el " + DateTime.Now.ToShortDateString(), listaPerfiles[0].CA,null); //Acceso para Terminales REmotos por Vpn para Oes

        //    }
        //}

        private void SaveAsPDF(string path, string title, string subtitle,string config, string ca, string cert, string key, int zoom = 50)
        {
            System.IO.FileStream fs = new FileStream(path, FileMode.Create);
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.

            // Open the document to enable you to write to the document
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            document.Open();
            document.SetMargins(10, 10, 10, 10);
            // Add a simple and wellknown phrase to the document in a flow layout manner
            PdfContentByte cb = writer.DirectContent; 
            
            cb.BeginText();
            cb.SetFontAndSize(f_cn, 12);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "This text is left aligned", 200, 800, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
            //    ,0,600,0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, , 280, 788, 0);
            Paragraph pClasificacion = new Paragraph("DIFUSIÓN LIMITADA");
            pClasificacion.Alignment = PdfContentByte.ALIGN_CENTER;

            Paragraph pTitulo = new Paragraph(title);            
            iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font();
            fuenteTitulo.SetFamily("Arial Bold");
            fuenteTitulo.SetStyle(1);
            fuenteTitulo.Size = 20;
            pTitulo.Font = fuenteTitulo;
            pTitulo.Alignment = PdfContentByte.ALIGN_CENTER;

            Paragraph pSubTitulo = new Paragraph(subtitle);
            pSubTitulo.Alignment = PdfContentByte.ALIGN_CENTER;
            document.Add(pClasificacion);
            document.Add(pTitulo);
            document.Add(pSubTitulo);

            //AÑADIMOS config
            Bitmap bitmap1 = new Bitmap(crearQR(config, 177, 177));
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap1), System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase("1. Config. Servidor"), 150, 410, 0);

            //img.ScaleAbsolute(260,260);
            img.Alignment = PdfContentByte.ALIGN_CENTER;

            img.SetAbsolutePosition(30, 435);
            cb.AddImage(img);

            //AÑADIMOS el resto
            if (!String.IsNullOrEmpty(ca))
            {
                Bitmap bitmap = new Bitmap(crearQR(ca, 354, 354));
                iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase("2. Cert. Servidor (CA)"), 400, 410, 0);                                                                                                                          //img.ScaleAbsolute(216, 70);
                //img2.ScaleAbsolute(177, 260);
                img2.Alignment = PdfContentByte.ALIGN_CENTER;
                img2.SetAbsolutePosition(275, 435);
                cb.AddImage(img2);
            }

            if (!String.IsNullOrEmpty(cert))
            {
                Bitmap bitmap = new Bitmap(crearQR(cert, 354, 354));
                iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase("3. Certificado usuario"), 150, 60, 0);

                //img2.ScaleAbsolute(177, 177);
                img2.Alignment = PdfContentByte.ALIGN_CENTER;
                img2.SetAbsolutePosition(15, 90);
                cb.AddImage(img2);
            }
            if (!String.IsNullOrEmpty(key))
            {
                Bitmap bitmap = new Bitmap(crearQR(key, 354, 354));
                iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase("4. Key usuario"), 400, 60, 0);                                                                                                                  //img.ScaleAbsolute(216, 70);
                //img2.ScaleAbsolute(260, 260);
                img2.Alignment = PdfContentByte.ALIGN_CENTER;
                img2.SetAbsolutePosition(300, 90);
                cb.AddImage(img2);
            }
            cb.EndText();
            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
        }

        private void btnCertKeyUser_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Ruta para guardar 3.Certificado-#USER# y 4.Key-#USER#";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (perfilPorBloques ppb in listaPerfiles)
                {
                    string user = ppb.NombrePerfil;
                    string pathFile = Path.Combine(folderBrowserDialog1.SelectedPath, "3y4.CertificadoKey-" + user + ".pdf");
                    SaveAsPDF(pathFile, "CERTIFICADO del usuario " + user, "Impreso el " + DateTime.Now.ToShortDateString(),ppb.Config,ppb.CA, ppb.CertUser, ppb.KeyUser, 45); //Acceso para Terminales REmotos por Vpn para Oes
                    //pathFile = Path.Combine(folderBrowserDialog1.SelectedPath,"4.Key-" + user + ".pdf");
                    //SaveAsPDF(pathFile, "KEY del usuario " + user, "Impreso el " + DateTime.Now.ToShortDateString(), ppb.KeyUser, 40); //Acceso para Terminales REmotos por Vpn para Oes
                }
            }
        }

        private static Bitmap ImageTrim(Bitmap img)
        {
            //get image data
            BitmapData bd = img.LockBits(new System.Drawing.Rectangle(Point.Empty, img.Size),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int[] rgbValues = new int[img.Height * img.Width];
            Marshal.Copy(bd.Scan0, rgbValues, 0, rgbValues.Length);
            img.UnlockBits(bd);


            #region determine bounds
            int left = bd.Width;
            int top = bd.Height;
            int right = 0;
            int bottom = 0;

            //determine top
            for (int i = 0; i < rgbValues.Length; i++)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    top = r;
                    break;
                }
            }

            //determine bottom
            for (int i = rgbValues.Length - 1; i >= 0; i--)
            {
                int color = rgbValues[i] & 0xffffff;
                if (color != 0xffffff)
                {
                    int r = i / bd.Width;
                    int c = i % bd.Width;

                    if (left > c)
                    {
                        left = c;
                    }
                    if (right < c)
                    {
                        right = c;
                    }
                    bottom = r;
                    break;
                }
            }

            if (bottom > top)
            {
                for (int r = top + 1; r < bottom; r++)
                {
                    //determine left
                    for (int c = 0; c < left; c++)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (left > c)
                            {
                                left = c;
                                break;
                            }
                        }
                    }

                    //determine right
                    for (int c = bd.Width - 1; c > right; c--)
                    {
                        int color = rgbValues[r * bd.Width + c] & 0xffffff;
                        if (color != 0xffffff)
                        {
                            if (right < c)
                            {
                                right = c;
                                break;
                            }
                        }
                    }
                }
            }

            int width = right - left + 1;
            int height = bottom - top + 1;
            #endregion

            //copy image data
            int[] imgData = new int[width * height];
            for (int r = top; r <= bottom; r++)
            {
                Array.Copy(rgbValues, r * bd.Width + left, imgData, (r - top) * width, width);
            }

            //create new image
            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData nbd
                = newImage.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(imgData, 0, nbd.Scan0, imgData.Length);
            newImage.UnlockBits(nbd);

            return newImage;
        }
    }
}

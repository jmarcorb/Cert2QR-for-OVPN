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
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        String nombreUsuario = String.Empty;
       
        StringFormat sf = new StringFormat();
        List<perfilPorBloques> listaPerfiles = new List<perfilPorBloques>();

        const int leftBoundsCol1 = 150;
        const int leftBoundsCol2 = 150;
        const int bottomBoundsRow2 = 40;//+30 para la img del qr
        const int bottomBoundsRow1 = 400;//+30 para la img del qr

        public frmQRProfilePrinter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Genera imagen QR en base al texto introducido. Los límites de texto los impone la versión de QR. y el grado de protección frente a errores. El número de filas y columnas varía en función de la versión y de la corrección de errores (https://www.qrcode.com/en/about/version.html). 39 --> 173x173
        /// </summary>
        /// <param name="textoAcodificar">Texto que se codificará.</param>
        /// /// <param name="pixelPorColumna">Número de píxeles que tendrá cada fila/columna. 1 o 2 debería bastar.</param>
        /// <param name="qrVersion">Versión que define capacidad  en texto y número de columnas. 39--> 173x173 --> 1774char (correción errores alta).</param>
        /// <returns></returns>
        private System.Drawing.Image crearQR(String textoAcodificar, int pixelPorColumna, int qrVersion)
        {
            QRCodeData qrData = qrGenerator.CreateQrCode(textoAcodificar, QRCodeGenerator.ECCLevel.M,false,false,QRCodeGenerator.EciMode.Default,qrVersion);
            QRCode qrCode = new QRCode(qrData);
            
            Bitmap qrImage = qrCode.GetGraphic(pixelPorColumna, Color.Black, Color.White, false);

            return qrImage;
        }

        private void frmQRProfilePrinter_Load(object sender, EventArgs e)
        {
            

            openFileDialog1.Filter = "Ficheros OpenVPN (*.ovpn)|*.ovpn";
            openFileDialog1.FileName = "";

         

            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            folderBrowserDialog1.ShowNewFolderButton = true;



        }



    

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
                        btnSaveServerConfigCert.Enabled = true;
                        
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

        private void SaveAllPDF(string path, string title, string subtitle,string config, string ca, string cert, string key)
        {
            System.IO.FileStream fs = new FileStream(path, FileMode.Create);
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 15, 25);
            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.

            // Open the document to enable you to write to the document
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            document.Open();
            //document.SetMargins(20, 15, 15, 20);
            // Add a simple and wellknown phrase to the document in a flow layout manner
            PdfContentByte cb = writer.DirectContent;

            cb.BeginText();
            //cb.SetFontAndSize(f_cn, 12);

            //Paragraph pClasificacion = new Paragraph("DIFUSIÓN LIMITADA");
            //pClasificacion.Alignment = PdfContentByte.ALIGN_CENTER;

            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(title + " " + subtitle), leftBoundsCol1 - 100, bottomBoundsRow1, 90);

            //Paragraph pTitulo = new Paragraph();
            //iTextSharp.text.Font fuenteTitulo = new iTextSharp.text.Font();
            //fuenteTitulo.SetFamily("Arial Bold");
            //fuenteTitulo.SetStyle(1);
            //fuenteTitulo.Size = 16;
            //pTitulo.Font = fuenteTitulo;
            //pTitulo.Alignment = PdfContentByte.ALIGN_CENTER;

            //Paragraph pSubTitulo = new Paragraph(subtitle);
            //pSubTitulo.Alignment = PdfContentByte.ALIGN_CENTER;
            ////document.Add(pClasificacion);
            //document.Add(pTitulo);
            //document.Add(pSubTitulo);
            Bitmap bitmap = null;
            iTextSharp.text.Image img = null;
            //if (!String.IsNullOrEmpty(config))
            //{

            //    bitmap = new Bitmap(crearQR(config, 2, -1));
            //    img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("1. Configuración de Servidor"), leftBoundsCol1, bottomBoundsRow1, 0);
            //    img.SetAbsolutePosition(leftBoundsCol1, bottomBoundsRow1+30);
            //    cb.AddImage(img);
            //    bitmap.Dispose();
            //    bitmap = null;
            //}
            //if (!String.IsNullOrEmpty(ca))
            //{

            //    bitmap = new Bitmap(crearQR(ca, 1, -1));
            //    img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("2. Certificado CA de Servidor"), leftBoundsCol2, bottomBoundsRow1, 0);                                                                                                                  //img.ScaleAbsolute(216, 70);
            //    img.SetAbsolutePosition(leftBoundsCol2, bottomBoundsRow1+30);
            //    cb.AddImage(img);
            //    bitmap.Dispose();
            //    bitmap = null;
            //}
            if (!String.IsNullOrEmpty(cert))
            {

                bitmap = new Bitmap(crearQR(cert, 20, -1));
                bitmap.Save(@"c:\BORRAME\cert.bmp");
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("3. Certificado del usuario"), leftBoundsCol1-70, bottomBoundsRow1+200, 90);
                //img.CompressionLevel = 0;
                img.ScaleAbsolute(360, 360);
                img.SetAbsolutePosition(leftBoundsCol1-35, bottomBoundsRow1+45);

                cb.AddImage(img);
                
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(key))
            {

                bitmap = new Bitmap(crearQR(key, 20, -1));
                img = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("4. Key del usuario"), leftBoundsCol2-70, bottomBoundsRow2+200, 90);                                                                                                                  //img.ScaleAbsolute(216, 70);
                //img.Alignment = PdfContentByte.ALIGN_LEFT;
                img.ScaleAbsolute(370, 370);
                img.SetAbsolutePosition(leftBoundsCol2-40, bottomBoundsRow2-10);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            cb.EndText();
            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
        }


        private void SaveUserCertKeyPDF(string path, string title, string subtitle, string cert, string key)
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
            Bitmap bitmap = null;
            iTextSharp.text.Image img = null;
            
            if (!String.IsNullOrEmpty(cert))
            {
               
                bitmap = new Bitmap(crearQR(cert, 1, -1));
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("3. Certificado usuario"), 65, 405, 0);
                //img.Alignment = PdfContentByte.ALIGN_CENTER;
                
                img.SetAbsolutePosition(65, 435);
                
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(key))
            {
               
                bitmap = new Bitmap(crearQR(key, 1, -1));
                img = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("4. Key usuario"), 55, 30, 0);                                                                                                                  //img.ScaleAbsolute(216, 70);
                //img.Alignment = PdfContentByte.ALIGN_LEFT;
                img.SetAbsolutePosition(55, 60);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            cb.EndText();
            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
        }

        private void SaveServerConfigCaPDF(string path, string title, string subtitle, string config, string ca)
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
            Bitmap bitmap = null;
            iTextSharp.text.Image img = null;

            if (!String.IsNullOrEmpty(config))
            {

                bitmap = new Bitmap(crearQR(config, 3, -1));
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("1. Configuración de Servidor"), 180, 405, 0);
                //img.Alignment = PdfContentByte.ALIGN_CENTER;

                img.SetAbsolutePosition(180, 435);

                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(ca))
            {

                bitmap = new Bitmap(crearQR(ca, 2, -1));
                img = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("2. Certificado CA de Servidor"), 170, 30, 0);                                                                                                                  //img.ScaleAbsolute(216, 70);
                //img.Alignment = PdfContentByte.ALIGN_LEFT;
                img.SetAbsolutePosition(170, 60);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
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
                    string pathFile = Path.Combine(folderBrowserDialog1.SelectedPath, "Acceso ATREVO-" + user + ".pdf");
                    SaveAllPDF(pathFile, "CERTIFICADO del usuario " + user, "Impreso el " + DateTime.Now.ToShortDateString(),ppb.Config,ppb.CA, ppb.CertUser, ppb.KeyUser); //Acceso para Terminales REmotos por Vpn para Oes
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

        private void btnSaveServerConfigCert_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Ruta para guardar 1.Config Servidor y 2.Certificado CA Servidor";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFile = Path.Combine(folderBrowserDialog1.SelectedPath, "1y2.ConfigCA.pdf");
                SaveServerConfigCaPDF(pathFile, "Datos de Acceso a red ATREVO (Acceso de Terminales REmotos por VPN para OE)\nDatos comunes a todos los usuarios", "Impreso el " + DateTime.Now.ToShortDateString(), listaPerfiles[0].Config, listaPerfiles[0].CA); //Acceso para Terminales REmotos por Vpn para Oes
            }
        }
          
        
    }
}

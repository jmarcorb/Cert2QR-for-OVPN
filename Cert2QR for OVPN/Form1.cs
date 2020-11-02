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

        const int leftBoundsCol1 = 20;
        const int leftBoundsCol2 = 310;
        const int bottomBoundsRow2 = 35;//+30 para la img del qr
        const int bottomBoundsRow1 = 420;//+30 para la img del qr
        const int bottomBoundsRowTitle = 800;
        const int kPixelPerColInQR = 10;
        const int kLadoFinalQR = 250;
        const int kSeparacionTextoQR = 35;
        const int kRotacionTexto = 0;

        public frmQRProfilePrinter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Genera imagen QR en base al texto introducido. Los límites de texto los impone la versión de QR y el grado de protección frente a errores. El número de filas y columnas varía en función de la versión y de la corrección de errores (https://www.qrcode.com/en/about/version.html). 39 --> 173x173
        /// </summary>
        /// <param name="textoAcodificar">Texto que se codificará.</param>
        /// /// <param name="pixelPorColumna">Número de píxeles que tendrá cada fila/columna. 1 o 2 debería bastar.</param>
        /// <param name="qrVersion">Versión que define capacidad  en texto y número de columnas. 39--> 173x173 --> 1774char (correción errores alta).</param>
        /// <returns></returns>
        private System.Drawing.Image crearQR(String textoAcodificar, int pixelPorColumna, int qrVersion = -1)
        {
            QRCodeData qrData = qrGenerator.CreateQrCode(textoAcodificar, QRCodeGenerator.ECCLevel.L,false,false,QRCodeGenerator.EciMode.Default,qrVersion);
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
        
                bool flagConfigOK = true;
                bool flagCAOK = true;
                bool flagCertOK = true;
                bool flagKeyOK = true;
                listaPerfiles.Clear();
                textBox1.Clear();
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
                        int positionIniCA = lecturaCOMPLETA.IndexOf("<ca>");
                        int numCharsCA = lecturaCOMPLETA.IndexOf("</ca>") + 5 - positionIniCA;
                        lecturaCA = lecturaCOMPLETA.Substring(positionIniCA, numCharsCA);

                        //CERT
                        int positionIniCert = lecturaCOMPLETA.IndexOf("<cert>");
                        int numCharsCERT = lecturaCOMPLETA.IndexOf("</cert>") + 7 - positionIniCert;
                        lecturaCERT = lecturaCOMPLETA.Substring(positionIniCert, numCharsCERT);

                        //KEY
                        int positionIniKEY = lecturaCOMPLETA.IndexOf("<key>");
                        int numCharsKEY = lecturaCOMPLETA.IndexOf("</key>") + 6 - positionIniKEY;
                        lecturaKEY = lecturaCOMPLETA.Substring(positionIniKEY, numCharsKEY);

                        //CONFIG
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
                        textBox1.AppendText("Ficheros comprobados. Tienen la estructura requerida. Puede proceder a la generación de PDF."+Environment.NewLine);
                        btnCertKeyUser.Enabled = true;
                        
                    }
                }
                catch (Exception ex)
                {
                   
                    btnCertKeyUser.Enabled = false;
                    MessageBox.Show("Error leyendo configuración VPN del fichero. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.AppendText("Error en la estructura de algún fichero seleccionado. Compruebe que se trata de ficheros .ovpn con configuración, ca, cert y key." + Environment.NewLine);

                }
                folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(openFileDialog1.FileName);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string SaveAllPDF(string path, string user, string config, string ca, string cert, string key)
        {
            string title = "CERTIFICADO del usuario " + user;
            string subtitle = "Impreso el " + DateTime.Now.ToShortDateString();
            System.IO.FileStream fs = new FileStream(path, FileMode.Create);
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 15, 25);
            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.

            // Open the document to enable you to write to the document
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.Open();
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            document.Open();
            //document.SetMargins(20, 15, 15, 20);
            // Add a simple and wellknown phrase to the document in a flow layout manner
            PdfContentByte cb = writer.DirectContent;

            
            cb.SetFontAndSize(f_cn, 12);

            //Paragraph pClasificacion = new Paragraph("DIFUSIÓN LIMITADA");
            //pClasificacion.Alignment = PdfContentByte.ALIGN_CENTER;
            cb.BeginText();
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(title + " " + subtitle), leftBoundsCol1, bottomBoundsRowTitle, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("1. Configuración de Servidor"), leftBoundsCol1, bottomBoundsRow1 + kLadoFinalQR + kSeparacionTextoQR, kRotacionTexto);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("2. Certificado CA de Servidor"), leftBoundsCol2, bottomBoundsRow1 + kLadoFinalQR + kSeparacionTextoQR, kRotacionTexto);                                                                                                                  //img.ScaleAbsolute(216, 70);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("3. Certificado del usuario"), leftBoundsCol1, bottomBoundsRow2 + kLadoFinalQR + kSeparacionTextoQR, kRotacionTexto);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("4. Key del usuario"), leftBoundsCol2, bottomBoundsRow2 + kLadoFinalQR +kSeparacionTextoQR, kRotacionTexto);                                                                                                                  //img.ScaleAbsolute(216, 70);

            cb.EndText();

            Bitmap bitmap = null;
            iTextSharp.text.Image img = null;
            if (!String.IsNullOrEmpty(config))
            {

                bitmap = new Bitmap(crearQR(config, kPixelPerColInQR, -1));
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                img.ScaleAbsolute(kLadoFinalQR, kLadoFinalQR);
                img.SetAbsolutePosition(leftBoundsCol1, bottomBoundsRow1);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(ca))
            {

                bitmap = new Bitmap(crearQR(ca, kPixelPerColInQR, -1));
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                img.ScaleAbsolute(kLadoFinalQR, kLadoFinalQR);
                img.SetAbsolutePosition(leftBoundsCol2, bottomBoundsRow1);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(cert))
            {

                bitmap = new Bitmap(crearQR(cert, kPixelPerColInQR, -1));
                img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
                img.ScaleAbsolute(kLadoFinalQR, kLadoFinalQR);
                img.SetAbsolutePosition(leftBoundsCol1, bottomBoundsRow2);

                cb.AddImage(img);
                
                bitmap.Dispose();
                bitmap = null;
            }
            if (!String.IsNullOrEmpty(key))
            {

                bitmap = new Bitmap(crearQR(key, kPixelPerColInQR, -1));
                img = iTextSharp.text.Image.GetInstance(ImageTrim(bitmap), ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");

                img.ScaleAbsolute(kLadoFinalQR, kLadoFinalQR);
                img.SetAbsolutePosition(leftBoundsCol2, bottomBoundsRow2);
                cb.AddImage(img);
                bitmap.Dispose();
                bitmap = null;
            }
            
            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
            // Always close open filehandles explicity
            fs.Close();
            return String.Format("PDF de usuario {0} generado OK." + Environment.NewLine, user);
        }


    
   
        private void btnCertKeyUser_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Ruta para guardar 3.Certificado-#USER# y 4.Key-#USER#";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                btnSelectOVPNfile.Enabled = false;
                btnCertKeyUser.Enabled = false;
                tbSelectedOVPNfile.Enabled = false;
                backgroundWorker1.RunWorkerAsync(listaPerfiles);
                
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

   
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           
            List<perfilPorBloques> lppb = (List<perfilPorBloques>)e.Argument;
            for(int i=0;i<lppb.Count;i++)
            {
                
                string user = lppb[i].NombrePerfil;
                string pathFile = Path.Combine(folderBrowserDialog1.SelectedPath, "Acceso ATREVO-" + user + ".pdf");
                string resultado = SaveAllPDF(pathFile, user, lppb[i].Config, lppb[i].CA, lppb[i].CertUser, lppb[i].KeyUser); //Acceso para Terminales REmotos por Vpn para Oes
                backgroundWorker1.ReportProgress((i+1) * 100 / lppb.Count, resultado);
                
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            textBox1.AppendText((string)e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            textBox1.AppendText(Environment.NewLine + "FIN - Generación de PDFs finalizado");
            btnSelectOVPNfile.Enabled = true;
            btnCertKeyUser.Enabled = true;
            tbSelectedOVPNfile.Enabled = true;
        }
    }
}

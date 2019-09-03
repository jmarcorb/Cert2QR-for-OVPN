using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
//using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using System.Drawing.Printing;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;

namespace Cert2QR_for_OVPN
{
    public partial class frmQRProfilePrinter : Form
    {
        QrCodeEncodingOptions options;
        BarcodeWriter writer;
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
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 230,
                Height = 230,
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

        private void btnSaveServerConfigCert_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Ruta para guardar configuración y CA del servidor";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFichero = Path.Combine(folderBrowserDialog1.SelectedPath, "1.Configuración de servidor" + ".pdf");

                SaveAsPDF(pathFichero, "CONFIGURACIÓN de Servidor VPN - ATREVO v1", "Impreso el " + DateTime.Now.ToShortDateString(), listaPerfiles[0].Config); //Acceso para Terminales REmotos por Vpn para Oes


                pathFichero = Path.Combine(folderBrowserDialog1.SelectedPath, "2.Certificado del servidor" + ".pdf");


                SaveAsPDF(pathFichero, "CERTIFICADO del Servidor VPN - ATREVO v1", "Impreso el " + DateTime.Now.ToShortDateString(), listaPerfiles[0].CA, 60); //Acceso para Terminales REmotos por Vpn para Oes

            }
        }

        private void SaveAsPDF(string path, string title, string subtitle, string toQR, int zoom = 50)
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
            fuenteTitulo.Size = 18;
            pTitulo.Font = fuenteTitulo;
            pTitulo.Alignment = PdfContentByte.ALIGN_CENTER;

            Paragraph pSubTitulo = new Paragraph(subtitle);
            pSubTitulo.Alignment = PdfContentByte.ALIGN_CENTER;
            document.Add(pClasificacion);
            document.Add(pTitulo);
            document.Add(pSubTitulo);

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(crearQR(toQR, 600, 600), System.Drawing.Imaging.ImageFormat.Bmp); //.GetInstance("http://www.c-sharpcorner.com/App_Themes/CSharp/Images/CSSiteLogo.gif");
            //img.ScaleAbsolute(216, 70);
            img.ScalePercent(zoom);
            img.Alignment = PdfContentByte.ALIGN_CENTER;

            img.SetAbsolutePosition(150, 250);
            cb.AddImage(img);
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
                    string pathFile = Path.Combine(folderBrowserDialog1.SelectedPath, "3.Certificado-" + user + ".pdf");
                    SaveAsPDF(pathFile, "CERTIFICADO del usuario " + user, "Impreso el " + DateTime.Now.ToShortDateString(), ppb.CertUser, 60); //Acceso para Terminales REmotos por Vpn para Oes
                    pathFile = Path.Combine(folderBrowserDialog1.SelectedPath,"4.Key-" + user + ".pdf");
                    SaveAsPDF(pathFile, "KEY del usuario " + user, "Impreso el " + DateTime.Now.ToShortDateString(), ppb.KeyUser, 60); //Acceso para Terminales REmotos por Vpn para Oes
                }
            }
        }
    }
}

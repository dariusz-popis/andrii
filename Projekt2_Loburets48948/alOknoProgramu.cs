using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt2_Loburets48948
{
    public partial class alFormPaintBrush : Form
    {
        const int almargin = 20; //ustalenie wartości marginesów
        Graphics alRysownica; //wprowadzenie zmiennej animacji
        Point alPunkt; int alPromienPunktu = 5;
        const int alOdstepPomiedzyKontrolkamiDuzy = 5;
        const int alOdstepPomiedzyKontrolkamiMaly = 3;
        Graphics alRysownicaTymczasowa;
        Graphics alWizualizacjaLinii;
        Pen alPioro = new Pen(Color.DeepPink);
        Pen alPioroTymczasowe = new Pen(Color.Gray, 1);
        Brush alWypelnienie = new SolidBrush(Color.Bisque);
        //deklaracja dla ciagłego rozciągania figur geometrycznych
        Bitmap alKopiaBitmapyRysownicy;

        public alFormPaintBrush()
        {
            InitializeComponent();
            alZwymiarowanieFormularza();
            alUstawieniaPoczatkowe();
            alUstawieniaRysownicy();
            alUstawieniaRysownicyTymczasowej();
        }
        public void alZwymiarowanieFormularza()
        {
            this.Location = new Point(20, 20); //inicjowanie nowej instancji klasy Punkt o podanych wspólrzędnych x,y
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.8F);//określenie paramrtru szerokości formy w oparciu o obecną szerokość granic okna
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.7F);//określenie paramrtru wysokości formy w oparciu o obecną szerokość granic okna
            this.StartPosition = FormStartPosition.Manual;//zainicjowanie pozycji startowej formy
        }
        public void alUstawieniaPoczatkowe()
        {
            alTxtGruboscLinii.Text = alPioro.Width.ToString();
            alPanelPodgladuLinii.BackColor = alPanelKolorTlaRysownicy.BackColor = Color.Bisque;
            alPanelKolorPiora.BackColor = alPioro.Color;
            alPanelKolorPedzla.BackColor = Color.DarkCyan;
            alPicRysownica.BackColor = alPanelKolorTlaRysownicy.BackColor;
        }
        public void alUstawieniaRysownicy()
        {
            alPicRysownica.Width = (int)(this.Width * 0.65F);//obliczenie szerokości rysownicy
            alPicRysownica.Height = (int)(this.Height * 0.65F);//obliczenie wysokości rysownicy
            alPicRysownica.BorderStyle = BorderStyle.Fixed3D;//styl obramowania kontrolki pictureBox, czyli Animacji
            alPicRysownica.Location = new Point(this.Left + almargin / 2, this.Top + 2 * almargin);//lokalizacja i zwymiarowanie kontrolki 'alpic
            alPicRysownica.Image = new Bitmap(alPicRysownica.Width, alPicRysownica.Height);//utworzenie bitmapy o rozmiarach kontrolki pictureBox
            alRysownica = Graphics.FromImage(alPicRysownica.Image);// utworzenie egzemplarza powierszchni graficznej na bitmapie
            alRysownica.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //MessageBox.Show(Screen.PrimaryScreen.Bounds.Height.ToString());
        }
        public void alUstawieniaRysownicyTymczasowej()
        {
            alKopiaBitmapyRysownicy = alPicRysownica.Image.Clone() as Bitmap;
            alRysownicaTymczasowa = Graphics.FromImage(alKopiaBitmapyRysownicy);
            alRysownicaTymczasowa.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            alPioroTymczasowe.StartCap = alPioroTymczasowe.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }
        public void alAktualizacjaPaneluPodgladuLinii()
        {
            alPanelPodgladuLinii.BackgroundImage = new Bitmap(alPanelPodgladuLinii.Width, alPanelPodgladuLinii.Height);//utworzenie bitmapy o rozmiarach kontrolki pictureBox
            alWizualizacjaLinii = Graphics.FromImage(alPanelPodgladuLinii.BackgroundImage);// utworzenie egzemplarza powierszchni graficznej na bitmapie
            alWizualizacjaLinii.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            alWizualizacjaLinii.DrawLine(alPioro, 0 + 2 * alOdstepPomiedzyKontrolkamiDuzy, alPanelPodgladuLinii.Height / 2,
               alPanelPodgladuLinii.Width - 2 * alOdstepPomiedzyKontrolkamiDuzy, alPanelPodgladuLinii.Height / 2);
        }

        private void alOknoProgramu_Load(object sender, EventArgs e)
        {
            int alszerokosckontrolki = (this.Width - alPicRysownica.Width - 5 * almargin) / 2;
            //sformatowanie górnych przycisków nad rysownicą
            alLblPozycja.Location = new Point(almargin + Math.Abs(alPicRysownica.Width - alLblPozycja.Width) / 2 - almargin, 5 * almargin / 3);
            //sformatowanie dolnego paska GroupBox
            alGroupBoxLinieKsztalty2D.Width = alPicRysownica.Width;
            alGroupBoxLinieKsztalty2D.Location = new Point(alPicRysownica.Location.X, alPicRysownica.Location.Y + alPicRysownica.Height + alOdstepPomiedzyKontrolkamiDuzy);

            //sformatowanie kontrolek z prawej strony od rysownicy (bliższa kolumna)
            //alWziernikZawartosc.TextAlign = HorizontalAlignment.Center;
            alPanelPodgladuLinii.Width = alszerokosckontrolki;
            alPanelPodgladuLinii.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + almargin, alPicRysownica.Location.Y);
            alLblWziernikLiniiInfo.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + almargin + Math.Abs(alszerokosckontrolki - alLblWziernikLiniiInfo.Width) / 2,
                alPanelPodgladuLinii.Location.Y - alLblWziernikLiniiInfo.Height - alOdstepPomiedzyKontrolkamiMaly);
            alBtnPrzesunFigury.Width = alszerokosckontrolki;
            alBtnPrzesunFigury.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + almargin, alPanelPodgladuLinii.Location.Y + alPanelPodgladuLinii.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alBtnWlaczPokazSlidera.Width = alszerokosckontrolki;
            alBtnWlaczPokazSlidera.Location = new Point(alBtnPrzesunFigury.Location.X, alBtnPrzesunFigury.Location.Y + alBtnPrzesunFigury.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alGroupBoxPokazKsztaltow.Width = alszerokosckontrolki;
            alGroupBoxPokazKsztaltow.Location = new Point(alBtnWlaczPokazSlidera.Location.X, alBtnWlaczPokazSlidera.Location.Y + alBtnWlaczPokazSlidera.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alLblIndeksKształtuInfo.Location = new Point(alGroupBoxPokazKsztaltow.Location.X + Math.Abs(alGroupBoxPokazKsztaltow.Width - alLblIndeksKształtuInfo.Width) / 2,
               alGroupBoxPokazKsztaltow.Location.Y + alGroupBoxPokazKsztaltow.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alTxtIndeksKsztaltu.Width = alszerokosckontrolki;
            alTxtIndeksKsztaltu.Location = new Point(alGroupBoxPokazKsztaltow.Location.X,
               alLblIndeksKształtuInfo.Location.Y + alLblIndeksKształtuInfo.Height + alOdstepPomiedzyKontrolkamiMaly);
            alBtnNastepny.Width = (alszerokosckontrolki - alOdstepPomiedzyKontrolkamiDuzy) / 2;
            alBtnNastepny.Location = new Point(alGroupBoxPokazKsztaltow.Location.X, alTxtIndeksKsztaltu.Location.Y + alTxtIndeksKsztaltu.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alBtnPoprzedni.Width = alBtnNastepny.Width;
            alBtnPoprzedni.Location = new Point(alBtnNastepny.Location.X + alBtnNastepny.Width + alOdstepPomiedzyKontrolkamiDuzy, alBtnNastepny.Location.Y);
            alBtnWylaczPokazKsztaltow.Width = alBtnWlaczPokazSlidera.Width;
            alBtnWylaczPokazKsztaltow.Location = new Point(alBtnWlaczPokazSlidera.Location.X, alBtnPoprzedni.Location.Y + alBtnPoprzedni.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alBtnZapiszBitmape.Width = alBtnWylaczPokazKsztaltow.Width;
            alBtnZapiszBitmape.Location = new Point(alBtnWylaczPokazKsztaltow.Location.X, alBtnWylaczPokazKsztaltow.Location.Y + alBtnWylaczPokazKsztaltow.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alBtmWczytajBitmape.Width = alBtnZapiszBitmape.Width;
            alBtmWczytajBitmape.Location = new Point(alBtnZapiszBitmape.Location.X, alBtnZapiszBitmape.Location.Y + alBtnZapiszBitmape.Height + alOdstepPomiedzyKontrolkamiDuzy);
            alTxtDodajInneFunkcjonalnosci.Width = alBtmWczytajBitmape.Width;
            alTxtDodajInneFunkcjonalnosci.Location = new Point(alBtmWczytajBitmape.Location.X - Math.Abs(alBtnZapiszBitmape.Width - alTxtDodajInneFunkcjonalnosci.Width) / 2,
               alBtmWczytajBitmape.Location.Y + alBtmWczytajBitmape.Height + alOdstepPomiedzyKontrolkamiDuzy);

            //sformatowanie kontrolek z prawej strony od rysownicy (dalsza kolumna)
            alTrbGruboscLinii.Width = alszerokosckontrolki;
            alTrbGruboscLinii.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + alTrbGruboscLinii.Width +
                2 * almargin - Math.Abs(alszerokosckontrolki - alTrbGruboscLinii.Width) / 2, alPicRysownica.Location.Y - 2 * alOdstepPomiedzyKontrolkamiDuzy);
            alLblTrbGruboscLiniiInfo.Location = new Point(alTrbGruboscLinii.Location.X + (alTrbGruboscLinii.Width - alLblTrbGruboscLiniiInfo.Width) / 2,
            alLblWziernikLiniiInfo.Location.Y);
            alLblWpiszGruboscLiniiInfo.Location = new Point(alTrbGruboscLinii.Location.X + (alTrbGruboscLinii.Width - alLblTrbGruboscLiniiInfo.Width) / 2,
                alTrbGruboscLinii.Location.Y + almargin + 2 * alOdstepPomiedzyKontrolkamiDuzy);
            alTxtGruboscLinii.Width = alszerokosckontrolki;
            alTxtGruboscLinii.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + alBtnWlaczPokazSlidera.Width + 2 * almargin,
                alLblWpiszGruboscLiniiInfo.Location.Y + alLblWpiszGruboscLiniiInfo.Height);
            alLblWybierzGruboscLinii.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + alBtnWlaczPokazSlidera.Width + 2 * almargin +
                Math.Abs(alszerokosckontrolki - alLblWybierzGruboscLinii.Width) / 2, alTxtGruboscLinii.Location.Y + alTxtGruboscLinii.Height + alOdstepPomiedzyKontrolkamiMaly);
            alNudUstawGruboscLiniiInfo.Width = alszerokosckontrolki;
            alNudUstawGruboscLiniiInfo.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + alBtnWlaczPokazSlidera.Width + 2 * almargin +
                Math.Abs(alszerokosckontrolki - alNudUstawGruboscLiniiInfo.Width) / 2, alLblWybierzGruboscLinii.Location.Y + alLblWybierzGruboscLinii.Height + alOdstepPomiedzyKontrolkamiMaly);
            alNudUstawGruboscLiniiInfo.TextAlign = HorizontalAlignment.Center;
            alBtnKsztaltyNaTorzeSzeregu.Width = alszerokosckontrolki;
            alBtnKsztaltyNaTorzeSzeregu.Location = new Point(alPicRysownica.Location.X + alPicRysownica.Width + alBtnWlaczPokazSlidera.Width + 2 * almargin +
                Math.Abs(alszerokosckontrolki - alBtnKsztaltyNaTorzeSzeregu.Width) / 2, alNudUstawGruboscLiniiInfo.Location.Y + alNudUstawGruboscLiniiInfo.Height + alOdstepPomiedzyKontrolkamiMaly);
            alLblUstawStylLiniiInfo.Location = new Point(alBtnKsztaltyNaTorzeSzeregu.Location.X + Math.Abs(alBtnKsztaltyNaTorzeSzeregu.Width - alLblUstawStylLiniiInfo.Width) / 2,
                alBtnKsztaltyNaTorzeSzeregu.Location.Y + alBtnKsztaltyNaTorzeSzeregu.Height + alOdstepPomiedzyKontrolkamiMaly);
            alCmbWybierzZListy.Width = alszerokosckontrolki;
            alCmbWybierzZListy.Location = new Point(alNudUstawGruboscLiniiInfo.Location.X,
                alLblUstawStylLiniiInfo.Location.Y + alLblUstawStylLiniiInfo.Height + alOdstepPomiedzyKontrolkamiMaly);
            alGroupBoxPioroGumka.Width = alszerokosckontrolki;
            alGroupBoxPioroGumka.Location = new Point(alBtnKsztaltyNaTorzeSzeregu.Location.X, alCmbWybierzZListy.Location.Y +
                alCmbWybierzZListy.Height + alOdstepPomiedzyKontrolkamiMaly);
            alBtnKolorPiora.Width = alszerokosckontrolki;
            alBtnKolorPiora.Location = new Point(alBtnKsztaltyNaTorzeSzeregu.Location.X, alGroupBoxPioroGumka.Location.Y +
                alGroupBoxPioroGumka.Height + alOdstepPomiedzyKontrolkamiMaly);
            alPanelKolorPiora.Height = alBtnKolorPiora.Height / 2;
            alPanelKolorPiora.Width = alTxtGruboscLinii.Width;
            alPanelKolorPiora.Location = new Point(alTxtGruboscLinii.Location.X, alBtnKolorPiora.Location.Y + alBtnKolorPiora.Height + alOdstepPomiedzyKontrolkamiMaly);
            alBtnKolorPedzla.Width = alBtnKolorPiora.Width;
            alBtnKolorPedzla.Location = new Point(alBtnKolorPiora.Location.X, alPanelKolorPiora.Location.Y + alPanelKolorPiora.Height + alOdstepPomiedzyKontrolkamiMaly);
            alPanelKolorPedzla.Height = alBtnKolorPedzla.Height / 2;
            alPanelKolorPedzla.Width = alPanelKolorPiora.Width;
            alPanelKolorPedzla.Location = new Point(alPanelKolorPiora.Location.X, alBtnKolorPedzla.Location.Y + alBtnKolorPedzla.Height + alOdstepPomiedzyKontrolkamiMaly);
            alBtnKolorTlaRysownicy.Width = alBtnKolorPedzla.Width;
            alBtnKolorTlaRysownicy.Location = new Point(alBtnKolorPedzla.Location.X, alPanelKolorPedzla.Location.Y + alPanelKolorPedzla.Height + alOdstepPomiedzyKontrolkamiMaly);
            alPanelKolorTlaRysownicy.Height = alBtnKolorTlaRysownicy.Height / 2;
            alPanelKolorTlaRysownicy.Width = alPanelKolorPedzla.Width;
            alPanelKolorTlaRysownicy.Location = new Point(alPanelKolorPedzla.Location.X, alBtnKolorTlaRysownicy.Location.Y + alBtnKolorTlaRysownicy.Height + alOdstepPomiedzyKontrolkamiMaly);

            alCmbWybierzZListy.SelectedIndex = 0;
        }
        private void alBtnKolorPiora_Click(object sender, EventArgs e)
        {
            if (alDlgKolorPiora.ShowDialog() == DialogResult.OK)
            {
                alPanelKolorPiora.BackColor = alDlgKolorPiora.Color;
                alPioro = new Pen(alPanelKolorPiora.BackColor);
            }
        }
        private void alBtnKolorWypelnienia_Click(object sender, EventArgs e)
        {
            if (alDlgKolorPedzla.ShowDialog() == DialogResult.OK)
            {
                alPanelKolorPedzla.BackColor = alDlgKolorPedzla.Color;
                alWypelnienie = new SolidBrush(alPanelKolorPedzla.BackColor);
            }
        }
        private void alBtnKolorTlaRysownicy_Click(object sender, EventArgs e)
        {
            if (alDlgKolorTlaRysownicy.ShowDialog() == DialogResult.OK)
            {
                alPanelKolorTlaRysownicy.BackColor = alDlgKolorTlaRysownicy.Color;
                alPicRysownica.BackColor = alPanelKolorTlaRysownicy.BackColor;
                alPanelPodgladuLinii.BackColor = alPanelKolorTlaRysownicy.BackColor;
            }
        }
        private void alTrbGruboscLinii_Scroll(object sender, EventArgs e)
        {
            int alGrubosclinii = alTrbGruboscLinii.Value;//przypisanie odczytanej wartości suwaka 'alTrbGruboscLinii' do zmiennej
            alAktualizujGruboscLiniiKontrolek(alGrubosclinii);
            alAktualizacjaPaneluPodgladuLinii();
        }
        private void alPicRysownica_Paint(object sender, PaintEventArgs e)
        {
            alAktualizacjaPaneluPodgladuLinii();
            alPanelPodgladuLinii.Refresh();
        }
        private void alPicRysownica_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                alPunkt = e.Location;
            alLblPozycja.Text = "Pozycja kursora" + " " + " X: " + e.Location.X.ToString() + " Y: " + e.Location.Y.ToString();
        }
        private void alPicRysownica_MouseUp(object sender, MouseEventArgs e)
        {
            alLblPozycja.Text = "Pozycja kursora" + " " + " X: " + e.Location.X.ToString() + " Y: " + e.Location.Y.ToString();

            //deklaracje zmiennych  pomocniczych i wyznaczenie konturów kreślonej figury geometrycznej
            int alLewyGornyNaroznikX = (alPunkt.X > e.Location.X) ? e.Location.X : alPunkt.X;
            int alLewyGornyNaroznikY = (alPunkt.Y > e.Location.Y) ? e.Location.Y : alPunkt.Y;
            int alSzerokosc = Math.Abs(alPunkt.X - e.Location.X);
            int alWysokosc = Math.Abs(alPunkt.Y - e.Location.Y); 
            alWypelnienie = new SolidBrush(alPanelKolorPedzla.BackColor);

            alWyborTypuLinii(alCmbWybierzZListy, alPioro);

            //sprawdzenie czy obsługiwane zdarzenie zostało spowodowane zwolnieniem lewego przycisku myszy

            if (e.Button == MouseButtons.Left)
            {
                //rozpoznanie wybranej figury geometrycznej i jej wykreślenie 
                //rozpoznanie wybranej kontrolki Radiobutton określającej wybraną figurę geometryczną
                if (alRbtnPunkt.Checked)
                {
                    alRysownica.FillEllipse(alWypelnienie, e.Location.X - alPromienPunktu,
                        e.Location.Y - alPromienPunktu, 2 * alPromienPunktu, 2 * alPromienPunktu);
                }
                if (alRbtnLiniaProsta.Checked)
                {
                    //kreślenie linii pomiędzy punktem zapamietanym w alPunkt a aklualnym polozeniem kursora e.Location
                    alPioro.StartCap = alPioro.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    alRysownica.DrawLine(alPioro, alPunkt, e.Location);
                }
                if (alRbtnLiniaKreslonaMysza.Checked)
                {
                    //kreślenie linii pomiędzy punktem zapamietanym w alPunkt a aklualnym polozeniem kursora e.Location
                    alRysownica.DrawLine(alPioro, alPunkt, e.Location);
                }
                if (alRbtnElipsa.Checked)
                {
                    alRysownica.DrawEllipse(alPioro, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alWysokosc);
                }
                if (alRbtnOkrag.Checked)
                {
                    alRysownica.DrawEllipse(alPioro, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alSzerokosc);
                }
                if (alRbtnKwadrat.Checked)
                {
                    alRysownica.DrawRectangle(alPioro, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alSzerokosc);
                }
                if (alRbtnProstokat.Checked)
                {
                    alRysownica.DrawRectangle(alPioro, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alWysokosc);
                }
                if (alRbtnDrawArc.Checked)
                {
                    alRysownica.DrawArc(alPioro, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alWysokosc, 0, 180);
                }
                if (alRbtnFillEllipse.Checked)
                {
                    alRysownica.FillEllipse(alWypelnienie, alLewyGornyNaroznikX, alLewyGornyNaroznikY, alSzerokosc, alWysokosc);
                }
                //odświezenie obrazu
                alPicRysownica.Refresh();
            }
        }
        private void alWyborTypuLinii(ComboBox comboBox, Pen alPioro)
        {
            // zdefiniowania parametry rysowania linii gry typ linii jest odmienny od linii ciągłej 
            if (comboBox.SelectedItem.ToString().Trim() == "DashDot")
            {
                float[] alDashDotValues = { 3f, 0.4f, 1f, 0.8f };
                alPioro.DashPattern = alDashDotValues;
            }
            if (comboBox.SelectedItem.ToString().Trim() == "DashDotDot")
            {
                float[] alDashDotDotValues = { 3f, 0.4f, 1f, 0.4f, 1f, 0.8f };
                alPioro.DashPattern = alDashDotDotValues;
            }
            if (comboBox.SelectedItem.ToString().Trim() == "Dash")
            {
                float[] alDashValues = { 3f, 0.8f };
                alPioro.DashPattern = alDashValues;
            }
            if (comboBox.SelectedItem.ToString().Trim() == "Dot")
            {
                float[] alDotValues = { 1f, 0.8f };
                alPioro.DashPattern = alDotValues;
            }
        }
        private void alPicRysownica_MouseMove(object sender, MouseEventArgs e)
        {
            alLblPozycja.Text = "Pozycja kursora" + " " + " X: " + e.Location.X.ToString() + " Y: " + e.Location.Y.ToString();

            alRysownicaTymczasowa = alPicRysownica.CreateGraphics();

            int alLewyGornyNaroznikX = (alPunkt.X > e.Location.X) ? e.Location.X : alPunkt.X;
            int alLewyGornyNaroznikY = (alPunkt.Y > e.Location.Y) ? e.Location.Y : alPunkt.Y;
            int alSzerokosc = Math.Abs(alPunkt.X - e.Location.X);
            int alWysokosc = Math.Abs(alPunkt.Y - e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {

                if (alRbtnLiniaKreslonaMysza.Checked)
                {
                    alWyborTypuLinii(alCmbWybierzZListy, alPioro);
                    alRysownica.DrawLine(alPioro, alPunkt, e.Location);
                    alPunkt = e.Location;
                    alPicRysownica.Refresh();
                }
                else
                {
                    //rozpoznanie zaznaczonej kontrolki radiobutton, określającą wybrana figure geometryczną
                    if (alRbtnPunkt.Checked)
                    {
                        //nie dotyczy
                    }
                    if (alRbtnLiniaProsta.Checked)
                    {
                        alRysownicaTymczasowa.DrawLine(alPioroTymczasowe, alPunkt.X, alPunkt.Y, e.Location.X, e.Location.Y);
                    }
                    if (alRbtnElipsa.Checked)
                    {
                        alRysownicaTymczasowa.DrawEllipse(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                            alSzerokosc, alWysokosc));
                    }
                    if (alRbtnOkrag.Checked)
                    {
                        alRysownicaTymczasowa.DrawEllipse(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                            alSzerokosc, alSzerokosc));
                    }
                    if (alRbtnKwadrat.Checked)
                    {
                        alRysownicaTymczasowa.DrawRectangle(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                            alSzerokosc, alSzerokosc));
                    }
                    if (alRbtnProstokat.Checked)
                    {
                        alRysownicaTymczasowa.DrawRectangle(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                            alSzerokosc, alWysokosc));
                    }
                    if (alRbtnDrawArc.Checked)
                    {
                        alRysownicaTymczasowa.DrawArc(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                        alSzerokosc, alWysokosc), 0, 180);
                    }
                    if (alRbtnFillEllipse.Checked)
                    {
                        alRysownicaTymczasowa.DrawEllipse(alPioroTymczasowe, new Rectangle(alLewyGornyNaroznikX, alLewyGornyNaroznikY,
                           alSzerokosc, alWysokosc));
                    }
                }
            }
            alPicRysownica.Refresh();
        }
        private void alBtnZapiszBitmape_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(alPicRysownica.Width);
                int height = Convert.ToInt32(alPicRysownica.Height);
                Bitmap bmp = new Bitmap(width, height);
                alPicRysownica.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
            }
        }
        private void alCmbWybierzZListy_SelectedIndexChanged(object sender, EventArgs e)
        {
            alWyborTypuLinii(alCmbWybierzZListy, alPioro);
            alPicRysownica_Paint(null, null);
        }
        private void alAktualizujGruboscLiniiKontrolek(int alGrubosc)
        {
            alTrbGruboscLinii.Value = alGrubosc;
            alTxtGruboscLinii.Text = alGrubosc.ToString();
            alLblTrbGruboscLiniiInfo.Text = $"Grubość linii = {alGrubosc}";//przypisanie wartości zmiennej do pola lablu 'allabelRozmiarObiektu'
            alNudUstawGruboscLiniiInfo.Value = alGrubosc;
            alAktualizacjaPaneluPodgladuLinii();
        }
        private void alTxtGruboscLinii_TextChanged(object sender, EventArgs e)
        {
            int alGrubosc = 0;
            var alCzyUdaneParsowanie = int.TryParse(alTxtGruboscLinii.Text, out alGrubosc);
            if (alCzyUdaneParsowanie & alGrubosc <= alTrbGruboscLinii.Maximum & alGrubosc >= alTrbGruboscLinii.Minimum)
            {
                alUstawZalezneKonrtolki(alGrubosc);
            }
            else if (alCzyUdaneParsowanie & alGrubosc < alTrbGruboscLinii.Minimum)
            {
                alGrubosc = alTrbGruboscLinii.Minimum;
            }
            else if (alCzyUdaneParsowanie & alGrubosc > alTrbGruboscLinii.Maximum)
            {
                alGrubosc = alTrbGruboscLinii.Maximum;
            }
            else
            {
                alGrubosc = (int)alPioro.Width;
            }
            alUstawZalezneKonrtolki(alGrubosc);
        }
        private void alUstawZalezneKonrtolki(int alGrubosc)
        {
            alTxtGruboscLinii.Text = alGrubosc.ToString();
            alAktualizujGruboscLiniiKontrolek(alGrubosc);
            alAktualizacjaPaneluPodgladuLinii();
            alPioro.Width = alGrubosc;
            alPanelPodgladuLinii.Refresh();
        }
        private void alNudUstawGruboscLiniiInfo_ValueChanged(object sender, EventArgs e)
        {
            alAktualizujGruboscLiniiKontrolek((int)alNudUstawGruboscLiniiInfo.Value);
        }
    }
}

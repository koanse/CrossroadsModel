using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace CrossroadsModel
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ParamsForm pf = new ParamsForm();
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    Thread t = new Thread(ThreadProc);
                    Form1 form1 = new Form1(t);
                    ThreadProcParams tpp = new ThreadProcParams(form1, pf.sleepTime,
                        pf.lambdaAudi, pf.lambdaBus, pf.lambdaTruck, pf.visualisation);
                    t.Start(tpp);
                    Application.Run(form1);
                }
            }
            catch
            {
                MessageBox.Show("Непредвиденная ошибка, попробуйте " +
                "изменить начальные параметры", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static void ThreadProc(Object o)
        {
            //try
            {
                ThreadProcParams tpp = o as ThreadProcParams;
                Form1 f = tpp.form;
                float lambdaAudi, lambdaBus, lambdaTruck;
                lambdaAudi = tpp.lambdaAudi;
                lambdaBus = tpp.lambdaBus;
                lambdaTruck = tpp.lambdaTruck;
                int sleepTime = tpp.sleepTime;
                bool visualisation = tpp.visualisation;
                Bitmap b = new Bitmap(600, 600);
                Graphics g = Graphics.FromImage(b);
                f.pictureBox1.Image = b;

                float x = 175, y = 175, w = 30, xMax = 600, yMax = 600;
                float epsilon = 0.001f;

                // Контрольные точки
                // Дорога от площади Революции
                CheckPoint cpRevBackStart1 = new CheckPoint(x + 3.5f * w, 0);
                CheckPoint cpRevBackTrafLight1 = new CheckPoint(x + 3.5f * w, y + epsilon);
                CheckPoint cpRevForwStart1 = new CheckPoint(x + 4.5f * w, 0);
                CheckPoint cpRevForwTrafLight1 = new CheckPoint(x + 4.5f * w, y);
                CheckPoint cpRevForwStart2 = new CheckPoint(x + 5.5f * w, 0);
                CheckPoint cpRevForwTrafLight2 = new CheckPoint(x + 5.5f * w, y);
                CheckPoint cpRevForwStart3 = new CheckPoint(x + 6.5f * w, 0);
                CheckPoint cpRevForwTrafLight3 = new CheckPoint(x + 6.5f * w, y);

                CheckPoint cpRevForwMiddle = new CheckPoint(x + 4.5f * w, y + 3 * w);
                CheckPoint cpRevForwLeft1 = new CheckPoint(x + 2 * w + 2.5f * w * (float)Math.Cos(0.7f),
                    y + 3 * w + 2.5f * w * (float)Math.Sin(0.7f));
                CheckPoint cpRevForwLeft2 = new CheckPoint(x + 2 * w, y + 5.5f * w);

                // Дорога от стадиона
                CheckPoint cpStaForwStart1 = new CheckPoint(x + 3.5f * w, yMax);
                CheckPoint cpStaForwTrafLight1 = new CheckPoint(x + 3.5f * w, y + 10 * w);
                CheckPoint cpStaBackStart1 = new CheckPoint(x + 4.5f * w, yMax);
                CheckPoint cpStaBackTrafLight1 = new CheckPoint(x + 4.5f * w, y + 10 * w);
                CheckPoint cpStaBackStart2 = new CheckPoint(x + 5.5f * w, yMax);
                CheckPoint cpStaBackTrafLight2 = new CheckPoint(x + 5.5f * w, y + 10 * w);
                CheckPoint cpStaBackStart3 = new CheckPoint(x + 6.5f * w, yMax);
                CheckPoint cpStaBackTrafLight3 = new CheckPoint(x + 6.5f * w, y + 10 * w);

                // Дорога от Фрунзе
                CheckPoint cpFruBackStart1 = new CheckPoint(0, y + 6.5f * w);
                CheckPoint cpFruBackTrafLight1 = new CheckPoint(x, y + 6.5f * w);
                CheckPoint cpFruBackStart2 = new CheckPoint(0, y + 5.5f * w);
                CheckPoint cpFruBackTrafLight2 = new CheckPoint(x, y + 5.5f * w);
                CheckPoint cpFruForwStart1 = new CheckPoint(0, y + 4.5f * w);
                CheckPoint cpFruForwTrafLight1 = new CheckPoint(x, y + 4.5f * w);
                CheckPoint cpFruForwStart2 = new CheckPoint(0, y + 3.5f * w);
                CheckPoint cpFruForwTrafLight2 = new CheckPoint(x, y + 3.5f * w);

                CheckPoint cpFruForwMiddle = new CheckPoint(x + 2 * w, y + 3 * w);

                // Дорога от театра
                CheckPoint cpTheForwStart1 = new CheckPoint(xMax, y + 7.5f * w);
                CheckPoint cpTheForwTrafLight1 = new CheckPoint(x + 9 * w, y + 7.5f * w);
                CheckPoint cpTheForwStart2 = new CheckPoint(xMax, y + 6.5f * w);
                CheckPoint cpTheForwTrafLight2 = new CheckPoint(x + 9 * w, y + 6.5f * w);
                CheckPoint cpTheForwStart3 = new CheckPoint(xMax, y + 5.5f * w);
                CheckPoint cpTheForwTrafLight3 = new CheckPoint(x + 9 * w, y + 5.5f * w);
                CheckPoint cpTheBackStart1 = new CheckPoint(xMax, y + 4.5f * w);
                CheckPoint cpTheBackTrafLight1 = new CheckPoint(x + 9 * w, y + 4.5f * w);
                CheckPoint cpTheBackStart2 = new CheckPoint(xMax, y + 3.5f * w);
                CheckPoint cpTheBackTrafLight2 = new CheckPoint(x + 9 * w, y + 3.5f * w);
                CheckPoint cpTheBackStart3 = new CheckPoint(xMax, y + 2.5f * w);
                CheckPoint cpTheBackTrafLight3 = new CheckPoint(x + 9 * w, y + 2.5f * w);

                // Точки для поворота
                // Площадь Революции - театр
                float sqrt2 = (float)Math.Sqrt(2);
                PointF pRevThe2 = new PointF(x + 9 * w - 3.5f * w / sqrt2, y + 3.5f * w / sqrt2);
                PointF pRevThe3 = new PointF(x + 9 * w - 2.5f * w / sqrt2, y + 2.5f * w / sqrt2);
                // Площадь Революции - Фрунзе
                PointF pRevFru1 = new PointF(x + 2 * w + 2.5f * w * (float)Math.Cos(0.1f),
                    y + 3 * w + 2.5f * w * (float)Math.Sin(0.1f));
                PointF pRevFru2 = new PointF(x + 2 * w + 2.5f * w / sqrt2, y + 3 * w + 2.5f * w / sqrt2);
                // Театр - стадион
                PointF pTheSta2 = new PointF(x + 9 * w - 3.5f * w / sqrt2, y + 10 * w - 3.5f * w / sqrt2);
                PointF pTheSta3 = new PointF(x + 9 * w - 2.5f * w / sqrt2, y + 10 * w - 2.5f * w / sqrt2);
                // Стадион - Фрунзе
                PointF pStaFru1 = new PointF(x + 3.5f * w / sqrt2, y + 10 * w - 3.5f * w / sqrt2);
                PointF pStaFru2 = new PointF(x + 4.5f * w / sqrt2, y + 10.1f * w - 4.5f * w / sqrt2);
                // Фрунзе - театр
                PointF pFruThe1 = new PointF(x + 1 * w, y + 3.3f * w);
                PointF pFruThe2 = new PointF(x + 4 * w, y + 2.6f * w);
                // Фрунзе - площадь Революции
                PointF pFruRev1 = new PointF(x + 3.5f * w / sqrt2, y + 3.5f * w / sqrt2);

                // Треки
                // Дорога от площади Революции
                float minDistCoeff1 = 0.8f, minDistCoeff2 = 0.7f;
                Track tRevSta11 = new StraightTrack(cpRevForwTrafLight1, cpRevForwMiddle, w);
                Track tRevSta12 = new StraightTrack(cpRevForwMiddle, cpStaBackTrafLight1, w);
                Track tRevSta2 = new StraightTrack(cpRevForwTrafLight2, cpStaBackTrafLight2, w);
                Track tRevSta3 = new StraightTrack(cpRevForwTrafLight3, cpStaBackTrafLight3, w);
                Track tRevThe2 = new ArchTrack(cpRevForwTrafLight2, cpTheBackTrafLight2, pRevThe2, minDistCoeff1, w);
                Track tRevThe3 = new ArchTrack(cpRevForwTrafLight3, cpTheBackTrafLight3, pRevThe3, minDistCoeff1, w);
                Track tRevFru21 = new ArchTrack(cpRevForwMiddle, cpRevForwLeft1, pRevFru1, minDistCoeff2, w);
                Track tRevFru22 = new ArchTrack(cpRevForwLeft1, cpRevForwLeft2, pRevFru2, minDistCoeff2, w);
                Track tRevFru23 = new StraightTrack(cpRevForwLeft2, cpFruBackTrafLight2, w);

                Track tRevBack1 = new StraightTrack(cpRevBackTrafLight1, cpRevBackStart1, w);
                Track tRevForw1 = new StraightTrack(cpRevForwStart1, cpRevForwTrafLight1, w);
                Track tRevForw2 = new StraightTrack(cpRevForwStart2, cpRevForwTrafLight2, w);
                Track tRevForw3 = new StraightTrack(cpRevForwStart3, cpRevForwTrafLight3, w);

                // Дорога от стадиона
                Track tStaRev1 = new StraightTrack(cpStaForwTrafLight1, cpRevBackTrafLight1, w);
                Track tStaFru1 = new ArchTrack(cpStaForwTrafLight1, cpFruBackTrafLight1, pStaFru1, minDistCoeff1, w);
                Track tStaFru2 = new ArchTrack(cpStaBackTrafLight2, cpFruBackTrafLight2, pStaFru2, minDistCoeff1, w);

                Track tStaForw1 = new StraightTrack(cpStaForwStart1, cpStaForwTrafLight1, w);
                Track tStaBack1 = new StraightTrack(cpStaBackTrafLight1, cpStaBackStart1, w);
                Track tStaBack2 = new StraightTrack(cpStaBackTrafLight2, cpStaBackStart2, w);
                Track tStaBack3 = new StraightTrack(cpStaBackTrafLight3, cpStaBackStart3, w);

                // Дорога от Фрунзе
                Track tFruThe1 = new StraightTrack(cpFruForwTrafLight1, cpTheBackTrafLight1, w);
                Track tFruThe2 = new StraightTrack(cpFruForwTrafLight2, cpTheBackTrafLight2, w);
                Track tFruThe31 = new ArchTrack(cpFruForwTrafLight2, cpFruForwMiddle, pFruThe1, minDistCoeff2, w);
                Track tFruThe32 = new ArchTrack(cpFruForwMiddle, cpTheBackTrafLight3, pFruThe2, minDistCoeff2, w);
                Track tFruRev1 = new ArchTrack(cpFruForwTrafLight2, cpRevBackTrafLight1, pFruRev1, minDistCoeff1, w);

                Track tFruBack1 = new StraightTrack(cpFruBackTrafLight1, cpFruBackStart1, w);
                Track tFruBack2 = new StraightTrack(cpFruBackTrafLight2, cpFruBackStart2, w);
                Track tFruForw1 = new StraightTrack(cpFruForwStart1, cpFruForwTrafLight1, w);
                Track tFruForw2 = new StraightTrack(cpFruForwStart2, cpFruForwTrafLight2, w);

                // Дорога от театра
                Track tTheFru2 = new StraightTrack(cpTheForwTrafLight2, cpFruBackTrafLight1, w);
                Track tTheFru3 = new StraightTrack(cpTheForwTrafLight3, cpFruBackTrafLight2, w);
                Track tTheSta1 = new ArchTrack(cpTheForwTrafLight1, cpStaBackTrafLight3, pTheSta3, minDistCoeff1, w);
                Track tTheSta2 = new ArchTrack(cpTheForwTrafLight2, cpStaBackTrafLight2, pTheSta2, minDistCoeff1, w);

                Track tTheForw1 = new StraightTrack(cpTheForwStart1, cpTheForwTrafLight1, w);
                Track tTheForw2 = new StraightTrack(cpTheForwStart2, cpTheForwTrafLight2, w);
                Track tTheForw3 = new StraightTrack(cpTheForwStart3, cpTheForwTrafLight3, w);
                Track tTheBack1 = new StraightTrack(cpTheBackTrafLight1, cpTheBackStart1, w);
                Track tTheBack2 = new StraightTrack(cpTheBackTrafLight2, cpTheBackStart2, w);
                Track tTheBack3 = new StraightTrack(cpTheBackTrafLight3, cpTheBackStart3, w);

                // Имена треков
                tRevForw1.name = "Первая полоса (пл. Революции)";
                tRevForw2.name = "Вторая полоса (пл. Революции)";
                tRevForw3.name = "Третья полоса (пл. Революции)";
                tStaForw1.name = "Первая полоса (стадион)";
                tFruForw1.name = "Первая полоса (ул. Фрунзе)";
                tFruForw2.name = "Вторая полоса (ул. Фрунзе)";
                tTheForw1.name = "Первая полоса (театр)";
                tTheForw2.name = "Вторая полоса (театр)";
                tTheForw3.name = "Третья полоса (театр)";

                // Машины
                Bitmap bmpAudi, bmpBus, bmpTruck;
                bmpAudi = Properties.Resources.AudiSmall;
                bmpBus = Properties.Resources.BusSmall;
                bmpTruck = Properties.Resources.TruckSmall;
                bmpAudi.MakeTransparent(Color.Magenta);
                bmpBus.MakeTransparent(Color.Magenta);
                bmpTruck.MakeTransparent(Color.Magenta);
                Car cAudi = new Car(bmpAudi, 4, 0.5f, -1.4f, 5.5f);
                Car cBus = new Car(bmpBus, 4, 0.4f, -1.3f, 5);
                Car cTruck = new Car(bmpTruck, 4, 0.3f, -1.2f, 4);

                // Дорога от площади Революции
                Path pathRev1Sta1 = new Path(new Track[] { tRevForw1, tRevSta11, tRevSta12, tStaBack1 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 1.2f, cAudi, 5),
                new ExpGenerator(lambdaBus * 0.7f, cBus, 7),
                new ExpGenerator(lambdaTruck * 0.5f, cTruck, 6) });
                Path pathRev1Fru2 = new Path(new Track[] { tRevForw1, tRevSta11, tRevFru21, tRevFru22, tRevFru23, tFruBack2 }, new Generator[] {
                new ExpGenerator(lambdaBus * 1.7f, cBus, 7) });
                Path pathRev2Sta2 = new Path(new Track[] { tRevForw2, tRevSta2, tStaBack2 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 1.0f, cAudi, 84),
                new ExpGenerator(lambdaBus * 0.8f, cBus, 63),
                new ExpGenerator(lambdaTruck * 1.1f, cTruck, 4) });
                Path pathRev3Sta3 = new Path(new Track[] { tRevForw3, tRevSta3, tStaBack3 }, new Generator[] { });
                Path pathRev2The2 = new Path(new Track[] { tRevForw2, tRevThe2, tTheBack2 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 0.4f, cAudi, 52),
                new ExpGenerator(lambdaBus * 1.2f, cBus, 33),
                new ExpGenerator(lambdaTruck * 0.7f, cTruck, 26) });
                Path pathRev3The3 = new Path(new Track[] { tRevForw3, tRevThe3, tTheBack3 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 0.9f, cAudi, 22),
                new ExpGenerator(lambdaBus * 1.0f, cBus, 90),
                new ExpGenerator(lambdaTruck * 1.1f, cTruck, 5) });

                // Дорога от стадиона
                Path pathSta1Rev1 = new Path(new Track[] { tStaForw1, tStaRev1, tRevBack1 }, new Generator[] {
                new ExpGenerator(lambdaBus * 1.2f, cBus, 190) });
                Path pathSta1Fru1 = new Path(new Track[] { tStaForw1, tStaFru1, tFruBack1 }, new Generator[] {
                new ExpGenerator(lambdaBus * 0.2f, cBus, 190) });
                Path pathSta2Fru2 = new Path(new Track[] { tStaFru2 }, new Generator[] { });

                // Дорога от Фрунзе
                Path pathFru1The1 = new Path(new Track[] { tFruForw1, tFruThe1, tTheBack1 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 0.8f, cAudi, 122),
                new ExpGenerator(lambdaBus * 1.1f, cBus, 44),
                new ExpGenerator(lambdaTruck * 0.5f, cTruck, 52) });
                Path pathFru2The2 = new Path(new Track[] { tFruForw2, tFruThe2, tTheBack2 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 1.2f, cAudi, 122),
                new ExpGenerator(lambdaBus * 0.1f, cBus, 40),
                new ExpGenerator(lambdaTruck * 0.6f, cTruck, 15) });
                Path pathFru2The3 = new Path(new Track[] { tFruForw2, tFruThe31, tFruThe32, tTheBack3 }, new Generator[] {
                new ExpGenerator(lambdaBus * 1.0f, cBus, 40) });
                Path pathFru2Rev1 = new Path(new Track[] { tFruForw2, tFruRev1, tRevBack1 }, new Generator[] {
                new ExpGenerator(lambdaBus * 0.1f, cBus, 40) });

                // Дорога от театра
                Path pathThe1Sta3 = new Path(new Track[] { tTheForw1, tTheSta1, tStaBack3 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 1.1f, cAudi, 72) });
                Path pathThe2Fru1 = new Path(new Track[] { tTheForw2, tTheFru2, tFruBack1 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 1.3f, cAudi, 62),
                new ExpGenerator(lambdaBus * 0.9f, cBus, 79),
                new ExpGenerator(lambdaTruck * 0.9f, cTruck, 55) });
                Path pathThe3Fru2 = new Path(new Track[] { tTheForw3, tTheFru3, tFruBack2 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 0.9f, cAudi, 222),
                new ExpGenerator(lambdaBus * 1.0f, cBus, 91),
                new ExpGenerator(lambdaTruck * 0.8f, cTruck, 35) });
                Path pathThe2Sta2 = new Path(new Track[] { tTheForw2, tTheSta2, tStaBack2 }, new Generator[] {
                new ExpGenerator(lambdaAudi * 0.6f, cAudi, 72) });


                Director d = new Director(new Path[] {
                pathRev1Sta1, pathRev1Fru2, pathRev2Sta2, pathRev2The2, pathRev3The3, pathRev3Sta3,
                pathSta1Rev1, pathSta1Fru1, pathSta2Fru2,
                pathFru1The1, pathFru2The2, pathFru2The3, pathFru2Rev1,
                pathThe1Sta3, pathThe2Sta2, pathThe2Fru1, pathThe3Fru2
            });

                g.TranslateTransform(0, b.Height);
                g.ScaleTransform(1, -1);

                float timeTrafLigRevSta = 200, timeTrafLigFruThe = 200, counterRevSta, counterFruThe;
                float distSafe = 6 * w;
                bool openedRevSta = false, openedFruThe = true;
                cpStaForwTrafLight1.status =
                            cpRevForwTrafLight1.status =
                            cpRevForwTrafLight2.status =
                            cpRevForwTrafLight3.status = CheckPointStatus.Stop;
                counterRevSta = timeTrafLigRevSta;
                counterFruThe = timeTrafLigFruThe;
                f.pictureBox1.Invalidate();
                Bitmap bGreen = Properties.Resources.Green;
                Bitmap bRed = Properties.Resources.Red;
                bGreen.MakeTransparent(Color.Magenta);
                bRed.MakeTransparent(Color.Magenta);

                g.Clear(Color.LightGray);
                d.DrawTracks(g);
                for (ulong t = 0; t < 1000000; t++)
                {
                    d.DoIteration(1);
                    if (visualisation)
                    {
                        g.Clear(Color.LightGray);
                        d.DrawTracks(g);
                        d.DrawCars(g);
                        RectangleF destRectSta = new RectangleF(x + 2 * w, y + 10 * w, w / 2, w / 2);
                        RectangleF destRectRev = new RectangleF(x + 7.5f * w, y, w / 2, w / 2);
                        RectangleF destRectFru = new RectangleF(x, y + 2 * w, w / 2, w / 2);
                        RectangleF destRectThe = new RectangleF(x + 9 * w, y + 8.5f * w, w / 2, w / 2);
                        Rectangle sourceRect = new Rectangle(0, 0, bGreen.Width, bGreen.Height);
                        if (openedFruThe)
                        {
                            g.DrawImage(bGreen, destRectSta, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bGreen, destRectRev, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bRed, destRectFru, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bRed, destRectThe, sourceRect, GraphicsUnit.Pixel);
                            
                        }
                        else
                        {
                            g.DrawImage(bRed, destRectSta, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bRed, destRectRev, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bGreen, destRectFru, sourceRect, GraphicsUnit.Pixel);
                            g.DrawImage(bGreen, destRectThe, sourceRect, GraphicsUnit.Pixel);
                        }
                        f.pictureBox1.Invalidate();
                    }
                    f.textBox1.Text = d.MakeStatistics(t);

                    counterRevSta--;
                    counterFruThe--;
                    if (counterRevSta == 0)
                    {
                        counterRevSta = timeTrafLigRevSta;
                        if (openedRevSta)
                            openedRevSta = false;
                        else
                            openedRevSta = true;
                    }
                    if (counterFruThe == 0)
                    {
                        counterFruThe = timeTrafLigFruThe;
                        if (openedFruThe)
                            openedFruThe = false;
                        else
                            openedFruThe = true;
                    }
                    if (openedRevSta &&
                        tTheFru2.cars.Count == 0 &&
                        tTheFru3.cars.Count == 0 &&
                        tTheSta1.cars.Count == 0 &&
                        tTheSta2.cars.Count == 0 &&
                        tFruThe1.cars.Count == 0 &&
                        tFruThe2.cars.Count == 0 &&
                        tFruThe31.cars.Count == 0 &&
                        tFruThe32.cars.Count == 0 &&
                        tFruRev1.cars.Count == 0)
                    {
                        cpStaForwTrafLight1.status =
                            cpRevForwTrafLight1.status =
                            cpRevForwTrafLight2.status =
                            cpRevForwTrafLight3.status = CheckPointStatus.Go;
                    }
                    else
                    {
                        cpStaForwTrafLight1.status =
                             cpRevForwTrafLight1.status =
                             cpRevForwTrafLight2.status =
                             cpRevForwTrafLight3.status = CheckPointStatus.Stop;
                    }

                    if (openedFruThe &&
                        tStaRev1.cars.Count == 0 &&
                        tStaFru1.cars.Count == 0 &&
                        tStaFru2.cars.Count == 0 &&
                        tRevSta11.cars.Count == 0 &&
                        tRevSta12.cars.Count == 0 &&
                        tRevFru21.cars.Count == 0 &&
                        tRevFru22.cars.Count == 0 &&
                        tRevSta2.cars.Count == 0 &&
                        tRevThe2.cars.Count == 0 &&
                        tRevThe3.cars.Count == 0)
                    {
                        cpTheForwTrafLight1.status =
                            cpTheForwTrafLight2.status =
                            cpTheForwTrafLight3.status =
                            cpFruForwTrafLight1.status =
                            cpFruForwTrafLight2.status = CheckPointStatus.Go;
                    }
                    else
                    {
                        cpTheForwTrafLight1.status =
                            cpTheForwTrafLight2.status =
                            cpTheForwTrafLight3.status =
                            cpFruForwTrafLight1.status =
                            cpFruForwTrafLight2.status = CheckPointStatus.Stop;
                    }
                    if ((tStaForw1.cars.Count == 0 ||
                        cpStaForwTrafLight1.status == CheckPointStatus.Stop) &&
                        tStaRev1.GetDistance(0, true) >= distSafe)
                        cpRevForwLeft1.status = CheckPointStatus.Go;
                    else
                        cpRevForwLeft1.status = CheckPointStatus.Stop;
                    if (sleepTime > 0)
                        Thread.Sleep(sleepTime);
                }
            }
            //catch
            //{
             //   MessageBox.Show("Непредвиденная ошибка, попробуйте " +
              //  "изменить начальные параметры", "Ошибка",
              //  MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}        
        }
    }
    public abstract class Track
    {
        public string name = "Noname";
        public CheckPoint pStart;
        public CheckPoint pEnd;
        public ArrayList cars, carsBuf;
        public float length, minDistCoeff, width;
        public long sumCars, sumTime, sumWaitTime, sumSurvedCars;
        public float GetAngle(float l)
        {
            PointF p1, p2;
            float epsilon = 0.01f;
            p1 = GetPoint(l);
            p2 = GetPoint(l + epsilon);
            float x1, x2, y1, y2;
            x1 = p1.X;
            y1 = p1.Y;
            x2 = p2.X;
            y2 = p2.Y;
            float alpha = (float)Math.Acos((x2 - x1) /
                (float)Math.Sqrt((x2 - x1) * (x2 - x1) +
                (y2 - y1) * (y2 - y1)));
            if (y2 - y1 < 0)
                alpha = 2 * (float)Math.PI - alpha;
            return alpha * 180 / (float)Math.PI;
        }
        public abstract PointF GetPoint(float l);
        public abstract void Draw(Graphics g);
        public float GetDistance(float l, bool equal)
        {
            float min = float.MaxValue;
            if (equal)
            {
                foreach (Car c in cars)
                    if (c.l - c.length / 2 - l < min &&
                        c.l >= l)
                        min = c.l - c.length / 2 - l;
            }
            else
                foreach (Car c in cars)
                    if (c.l - c.length / 2 - l < min &&
                        c.l > l)
                        min = c.l - c.length / 2 - l;
            if (min < float.MaxValue)
                return min;
            if (pEnd.status == CheckPointStatus.Stop)
                return length - l;
            return float.PositiveInfinity;
        }
        public void ProcessBuf()
        {
            if (carsBuf.Count == 0)
                return;
            Car c = carsBuf[0] as Car;
            float d = GetDistance(0, true);
            if (c.length + c.minDistance < d)
            {
                carsBuf.RemoveAt(0);
                c.EnterTheModel(c.path);
            }
        }
    }
    public class StraightTrack : Track
    {
        public StraightTrack(CheckPoint pStart, CheckPoint pEnd, float width)
        {
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.cars = new ArrayList();
            this.carsBuf = new ArrayList();
            float x0, x1, y0, y1;
            x0 = pStart.x;
            y0 = pStart.y;
            x1 = pEnd.x;
            y1 = pEnd.y;
            this.length = (float)Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
            this.minDistCoeff = 1;
            this.width = width;
            pStart.outTracks.Add(this);
            pEnd.inTracks.Add(this);
        }
        public override PointF GetPoint(float l)
        {
            float t = l / length;
            float x0, x1, y0, y1, a, b;
            x0 = pStart.x;
            y0 = pStart.y;
            x1 = pEnd.x;
            y1 = pEnd.y;
            a = x1 - x0;
            b = y1 - y0;
            return new PointF(a * t + x0, b * t + y0);
        }
        public override void Draw(Graphics g)
        {
            float x1, y1, x2, y2;
            x1 = pStart.x;
            y1 = pStart.y;
            x2 = pEnd.x;
            y2 = pEnd.y;
            Pen p = new Pen(Brushes.Gray, width);
            g.DrawLine(p, x1, y1, x2, y2);
            g.FillEllipse(Brushes.Gray, x1 - width / 2, y1 - width / 2,
                width, width);
            g.FillEllipse(Brushes.Gray, x2 - width / 2, y2 - width / 2,
                width, width);
        }
    }
    public class ArchTrack : Track
    {
        float xCen, yCen, r, sign, tInit;
        public ArchTrack(CheckPoint pStart, CheckPoint pEnd,
            PointF pMiddle, float minDistCoeff, float width)
        {
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.cars = new ArrayList();
            this.carsBuf = new ArrayList();
            this.minDistCoeff = minDistCoeff;
            this.width = width;
            float x1, x2, x3, y1, y2, y3;
            x1 = pStart.x;
            y1 = pStart.y;
            x2 = pMiddle.X;
            y2 = pMiddle.Y;
            x3 = pEnd.x;
            y3 = pEnd.y;
            float xN, yN, xM, yM;
            xN = (x1 + x2) / 2;
            yN = (y1 + y2) / 2;
            xM = (x2 + x3) / 2;
            yM = (y2 + y3) / 2;
            float xVN, yVN, xVM, yVM, delta;
            xVN = (y2 - y1);
            yVN = -(x2 - x1);
            xVM = y3 - y2;
            yVM = -(x3 - x2);
            delta = xVN * yVM - xVM * yVN;
            xCen = xM * xVN * yVM + xVM * xVN * yN - xVM * xVN * yM - xN * xVM * yVN;
            xCen /= delta;
            yCen = xM * yVM * yVN + xVN * yN * yVM - xVM * yM * yVN - xN * yVM * yVN;
            yCen /= delta;
            float v, tEnd;
            v = (x2 - x1) * (y3 - y2) - (x3 - x2) * (y2 - y1);
            r = (float)Math.Sqrt((x1 - xCen) * (x1 - xCen) + (y1 - yCen) * (y1 - yCen));
            tInit = (float)Math.Acos((x1 - xCen) / r);
            if (y1 - yCen < 0)
                tInit = 2 * (float)Math.PI - tInit;
            tEnd = (float)Math.Acos((x3 - xCen) / r);
            if (y3 - yCen < 0)
                tEnd = 2 * (float)Math.PI - tEnd;
            if (v >= 0)
                sign = 1;
            else
                sign = -1;
            length = (float)Math.Abs(r * (tEnd - tInit));
            pStart.outTracks.Add(this);
            pEnd.inTracks.Add(this);
        }
        public override PointF GetPoint(float l)
        {
            float x, y, t;
            t = l / r;
            x = r * (float)Math.Cos(sign * t + tInit) + xCen;
            y = r * (float)Math.Sin(sign * t + tInit) + yCen;
            return new PointF(x, y);
        }
        public override void Draw(Graphics g)
        {
            float x1, y1, x2, y2;
            x1 = pStart.x;
            y1 = pStart.y;
            x2 = pEnd.x;
            y2 = pEnd.y;
            Pen p = new Pen(Brushes.Gray, width);
            g.DrawArc(p, xCen - r, yCen - r, r * 2, r * 2,
                tInit * 180 / (float)Math.PI,
                sign * length / r * 180 / (float)Math.PI);
            g.FillEllipse(Brushes.Gray, x1 - width / 2, y1 - width / 2,
                width, width);
            g.FillEllipse(Brushes.Gray, x2 - width / 2, y2 - width / 2,
                width, width);
        }
    }
    public class CheckPoint
    {
        public float x, y;
        public CheckPointStatus status;
        public ArrayList inTracks;
        public ArrayList outTracks;
        public CheckPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.inTracks = new ArrayList();
            this.outTracks = new ArrayList();
            this.status = CheckPointStatus.Go;
        }
        public void Draw(Graphics g) { }
    }
    public class Path
    {
        public Track[] tracks;
        public ArrayList carsBuf;
        public Generator[] generators;
        public Path(Track[] tracks, Generator[] generators)
        {
            this.tracks = tracks;
            this.carsBuf = new ArrayList();
            this.generators = generators;
        }
        public Track GetNextTrack(Track track)
        {
            int i = 0;
            while (tracks[i] != track &&
                i < tracks.Length)
                i++;
            if (i < tracks.Length - 1)
                return tracks[i + 1];
            return null;
        }
        public float GetMinDistance(Track track, float l)
        {
            float dist = track.GetDistance(l, false);
            if (track.pEnd.outTracks == null ||
                dist < track.length - l)
                return dist;
            Track tNext = GetNextTrack(track);
            float distMin = float.MaxValue;
            foreach (Track t in track.pEnd.outTracks)
            {
                float distTmp = t.GetDistance(0, true);
                if (distTmp > 0)
                {
                    distTmp *= t.minDistCoeff;
                    if (t != tNext)
                        distTmp = (float)Math.Pow(distTmp, 1.1);
                }
                if (distTmp < distMin)
                    distMin = distTmp;
            }
            if (dist == track.length - l)
                if (distMin < 0)
                    return dist + distMin;
                else return dist;
            else
                return track.length - l + distMin;
        }
        public void Draw(Graphics g)
        {
            foreach (Track t in tracks)
                t.Draw(g);
        }
    }
    public class Car: ICloneable
    {
        public float l, length, minDistance, width;
        public float a, aMax, aMin;
        public float v, vMax;
        public Path path;
        public Track track;
        public Bitmap bmp;
        public long sumTrackWaitTime;
        public Car(Bitmap bmp, float minDistance,
            float aMax, float aMin, float vMax)
        {
            this.bmp = bmp;
            this.length = bmp.Width;
            this.width = bmp.Height;
            this.minDistance = minDistance;
            this.aMax = aMax;
            this.aMin = aMin;
            this.vMax = vMax;
            this.l = 0;
            this.a = 0;
            this.v = 0;
            this.path = null;
            this.track = null;
        }
        public void EnterTheModel(Path path)
        {
            this.path = path;
            this.track = path.tracks[0];
            this.l = 0;
            this.a = 0;
            this.v = GetOptimalVelocity() / 2;
            track.cars.Add(this);
        }
        public void DoDriving()
        {
            if (path == null || track == null)
                return;
            a = GetOptimalAccel();
        }
        public void DoMove(int ticks)
        {
            if (path == null || track == null)
                return;
            v += a * ticks;
            float lNext = l + v * ticks;
            if (lNext < l)
                lNext = l;
            if (lNext <= track.length)
            {
                sumTrackWaitTime++;
                l = lNext;
            }
            else
            {
                track.cars.Remove(this);
                track.sumWaitTime += sumTrackWaitTime;
                track.sumSurvedCars++;
                sumTrackWaitTime = 0;
                l = lNext - track.length;
                track = path.GetNextTrack(track);
                if (track != null)
                    track.cars.Insert(0, this);
                else
                {
                    path = null;
                    l = 0;
                }
            }
        }
        public void Draw(Graphics g)
        {
            if (path == null || track == null)
                return;
            GraphicsState gs = g.Save();
            PointF p = track.GetPoint(l);
            float angle = track.GetAngle(l);
            g.TranslateTransform(p.X, p.Y);
            g.RotateTransform(angle);
            g.DrawImage(bmp, -length / 2, -width / 2);
            g.Restore(gs);
        }
        public object Clone()
        {
            Car c = new Car(bmp, minDistance, aMax, aMin, vMax);
            c.l = l;
            c.a = a;
            c.v = v;
            c.path = path;
            c.track = track;
            return c;
        }
        float GetOptimalVelocity()
        {
            float d = path.GetMinDistance(track, l);
            if (d == float.PositiveInfinity)
                return vMax;
            d -= minDistance;
            d -= length / 2;
            float tCollision, vOpt;
            if (d <= 0)
                return 0;
            else
                tCollision = d / v;
            vOpt = -aMin * tCollision;
            if (vOpt > vMax)
                vOpt = vMax;
            if (vOpt < 0)
                return 0;
            return vOpt;
        }
        float GetOptimalAccel()
        {
            float vOpt = GetOptimalVelocity();
            float aOpt = vOpt - v;
            if (aOpt > aMax)
                aOpt = aMax;
            if (aOpt < aMin)
                aOpt = aMin;
            return aOpt;
        }
    }
    public class Director
    {
        Path[] paths;
        ArrayList cars;
        public string statistics;
        public Director(Path[] paths)
        {
            this.paths = paths;
            this.cars = new ArrayList();
        }
        public void DoIteration(int ticks)
        {
            foreach (Path p in paths)
                foreach (Generator g in p.generators)
                {
                    Car c = g.DoTicks(ticks);
                    if (c != null)
                    {
                        c.path = p;
                        c.track = null;
                        p.tracks[0].carsBuf.Insert(0, c);
                        cars.Add(c);
                    }
                }
            foreach (Path p in paths)
                p.tracks[0].ProcessBuf();
            foreach (Car c in cars)
                c.DoDriving();
            ArrayList carsToRemove = new ArrayList();
            foreach (Car c in cars)
            {
                c.DoMove(ticks);
                if (c.path == null)
                    carsToRemove.Add(c);
            }
            foreach (Car c in carsToRemove)
                cars.Remove(c);
            foreach (Path p in paths)
            {
                int i;
                for (i = 0; i < paths.Length; i++)
                    if (paths[i].tracks[0] == p.tracks[0])
                        break;
                if (p != paths[i])
                    continue;
                int sum = p.tracks[0].cars.Count + p.tracks[0].carsBuf.Count;
                p.tracks[0].sumCars += sum;
                if (sum > 0)
                    p.tracks[0].sumTime++;
                foreach (Car c in p.tracks[0].carsBuf)
                    c.sumTrackWaitTime++;
            }
        }
        public void DrawCars(Graphics g)
        {
            foreach (Car c in cars)
                c.Draw(g);
        }
        public void DrawTracks(Graphics g)
        {
            foreach (Path p in paths)
                p.Draw(g);
        }
        public string MakeStatistics(ulong iterNum)
        {
            string s = "Итерация номер " + iterNum.ToString() + "\r\n";
            foreach (Path p in paths)
            {
                Track t = p.tracks[0];
                int i;
                for (i = 0; i < paths.Length; i++)
                    if (paths[i].tracks[0] == t)
                        break;
                if (paths[i] != p)
                    continue;
                if (t.name != "Noname")
                {
                    s += t.name + ":\r\n        ";
                    float avCars = (float)t.sumCars / (iterNum + 1);
                    int sumCars = t.cars.Count + t.carsBuf.Count;
                    s += sumCars.ToString() + " машин(ы) (в текущий момент), ";
                    s += avCars.ToString() + " машин(ы) (в среднем)\r\n        ";
                    float avUtil = (float)t.sumTime / (iterNum + 1) * 100;
                    float avWaitTime;
                    if (t.sumSurvedCars != 0)
                    {
                        avWaitTime = t.sumWaitTime / t.sumSurvedCars;
                        s += avWaitTime.ToString() + " среднее время пребывания машины\r\n        ";
                    }
                    else
                        s += "среднее время пребывания машины пока не определено\r\n        ";
                    s += avUtil.ToString() + "% использования трека, ";
                    s += t.sumSurvedCars.ToString() + " машин(ы) проехало\r\n";
                }
            }
            return s;
        }
    }
    public abstract class Generator : ICloneable
    {
        public int counter;
        public Car carPrototype;
        public Car DoTicks(int ticks)
        {
            counter -= ticks;
            if (counter <= 0)
            {
                float period = GeneratePeriod();
                counter = (int)Math.Round(period);
                return carPrototype.Clone() as Car;
            }
            return null;
        }
        protected abstract float GeneratePeriod();
        public abstract object Clone();
    }
    public class ExpGenerator : Generator
    {
        public float lambda;
        Random random;
        public ExpGenerator(float lambda, Car carPrototype, int counter)
        {
            this.lambda = lambda;
            this.random = new Random();
            this.carPrototype = carPrototype;
            this.counter = random.Next((int)lambda * 2);
        }
        protected override float GeneratePeriod()
        {
            float r = (float)random.NextDouble();
            return -(float)Math.Log(r) / lambda;
        }
        public override object Clone()
        {
            return new ExpGenerator(lambda, (Car)carPrototype.Clone(), counter);
        }
    }
    public class UniGenerator : Generator
    {
        public float a, b;
        Random random;
        public UniGenerator(float a, float b, Car carPrototype, int counter)
        {
            this.a = a;
            this.b = b;
            this.random = new Random();
            this.carPrototype = carPrototype;
            this.counter = random.Next((int)b * 2);
        }
        protected override float GeneratePeriod()
        {
            float r = (float)random.NextDouble();
            return a + (b - a) * r;
        }
        public override object Clone()
        {
            return new UniGenerator(a, b, (Car)carPrototype.Clone(), counter);
        }
    }
    public enum CheckPointStatus
    {
        Go, Stop
    }
    public class ThreadProcParams
    {
        public Form1 form;
        public int sleepTime;
        public float lambdaAudi, lambdaBus, lambdaTruck;
        public bool visualisation;
        public ThreadProcParams(Form1 form, int sleepTime, float lambdaAudi,
            float lambdaBus, float lambdaTruck, bool visualisation)
        {
            this.form = form;
            this.sleepTime = sleepTime;
            this.lambdaAudi = lambdaAudi;
            this.lambdaBus = lambdaBus;
            this.lambdaTruck = lambdaTruck;
            this.visualisation = visualisation;
        }
    }
}
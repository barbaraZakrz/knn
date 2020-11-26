using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace zadanie3
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public class Obiekt
    {
        public List<double> parametry;
        public int klasa;

        public Obiekt(List<double> parametryIn, int klasaIn)
        {
            parametry = parametryIn;
            klasa = klasaIn;
       
        }
    }

    public partial class MainWindow : Window
    {

        public static List<Obiekt> importData(string fileName)
        {
            string line;
            string[] lineSplit;
            List<Obiekt> allData = new List<Obiekt>();

            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                List<double> parametry = new List<double>();
                lineSplit = line.Split('\t');
                for (int i = 0; i < (lineSplit.Length -1); i++)
                {
                    parametry.Add(double.Parse(lineSplit[i], CultureInfo.InvariantCulture));
                }
                Obiekt data = new Obiekt(parametry,int.Parse(lineSplit[lineSplit.Length-1]));
                allData.Add(data); 
            }

            file.Close();
            return allData;
        }

        public static List<Obiekt> normalizacja(List<Obiekt> lista)
        {
            List<double> min = new List<double>(lista[0].parametry);
            List<double> max = new List<double>(lista[0].parametry);

            for (int i=1; i<lista.Count; i++)
            {
                for (int j=0; j<min.Count; j++)
                {
                    if (lista[i].parametry[j] < min[j])
                    {
                        min[j] = lista[i].parametry[j];
                    }
                    if (lista[i].parametry[j] > max[j])
                    {
                        max[j] = lista[i].parametry[j];
                    }
                }
            }

            for (int i = 0; i < lista.Count; i++)
            {
                for (int j = 0; j < min.Count; j++)
                {
                    lista[i].parametry[j] = (lista[i].parametry[j] - min[j]) / (max[j] - min[j]);
                }
            }

            return lista; 
        }

        public static double euklides(Obiekt obiekt1, Obiekt obiekt2)
        {
            double suma = 0;
            for (int i = 0; i < obiekt1.parametry.Count; i++)
            {
                suma += Math.Pow(obiekt1.parametry[i] - obiekt2.parametry[i], 2);
            }
            return Math.Sqrt(suma);
        }
        
        public static double manhattan(Obiekt obiekt1, Obiekt obiekt2)
        {
            double suma = 0;
            for (int i = 0; i < obiekt1.parametry.Count; i++)
            {
                suma += Math.Abs(obiekt1.parametry[i] - obiekt2.parametry[i]);
            }
            return suma;
        }

        public static double czebyszew(Obiekt obiekt1, Obiekt obiekt2)
        {
            double suma = 0;
            for (int i = 0; i < obiekt1.parametry.Count; i++)
            {
                if (suma < Math.Abs(obiekt1.parametry[i] - obiekt2.parametry[i]))
                {
                    suma = Math.Abs(obiekt1.parametry[i] - obiekt2.parametry[i]);
                }
            }
            return suma;
        }

        public static double minkowski(Obiekt obiekt1, Obiekt obiekt2, double p)
        {
            double suma = 0;
            for (int i = 0; i < obiekt1.parametry.Count; i++)
            {
                suma += Math.Pow(Math.Abs(obiekt1.parametry[i] - obiekt2.parametry[i]), p);
            }
            return Math.Pow(suma, 1/p);
        }

        public static double logarytm(Obiekt obiekt1, Obiekt obiekt2)
        {
            double suma = 0;
            for (int i = 0; i < obiekt1.parametry.Count; i++)
            {
                suma += Math.Abs(Math.Log(obiekt1.parametry[i]) - Math.Log(obiekt2.parametry[i]));
            }
            return suma;
        }

        public static int knn(List<Obiekt> lista, Obiekt obiekt, int k, int metryka, double p = 2)
        {
            List<Obiekt> posortowane = new List<Obiekt>();
            posortowane.Add(lista[0]);

            switch (metryka)
            {
                case 1:
                    for (int i = 1; i < lista.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < posortowane.Count; j++)
                        {
                            if (euklides(obiekt, lista[i]) < euklides(obiekt, posortowane[j]))
                            {
                                posortowane.Insert(j, lista[i]);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            posortowane.Add(lista[i]);
                        }
                    }
                    break;
                case 2:
                    for (int i = 1; i < lista.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < posortowane.Count; j++)
                        {
                            if (manhattan(obiekt, lista[i]) < manhattan(obiekt, posortowane[j]))
                            {
                                posortowane.Insert(j, lista[i]);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            posortowane.Add(lista[i]);
                        }
                    }
                    break;
                case 3:
                    for (int i = 1; i < lista.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < posortowane.Count; j++)
                        {
                            if (czebyszew(obiekt, lista[i]) < czebyszew(obiekt, posortowane[j]))
                            {
                                posortowane.Insert(j, lista[i]);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            posortowane.Add(lista[i]);
                        }
                    }
                    break;
                case 4:
                    for (int i = 1; i < lista.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < posortowane.Count; j++)
                        {
                            if (minkowski(obiekt, lista[i],p) < minkowski(obiekt, posortowane[j],p))
                            {
                                posortowane.Insert(j, lista[i]);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            posortowane.Add(lista[i]);
                        }
                    }
                    break;
                case 5:
                    for (int i = 1; i < lista.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < posortowane.Count; j++)
                        {
                            if (euklides(obiekt, lista[i]) < euklides(obiekt, posortowane[j]))
                            {
                                posortowane.Insert(j, lista[i]);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            posortowane.Add(lista[i]);
                        }
                    }
                    break;
            }

            Dictionary<int, int> klasyDec = new Dictionary<int, int>();
            for (int i = 0; i < k; i++)
            {
                if (klasyDec.ContainsKey(posortowane[i].klasa))
                {
                    klasyDec[posortowane[i].klasa]++;
                }
                else
                {
                    klasyDec.Add(posortowane[i].klasa, 1);
                }
            }
            int klasa = klasyDec.Aggregate((x, y) => x.Value < y.Value ? x : y).Key; //znalezione na stackoverflow, wyszukuje klucz o najmniejszej wartości 
            return klasa;


        }

        public static double jedenKontraReszta(List<Obiekt> lista, int k, int metryka, double p = 2)
        {
            int poprawne = 0;
            for (int i = 0; i<lista.Count; i++)
            {
                if (knn(lista, lista[i], k, metryka, p) == lista[i].klasa)
                {
                    poprawne++;
                }
            }
            return Convert.ToDouble(poprawne) / Convert.ToDouble(lista.Count);
        }

        public static string toText(List<Obiekt> list)
        {
            string text = "";
            for (int i = 0; i < list.Count; i++)
            {
                for (int j=0; j < list[i].parametry.Count; j++)
                {
                    text += list[i].parametry[j].ToString("0.##");
                    text += " ";

                }
                text += "klasa = ";
                text += list[i].klasa.ToString();
                text += "\n";
            }
            return text;
        }

        public void sprawdz(List<Obiekt> lista, int k = 3, double p = 2)
        {
            string text = "";
            text += "euklides: " + jedenKontraReszta(lista, k, 1, p).ToString("0.####") + "\n";
            text += "manhattan: " + jedenKontraReszta(lista, k, 2, p).ToString("0.####") + "\n";
            text += "czebyszew: " + jedenKontraReszta(lista, k, 3, p).ToString("0.####") + "\n";
            text += "minkowski: " + jedenKontraReszta(lista, k, 4, p).ToString("0.####") + "\n";
            text += "logarytm: " + jedenKontraReszta(lista, k, 5, p).ToString("0.####") + "\n";
            Sprawdzenie.Text = text;
        }

        public int sprawdzK(List<Obiekt> lista, int k)
        {
            Dictionary<int, int> klasyDec = new Dictionary<int, int>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (klasyDec.ContainsKey(lista[i].klasa))
                {
                    klasyDec[lista[i].klasa]++;
                }
                else
                {
                    klasyDec.Add(lista[i].klasa, 1);
                }
            }
            if(k>klasyDec.Min(m => m.Value))
            {
                return klasyDec.Min(m => m.Value);
            }
            else
            {
                return k;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Uruchom_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "TXT Files (*.txt)|*.txt" }; //okno wyboru pliku ze stackoverflow
            var result = ofd.ShowDialog();
            if (result == false) return;
            FileText.Text = ofd.FileName;

            List<Obiekt> allData = new List<Obiekt>();
            allData = importData(ofd.FileName);

            allData = normalizacja(allData);
            Debug.Text = toText(allData);
            int k;
            if( !int.TryParse(InputK.Text, out k))
            {
                k = 3;
            }
            double p;
            if (!double.TryParse(InputK.Text, out p))
            {
                p = 2;
            }
            sprawdz(allData, sprawdzK(allData,k), p);
            
        }

        private void Debug_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Sprawdzenie_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InputP_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InputK_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FileText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

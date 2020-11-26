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

        public static int knn(List<Obiekt> lista, Obiekt obiekt, int k)
        {
            List<Obiekt> posortowane = new List<Obiekt>();
            posortowane[0] = lista[0];
            for (int i =1; i<lista.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j<posortowane.Count; j++)
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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Debug_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Uruchom_Click(object sender, RoutedEventArgs e)
        {
            List<Obiekt> allData = new List<Obiekt>();
            allData = importData("iris.txt");
            allData = normalizacja(allData);
            Debug.Text = toText(allData);
        }
    }
}

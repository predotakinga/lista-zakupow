using System.ComponentModel;
using System.Windows.Input;
using MySql.Data.MySqlClient;


namespace WpfApp10.ViewModel
{
    using BaseClass;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using WpfApp10.Model;
    using System.Windows;
    using System;


    class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged(params string[] namesOfProperties)
        {
            if (PropertyChanged != null)
            {
                foreach (var prop in namesOfProperties)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }



        private IList<Miary> miary = new List<Miary>();
        private Miary wybranaMiara;


        public ObservableCollection<Produkt> produkty { get; set; }
        private string selectedProduct;
        public Produkt selectedListProduct;

        private string[] productValues = new string[4];
        private int numberOfProduct;
        private string type;
        private double cena;
        private double priceAtLast;
        public int counter = 0;
       
        ObservableCollection<string> productsCategory;
        ObservableCollection<string> listOfProducts;
        
        private double sumary;
        public string SelectedProduct
        {
            get { return selectedProduct; }

            set
            {
                selectedProduct = value;
                onPropertyChanged(nameof(SelectedProduct));
            }
        }
        public Produkt SelectedListProduct
        {
            get { return selectedListProduct; }

            set
            {
                selectedListProduct = value;
                onPropertyChanged(nameof(SelectedListProduct));
            }
        }
        public string[] ProductValues
        {
            get { return productValues; }

            set
            {
                productValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProductValues)));
            }
        }
        public int NumberOfProduct
        {
            get { return numberOfProduct; }

            set
            {
                numberOfProduct = value;
                onPropertyChanged(nameof(NumberOfProduct));
            }
        }
        public string Type
        {
            get { return type; }

            set
            {
                type = value;
                onPropertyChanged(nameof(Type));
            }
        }
        public ObservableCollection<Produkt> Produkty
        {
            get
            {
                return produkty;
            }
            set
            {
                produkty = value;
                onPropertyChanged(nameof(Produkty));
            }
        }
        public ObservableCollection<string> ProductsCategory
        {
            get
            {
                return productsCategory;
            }
            set
            {
                productsCategory = value;
                onPropertyChanged(nameof(ProductsCategory));
            }
        }

        public ObservableCollection<string> ListOfProducts
        {
            get
            {
                return listOfProducts;
            }
            set
            {
                listOfProducts = value;
                onPropertyChanged(nameof(ListOfProducts));
            }
        }

        public double Sumary
        {

            get { return sumary; }

            private set
            {
                sumary = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sumary)));
            }
        }
        public MainViewModel()
        {
            Produkty = new ObservableCollection<Produkt>();
            PopulateComoboBoxes();
            GettingList();
        }

        private ICommand zwieksz;
        public ICommand Zwieksz
        {
            get
            {
                return zwieksz ?? (zwieksz = new RelayCommand(
                                         (p) =>
                                         {
                                             



                                         },
                                         p => true));



            }
        }
        private ICommand click;
        public ICommand Click1
        {
            get
            {
                return click ?? (click = new BaseClass.RelayCommand(p => { HandleClick(p); }, p => true));
            }
        }
        private ICommand click2;
        public ICommand Click2
        {
            get
            {
                return click2 ?? (click2 = new BaseClass.RelayCommand(p => { clear(); }, p => true));
            }
        }
        private ICommand click3;
        public ICommand Click3
        {
            get
            {
                return click3 ?? (click3 = new BaseClass.RelayCommand(p => { deleteProduct(); }, p => true));
            }
        }
        private ICommand dodaj = null;

        public ICommand Dodaj
        {

            get
            {
              
                return dodaj ?? (dodaj = new BaseClass.RelayCommand(p => { Add(p);  }, p => true));
            }
        }

        public IList<Miary> Miara
        {
            get
            {
                return miary;
            }
            set
            {
                miary = value;
            }
        }
        public Miary WybranaMiara
        {
            get
            {
                return wybranaMiara;
            }
            set
            {
                wybranaMiara = value;
            }
        }
        private void clear()
        {
            Produkty.Clear();
            Produkt.SetSuma(0);
            Sumary = Produkt.GetSuma();

            string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
            MySqlCommand myCommand = null;
            MySqlConnection conn = null;
            string clear_all;
            conn = new MySqlConnection(connstring);
            conn.Open();

            clear_all = "DELETE FROM listazakupow";
            myCommand = new MySqlCommand(clear_all, conn);

            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();

            myReader.Read();
            myReader.Close();
            conn.Close();

            counter = 0;
        }
        private void deleteProduct()
        {
            string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
            MySqlCommand myCommand = null;
            MySqlConnection conn = null;
            conn = new MySqlConnection(connstring);
            conn.Open();

            string query = "delete from listazakupow order by ID desc limit 1";
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "listazakupow");
            DataTable dt = ds.Tables["listazakupow"];
            conn.Close();


            Produkty.RemoveAt(counter-1);
            counter--;
        }

        private void GettingList()
        {
            ObservableCollection<string> zamowienieDB = new ObservableCollection<string>();

            string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
            string query = "SELECT Nazwa, Ilosc, Rodzaj, Cena FROM listazakupow";
            MySqlConnection conn = new MySqlConnection(connstring);
            MySqlCommand myCommand = new MySqlCommand(query, conn);
            conn.Open();

            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                zamowienieDB.Add(myReader.GetString(0));
            }

            listOfProducts = zamowienieDB;
            ListOfProducts = listOfProducts;
            conn.Close();
        }

        private void Add(object box)
        {
            string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
            MySqlCommand myCommand = null;
            MySqlConnection conn = null;
            string query_cena;
            conn = new MySqlConnection(connstring);
            conn.Open();

            if ((selectedProduct != null) && (wybranaMiara != null) && (numberOfProduct != 0))
            {
                if (wybranaMiara.typMiary == "kg")
                {
                    query_cena = "SELECT CenaZaKg FROM produkty WHERE Nazwa='" + SelectedProduct + "'";
                    myCommand = new MySqlCommand(query_cena, conn);

                    MySqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();

                    myReader.Read();

                    cena = myReader.GetDouble(0);
                    myReader.Close();
                    conn.Close();
                }
                else if (wybranaMiara.typMiary == "sztuki") 
                {
                    query_cena = "SELECT CenaZaSzt FROM produkty WHERE Nazwa='" + SelectedProduct + "'";
                    myCommand = new MySqlCommand(query_cena, conn);

                    MySqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();

                    myReader.Read();

                    cena = myReader.GetDouble(0);
                    myReader.Close();
                    conn.Close();
                    System.Diagnostics.Debug.Print(cena.ToString());
                }
                else 
                {
                    query_cena = "SELECT CenaZaPaczke FROM produkty WHERE Nazwa='" + SelectedProduct + "'";
                    myCommand = new MySqlCommand(query_cena, conn);

                    MySqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();

                    myReader.Read();

                    cena = myReader.GetDouble(0);
                    myReader.Close();
                    conn.Close();
                    System.Diagnostics.Debug.Print(cena.ToString());
                }
                //cena = ConverterName() * ConverterMiara();
                produkty.Add(new Produkt(selectedProduct, wybranaMiara.ToString(), cena, numberOfProduct));
                Produkt.SetSuma(cena, numberOfProduct);
                Sumary = Produkt.GetSuma();
                priceAtLast = cena * numberOfProduct;
                counter++;
            }
            Produkty = produkty;

            conn.Open();
            string query = "insert into listazakupow (Nazwa, Rodzaj, Ilosc, Cena) VALUES ('"+ SelectedProduct + "', '" + wybranaMiara + "'," + numberOfProduct + ", '" + Math.Round(priceAtLast, 2).ToString().Replace(",", ".") + "');";
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "listazakupow");
            DataTable dt = ds.Tables["listazakupow"];
            conn.Close();
        }

        private void HandleClick(object box)
        {
            string indS = (string)box;
            int ind = int.Parse(indS);
            if(ind == 0)
            {
                ObservableCollection<string> warzywaDB = new ObservableCollection<string>();  
                string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
                string query = "SELECT Nazwa FROM produkty WHERE Kategoria='Warzywa'";
                MySqlConnection conn = new MySqlConnection(connstring);
                MySqlCommand myCommand = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    warzywaDB.Add(myReader.GetString(0));
                }

                productsCategory = warzywaDB;

                conn.Close();
            }
            else if (ind == 1)
            {
                ObservableCollection<string> owoceDB = new ObservableCollection<string>();
                string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
                string query = "SELECT Nazwa FROM produkty WHERE Kategoria='Owoce'";
                MySqlConnection conn = new MySqlConnection(connstring);
                MySqlCommand myCommand = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    owoceDB.Add(myReader.GetString(0));
                }

                productsCategory = owoceDB;

                conn.Close();
            }
            else if (ind == 2)
            {
                ObservableCollection<string> artspozywczeDB = new ObservableCollection<string>();
                string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
                string query = "SELECT Nazwa FROM produkty WHERE Kategoria='Art. Spożywcze'";
                MySqlConnection conn = new MySqlConnection(connstring);
                MySqlCommand myCommand = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    artspozywczeDB.Add(myReader.GetString(0));
                }

                productsCategory = artspozywczeDB;

                conn.Close();
            }
            else if (ind == 3)
            {
                ObservableCollection<string> chemiaDB = new ObservableCollection<string>();
                string connstring = @"server=127.0.0.1;port=3306;userid=root;database=zakupy;SslMode=none";
                string query = "SELECT Nazwa FROM produkty WHERE Kategoria='Chemia'";
                MySqlConnection conn = new MySqlConnection(connstring);
                MySqlCommand myCommand = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    chemiaDB.Add(myReader.GetString(0));
                }

                productsCategory = chemiaDB;

                conn.Close();
            }
            ProductsCategory = productsCategory;
        }
        
        private void PopulateComoboBoxes()
        {


            Miara = new List<Miary>() { new Miary(){id =1, typMiary = "kg"},
            new Miary(){id= 2, typMiary = "sztuki"},
            new Miary(){id = 3 ,typMiary = "paczki" }
           };


        }
    }
}
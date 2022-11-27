using Disconnected_SQL.Commands;
using Disconnected_SQL.Models;
using Disconnected_SQL.Services;
using Disconnected_SQL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Disconnected_SQL.ViewModels
{
    internal class AppViewModel : BaseViewModel
    {

        private ObservableCollection<Author> authors;

        public ObservableCollection<Author> Authors
        {
            get { return authors; }
            set { authors = value; OnPropertyChanged(); }
        }

        public Author Author { get; set; }
        public RelayCommand ShowAllCommand { get; set; }

        public RelayCommand InsertCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand UpdateCommand { get; set; }
        public DataTable DataTable { get; set; }

        private DataView dataView;
        public DataView DataView
        {
            get { return dataView; }
            set { dataView = value; OnPropertyChanged(); }
        }

        private Author selectedAuthor;

        public Author SelectedAuthor
        {
            get { return selectedAuthor; }
            set { selectedAuthor = value;OnPropertyChanged(); }
        }



        public List<Author> ConvertDataTableToList(DataTable dT)
        {
            var authors = new List<Author>();
            for (int i = 0; i < dT.Rows.Count; i++)
            {
                var author = new Author();
                author.Id = Convert.ToInt32(dT.Rows[i]["ID"]);
                author.FirstName = dT.Rows[i]["FirstName"].ToString();
                author.LastName = dT.Rows[i]["LastName"].ToString();
                authors.Add(author);
            }
            return authors;
        }

        public AppViewModel()
        {
            Authors = new ObservableCollection<Author>();
            SelectedAuthor = new Author();
            Author = new Author();
            DataTable = new DataTable();

            ShowAllCommand = new RelayCommand(c =>
            {
                DMLOperations operations = new DMLOperations();
                DataTable = operations.DisplayData();
                Authors=new ObservableCollection<Author>(ConvertDataTableToList(DataTable));
            });

            InsertCommand = new RelayCommand(c =>
            {
                var insertWindow = new InsertWindow();
                var insertViewModel = new InsertViewModel();
                insertWindow.DataContext = insertViewModel;

                insertWindow.ShowDialog();

                Author = insertViewModel.Author;
                Authors = new ObservableCollection<Author>(ConvertDataTableToList(DataTable));

                if (!Authors.Any(d => d.Id == Author.Id))
                {
                    if (Author.FirstName != null && Author.LastName != null)
                    {
                        DMLOperations operations = new DMLOperations();
                        DataTable = operations.InsertData(Author.Id, Author.FirstName, Author.LastName);
                        Authors= new ObservableCollection<Author>(ConvertDataTableToList(DataTable));
                        MessageBox.Show($"{Author.FirstName} {Author.LastName} has been added successfully");
                    }
                    else
                    {
                        MessageBox.Show($"First name and Last name should be filled", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); ;

                    }
                }
                else
                {
                    MessageBox.Show($"Author with ID {Author.Id} already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                }
            });

            DeleteCommand = new RelayCommand(c =>
            {
                var deleteWindow = new DeleteView();
                var viewModel = new DeleteViewModel();
                deleteWindow.DataContext = viewModel;
                deleteWindow.ShowDialog();

                Authors = new ObservableCollection<Author>(ConvertDataTableToList(DataTable));
                Author = Authors.ToList().Find(d => d.Id == viewModel.ID);
                if (Authors.Any(d => d.Id == viewModel.ID))
                {
                    DMLOperations operations = new DMLOperations();
                    DataTable = operations.DeleteData(viewModel.ID);
                    Authors = new ObservableCollection<Author>(ConvertDataTableToList(DataTable));
                    MessageBox.Show($"{Author.FirstName} {Author.LastName} has been deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Author with ID {viewModel.ID} does not exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            UpdateCommand = new RelayCommand(c =>
            {
                var updateView = new UpdateWindow();
                var viewModel = new UpdateViewModel();
                updateView.DataContext = viewModel;
                viewModel.Author = SelectedAuthor;
                string oldName = SelectedAuthor.FirstName;
                string oldSurname = SelectedAuthor.LastName;
                updateView.ShowDialog();
                Author = Authors.ToList().Find(d => d.Id == viewModel.Author.Id);
                if (SelectedAuthor != null)
                {

                    if (oldName != viewModel.Author.FirstName || oldSurname != viewModel.Author.LastName)
                    {
                        DMLOperations operations = new DMLOperations();
                        DataTable = operations.UpdateData(viewModel.Author.FirstName, viewModel.Author.LastName, viewModel.Author.Id);
                        Authors = new ObservableCollection<Author>(ConvertDataTableToList(DataTable));
                        MessageBox.Show($"Author with ID {Author.Id} has been updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Wrong Input Format");
                    }
                }
                else
                {
                    MessageBox.Show("No author has been selected");
                }
            });



        }
    }
}
